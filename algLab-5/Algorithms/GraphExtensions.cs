using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using algLab_5.Data;
using algLab_5.Models.Graph;
using algLab_5.Services;
using algLab_5.Services.Logger;
using algLab_5.Views.Graph;

namespace algLab_5.Algorithms
{
    public static class GraphExtensions
    {
        /// <summary> Выполнить обход графа в глубину </summary>
        /// <param name="startVertex"> Стартовая вершина </param>
        /// <param name="logger"> Логгер </param>
        public static async IAsyncEnumerable<VertexElement>? ExecuteDfs(this VertexElement startVertex, Logger? logger = null)
        {
            ConsoleHandler.SetIsWriteTitle();
            logger?.Info("Начинается демонстрация работы алгоритма поиск в глубину (DFS). [реализация на стеке]");
            logger?.Info("Инициализируем стек для добавления вершин графа.");
            var stack = new Stack<VertexElement>();

            logger?.Info($"Добавляем стартовую вершину \"{startVertex.Data}\" в стек.");
            stack.Push(startVertex);

            var isFinal = true;

            logger?.Info("Пока созданный стек не пуст будем выполнять итерации цикла.");
            while (stack.Count > 0)
            {
                var currentVertex = stack.Pop();
                if (currentVertex.IsVisited) continue;
                logger?.Info($"Посещаем элемент \"{currentVertex.Data}\".");
                currentVertex.SetVisited();

                await Task.Run(() => ControlPanelProvider.Continue(logger));
                if (ControlPanelProvider.IsReset) // Была нажата кнопка сброса демонстрации алгоритма
                {
                    isFinal = false;
                    break; 
                }

                yield return currentVertex;

                logger?.Info($"Выполняем проход по всем инцидентным вершинам текущей вершины \"{currentVertex.Data}\".");
                foreach (var edge in currentVertex.EdgesList)
                {
                    if (!edge.IsVisited)
                    {
                        logger?.Info($"Выполняем проход по ребру \"{edge.Weight}\".");
                        edge.SetVisited();
                        if (edge is EdgeElement edgeElement)
                        {
                            var el = edgeElement.InitialVertexId == currentVertex.Id ? edgeElement.DestinationVertexElement : edgeElement.InitialVertexElement;
                            if (el != null)
                            {
                                logger?.Info($"Добавляем вершину \"{currentVertex.Data}\" в стек.");
                                stack.Push(el);
                            }
                        }
                    }
                }
            }

            if (isFinal) logger?.Info("Обход графа завершён.");
        }

        /// <summary> Выполнить обход графа в ширину </summary>
        /// <param name="startVertex"> Стартовая вершина </param>
        /// <param name="logger"> Логгер </param>
        public static async IAsyncEnumerable<VertexElement> ExecuteBfs(this VertexElement startVertex, Logger? logger = null)
        {
            ConsoleHandler.SetIsWriteTitle();
            logger?.Info("Начинается демонстрация работы алгоритма поиск в ширину (BFS). [реализация на очереди]");
            logger?.Info("Инициализируем очереди для добавления вершин графа.");
            var queue = new Queue<VertexElement>();

            logger?.Info($"Добавляем стартовую вершину \"{startVertex.Data}\" в очередь.");
            queue.Enqueue(startVertex);

            var isFinal = true;

            logger?.Info("Пока созданная очередь не пуста будем выполнять итерации цикла.");
            while (queue.Count > 0)
            {
                var currentVertex = queue.Dequeue();
                if (currentVertex.IsVisited) continue;
                logger?.Info($"Посещаем элемент \"{currentVertex.Data}\".");
                currentVertex.SetVisited();

                await Task.Run(() => ControlPanelProvider.Continue(logger));
                if (ControlPanelProvider.IsReset) // Была нажата кнопка сброса демонстрации алгоритма
                {
                    isFinal = false;
                    break; 
                }

                yield return currentVertex;

                logger?.Info($"Выполняем проход по всем инцидентным вершинам текущей вершины \"{currentVertex.Data}\".");
                foreach (var edge in currentVertex.EdgesList)
                {
                    if (!edge.IsVisited)
                    {
                        logger?.Info($"Выполняем проход по ребру \"{edge.Weight}\".");
                        edge.SetVisited();
                        if (edge is EdgeElement edgeElement)
                        {
                            var el = edgeElement.InitialVertexId == currentVertex.Id ? edgeElement.DestinationVertexElement : edgeElement.InitialVertexElement;
                            if (el != null)
                            {
                                logger?.Info($"Добавляем вершину \"{currentVertex.Data}\" в очередь.");
                                queue.Enqueue(el);
                            }
                        }
                    }
                }
            }

            if (isFinal) logger?.Info("Обход графа завершён.");
        }

        /// <summary> Класс данных для алгоритма Дейкстры </summary>
        private class DijkstraData
        {
            /// <summary> Стоимость </summary>
            public int Price { get; set; }
            /// <summary> Предыдущая вершина </summary>
            public VertexElement? Previous { get; set; }
        }

        /// <summary> Выполнить алгоритм Дейкстры </summary>
        /// <param name="graph"> Граф (список всех вершин) </param>
        /// <param name="start"> Стартовая вершина </param>
        /// <param name="end"> Конечна вершина </param>
        /// <param name="logger"> Логгер </param>
        public static async Task<List<VertexElement>?> ExecuteDijkstra(this List<VertexElement> graph, VertexElement start, VertexElement end, Logger? logger = null)
        {
            var isFinal = true;

            graph.ForEach(x => x.TextBox.Text = "");

            ConsoleHandler.SetIsWriteTitle();
            logger?.Info("Начинается демонстрация работы алгоритма поиск кратчайшего пути между двумя вершинами. [Алгоритм Дейкстры]");
            logger?.Info("!!! По мере определения цены для каждой вершины, её значение будет отображаться вместо имени вершины.");

            logger?.Info("Создаём список всех не посещённых вершин графа.");
            var noVisited = graph.ToList();

            await Task.Run(() => ControlPanelProvider.Continue(logger));
            if (ControlPanelProvider.IsReset) return null; // Была нажата кнопка сброса демонстрации алгоритма

            logger?.Info("Заводим трекинг — в нём будем хранить информацию об оценках каждой вершины.");
            var track = new Dictionary<VertexElement, DijkstraData>();

            logger?.Info("Складываем начальную вершину в трекинг. Начальная цена равна нулю.");
            track[start] = new DijkstraData {Previous = null, Price = 0 };
            start.SetVisited();
            start.TextBox.Text = "0";

            await Task.Run(() => ControlPanelProvider.Continue(logger));
            if (ControlPanelProvider.IsReset) return null; // Была нажата кнопка сброса демонстрации алгоритма

            logger?.Info("Запускаем вечный цикл.");
            while (true)
            {
                VertexElement? toOpen = null;
                var bestPrice = int.MaxValue;

                logger?.Info("Определяем какую вершину будем раскрывать. Выполняем проход по списку не посещённых вершин.");
                foreach (var vertexElement in noVisited)
                {
                    if (track.ContainsKey(vertexElement) && track[vertexElement].Price < bestPrice)
                    {
                        toOpen = vertexElement;
                        bestPrice = track[vertexElement].Price;
                    }
                }

                logger?.Info($"Принцип Жадного алгоритма. Нашли вершину с самой низкой ценой: {bestPrice} из не посещённых.");
                toOpen?.SetVisited();
                if (toOpen != null) toOpen.TextBox.Text = bestPrice.ToString();

                await Task.Run(() => ControlPanelProvider.Continue(logger));
                if (ControlPanelProvider.IsReset)
                {
                    isFinal = false;
                    break; // Была нажата кнопка сброса демонстрации алгоритма
                }

                if (toOpen == null)
                {
                    logger?.Info("Выбранная для раскрытия вершина оказалась равна NULL.");
                    return null;
                }

                if (toOpen == end)
                {
                    logger?.Info("Выбранная для раскрытия вершина является конечной. Выполняем выход из вечного цикла.");
                    break;
                }

                logger?.Info("Выполняем проход по всем инцидентным вершинам раскрываемой вершины.");
                foreach (var edge in toOpen.EdgesList)
                {
                    if (edge is EdgeElement edgeElement)
                    {
                        logger?.Info($"Выполняем проход по ребру \"{edge.Weight}\".");
                        edgeElement.SetVisited();

                        await Task.Run(() => ControlPanelProvider.Continue(logger));
                        if (ControlPanelProvider.IsReset)
                        {
                            isFinal = false;
                            return null; // Была нажата кнопка сброса демонстрации алгоритма
                        }

                        var currentPrice = track[toOpen].Price + edge.Weight;
                        logger?.Info($"Вычисляем текущую цену (цена раскрываемой вершины + вес ребра) = {track[toOpen].Price} + {edge.Weight} = {currentPrice}.");

                        var nextVertex = edgeElement.InitialVertexElement == toOpen ? edgeElement.DestinationVertexElement : edgeElement.InitialVertexElement;
                        logger?.Info($"Следующая вершина: {nextVertex?.Data}.");

                        if (nextVertex != null)
                        {
                            if (!track.ContainsKey(nextVertex) || track[nextVertex].Price > currentPrice)
                            {
                                track[nextVertex] = new DijkstraData {Price = currentPrice, Previous = toOpen};
                                logger?.Info($"Добавляем вершину \"{nextVertex.Data}\" в трекинг.");
                                nextVertex.SetVisited();
                                nextVertex.TextBox.Text = currentPrice.ToString();
                            }
                        }

                        await Task.Run(() => ControlPanelProvider.Continue(logger));
                        if (ControlPanelProvider.IsReset)
                        {
                            isFinal = false;
                            return null; // Была нажата кнопка сброса демонстрации алгоритма
                        }
                    }
                }

                logger?.Info($"Устанавливаем вершину \"{toOpen.Data}\" как посещённую.");
                noVisited.Remove(toOpen);
            }

            var result = new List<VertexElement>();
            if (isFinal)
            {
                logger?.Info("Собираем список с результатом.");
                var tmp = end;
                while (tmp != null)
                {
                    result.Add(tmp);
                    tmp = track[tmp].Previous;
                }

                result.Reverse();

                logger?.Info("Список с результатом готов.");
            }

            logger?.Info("Продолжить, чтобы убрать цены вершин и вернуть их имена.");

            await Task.Run(() => ControlPanelProvider.Continue(logger));
            if (ControlPanelProvider.IsReset) return null; // Была нажата кнопка сброса демонстрации алгоритма

            graph.ForEach(x => x.SetData());

            return result;
        }

        private static bool Bfs(this Dictionary<Edge, int> rGraph, VertexElement source, VertexElement sink, List<Edge> parent, Logger? logger = null)
        {
            var queue = new Queue<VertexElement>();
            queue.Enqueue(source);
            source.SetVisited();
            //parent.Remove(source); //parent[s] = -1;

            while (queue.Count > 0)
            {
                var currentVertex = queue.Dequeue();

                //for (var i = 0; i < currentVertex.EdgesList.Count; i++)
                //{
                //    if (!currentVertex.EdgesList[i].IsVisited && )
                //}

                foreach (var edge in currentVertex.EdgesList)
                {
                    if (!edge.IsVisited && rGraph[edge] > 0)
                    {
                        if (currentVertex == sink)
                        {
                            parent.Add(edge);
                            return true;
                        }
                        if (edge is EdgeElement edgeElement)
                        {
                            var el = edgeElement.InitialVertexId == currentVertex.Id ? edgeElement.DestinationVertexElement : edgeElement.InitialVertexElement;
                            if (el != null)
                            {
                                queue.Enqueue(el);
                                parent.Add(edge);
                                edge.SetVisited();
                            }
                        }
                    }
                }
            }

            return false;
        }

        public static int ExecuteFindMaxFlow(this DataProvider graph, VertexElement source, VertexElement sink, Logger? logger = null)
        {
            var rGraph = new Dictionary<Edge, int>();
            foreach (var edgeElement in graph.GetEdgeElementsData()) rGraph.Add(edgeElement, edgeElement.Weight);

            var parent = new List<Edge>();
            var maxFlow = 0;

            while (rGraph.Bfs(source, sink, parent))
            {
                var pathFlow = int.MaxValue;
                foreach (var edge in parent) pathFlow = Math.Min(pathFlow, rGraph[edge]);

                foreach (var edge in parent)
                {
                    rGraph[edge] -= pathFlow;
                    // rGraph[edge] += pathFlow;
                }

                maxFlow += pathFlow;
            }

            return maxFlow;
        }
    }
}
