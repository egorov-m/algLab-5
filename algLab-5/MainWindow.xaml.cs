using System.Collections.Generic;
using algLab_5.Data;
using algLab_5.Models;
using algLab_5.Tools;
using algLab_5.Tools.Base;
using System.Windows;
using algLab_5.Services;
using algLab_5.Services.Logger;

namespace algLab_5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly StatusBarUpdater _statusBarUpdater;
        private readonly DataProvider _dataProvider;
        private readonly ControlPanelProvider _controlPanelProvider;
        private readonly ConsoleProvider _consoleProvider;
        private readonly Logger _logger;
        private readonly ToolArgs _toolArgs;
        private Tool? _currentTool;

        private StatusSaved _savingStatus = StatusSaved.Saved;

        public MainWindow()
        {
            InitializeComponent();

            _statusBarUpdater = new StatusBarUpdater(tbIsSavedProject, tbCurrentState, tbCoordinates, tbIsHover);
            _dataProvider = new DataProvider();
            _consoleProvider = new ConsoleProvider(spConsoleContainer);
            _logger = Logger.GetLogger("loggerGraph", Level.Info, new List<IMessageHandler>() {new ConsoleHandler(_consoleProvider), new FileHandler()});

            _controlPanelProvider = new ControlPanelProvider(btnAlgDemoMode, btnAlgStepBack, btnAlgStepForward, tbDelayAlgStep);

            _toolArgs = new ToolArgs(this, Canvas, CanvasBorder, _statusBarUpdater, _dataProvider, _logger, OnChangeStatusSaved);

            ConsoleHandler.SetIsWriteTitle();
            _logger.Info("Программа успешно запущена!");

            _currentTool = new ArrowTool(_toolArgs);
        }

        /// <summary> Изменения статуса сохранён ли проект </summary>
        private void OnChangeStatusSaved(StatusSaved status)
        {
            _savingStatus = status;
            _statusBarUpdater.UpdateSaveProjectInfo(_savingStatus);
        }

        private void BtnAddVertexOnClick(object sender, RoutedEventArgs e)
        {
            _currentTool?.Unload();

            ConsoleHandler.SetIsWriteTitle();
            ConsoleHandler.SetIsEmptyLineBeforeTitle();
            _logger.Info("Выбран инструмент добавления вершины графа.");
            _currentTool = new AddElementTool(_toolArgs);
        }

        private void BtnAddEdgeOnClick(object sender, RoutedEventArgs e)
        {
            _currentTool?.Unload();

            ConsoleHandler.SetIsWriteTitle();
            ConsoleHandler.SetIsEmptyLineBeforeTitle();
            _logger.Info("Выбран инструмент добавления ребра графа.");
            _currentTool = new AddConnectionTool(_toolArgs, ConnectionType.Default);
        }

        private void BtnRemoveElementOnClick(object sender, RoutedEventArgs e)
        {
            _currentTool?.Unload();

            ConsoleHandler.SetIsWriteTitle();
            ConsoleHandler.SetIsEmptyLineBeforeTitle();
            _logger.Info("Выбран инструмент удаления элемента графа.");
            _currentTool = new RemoveElementTool(_toolArgs);
        }

        private void BtnEditElementOnClick(object sender, RoutedEventArgs e)
        {
            _currentTool?.Unload();

            ConsoleHandler.SetIsWriteTitle();
            ConsoleHandler.SetIsEmptyLineBeforeTitle();
            _logger.Info("Выбран инструмент редактирования элемента графа.");
            _currentTool = new EditDataTool(_toolArgs);
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
