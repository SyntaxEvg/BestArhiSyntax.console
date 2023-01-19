using System.ComponentModel.DataAnnotations;

namespace Interfaces.Base
{
    public interface ICommandMessageRequest
    {
        public Guid id { get; init; }

        /// <summary>
        /// Пароль,если подключение не стандартное
        /// </summary>
        public string? password { get; set; }
        public string? SmtpHost { get; set; }
        public string? SmtpPort { get; set; }

        /// <summary>
        /// От кого сообщение
        /// </summary>
        public string? From { get; init; } //так как проектов может быть много, отсылать письма надо  с разных, этот параметр будет обязательный
        /// <summary>
        /// Кому сообщение, может  быть  массивом SPLIT(';')
        /// </summary>
        public string To { get; init; }
        /// <summary>
        /// Получатели в списке Blind-Carpbon-Copy не будут видны другие получатели сообщения
        /// </summary>
        public string Bcc { get; set; } 

        /// <summary>
        /// Тема обычно представляет собой короткую строку, обозначающую тему сообщения. 
        /// </summary>
        public string Subject { get; init; } 

        /// <summary>
        /// Получает текст сообщения в формате HTML, если он существует.
        /// </summary>
        public string BodyHtml { get; set; }
        /// <summary>
        /// Добавляем получателей копии к электронному письму.
        /// Адреса электронной почты разбиты на ';', ',', ' ', '&', '|'.
        /// Имена автоматически извлекаются из адресов электронной почты.
        /// <summary>
        /// <param name="emailAddress">Адрес электронной почты получателя</param>
        public string Cc { get; set; }

        /// <summary>
        /// Имя файла,которое вы вложили
        /// </summary>
        public string NameFile { get; set; }
        /// <summary>
        /// Добавить файл,проверит,не битый ли он, отравлять в ,base64
        /// </summary>
        public string AttachFile { get; set; }
    }
    public class test : ICommandMessageRequest
    {
        public Guid id { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }
        public string? password { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string? SmtpHost { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string? SmtpPort { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string? From { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }
        public string To { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }
        public string Bcc { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Subject { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }
        public string BodyHtml { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Cc { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string NameFile { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string AttachFile { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}