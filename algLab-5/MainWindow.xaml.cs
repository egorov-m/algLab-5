﻿using System.Windows;
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
        private readonly ShapesRepository _shapesRepository;
        private ToolArgs _toolArgs;
        private Tool? _currentTool;

        private StatusSaved _savingStatus = StatusSaved.Saved;

        public MainWindow()
        {
            InitializeComponent();

            _shapesRepository = new ShapesRepository(Canvas);
            _statusBarUpdater = new StatusBarUpdater(tbIsSavedProject, tbCurrentState, tbCoordinates, tbIsHover);


            _toolArgs = new ToolArgs(this, Canvas, CanvasBorder, _statusBarUpdater, _shapesRepository, OnChangeStatusSaved);

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
