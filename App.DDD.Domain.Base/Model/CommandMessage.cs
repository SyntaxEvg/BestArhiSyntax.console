using Interfaces.Base;
using System.ComponentModel.DataAnnotations;

namespace App.DDD.Domain.Models
{
    public class CommandMessageRequest : ICommandMessageRequest
    {

        public Guid id { get; init; } = Guid.NewGuid();
        /// <summary>
        /// От кого сообщение
        /// </summary>
        [Required]
        public string From { get; init; } //так как проектов может быть много, отсылать письма надо с разных, этот параметр будет обязательный
        /// <summary>
        /// Кому сообщение, может  быть строкой разделенной SPLIIT(';')
        /// </summary>
        [Required]
        public string To { get; init; }
        /// <summary>
        /// Получатели в списке Blind-Carpbon-Copy не будут видны другие получатели сообщения
        /// </summary>
        public string? Bcc { get; set; }

        /// <summary>
        /// Тема обычно представляет собой короткую строку, обозначающую тему сообщения
        /// </summary>
        public string Subject { get; init; } = "Re:";

        /// <summary>
        /// Получает текст сообщения в формате HTML, если он существует
        /// </summary>
        public string? BodyHtml { get; set; } = "<h3>Hello</h3>";



        /// <summary>
        /// Добавляем получателей копии к электронному письму.
        /// Адреса электронной почты разбиты на ';'
        /// Имена автоматически извлекаются из адресов электронной почты
        /// <summary>
        /// <param name="emailAddress">Адрес электронной почты получателя</param>
        public string? Cc { get; set; }

        /// <summary>
        /// Имя файла,которое вы вложили
        /// </summary>
        public string NameFile { get; set; } = "test";

        /// <summary>
        /// Добавить файл,проверит,не битый ли он, отравлять в ,base64
        /// </summary>
        public string? AttachFile { get; set; }
        /// <summary>
        /// Если указан, будет создана новая конфигурация
        /// </summary>
        public string? password { get; set; }
        /// <summary>
        ///  Если указан, будет создана новая конфигурация
        /// </summary>
        public string? SmtpHost { get; set; }
        /// <summary>
        /// не ноль,  Если указан, будет создана новая конфигурация
        /// </summary>
        public string? SmtpPort { get; set; }

        public CommandMessageRequest()
        {

        }
    }

    /// <summary>
    /// для отправки ответа на Шину или REST API
    /// </summary>
    public class CommandMessageResponse
    {
        public Guid id { get; init; }
        public bool Status { get; init; }

        public CommandMessageResponse()
        {

        }
    }
}

//.Receiver