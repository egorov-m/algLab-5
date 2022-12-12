using System;
using System.ComponentModel;
using System.Windows;

namespace algLab_5.Views.Windows
{
    /// <summary> Логика взаимодействия для ExitWindow.xaml </summary>
    public partial class ExitWindow : Window
    {
        private readonly Action _savedChange;

        public ExitWindow(Action savedChange)
        {
            InitializeComponent();
            _savedChange = savedChange;
        }

        /// <summary> Обработчик нажатия кнопки сохранить изменения и выйти </summary>
        /// <param name="sender"> Кнопка </param>
        /// <param name="e"> Событие клика </param>
        private void BthSaveExitOnClick(object sender, RoutedEventArgs e)
        {
            _savedChange();
            Environment.Exit(0);
        }

        /// <summary> Обработчик нажатия кнопки забыть изменения и выйти </summary>
        /// <param name="sender"> Кнопка </param>
        /// <param name="e"> Событие клика </param>
        private void BthForgetExitOnClick(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary> Обработчик нажатия кнопки (крестик) закрытия окна </summary>
        /// <param name="sender"> Кнопка </param>
        /// <param name="e"> Событие клика </param>
        private void ExitWindowOnClosing(object? sender, CancelEventArgs e)
        {
            e.Cancel = false;
        }
    }
}
