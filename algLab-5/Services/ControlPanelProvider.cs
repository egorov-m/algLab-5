using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace algLab_5.Services
{
    /// <summary> Класс поставщик панели управления </summary>
    public class ControlPanelProvider
    {
        /// <summary> Режим демонстрации работы алгоритма </summary>
        private DemoMode _demoMode;

        /// <summary> Задержка в миллисекундах демонстрации работы алгоритма в автоматическом режиме </summary>
        private static int _delay;

        /// <summary> Выполнять ли шаг вперёд </summary>
        private static bool _isStepForward;

        /// <summary> Выполнять ли шаг назад </summary>
        private static bool _isStepBack;

        private enum DemoMode
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
        private readonly Button _btnStepBack;
        /// <summary> Кнопка шаг вперёд демонстрации </summary>
        private readonly Button _btnStepForward;
        /// <summary> Текстовое поля задержки демонстрации </summary>
        private readonly TextBox _textBoxDelay;

        /// <summary> Регулярное выражения для проверки соответствия вводимой задержки </summary>
        private readonly Regex _regexDelay = new (@"[0-9]+");

        public ControlPanelProvider(Button btnDemoMode, Button btnStepBack, Button btnStepForward, TextBox textBoxDelay)
        {
            _btnDemoMode    = btnDemoMode;
            if (_btnDemoMode.Content is StackPanel sp)
            {
                if (sp.Children[0] is TextBlock tb1) _tbBtnDemoModeTitle = tb1;
                if (sp.Children[1] is TextBlock tb2) _tbBtnDemoModeSubtitle = tb2;
            }

            _btnStepBack    = btnStepBack;
            _btnStepForward = btnStepForward;
            _textBoxDelay        = textBoxDelay;

            _btnDemoMode.Click    += BtnChangeDemoModeOnClick;
            _btnStepBack.Click    += BtnExecuteStepBackOnClick;
            _btnStepForward.Click += BtnExecuteStepForwardOnClick;

            _textBoxDelay.PreviewTextInput += TextBoxDelayOnPreviewTextInput;
            _textBoxDelay.KeyDown += TextBoxDelayOnKeyDown;

            SetDemoMode(DemoMode.Automatic);
            SetDelay(500);
        }

        /// <summary> Установка режима демонстрации </summary>
        /// <param name="demoMode"> Режим демонстрации </param>
        private void SetDemoMode(DemoMode demoMode)
        {
            _demoMode = demoMode;

            _tbBtnDemoModeTitle.Text = demoMode.ToString("G");
            if (demoMode == DemoMode.Automatic)
            {
                _tbBtnDemoModeSubtitle.Visibility = Visibility.Visible;
                _tbBtnDemoModeSubtitle.Text = $"{_delay} ms";
                _btnStepBack.IsEnabled    = false;
                _btnStepForward.IsEnabled = false;
            }
            else
            {
                _tbBtnDemoModeSubtitle.Visibility = Visibility.Collapsed;
                _btnStepBack.IsEnabled    = true;
                _btnStepForward.IsEnabled = true;
            }
        }

        /// <summary> Установить задержку демонстрации </summary>
        /// <param name="delay"> Задержка </param>
        private void SetDelay(int delay)
        {
            _delay = delay;
            var str = _delay.ToString(CultureInfo.InvariantCulture);
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
                if (!int.TryParse(_textBoxDelay.Text, out var delay)) throw new ArgumentException("ОШИБКА! Задержка должна быть указана в виде целого неотрицательного числа.");
                
                SetDelay(delay);
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
            if (_demoMode == DemoMode.Automatic) SetDemoMode(DemoMode.Capture);
            else  if (_demoMode == DemoMode.Capture) SetDemoMode(DemoMode.Automatic);
        }

        /// <summary> Обработчик нажатия кнопки выполнения шага назад </summary>
        /// <param name="sender"> Кнопка </param>
        /// <param name="e"> Событие клика </param>
        private void BtnExecuteStepBackOnClick(object sender, RoutedEventArgs e) => _isStepBack = true;

        /// <summary> Обработчик нажатия кнопки выполнения шага вперёд </summary>
        /// <param name="sender"> Кнопка </param>
        /// <param name="e"> Событие клика </param>
        private void BtnExecuteStepForwardOnClick(object sender, RoutedEventArgs e) => _isStepForward = true;
    }
}
