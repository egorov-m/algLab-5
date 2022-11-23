namespace algLab_5.Services.Logger
{
    /// <summary> Обработчик Записывающий сообщения в консоль </summary>
    public class ConsoleHandler : IMessageHandler
    {
        /// <summary> Объект поставщика консоли </summary>
        private readonly ConsoleProvider _consoleProvider;
        /// <summary> Будет ли выполняться печать в стиле заголовка </summary>
        private static bool _isWriteTitle;
        /// <summary> Добавлять ли пустую строку перед заголовком в консоли </summary>
        private static bool _isEmptyLineBeforeTitle;

        public ConsoleHandler(ConsoleProvider consoleProvider)
        {
            _consoleProvider = consoleProvider;
        }

        /// <summary> Разово поставить режим печати заголовка </summary>
        public static void SetIsWriteTitle() => _isWriteTitle = true;
        
        /// <summary> Разово установить перед заголовком пустую строку </summary>
        public static void SetIsEmptyLineBeforeTitle() => _isEmptyLineBeforeTitle = true;

        /// <summary> Выполнить запись в консоль </summary>
        /// <param name="message"> Сообщение </param>
        public void Log(string message)
        {
            if (_isWriteTitle)
            {
                _consoleProvider.ConsoleWriteLineTitle(message, _isEmptyLineBeforeTitle);
                _isWriteTitle = false;
                _isEmptyLineBeforeTitle = false;
            }
            else
            {
                _consoleProvider.ConsoleWriteLine(message);
            }
        }
    }
}