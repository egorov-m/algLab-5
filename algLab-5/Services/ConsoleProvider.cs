using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace algLab_5.Services
{
    /// <summary> Класс управления внутренней консолью </summary>
    public class ConsoleProvider
    {
        /// <summary> Семейство шрифтов текста в консоли </summary>
        private static readonly FontFamily TextFontFamily = new ("Lucida Console");
        /// <summary> Размер шрифта текста в консоли </summary>
        private const int TextFontSize = 9;
        /// <summary> Цвет заголовка в консоли </summary>
        private static readonly Color TextTitleColor = Color.FromRgb(113, 96, 232);
        /// <summary> Цвет текста в консоли </summary>
        private static readonly Color TextColor = Color.FromRgb(214, 214, 214);
        /// <summary> Вес текста в консоли </summary>
        private static readonly FontWeight TextTitleFontWeight = FontWeight.FromOpenTypeWeight(500);

        /// <summary> Контейнер текстовых элементов консоли </summary>
        private readonly StackPanel _consoleContainer;

        public ConsoleProvider(StackPanel consoleContainer)
        {
            _consoleContainer = consoleContainer;
        }

        /// <summary> Печатать заголовок в консоль с новой строки </summary>
        /// <param name="text"> Текст для печати </param>
        /// <param name="isEmptyLineBefore"> Печатать ли пустую строку перед заголовком </param>
        public void ConsoleWriteLineTitle(string text, bool isEmptyLineBefore = false)
        {
            var textBlock = new TextBlock()
            {
                Text = text,
                Foreground = new SolidColorBrush(TextTitleColor),
                FontFamily = TextFontFamily,
                FontSize = TextFontSize,
                FontWeight = TextTitleFontWeight
            };

            if (isEmptyLineBefore) _consoleContainer.Children.Add(new TextBlock() {FontSize = TextFontSize});
            _consoleContainer.Children.Add(textBlock);
        }

        /// <summary> Печатать текст в консоль с ново строки </summary>
        /// <param name="text"> Текст для печати </param>
        public void ConsoleWriteLine(string text)
        {
            var textBlock = new TextBlock()
            {
                Text = text,
                Foreground = new SolidColorBrush(TextColor),
                FontFamily = TextFontFamily,
                FontSize = TextFontSize,
            };

            _consoleContainer.Children.Add(textBlock);
        }
    }
}
