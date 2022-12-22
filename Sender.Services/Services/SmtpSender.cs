﻿using MimeKit;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Abstractions;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sender.Services.Interfaces;
using Sender.Services.Extensions;
using Sender.Services.Models;

namespace Sender.Services.Services
{
    /// <inheritdoc cref="ISmtpSender" />
    public sealed class SmtpSender : ISmtpSender
    {
        private Func<ISmtpClient, Task> _customAuthenticationMethod;

        /// <summary>
        /// После указана времени экземляр класса будет удален из памяти SmtpSender
        /// </summary>
        public DateTime DateExpirationSmtpSender;// = DateTime.UtcNow.AddDays(1);

        private readonly ILogger<SmtpSender> _logger;
        private readonly ISmtpClient _smtpClient;
        private readonly EmailSenderOptions _senderOptions;

        /// <inheritdoc cref="ISmtpSender" />
        public SmtpSender(IOptions<EmailSenderOptions> senderOptions, ILogger<SmtpSender> logger = null, IProtocolLogger protocolLogger = null, ISmtpClient smtpClient = null)
        {
            DateExpirationSmtpSender = DateTime.UtcNow.AddHours(senderOptions.Value.DateExpirationSmtpSender);
            _logger = logger ?? NullLogger<SmtpSender>.Instance;
            _senderOptions = senderOptions.Value;
            if (string.IsNullOrWhiteSpace(_senderOptions.SmtpHost))
                throw new NullReferenceException(nameof(EmailSenderOptions.SmtpHost));
            var smtpLogger = protocolLogger ?? GetProtocolLogger(smtpClient == null ? _senderOptions.ProtocolLog : null, _senderOptions.ProtocolLogFileAppend);
            _smtpClient = smtpClient ?? (smtpLogger !=null ? new SmtpClient(smtpLogger) : new SmtpClient());
        }

        //public static SmtpSender Create(string smtpHost, ushort smtpPort = 0, string username = null, string password = null, string protocolLog = null, bool protocolLogFileAppend = false)
        //{
        //    var smtpCredential = new NetworkCredential(username, password);
        //    var sender = Create(smtpHost, smtpCredential, smtpPort, protocolLog);
        //    return sender;
        //}


        public SmtpSender(string smtpHost,ushort smtpPort, NetworkCredential smtpCredential, ILogger<SmtpSender> logger = null, string protocolLog = null, bool protocolLogFileAppend = false, IProtocolLogger protocolLogger = null, ISmtpClient smtpClient = null)
        {
            EmailSenderOptions emailopt = new EmailSenderOptions
            {
                SmtpHost = smtpHost,
                SmtpPort = smtpPort,
                SmtpCredential = smtpCredential,
                ProtocolLog = protocolLog,
                ProtocolLogFileAppend = protocolLogFileAppend
            };

            _logger = logger ?? NullLogger<SmtpSender>.Instance;
            _senderOptions = emailopt;
            if (string.IsNullOrWhiteSpace(_senderOptions.SmtpHost))
                throw new NullReferenceException(nameof(EmailSenderOptions.SmtpHost));

            var smtpLogger = protocolLogger ?? GetProtocolLogger(smtpClient == null ? _senderOptions.ProtocolLog : null, _senderOptions.ProtocolLogFileAppend);
            _smtpClient = smtpClient ?? (smtpLogger != null ? new SmtpClient(smtpLogger) : new SmtpClient());
        }


        //public static SmtpSender Create(string smtpHost, NetworkCredential smtpCredential, ushort smtpPort = 0, string protocolLog = null, bool protocolLogFileAppend = false)
        //{
        //    DateExpirationClass = DateTime.UtcNow.AddDays(1);
        //    var senderOptions = new EmailSenderOptions(smtpHost, smtpCredential, smtpPort, protocolLog, protocolLogFileAppend);
        //    var sender = Create(senderOptions);
        //    return sender;
        //}

        //public static SmtpSender Create(EmailSenderOptions emailSenderOptions, ILogger<SmtpSender> logger = null)
        //{
        //    DateExpirationClass = DateTime.UtcNow.AddDays(1);
        //    var senderOptions = Options.Create(emailSenderOptions);
        //    var sender = new SmtpSender(senderOptions, logger);
        //    return sender;
        //}

        //public SmtpSender SetProtocolLog(string logFilePath, bool append = false)
        //{
        //    DateExpirationClass = DateTime.UtcNow.AddDays(1);
        //    _senderOptions.ProtocolLog = logFilePath;
        //    _senderOptions.ProtocolLogFileAppend = append;
        //    var sender = Create(_senderOptions, _logger);
        //    return sender;
        //}

        public SmtpSender SetPort(ushort smtpPort)
        {
            _senderOptions.SmtpPort = smtpPort;
            return this;
        }

        public SmtpSender SetCredential(string username, string password)
        {
            _senderOptions.SmtpCredential = new NetworkCredential(username, password);
            return this;
        }

        public SmtpSender SetCustomAuthentication(Func<ISmtpClient, Task> customAuthenticationMethod)
        {
            _customAuthenticationMethod = customAuthenticationMethod;
            return this;
        }

        public IEmailWriter WriteEmail => new EmailWriter(this);

        public static IProtocolLogger GetProtocolLogger(string logFilePath = null, bool append = false, IFileSystem fileSystem = null)
        {
            IProtocolLogger protocolLogger = null;
            if (logFilePath?.Equals("console", StringComparison.OrdinalIgnoreCase) ?? false)
            {
                protocolLogger = new ProtocolLogger(Console.OpenStandardError());
            }
            else if (!string.IsNullOrWhiteSpace(logFilePath))
            {
                bool isMockFileSystem = fileSystem != null &&
                    fileSystem.GetType().Name == "MockFileSystem";
                var _fileSystem = fileSystem ?? new FileSystem();
                var directoryName = _fileSystem.Path.GetDirectoryName(logFilePath);
                if (!string.IsNullOrWhiteSpace(directoryName))
                    _fileSystem.Directory.CreateDirectory(directoryName);
                if (isMockFileSystem)
                    protocolLogger = new ProtocolLogger(Stream.Null);
                else
                    protocolLogger = new ProtocolLogger(logFilePath, append);
            }
            return protocolLogger;
        }

        public async ValueTask<ISmtpClient> ConnectSmtpClientAsync(CancellationToken cancellationToken = default) =>
            await GetConnectedAuthenticatedAsync(cancellationToken).ConfigureAwait(false);

        internal async ValueTask<ISmtpClient> GetConnectedAuthenticatedAsync(CancellationToken cancellationToken = default)
        {
            await ConnectAsync(cancellationToken).ConfigureAwait(false);
            await AuthenticateAsync(cancellationToken).ConfigureAwait(false);
            return _smtpClient;
        }

        internal async ValueTask ConnectAsync(CancellationToken cancellationToken = default)
        {
            if (!_smtpClient.IsConnected && !string.IsNullOrEmpty(_senderOptions.SmtpHost))
            {
                await _smtpClient.ConnectAsync(_senderOptions.SmtpHost, _senderOptions.SmtpPort, cancellationToken: cancellationToken).ConfigureAwait(false);
                _logger.LogTrace($"SMTP client connected to {_senderOptions.SmtpHost}.");
                if (_smtpClient.Capabilities.HasFlag(SmtpCapabilities.Size))
                    _logger.LogDebug($"The SMTP server has a size restriction on messages: {_smtpClient.MaxSize}.");
            }
        }

        /// <summary>
        /// Authenticating via a SASL mechanism may be a multi-step process.
        /// <see href="http://www.mimekit.net/docs/html/T_MailKit_Security_SaslMechanism.htm"/>
        /// <seealso href="http://www.mimekit.net/docs/html/T_MailKit_Security_SaslMechanismOAuth2.htm"/>
        /// </summary>
        internal async ValueTask AuthenticateAsync(CancellationToken cancellationToken = default)
        {
            if (_senderOptions.SmtpCredential != null && !_smtpClient.IsAuthenticated)
            {
                if (_customAuthenticationMethod != null) // for XOAUTH2 and OAUTHBEARER
                    await _customAuthenticationMethod(_smtpClient).ConfigureAwait(false);
                else
                {
                    var ntlm = _smtpClient.AuthenticationMechanisms.Contains("NTLM") ?
                        new SaslMechanismNtlm(_senderOptions.SmtpCredential) : null;
                    if (ntlm?.Workstation != null)
                        await _smtpClient.AuthenticateAsync(ntlm, cancellationToken).ConfigureAwait(false);
                    else
                        await _smtpClient.AuthenticateAsync(_senderOptions.SmtpCredential, cancellationToken).ConfigureAwait(false);
                }
                _logger.LogTrace($"SMTP client authenticated with {_senderOptions.SmtpHost}.");
            }
        }

        public void RemoveAuthenticationMechanism(string authenticationMechanismsName)
        {
            if (_smtpClient.AuthenticationMechanisms.Contains(authenticationMechanismsName))
                _smtpClient.AuthenticationMechanisms.Remove(authenticationMechanismsName);
        }

        public static bool ValidateEmailAddresses(IEnumerable<string> sourceEmailAddresses, IEnumerable<string> destinationEmailAddresses, ILogger logger)
        {
            if (sourceEmailAddresses is null)
                throw new ArgumentNullException(nameof(sourceEmailAddresses));
            if (destinationEmailAddresses is null)
                throw new ArgumentNullException(nameof(destinationEmailAddresses));
            if (logger is null)
                logger = NullLogger.Instance;
            bool isValid = true;
            int sourceEmailAddressCount = 0, destinationEmailAddressCount = 0;
            foreach (var from in sourceEmailAddresses)
            {
                //if (from == "string")
                //{
                //    continue;
                //}
                if (!from.Contains("@"))
                {
                    logger.LogWarning($"From address is invalid ({from})");
                    isValid = false;
                }
                var t =destinationEmailAddresses.ToList();
                foreach (var to in destinationEmailAddresses)
                {
                    //if (to == "string")
                    //{
                    //    continue;
                    //}
                    if (!to.Contains("@"))
                    {
                        logger.LogWarning($"To address is invalid ({to})");
                        isValid = false;
                    }
                    if (to.Equals(from, StringComparison.OrdinalIgnoreCase))
                    {
                        logger.LogWarning($"Circular reference, To ({to}) == From ({from})");
                        isValid = false;
                    }
                    destinationEmailAddressCount++;
                }
                sourceEmailAddressCount++;
            }
            if (sourceEmailAddressCount == 0)
                logger.LogWarning("Source email address not specified");
            else if (destinationEmailAddressCount == 0)
                logger.LogWarning("Destination email address not specified");
            isValid &= sourceEmailAddressCount > 0 && destinationEmailAddressCount > 0;
            return isValid;
        }

        public static bool ValidateMimeMessage(MimeMessage mimeMessage, ILogger logger = null)
        {
            bool isValid = false;
            if (mimeMessage != null)
            {
                if (!mimeMessage.BodyParts.Any())
                    mimeMessage.Body = new TextPart { Text = string.Empty };
                var from = mimeMessage.From.Mailboxes.Select(m => m.Address);
                var toCcBcc = mimeMessage.To.Mailboxes.Select(m => m.Address)
                    .Concat(mimeMessage.Cc.Mailboxes.Select(m => m.Address))
                    .Concat(mimeMessage.Bcc.Mailboxes.Select(m => m.Address));
                isValid = ValidateEmailAddresses(from, toCcBcc, logger);
                if (mimeMessage.ReplyTo.Count == 0 && mimeMessage.From.Count == 0)
                    mimeMessage.ReplyTo.Add(new MailboxAddress("Unmonitored", $"noreply@localhost"));
                if (mimeMessage.From.Count == 0)
                    mimeMessage.From.Add(new MailboxAddress("LocalHost", $"{Guid.NewGuid():N}@localhost"));
            }
            return isValid;
        }

        public static string GetEnvelope(MimeMessage mimeMessage, bool includeTextBody = false)
        {
            string envelope = string.Empty;
            using (var text = new StringWriter())
            {
                text.Write("Message-Id: {0}. ", mimeMessage.MessageId);
                text.Write("Date: {0}. ", mimeMessage.Date);
                if (mimeMessage.From.Count > 0)
                    text.Write("From: {0}. ", string.Join("; ", mimeMessage.From));
                if (mimeMessage.To.Count > 0)
                    text.Write("To: {0}. ", string.Join("; ", mimeMessage.To));
                if (mimeMessage.Cc.Count > 0)
                    text.Write("Cc: {0}. ", string.Join("; ", mimeMessage.Cc));
                if (mimeMessage.Bcc.Count > 0)
                    text.Write("Bcc: {0}. ", string.Join("; ", mimeMessage.Bcc));
                text.Write("Subject: \"{0}\". ", mimeMessage.Subject);
                var attachmentCount = mimeMessage.Attachments.Count();
                if (attachmentCount > 0)
                    text.Write("{0} Attachment{1}: '{2}'. ",
                        attachmentCount, attachmentCount == 1 ? "" : "s",
                        string.Join("', '", mimeMessage.Attachments.GetAttachmentNames()));
                if (includeTextBody && mimeMessage.TextBody?.Length > 0)
                    text.Write($"TextBody: \"{mimeMessage.TextBody}\". ");
                envelope = text.ToString();
            }
            return envelope;
        }

        public async Task SendAsync(MimeMessage mimeMessage, CancellationToken cancellationToken = default, ITransferProgress transferProgress = null)
        {
            var flagValid = ValidateMimeMessage(mimeMessage, _logger);
            //if (!flagValid)
            //{
            //    // return false;
            //    return Task.FromException<bool>(new InvalidOperationException("Validation Email IsFaulted"));
            //}
            // throw new ArgumentException($"Validation Email IsFaulted");
             await ConnectSmtpClientAsync(cancellationToken);
            _logger.LogTrace($"Sending {GetEnvelope(mimeMessage, includeTextBody: true)}");
            string serverResponse = await _smtpClient.SendAsync(mimeMessage, cancellationToken, transferProgress).ConfigureAwait(false);
            _logger.LogTrace($"Server response: \"{serverResponse}\".");
        }

        public async Task<(bool,string)> TrySendAsync(MimeMessage mimeMessage, CancellationToken cancellationToken = default, ITransferProgress transferProgress = null)
        {
            bool isSent = false;
            try
            {
                await SendAsync(mimeMessage, cancellationToken, transferProgress).ConfigureAwait(false);
                isSent = true;
            }
            catch (AuthenticationException ex)
            {
                var mess = $"Failed to authenticate with mail server. {_senderOptions}";
                _logger.LogError(ex, mess);
                return (isSent, mess);

            }
            catch (InvalidOperationException ex)
            {
                var mess = $"Failed to connect to mail server. {_senderOptions}";
                _logger.LogError(ex, mess);
                return (isSent, mess);
            }
            catch (Exception ex)
            {
                var mess = $"Failed to send email. {mimeMessage}";
                _logger.LogError(ex, mess);
                return (isSent, mess);
            }
            return (isSent,null);
        }

        public ISmtpSender Copy() => MemberwiseClone() as ISmtpSender;

        public override string ToString() => _senderOptions.ToString();

        public async Task DisconnectAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Disconnecting SMTP email client...");
            if (_smtpClient.IsConnected)
                await _smtpClient.DisconnectAsync(true, cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask DisposeAsync()
        {
            await DisconnectAsync().ConfigureAwait(false);
            _smtpClient.Dispose();
        }

        public void Dispose()
        {
            DisconnectAsync().GetAwaiter().GetResult();
            _smtpClient.Dispose();
        }
    }
}