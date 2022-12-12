using System;
using algLab_5.Views.Graph;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using algLab_5.Models.Graph;
using algLab_5.Services.Logger;
using algLab_5.Views.Utils;

namespace algLab_5.Data
{
    /// <summary> Класс поставляющий данные </summary>
    public class DataProvider
    {
        /// <summary> Вершины графа </summary>
        private readonly List<Vertex> _dataVertexElements;
        /// <summary> Рёбра графа </summary>
        private readonly List<Edge> _dataEdgeElements;

        /// <summary> Размер окна просмотра по горизонтали </summary>
        private const int ViewportSizeX = 850;
        /// <summary> Размер окна просмотра по вертикали </summary>
        private const int ViewportSizeY = 440;

        /// <summary> Инициализация DataProvider без загрузки данных </summary>
        public DataProvider()
        {
            _dataVertexElements = new List<Vertex>();
            _dataEdgeElements = new List<Edge>();
        }

        /// <summary> Получить вершины графа от поставщика </summary>
        public List<Vertex> GetVertexElementsData() => _dataVertexElements;

        /// <summary> Получить рёбра графа от поставщика </summary>
        public List<Edge> GetEdgeElementsData() => _dataEdgeElements;

        /// <summary> Инициализация DataProvider с загрузкой существующих данных </summary>
        /// <param name="path"> Путь к файлу </param>
        /// <param name="workingDirectory"> Рабочая директория </param>
        /// <param name="fileFormatType"> Тип файла </param>
        /// <param name="formatDataGraph"> Тип файла </param>
        /// <param name="canvas"> Холст </param>
        /// <param name="logger"> Логгер </param>
        public DataProvider(string path, string? workingDirectory, Canvas canvas, FileFormatType fileFormatType, FormatDataGraph formatDataGraph, Logger? logger = null)
        {
            var dataLoader = new DataLoader(path, fileFormatType, formatDataGraph, logger);
            (_dataVertexElements, _dataEdgeElements) = dataLoader.GetModelElements();
            SetOnCanvas(canvas);
            logger?.Info("Граф успешно установлен на холст.");
        }

        /// <summary> Добавить элемент вершины графа </summary>
        /// <param name="vertexElement"> Элемент вершины </param>
        public void AddVertexElement(Vertex vertexElement)
        {
            _dataVertexElements.Add(vertexElement);
        }

        /// <summary> Добавить элемент ребра графа </summary>
        /// <param name="edgeElement"> Ребро графа </param>
        public bool AddEdgeElement(Edge? edgeElement)
        {
            var isAdd = !_dataEdgeElements.Any(item =>
                (item.InitialVertex == edgeElement?.InitialVertex &&
                 item.DestinationVertex == edgeElement.DestinationVertex) ||

                (item.DestinationVertex == edgeElement?.InitialVertex
                 && item.InitialVertex == edgeElement?.DestinationVertex));
            if (!isAdd) return isAdd;

            var unnecessaryEdgeElement = _dataEdgeElements.FirstOrDefault(item =>
                item.InitialVertex == edgeElement?.DestinationVertex &&
                item.DestinationVertex == null);
            if (unnecessaryEdgeElement != null) _dataEdgeElements.Remove(unnecessaryEdgeElement);
            if (edgeElement != null) _dataEdgeElements.Add(edgeElement);

            return isAdd;
        }

        /// <summary> Установить граф на холст </summary>
        /// <param name="canvas"> Холст </param>
        private void SetOnCanvas(Canvas canvas)
        {
            var alphaStep = 45.0;
            var alpha = 0.0;
            var diameterFactor = 1;
            var capacityElementsInCircle = 8;
            var countElementsInCircle = 0;
            var centerPosition = new Point(ViewportSizeX / 2, ViewportSizeY / 2);

            if (_dataVertexElements.Count > 0) _dataVertexElements[0].Draw(canvas, centerPosition);

            for (var i = 1; i < _dataVertexElements.Count; i++)
            {
                _dataVertexElements[i].Draw(canvas, 
                                      GetPositionOfVertexOnCircle(centerPosition, 
                                                                       ref alphaStep, 
                                                                       ref alpha, 
                                                                       ref diameterFactor, 
                                                                       ref countElementsInCircle, 
                                                                       ref capacityElementsInCircle));
            }

            _dataEdgeElements.ForEach(x => x.Draw(canvas));
        }

        /// <summary> Получить позицию следующей вершины графа [круговое расположение] </summary>
        /// <param name="centerPosition"> Позиция первой центральной вершины </param>
        /// <param name="alphaStep"> Текущий шаг обновления угла для расчёта радиус вектора </param>
        /// <param name="alpha"> Текущий угла для расчёта радиус вектора </param>
        /// <param name="diameterFactor"> Коэффициент для умножения диаметра вершины (расчёт отдалённости от центра) </param>
        /// <param name="countElementsInCircle"> Счётчик элементов </param>
        /// <param name="capacityElementsInCircle"> Вместимость элементов в кругах вокруг центральной вершины </param>
        private static Point GetPositionOfVertexOnCircle(Point centerPosition, 
                                                         ref double alphaStep, 
                                                         ref double alpha, 
                                                         ref int diameterFactor, 
                                                         ref int countElementsInCircle, 
                                                         ref int capacityElementsInCircle)
        {
            if (countElementsInCircle == capacityElementsInCircle)
            {
                alphaStep /= 2;
                diameterFactor++;
                capacityElementsInCircle *= 2;
            }

            var diffX = diameterFactor * Math.Sqrt(2) * Params.SizeVertexElement * 2 * Math.Cos(alpha * Math.PI / 180 % 360);
            var diffY = diameterFactor * Math.Sqrt(2) * Params.SizeVertexElement * 2 * Math.Sin(alpha * Math.PI / 180 % 360);

            alpha += alphaStep;
            countElementsInCircle++;

            return new Point(centerPosition.X + diffX, centerPosition.Y + diffY);
        }

        /// <summary> Получить случайную позицию для следующей вершины графа </summary>
        /// <param name="vertex"> Вершина графа </param>
        /// <param name="canvas"> Холст </param>
        /// <param name="points"> Найденные точки </param>
        [Obsolete("Метод для нахождения позиций вершин графа случайным способом не рекомендуется к использованию.")]
        private static Point GetRandomPositionOfVertex(Vertex vertex, Canvas canvas, List<Point> points)
        {
            var position = new Point(0, 0);
            var diameter = Params.SizeVertexElement;
            var random = new Random();

            if (vertex.EdgesList.Count > 1)
            {
                var multiplier = 1d / vertex.EdgesList.Count;
                position.X *= multiplier;
                position.Y *= multiplier;

                var offsetValue = random.NextDouble() * diameter + (random.Next(0, 1) > 0 ? -1 : 1) * 2 * diameter;
                position.Offset(offsetValue, offsetValue);
            }
            else
            {
                position.X = random.NextDouble() * ViewportSizeX;
                position.Y = random.NextDouble() * ViewportSizeY;
            }

            if (IsVerticesHaveSamePosition(position, diameter, points))
            {
                var offsetValue = random.NextDouble() * diameter + (random.Next(0, 1) > 0 ? -1 : 1) * 4 * diameter;
                position.Offset(offsetValue, offsetValue);
            }

            position.X = Math.Min(Math.Max(position.X, diameter), ViewportSizeX);
            position.Y = Math.Min(Math.Max(position.Y, diameter), ViewportSizeY);

            points.Add(position);
            return position;
        }

        /// <summary> Расположены ли вершины близко друг к другу </summary>
        /// <param name="position"> Позиция для проверки </param>
        /// <param name="diameter"> Диаметр вершин </param>
        /// <param name="points"> Найденные позиции </param>
        private static bool IsVerticesHaveSamePosition(Point position, float diameter,  IEnumerable<Point> points)
        {
            return points.Any(t => PointsDistance(t, position) < diameter * 2);
        }

        /// <summary> Найти дистанцию между двумя точками </summary>
        /// <param name="point1"> Первая точка </param>
        /// <param name="point2"> Вторая точка </param>
        private static double PointsDistance(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.X - point1.X, 2));
        }
    }
}
