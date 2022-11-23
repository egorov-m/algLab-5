using System.IO;
using System.Text;

namespace algLab_5.Services.Logger
{
    /// <summary> Обработчик записывающий сообщения в файл </summary>
    public class FileHandler : IMessageHandler
    {
        /// <summary> Имя файла </summary>
        private string _fileName;

        public FileHandler()
        {
            _fileName = "log";
        }

        public FileHandler(string fileName)
        {
            _fileName = fileName;
        }

        /// <summary> Установить имя файла лога </summary>
        /// <param name="fileName"> Имя файла </param>
        public void SetFileName(string fileName)
        {
            _fileName = fileName;
        }

        /// <summary> Выполнить запись сообщения в файл </summary>
        /// <param name="message"> Сообщение </param>
        public void Log(string message)
        {
            using var writer = new StreamWriter($"{_fileName}.txt", append: true, Encoding.UTF8);
            writer.AutoFlush = true;
            writer.WriteLine(message);
        }
    }
}
