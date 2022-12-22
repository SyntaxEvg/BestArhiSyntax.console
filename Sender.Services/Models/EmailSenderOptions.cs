﻿using System;
using System.Net;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Sender.Services.Models
{
    public class EmailSenderOptions
    {
        public const string SectionName = "EmailSender";

        [Required]
        public string SmtpHost { get; set; }
        public ushort SmtpPort { get; set; } = 0;
        public int DateExpirationSmtpSender { get; set; } = 24;
        public NetworkCredential SmtpCredential { get; set; } = null;
        public string ProtocolLog { get; set; } = null;
        public bool ProtocolLogFileAppend { get; set; } = false;

        public EmailSenderOptions() { }

        public EmailSenderOptions(string smtpHost, NetworkCredential smtpCredential = null, ushort smtpPort = 0, string protocolLog = null, bool protocolLogFileAppend = false)
        {
            if (string.IsNullOrWhiteSpace(smtpHost))
                throw new ArgumentNullException(nameof(smtpHost));
            var hostParts = smtpPort == 0 ? smtpHost.Split(':') : Array.Empty<string>();
            if (hostParts.Length == 2 && ushort.TryParse(hostParts.LastOrDefault(), out smtpPort))
                smtpHost = hostParts.FirstOrDefault();
            if (smtpCredential != null && smtpCredential.UserName == null && smtpCredential.Password == null)
                smtpCredential = null;
            else if (smtpCredential != null && smtpCredential.UserName == null)
                smtpCredential.UserName = string.Empty;

            SmtpHost = smtpHost;
            SmtpPort = smtpPort;
            SmtpCredential = smtpCredential;
            ProtocolLog = protocolLog;
            ProtocolLogFileAppend = protocolLogFileAppend;
        }

        public override string ToString() => $"{SmtpHost}:{SmtpPort}";
    }
}
