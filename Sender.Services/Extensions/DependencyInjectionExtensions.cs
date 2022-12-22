using System;
using System.IO.Abstractions;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Sender.Services.Models;
using Sender.Services.Interfaces;
using Sender.Services.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Sender.Services.Sender
{
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Add the .Sender configuration and services.
        /// Adds IOptions<<see cref="EmailSenderOptions"/>>,
        /// <see cref="IEmailWriter"/>, and <see cref="ISmtpSender"/>.
        /// </summary>
        /// <param name="services">Collection of service descriptors.</param>
        /// <param name="configuration">Application configuration properties.</param>
        /// <param name="sectionName">Configuration section name.</param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration configuration, string sectionName = EmailSenderOptions.SectionName)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));
            var configSection = configuration.GetRequiredSection(sectionName);
            services.Configure<EmailSenderOptions>(configSection);
            services.TryAddSingleton<IFileSystem, FileSystem>();
            //services.AddSingleton<ISmtpSender, SmtpSender>();
            services.TryAddScoped<ISmtpSender, SmtpSender>();
            services.TryAddScoped<IEmailWriter, EmailWriter>();
            services.AddHostedService<SubscrubeMessageHostedService>();//Так как используется Rx,нам нужно прописывать место где создается прописка в текущей сборке, //Подписка на получения сообщен,и отправка их в сервис почты

            return services;
        }

        ///// <summary>
        ///// Add the .Sender services.
        ///// Adds <see cref="IEmailWriter"/>, and <see cref="ISmtpSender"/>.
        ///// </summary>
        ///// <param name="services">Collection of service descriptors.</param>
        ///// <returns><see cref="IServiceCollection"/>.</returns>
        //private static IServiceCollection AddEmailSender(this IServiceCollection services)
        //{
        //    services.TryAddSingleton<IFileSystem, FileSystem>();
        //    //services.AddSingleton<ISmtpSender, SmtpSender>();
        //    services.TryAddScoped<ISmtpSender, SmtpSender>();
        //    services.TryAddScoped<IEmailWriter, EmailWriter>();
        //    //services.AddHostedService<SubscrubeMessageHostedService>(); //Подписка на получения сообщен,и отправка их в сервис почты

        //    // services.AddTransient<ISmtpSender, SmtpSender>();
        //    return services;
        //}

        /// <summary>
        /// Add the .Sender configuration and services.
        /// Adds IOptions<<see cref="EmailSenderOptions"/>>.
        /// </summary>
        /// <param name="services">Collection of service descriptors.</param>
        /// <param name="emailSenderOptions">Email sender options.</param>
        /// <returns><see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddEmailSender(this IServiceCollection services, Action<EmailSenderOptions> emailSenderOptions)
        {
            if (emailSenderOptions == null)
                throw new ArgumentNullException(nameof(emailSenderOptions));
            services.Configure(emailSenderOptions);
            return services;
        }
    }
}
