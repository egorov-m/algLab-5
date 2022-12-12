using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using algLab_5.Models.Graph;
using algLab_5.Services.Logger;
using algLab_5.Views.Graph;

namespace algLab_5.Data
{
    /// <summary> Класс управления загрузкой данных </summary>
    public class DataLoader
    {
        /// <summary> Путь до загружаемого файла </summary>
        private readonly string _path;
        /// <summary> Формат файла </summary>
        private readonly FileFormatType _fileType;
        /// <summary> Формат представления графа </summary>
        private readonly FormatDataGraph _formatDataGraph;
        /// <summary> Логгер </summary>
        private readonly Logger? _logger;

        /// <summary> Индекс колонки с данными вершины в матрицы инцидентности </summary>
        private const int IndexVertexDataInIncidenceMatrix = 0;
        /// <summary> Индекс строки с весами рёбер в матрицы инцидентности </summary>
        private const int IndexEdgeWeightInIncidenceMatrix = 0;

        public DataLoader(string path, FileFormatType fileType, FormatDataGraph formatDataGraph, Logger? logger = null)
        {
            _path = path;
            _fileType = fileType;
            _formatDataGraph = formatDataGraph;
            _logger = logger;
        }

        public DataLoader(string path, FileFormatType fileType, Logger? logger = null)
        {
            _path = path;
            _fileType = fileType;
            _formatDataGraph = FormatDataGraph.IncidenceMatrix;
            _logger = logger;
        }

        public DataLoader(string path, FormatDataGraph formatDataGraph, Logger? logger = null)
        {
            _path = path;
            _fileType = FileFormatType.Csv;
            _formatDataGraph = formatDataGraph;
            _logger = logger;
        }

        public DataLoader(string path, Logger? logger = null)
        {
            _path = path;
            _fileType = FileFormatType.Csv;
            _formatDataGraph = FormatDataGraph.IncidenceMatrix;
            _logger = logger;
        }

        /// <summary> Получить модель элементов графа </summary>
        public (List<Vertex> _dataVertexElements, List<Edge> _dataEdgeElements) GetModelElements()
        {
            List<string[]> lines;
            if (_fileType == FileFormatType.Csv)
            {
                lines = ReadAndParseCsvData(_path);
                var sb = lines.GetIncidenceMatrixForLog();
                
                _logger?.Info("Матрица инцидентности успешно прочитана.");
                _logger?.Info($"Имеет вид: {sb}");
            }
            else throw new ArgumentException($"ОШИБКА! В текущей версии программы формат {_fileType} не поддерживается. Выберите Сsv файл.");

            if (_formatDataGraph == FormatDataGraph.IncidenceMatrix)
            {
                var t = PreparingIncidenceMatrixGraphModels(lines);
                _logger?.Info("Граф успешно прочитан из матрицы.");
                return t;
            }
            throw new ArgumentException($"ОШИБКА! В текущей версии программы формат матрицы {_formatDataGraph} не поддерживается. Выберите матрицу инцидентности файл.");
        }

        /// <summary> Нахождение ошибок в матрице инцидентности </summary>
        /// <param name="lines"> Список массивов элементов строки матрицы инцидентности </param>
        /// <exception cref="FileFormatException"> Будет выброшено в случае ошибки </exception>
        private static void CheckErrorsIncidenceMatrix(List<string[]> lines)
        {
            var verticesData = new HashSet<string>();
            var count = lines[0].Length;
            foreach (var t in lines)
            {
                if (count != t.Length) throw new FileFormatException("Строки матрицы должны быть одинаковой длинны.");
                verticesData.Add(t[IndexVertexDataInIncidenceMatrix]);
            }

            if (verticesData.Count != lines.Count) throw new FileFormatException("Имена вершин графа не должны повторяться.");

            for (var i = 1; i < count; i++)
            {
                var countConnectionPoint = 0;
                for (var j = 1; j < lines.Count; j++)
                {
                    if (lines[j][i] == "1") countConnectionPoint++;
                }

                if (countConnectionPoint is not (2 or 0)) throw new FileFormatException("Каждое ребро должно быть прикреплено к двум вершинам.");
            }

            for (var i = 1; i < count; i++)
            {
                for (var j = i + 1; j < count; j++)
                {
                    var isEdgesMatch = true;
                    for (var k = 1; k < lines.Count; k++)
                    {
                        if (lines[k][i] != lines[k][j])
                        {
                            isEdgesMatch = false;
                        }
                    }

                    if (isEdgesMatch) throw new FileFormatException("Рёбра между двумя вершинами не должны повторяться.");
                }
            }
        }

        /// <summary> Считывать и парсить Csv данные </summary>
        /// <param name="path"> Путь до файла </param>
        /// <param name="separator"> Csv разделитель </param>
        private static List<string[]> ReadAndParseCsvData(string path, char separator = ';')
        {
            if (File.Exists(path)) return File.ReadAllLines(path).Select(x => x.Split(separator)).ToList();
            throw new ArgumentException("ОШИБКА! По указанному пути, файл отсутствует.");
        }
        
        /// <summary> Подготовка матрицы инцидентности </summary>
        /// <param name="lines"> Элементы матрицы </param>
        /// <exception cref="FileFormatException"> Будет выброшено в случае не корректности матрицы инцидентности </exception>
        private static (List<Vertex>, List<Edge>) PreparingIncidenceMatrixGraphModels(List<string[]> lines)
        {
            try
            {
                CheckErrorsIncidenceMatrix(lines);
                var vertexElements = new List<Vertex>();
                var edgeElements = new Edge[lines[IndexVertexDataInIncidenceMatrix].Length - 1].ToList();

                for (var i = 1; i < lines.Count; i++)
                {
                    var vertexElement = new VertexElement(lines[i][IndexVertexDataInIncidenceMatrix]);
                    vertexElements.Add(vertexElement);
                    for (var j = 1; j < lines[i].Length; j++)
                    {
                        if (lines[i][j] == "1")
                        {
                            var edgeElement = edgeElements[j - 1];
                            if (edgeElement != null)
                            {
                                edgeElement.DestinationVertex = vertexElement;
                                vertexElement.EdgesList.Add(edgeElement);
                            }
                            else
                            {
                                int.TryParse(lines[IndexEdgeWeightInIncidenceMatrix][j], out var weight);
                                edgeElement = new EdgeElement(vertexElement, vertexElement, weight);
                                vertexElement.EdgesList.Add(edgeElement);
                                edgeElements[j - 1] = edgeElement;
                            }
                        }

                    }
                }

                return (vertexElements, edgeElements);
            }
            catch (Exception e)
            {
                throw new FileFormatException($"ОШИБКА! Данные не корректны. Граф должен быть представлен в виде матрицы инцидентности.\n(Расшифровка: {e.Message}).");
            }
        }
    }
}
