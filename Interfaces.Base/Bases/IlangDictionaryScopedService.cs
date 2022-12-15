namespace Interfaces.Base.Base
{
    public interface IlangDictionaryScopedService
    {
       // string _culture { get; set; }

        void Create(string culture);
        /// <summary>
        /// Получить строку по культуре, и значения по заданному слову из словаря
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string Get(string value);
    }

    /// <summary>
    /// Отравка сообщений от поставщика к потребителю
    /// </summary>
    public interface ISendMessageScopedService<CommandMessage>
    {
        Task Send(CommandMessage mes);
    }

   

}
