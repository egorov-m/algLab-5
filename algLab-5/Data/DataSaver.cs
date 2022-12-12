using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using algLab_5.Models.Graph;
using algLab_5.Services.Logger;
using algLab_5.Views.Graph;
using Microsoft.Win32;

namespace algLab_5.Data
{
    /// <summary> Класс управления сохранением </summary>
    public class DataSaver
    {
        /// <summary> Путь до загружаемого файла </summary>
        private string _path;
        /// <summary> Имя проекта (файла) </summary>
        private string _projectName;
        /// <summary> Формат файла </summary>
        private readonly FileFormatType _fileFormatType;
        /// <summary> Формат представления графа </summary>
        private readonly FormatDataGraph _formatDataGraph;
        /// <summary> Логгер </summary>
        private readonly Logger? _logger;

        /// <summary> Индекс колонки с данными вершины в матрицы инцидентности </summary>
        private const int IndexVertexDataInIncidenceMatrix = 0;
        /// <summary> Индекс строки с весами рёбер в матрицы инцидентности </summary>
        private const int IndexEdgeWeightInIncidenceMatrix = 0;

        public DataSaver(string path, string projectName, FileFormatType fileFormatType, FormatDataGraph formatDataGraph, Logger? logger)
        {
            _path = path;
            _projectName = projectName;
            _fileFormatType = fileFormatType;
            _formatDataGraph = formatDataGraph;
            _logger = logger;
        }

        public DataSaver(string path, string projectName, FormatDataGraph formatDataGraph, Logger? logger)
        {
            _path = path;
            _projectName = projectName;
            _fileFormatType = FileFormatType.Csv;
            _formatDataGraph = formatDataGraph;
            _logger = logger;
        }

        public DataSaver(string path, string projectName, FileFormatType fileFormatType, Logger? logger)
        {
            _path = path;
            _projectName = projectName;
            _fileFormatType = fileFormatType;
            _formatDataGraph = FormatDataGraph.IncidenceMatrix;
            _logger = logger;
        }

        public DataSaver(string path, string projectName, Logger? logger)
        {
            _path = path;
            _projectName = projectName;
            _fileFormatType = FileFormatType.Csv;
            _formatDataGraph = FormatDataGraph.IncidenceMatrix;
            _logger = logger;
        }

        /// <summary> Выполнить сохранение данных </summary>
        /// <param name="vertexList"> Список вершин </param>
        /// <param name="edgeList"> Список рёбер </param>
        /// <param name="dataSaver"> Устройство сохранения данных </param>
        /// <param name="isSaveAs"> Было ли выбрано: "Сохранить как" </param>
        public static (string, string) SaveData(IReadOnlyList<Vertex> vertexList, IReadOnlyList<Edge> edgeList, DataSaver dataSaver, bool isSaveAs)
        {
            if (!isSaveAs)
            {
                dataSaver.SavingData(vertexList, edgeList);
                return (dataSaver._path, dataSaver._projectName);
            }
            else
            {
                if (dataSaver._fileFormatType == FileFormatType.Csv)
                {
                    var saveFileDialog = new SaveFileDialog()
                    {
                        Filter = "Csv file (*.csv)|*.csv",
                        FileName = dataSaver._projectName
                    };
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        dataSaver._path = saveFileDialog.FileName;
                        dataSaver._projectName = saveFileDialog.SafeFileName;
                        dataSaver.SavingData(vertexList, edgeList);
                    }

                    return (dataSaver._path, dataSaver._projectName);
                }
                else
                {
                    throw new ArgumentException("ОШИБКА! В текущей версии программы доступен только Csv.");
                }
            }
        }

        /// <summary> Сохранить данные графа </summary>
        /// <param name="vertexList"> Список вершин </param>
        /// <param name="edgeList"> Список рёбер </param>
        private void SavingData(IReadOnlyList<Vertex> vertexList, IReadOnlyList<Edge> edgeList)
        {
            string[,] matrix;
            StringBuilder sb;

            if (_formatDataGraph == FormatDataGraph.IncidenceMatrix)
            {
                matrix = ProcessingGraphIntoIncidenceMatrix(vertexList, edgeList);
                sb = matrix.GetIncidenceMatrixForLog();

                _logger?.Info("Матрица инцидентности успешно сформирована.");
                _logger?.Info($"Имеет вид: {sb}");
            }
            else
            {
                throw new ArgumentException("ОШИБКА! В текущей версии программы работа с графом доступна только в формате матрицы инцидентности.");
            }

            if (_fileFormatType == FileFormatType.Csv)
            {
                WriteMatrixInCsvFile(_path, matrix);
                _logger?.Info("Данные успешно сохранены.");
            }
            else
            {
                throw new ArgumentException("ОШИБКА! В текущей версии программы доступен только Csv.");
            }
        }

        /// <summary> Записать матрицу в Csv файл </summary>
        /// <param name="path"> Путь до файла </param>
        /// <param name="matrix"> Матрица для записи </param>
        private static void WriteMatrixInCsvFile(string path, string[,] matrix)
        {
            using var sw = new StreamWriter(path, false);
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                var sb = new StringBuilder();

                for (var j = 0; j < matrix.GetLength(1) - 1; j++) sb.Append($"{matrix[i, j]};");
                sb.Append(matrix[i, matrix.GetLength(1) - 1]);

                sw.WriteLine(sb);
            }
        }

        /// <summary> Преобразование графа в матрицу инцидентности </summary>
        /// <param name="vertexList"> Список вершин </param>
        /// <param name="edgeList"> Список рёбер </param>
        private static string[,] ProcessingGraphIntoIncidenceMatrix(IReadOnlyList<Vertex> vertexList, IReadOnlyList<Edge> edgeList)
        {
            var matrix = new string[vertexList.Count + 1, edgeList.Count + 1];
            matrix[0, 0] = "";

            for (var i = 1; i < edgeList.Count + 1; i++)
            {
                for (var j = 1; j < vertexList.Count + 1; j++)
                {
                    matrix[j, IndexVertexDataInIncidenceMatrix] = vertexList[j - 1].Data;
                    matrix[IndexEdgeWeightInIncidenceMatrix, i] = edgeList[i - 1].Weight.ToString();
                    if (edgeList[i - 1].InitialVertex == vertexList[j - 1] || edgeList[i - 1].DestinationVertex == vertexList[j - 1])
                    {
                        matrix[j, i] = "1";
                    }
                    else
                    {
                        matrix[j, i] = "0";
                    }
                }
            }

            return matrix;
        }
    }
}
