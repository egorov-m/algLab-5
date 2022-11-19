using System.Windows;

namespace algLab_5
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Глобальная обработка исключений.
        /// WPF позволяет глобально обработать исключения, используя событие DispatcherUnhandledException в классе Application
        /// </summary>
        /// <param name="sender"> Объект события </param>
        /// <param name="e"> Само событие </param>
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}
