using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace algLab_5.Services
{
    /// <summary> Класс поставщик панели управления </summary>
    public class ControlPanelProvider : IDisposable
    {
        /// <summary> Режим демонстрации работы алгоритма </summary>
        public static DemoMode DemonstrationMode { get; private set; }

        /// <summary> Задержка в миллисекундах демонстрации работы алгоритма в автоматическом режиме </summary>
        public static int Delay { get; private set; }

        /// <summary> Выполнять ли шаг вперёд </summary>
        public static bool IsStepForward { get; set; }

        /// <summary> Выполнять ли шаг назад </summary>
        public static bool IsReset;

        public enum DemoMode
        {
            Automatic,
            Capture
        }

        /// <summary> Кнопка выбора режима демонстрации </summary>
        private readonly Button _btnDemoMode;
        /// <summary> Текстовый блок заголовка кнопки смены режима демонстрации </summary>
        private readonly TextBlock _tbBtnDemoModeTitle;
        /// <summary> Текстовый блок подзаголовка кнопки смены режима демонстрации </summary>
        private readonly TextBlock _tbBtnDemoModeSubtitle;

        /// <summary> Кнопка шаг назад демонстрации </summary>
        private readonly Button _btnReset;
        /// <summary> Кнопка шаг вперёд демонстрации </summary>
        private readonly Button _btnStepForward;
        /// <summary> Текстовое поля задержки демонстрации </summary>
        private readonly TextBox _textBoxDelay;

        /// <summary> Регулярное выражения для проверки соответствия вводимой задержки </summary>
        private readonly Regex _regexDelay = new (@"[0-9]+");

        public ControlPanelProvider(Button btnDemoMode, Button btnReset, Button btnStepForward, TextBox textBoxDelay)
        {
            _btnDemoMode    = btnDemoMode;
            if (_btnDemoMode.Content is StackPanel sp)
            {
                if (sp.Children[0] is TextBlock tb1) _tbBtnDemoModeTitle = tb1;
                if (sp.Children[1] is TextBlock tb2) _tbBtnDemoModeSubtitle = tb2;
            }

            _btnReset    = btnReset;
            _btnStepForward = btnStepForward;
            _textBoxDelay        = textBoxDelay;

            _btnDemoMode.Click    += BtnChangeDemoModeOnClick;
            _btnReset.Click    += BtnExecuteResetOnClick;
            _btnStepForward.Click += BtnExecuteStepForwardOnClick;

            _textBoxDelay.PreviewTextInput += TextBoxDelayOnPreviewTextInput;
            _textBoxDelay.KeyDown += TextBoxDelayOnKeyDown;

            SetDemoMode(DemoMode.Automatic);
            SetDelay(500);
        }

        /// <summary> Продолжить демонстрацию работы алгоритма в соответствии с режимом </summary>
        public static void Continue(Logger.Logger? logger = null)
        {
            if (DemonstrationMode == DemoMode.Automatic)
            {
                logger?.Info($"Автоматически режим: пауза {Delay} ms.");
                Thread.Sleep(Delay);
            }
            else
            {
                if (DemonstrationMode == DemoMode.Capture)
                {
                    logger?.Info("Ручной режим: ожидаем нажатия кнопки \"Forward\".");
                    while (!IsStepForward)
                    {
                        if (DemonstrationMode == DemoMode.Automatic || IsReset) break;
                    }
                    IsStepForward = false;
                }
            }
        }

        /// <summary> Установка режима демонстрации </summary>
        /// <param name="demoMode"> Режим демонстрации </param>
        private void SetDemoMode(DemoMode demoMode)
        {
            DemonstrationMode = demoMode;

            _tbBtnDemoModeTitle.Text = demoMode.ToString("G");
            if (demoMode == DemoMode.Automatic)
            {
                _tbBtnDemoModeSubtitle.Visibility = Visibility.Visible;
                _tbBtnDemoModeSubtitle.Text = $"{Delay} ms";
                //_btnReset.IsEnabled    = false;
                _btnStepForward.IsEnabled = false;
            }
            else
            {
                _tbBtnDemoModeSubtitle.Visibility = Visibility.Collapsed;
                //_btnReset.IsEnabled    = true;
                _btnStepForward.IsEnabled = true;
            }
        }

        /// <summary> Установить задержку демонстрации </summary>
        /// <param name="delay"> Задержка </param>
        private void SetDelay(int delay)
        {
            Delay = delay;
            var str = Delay.ToString(CultureInfo.InvariantCulture);
            _textBoxDelay.Text = str;
            _tbBtnDemoModeSubtitle.Text = $"{str} ms";
        }

        /// <summary> обработка нажатия кнопки Enter и установка введённой задержки </summary>
        /// <param name="sender"> Текстовое поле для ввода задержки </param>
        /// <param name="e"> Событие нажатия клавиши </param>
        private void TextBoxDelayOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Keyboard.ClearFocus();
                if (_textBoxDelay.Text == "")
                {
                    SetDelay(0);
                }
                else
                {
                    if (!int.TryParse(_textBoxDelay.Text, out var delay)) throw new ArgumentException("ОШИБКА! Задержка должна быть указана в виде целого неотрицательного числа.");
                
                    SetDelay(delay);
                }
            }
        }

        /// <summary> Предварительная проверка вводимого текста на соответствие (целое неотрицательное число) </summary>
        /// <param name="sender"> Текстовое поле для ввода задержки </param>
        /// <param name="e"> Событие ввода текста </param>
        private void TextBoxDelayOnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regexDelay.IsMatch(e.Text);
        }

        /// <summary>
        /// Обработчик нажатия кнопки смены режима демонстрации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnChangeDemoModeOnClick(object sender, RoutedEventArgs e)
        {
            if (DemonstrationMode == DemoMode.Automatic) SetDemoMode(DemoMode.Capture);
            else  if (DemonstrationMode == DemoMode.Capture) SetDemoMode(DemoMode.Automatic);
        }

        /// <summary> Обработчик нажатия кнопки выполнения шага назад </summary>
        /// <param name="sender"> Кнопка </param>
        /// <param name="e"> Событие клика </param>
        private void BtnExecuteResetOnClick(object sender, RoutedEventArgs e)
        {
            IsReset = true;
            //_tool.Unload();
        }

        /// <summary> Обработчик нажатия кнопки выполнения шага вперёд </summary>
        /// <param name="sender"> Кнопка </param>
        /// <param name="e"> Событие клика </param>
        private void BtnExecuteStepForwardOnClick(object sender, RoutedEventArgs e) => IsStepForward = true;

        /// <summary> Выполнить сброс панели управления </summary>
        public void Dispose()
        {
            IsStepForward = false;
            IsReset = false;
        }
    }
}
