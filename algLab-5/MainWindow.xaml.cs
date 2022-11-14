using System.Windows;
using System.Windows.Controls;
using algLab_5.Data;
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
        private readonly DataProvider _dataProvider;
        private ToolArgs _toolArgs;
        private Tool? _currentTool;

        private StatusSaved _savingStatus = StatusSaved.Saved;

        public MainWindow()
        {
            InitializeComponent();

            _statusBarUpdater = new StatusBarUpdater(tbIsSavedProject, tbCurrentState, tbCoordinates, tbIsHover);
            _dataProvider = new DataProvider();

            _toolArgs = new ToolArgs(this, Canvas, CanvasBorder, _statusBarUpdater, _dataProvider, OnChangeStatusSaved);

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

        private void BtnAddVertexOnClick(object sender, RoutedEventArgs e)
        {
            _currentTool?.Unload();
            _currentTool = new AddElementTool(_toolArgs);
        }

        private void BtnAddEdgeOnClick(object sender, RoutedEventArgs e)
        {
            if (_currentTool != null) _currentTool.Unload(); 
            _currentTool= new AddConnectionTool(_toolArgs, ConnectionType.Default);
        }

        /// <summary> Сбрасываем инструмент </summary>
        public void DisableTool()
        {
            if (_currentTool != null) 
            {
                _currentTool.Unload();
                _currentTool = new ArrowTool(_toolArgs);
            }
        }
    }
}
