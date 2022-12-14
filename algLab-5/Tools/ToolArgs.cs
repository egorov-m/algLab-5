using algLab_5.Data;
using algLab_5.Models;
using System;
using System.Windows.Controls;
using algLab_5.Services;
using algLab_5.Services.Logger;

namespace algLab_5.Tools
{
    public class ToolArgs
    {
        public MainWindow MainWindow { get; private set; }
        public Canvas Canvas { get; private set; }
        public Border CanvasBorder { get; private set; }
        public StatusBarUpdater StatusBarUpdater { get; private set; }
        public DataProvider DataProvider { get; private set; }
        public Logger Logger { get; private set; }
        public ControlPanelProvider ControlPanelProvider { get; private set; }
        public Action<StatusSaved> SavedChange { get; private set; }

        public ToolArgs(MainWindow minWindow,
                        Canvas canvas,
                        Border canvasBorder,
                        StatusBarUpdater statusBarUpdater,
                        DataProvider dataProvider,
                        Logger logger,
                        ControlPanelProvider controlPanelProvider,
                        Action<StatusSaved> savedChange)
        {
            MainWindow = minWindow;
            Canvas = canvas;
            CanvasBorder = canvasBorder;
            StatusBarUpdater = statusBarUpdater;
            DataProvider = dataProvider;
            Logger = logger;
            ControlPanelProvider = controlPanelProvider;
            SavedChange = savedChange;
        }
    }
}
