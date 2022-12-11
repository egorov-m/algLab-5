using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using algLab_5.Views.Graph;

namespace algLab_5.Data
{
    public class DataLoader
    {
        private readonly string _path;
        private readonly FileFormatType _fileType;
        private readonly FormatDataGraph _formatDataGraph;

        private const int IndexVertexData = 0;
        private const int IndexEdgeWeight = 0;

        public DataLoader(string path, FileFormatType fileType, FormatDataGraph formatDataGraph)
        {
            _path = path;
            _fileType = fileType;
            _formatDataGraph = formatDataGraph;
        }

        public DataLoader(string path, FileFormatType fileType)
        {
            _path = path;
            _fileType = fileType;
            _formatDataGraph = FormatDataGraph.IncidenceMatrix;
        }

        public DataLoader(string path, FormatDataGraph formatDataGraph)
        {
            _path = path;
            _fileType = FileFormatType.Csv;
            _formatDataGraph = formatDataGraph;
        }

        public DataLoader(string path)
        {
            _path = path;
            _fileType = FileFormatType.Csv;
            _formatDataGraph = FormatDataGraph.IncidenceMatrix;
        }

        public (List<VertexElement> _dataVertexElements, List<EdgeElement> _dataEdgeElements) GetModelElements()
        {
            List<string[]> lines;
            if (_fileType == FileFormatType.Csv) lines = ReadAndParseCsvData(_path);
            else throw new ArgumentException($"ОШИБКА! В текущей версии программы формат {_fileType} не поддерживается. Выберите Сsv файл.");

            if (_formatDataGraph == FormatDataGraph.IncidenceMatrix) return PreparingIncidenceMatrixGraphModels(lines);
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
                verticesData.Add(t[IndexVertexData]);
            }

            if (verticesData.Count != lines.Count) throw new FileFormatException("Имена вершин графа не должны повторяться.");

            for (var i = 1; i < count; i++)
            {
                var countConnectionPoint = 0;
                //var countConnectionPoint = lines.Count(t => t[i] == "1");
                for (var j = 1; j < lines.Count; j++)
                {
                    if (lines[j][i] == "1") countConnectionPoint++;
                }

                if (countConnectionPoint is not (2 or 0)) throw new FileFormatException("Каждое ребро должно быть прикреплено к двум вершинам.");
            }
        }

        private static List<string[]> ReadAndParseCsvData(string path, char separator = ';')
        {
            if (File.Exists(path)) return File.ReadAllLines(path).Select(x => x.Split(separator)).ToList();
            throw new ArgumentException("ОШИБКА! По указанному пути, файл отсутствует.");
        }
        
        /// <summary> Подготовка матрицы инцидентности </summary>
        /// <param name="lines"> Элементы матрицы </param>
        /// <exception cref="FileFormatException"> Будет выброшено в случае не корректности матрицы инцидентности </exception>
        private static (List<VertexElement>, List<EdgeElement>) PreparingIncidenceMatrixGraphModels(List<string[]> lines)
        {
            try
            {
                CheckErrorsIncidenceMatrix(lines);
                var vertexElements = new List<VertexElement>();
                var edgeElements = new EdgeElement[lines[IndexVertexData].Length - 1].ToList();

                for (var i = 1; i < lines.Count; i++)
                {
                    var vertexElement = new VertexElement(lines[i][IndexVertexData]);
                    vertexElements.Add(vertexElement);
                    for (var j = 1; j < lines[i].Length; j++)
                    {
                        if (lines[i][j] == "1")
                        {
                            var edgeElement = edgeElements[j - 1];
                            if (edgeElement != null)
                            {
                                edgeElement.DestinationVertexElement = vertexElement;
                                vertexElement.EdgesList.Add(edgeElement);
                            }
                            else
                            {
                                int.TryParse(lines[IndexEdgeWeight][j], out var weight);
                                edgeElement = new EdgeElement(vertexElement, vertexElement.Id, weight);
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
