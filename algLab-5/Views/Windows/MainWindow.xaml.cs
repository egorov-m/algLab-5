﻿using System.Collections.Generic;
using algLab_5.Data;
using algLab_5.Models;
using algLab_5.Tools;
using algLab_5.Tools.Base;
using System.Windows;
using algLab_5.Services;
using algLab_5.Services.Logger;
using algLab_5.Views.Windows;
using System.Windows.Controls;

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
        private readonly TextBlock _tbSavingIndicator;
        private string? _pathProject;
        private string? _workingDirectory;

        private string _nameProject;
        private TextBlock _tbNameProject;

        public MainWindow()
        {
            InitializeComponent();

            _statusBarUpdater = new StatusBarUpdater(tbIsSavedProject, tbCurrentState, tbCoordinates, tbIsHover);
            //_dataProvider = new DataProvider();
            _consoleProvider = new ConsoleProvider(spConsoleContainer);
            _logger = Logger.GetLogger("loggerGraph", Level.Info, new List<IMessageHandler>() {new ConsoleHandler(_consoleProvider), new FileHandler()});

            _tbNameProject = tbProjectName;
            _tbSavingIndicator = tbIndicatorSaved;

            // Диалоговое окно загрузки данных
            var dataLoaderWindow = new DataLoaderWindow(Canvas, OnChangeStatusSaved);
            dataLoaderWindow.ShowDialog();
            _dataProvider = dataLoaderWindow.DataProvider;
            _pathProject = dataLoaderWindow.PathProject;
            _workingDirectory = dataLoaderWindow.WorkingDirectory;

            _nameProject = dataLoaderWindow.NameProject;
            _tbNameProject.Text = _nameProject;

            _controlPanelProvider = new ControlPanelProvider(btnAlgDemoMode, btnAlgReset, btnAlgStepForward, tbDelayAlgStep);

            _toolArgs = new ToolArgs(this, Canvas, CanvasBorder, _statusBarUpdater, _dataProvider, _logger, _controlPanelProvider, OnChangeStatusSaved);

            _currentTool = new ArrowTool(_toolArgs);

            ConsoleHandler.SetIsWriteTitle();
            _logger.Info("Программа успешно запущена!");
        }

        /// <summary> Изменения статуса сохранён ли проект </summary>
        private void OnChangeStatusSaved(StatusSaved status)
        {
            _savingStatus = status;
            _statusBarUpdater.UpdateSaveProjectInfo(_savingStatus);
            _tbSavingIndicator.Text = status == StatusSaved.Saved ? "" : "*";
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

        private void BtnAlgDfsOnClick(object sender, RoutedEventArgs e)
        {
            _currentTool?.Unload();

            ConsoleHandler.SetIsWriteTitle();
            ConsoleHandler.SetIsEmptyLineBeforeTitle();
            _logger.Info("Выбран инструмент демонстрации работы алгоритма (выбор одной вершины).");
            _currentTool = new DemoAlgorithmsSingleChoiceTool(_toolArgs, StatusTool.AlgDfs);
        }

        private void BtnAlgBfsOnClick(object sender, RoutedEventArgs e)
        {
            _currentTool?.Unload();

            ConsoleHandler.SetIsWriteTitle();
            ConsoleHandler.SetIsEmptyLineBeforeTitle();
            _logger.Info("Выбран инструмент демонстрации работы алгоритма (выбор одной вершины).");
            _currentTool = new DemoAlgorithmsSingleChoiceTool(_toolArgs, StatusTool.AlgBfs);
        }

        private void BtnAlgFindMaxFlowOnClick(object sender, RoutedEventArgs e)
        {
            _currentTool?.Unload();

            ConsoleHandler.SetIsWriteTitle();
            ConsoleHandler.SetIsEmptyLineBeforeTitle();
            _logger.Info("Выбран инструмент демонстрации работы алгоритма (выбор двух вершин).");
            _currentTool = new DemoAlgorithmsDuplexChoiceTool(_toolArgs, StatusTool.AlgFindMaxFlow);
        }

        private void BtnAlgDijkstraOnClick(object sender, RoutedEventArgs e)
        {
            _currentTool?.Unload();

            ConsoleHandler.SetIsWriteTitle();
            ConsoleHandler.SetIsEmptyLineBeforeTitle();
            _logger.Info("Выбран инструмент демонстрации работы алгоритма (выбор двух вершин).");
            _currentTool = new DemoAlgorithmsDuplexChoiceTool(_toolArgs, StatusTool.AlgDijkstra);
        }

        private void BtnAlgKruskalOnClick(object sender, RoutedEventArgs e)
        {
            _currentTool?.Unload();

            ConsoleHandler.SetIsWriteTitle();
            ConsoleHandler.SetIsEmptyLineBeforeTitle();
            _logger.Info("Выбран инструмент демонстрации работы алгоритма (выбор одной вершины).");
            _currentTool = new DemoAlgorithmsSingleChoiceTool(_toolArgs, StatusTool.AlgKruskal);
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