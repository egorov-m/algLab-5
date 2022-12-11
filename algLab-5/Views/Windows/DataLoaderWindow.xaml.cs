using algLab_5.Data;
using algLab_5.Models;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace algLab_5.Views.Windows
{
    /// <summary> Логика взаимодействия для DataLoaderWindow.xaml </summary>
    public partial class DataLoaderWindow : Window
    {
        private readonly Canvas _canvas;
        private readonly Action<StatusSaved> _savedChange;
        public DataProvider DataProvider { get; private set; }
        public string? PathProject { get; private set; }
        public string? WorkingDirectory { get; private set; }
        public string NameProject { get; private set; }


        public DataLoaderWindow(Canvas canvas, Action<StatusSaved> savedChange)
        {
            InitializeComponent();
            _canvas = canvas;
            _savedChange = savedChange;
        }

        /// <summary> Обработчик события нажатия кнопки создания нового файла </summary>
        /// <param name="sender"> Кнопка </param>
        /// <param name="e"> Событие клика </param>
        private void BthCreateNewFileOnClick(object sender, RoutedEventArgs e)
        {
            DataProvider = new DataProvider();
            NameProject = "newProject.csv";
            _savedChange(StatusSaved.Unsaved);
            Hide();
        }

        /// <summary> Обработчик события нажатия кнопки открытия существующего файла </summary>
        /// <param name="sender"> Кнопка </param>
        /// <param name="e"> Событие клика </param>
        private void BthOpenFileOnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Csv file (*.csv)|*.csv"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                PathProject = openFileDialog.FileName;
                WorkingDirectory = Path.GetDirectoryName(PathProject);
                NameProject = openFileDialog.SafeFileName;
                DataProvider = new DataProvider(PathProject, WorkingDirectory, _canvas, FileFormatType.Csv,  FormatDataGraph.IncidenceMatrix);
                Hide();
            }

        }

        /// <summary> Обработчик события нажатия кнопки выхода из программы из окна выбора файла </summary>
        /// <param name="sender"> Кнопка </param>
        /// <param name="e"> Событие клика </param>
        private void BthExitOnClick(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary> Обработчик закрытия окна выбора файла через крестик </summary>
        /// <param name="sender"> Кнопка </param>
        /// <param name="e"> Событие клика </param>
        private void DataLoaderWindowClosed(object? sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
