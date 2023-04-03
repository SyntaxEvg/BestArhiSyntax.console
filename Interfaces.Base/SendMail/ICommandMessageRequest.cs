using System.ComponentModel.DataAnnotations;

namespace Interfaces.Base
{
    public interface ICommandMessageRequest
    {
        public Guid id { get; set; }

        /// <summary>
        /// Пароль,если подключение не стандартное
        /// </summary>
        public string? password { get; set; }
        public string? SmtpHost { get; set; }
        public string? SmtpPort { get; set; }

        /// <summary>
        /// От кого сообщение
        /// </summary>
        public string? From { get; set; } //так как проектов может быть много, отсылать письма надо  с разных, этот параметр будет обязательный
        /// <summary>
        /// Кому сообщение, может  быть  массивом SPLIT(';')
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// Получатели в списке Blind-Carpbon-Copy не будут видны другие получатели сообщения
        /// </summary>
        public string Bcc { get; set; } 

        /// <summary>
        /// Тема обычно представляет собой короткую строку, обозначающую тему сообщения. 
        /// </summary>
        public string Subject { get; set; } 

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

        public Guid id {get; set;}
        public string? password {get; set;}
        public string? SmtpHost {get; set;}
        public string? SmtpPort {get; set;}
        public string? From {get; set;}
        public string To {get; set;}
        public string Bcc {get; set;}
        public string Subject {get; set;}
        public string BodyHtml {get; set;}
        public string Cc {get; set;}
        public string NameFile {get; set;}
        public string AttachFile {get; set;}
    }
}