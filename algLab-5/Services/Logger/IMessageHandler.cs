namespace algLab_5.Services.Logger
{
    /// <summary> Интерфейс обработчика логирования </summary>
    public interface IMessageHandler
    {
        /// <summary> Выполнить запись</summary>
        /// <param name="message"> Сообщение </param>
        void Log(string message);
    }
}
