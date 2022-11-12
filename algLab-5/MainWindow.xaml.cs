using System.Windows;
using algLab_5.Models;
using algLab_5.Tools;
using algLab_5.Tools.Base;

namespace algLab_5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly StatusBarUpdater _statusBarUpdater;
        private ToolArgs _toolArgs;
        private Tool? _currentTool;

        private StatusSaved _savingStatus = StatusSaved.Saved;

        public MainWindow()
        {
            InitializeComponent();

            _statusBarUpdater = new StatusBarUpdater(tbIsSavedProject, tbCurrentState, tbCoordinates, tbIsHover);

            _toolArgs = new ToolArgs(this, Canvas, CanvasBorder, _statusBarUpdater, OnChangeStatusSaved);

            _currentTool = new ArrowTool(_toolArgs);
        }

        /// <summary>
        /// Изменения статуса сохранён ли проект
        /// </summary>
        private void OnChangeStatusSaved(StatusSaved status)
        {
            _savingStatus = status;
            _statusBarUpdater.UpdateSaveProjectInfo(_savingStatus);
        }
    }
}
