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

        /// <summary> Получить ребро между двумя переданными вершинами </summary>
        /// <param name="x"> Первая вершина </param>
        /// <param name="y"> Вторая вершина </param>
        public static EdgeElement? GetEdgeBetweenVertices(VertexElement x, VertexElement y)
        {
            foreach (var edgeX in x.EdgesList)
            {
                foreach (var edgeY in y.EdgesList)
                {
                    if (edgeX == edgeY) return edgeX as EdgeElement;
                }
            }

            return null;
        }

        /// <summary> Алгоритм обхода графа в ширину для алгоритма Форда — Фалкерсона </summary>
        /// <param name="graph"> Граф </param>
        /// <param name="rGraph"> Граф остаточного потока </param>
        /// <param name="source"> Исток потока </param>
        /// <param name="sink"> Сток потока </param>
        /// <param name="path"> Заполняемый путь к стоку </param>
        /// <param name="logger"> Логгер </param>
        private static async Task<bool> Bfs(this DataProvider graph, Dictionary<EdgeElement, int> rGraph, VertexElement source, VertexElement sink, Dictionary<VertexElement, VertexElement?> path, Logger? logger = null)
        {
            logger?.Info("Начинаем выполнять обход граф в ширину.");
            logger?.Info("Инициализируем очереди для добавления вершин графа.");
            var queue = new Queue<VertexElement>();

            logger?.Info($"Добавляем исток \"{source.Data}\" в очередь.");
            queue.Enqueue(source);
            source.SetVisited();
            path[source] = null;

            var isFinal = true;

            logger?.Info("Пока созданная очередь не пуста будем выполнять итерации цикла.");
            while (queue.Count > 0)
            {
                var currentVertex = queue.Dequeue();
                logger?.Info($"Посещаем элемент \"{currentVertex.Data}\".");

                await Task.Run(() => ControlPanelProvider.Continue(logger));
                if (ControlPanelProvider.IsReset) // Была нажата кнопка сброса демонстрации алгоритма
                {
                    isFinal = false;
                    break; 
                }

                logger?.Info($"Выполняем проход по всем инцидентным вершинам текущей вершины \"{currentVertex.Data}\" с условием, что поток не полностью заполнен.");
                foreach (var edge in currentVertex.EdgesList)
                {
                    if (edge is EdgeElement edgeElement)
                    {
                        var v = currentVertex == edgeElement.InitialVertexElement ? edgeElement.DestinationVertexElement : edgeElement.InitialVertexElement;

                        if (v != null)
                        {
                            if (!v.IsVisited && rGraph[edgeElement] > 0)
                            {
                                logger?.Info($"Выполняем проход по ребру \"{edge.Weight}\".");

                                if (v == sink)
                                {
                                    edgeElement.SetVisited();
                                    v.SetVisited();

                                    if (isFinal) logger?.Info("Обход графа завершён. Сток достигнут!");

                                    await Task.Run(() => ControlPanelProvider.Continue(logger));

                                    path[v] = currentVertex;
                                    // Выполняем сбрасывание посещения вершин и рёбер графа
                                    graph.GetVertexElementsData().ForEach(x => x.SetNoVisited()); 
                                    graph.GetEdgeElementsData().ForEach(x => x?.SetNoVisited());
                                    return true;
                                }

                                logger?.Info($"Добавляем вершину \"{v.Data}\" в очередь.");
                                queue.Enqueue(v);
                                path[v] = currentVertex;
                                edgeElement.SetVisited();
                                v.SetVisited();
                            }
                        }
                    }
                }
            }

            if (isFinal) logger?.Info("Обход графа завершён. Сток не был достигнут!");
            await Task.Run(() => ControlPanelProvider.Continue(logger));
            // Выполняем сбрасывание посещения вершин и рёбер графа
            graph.GetVertexElementsData().ForEach(x => x.SetNoVisited()); 
            graph.GetEdgeElementsData().ForEach(x => x?.SetNoVisited());
            return false;
        }

        /// <summary> Выполнить алгоритм Форда — Фалкерсона </summary>
        /// <param name="graph"> Граф </param>
        /// <param name="source"> Исток потока </param>
        /// <param name="sink"> Сток потока </param>
        /// <param name="logger"> Логгер </param>
        public static async Task<int?> ExecuteFindMaxFlow(this DataProvider graph, VertexElement source, VertexElement sink, Logger? logger = null)
        {
            ConsoleHandler.SetIsWriteTitle();
            logger?.Info("Начинается демонстрация работы алгоритма поиск максимального потока между истоком и стоком. [Форда — Фалкерсона]");
            logger?.Info("!!! В процессе выполнения алгоритма на рёбрах графа будут указаны текущий максимальный поток и пропускная способность.");

            var maxFlow = 0;
            var path = new Dictionary<VertexElement, VertexElement?>();
            var rGraph = new Dictionary<EdgeElement, int>();

            logger?.Info("Инициализируем граф остаточного потока.");
            foreach (var edgeElement in graph.GetEdgeElementsData())
            {
                if (edgeElement != null)
                {
                    rGraph.Add(edgeElement, edgeElement.Weight);
                    edgeElement.SetDisplayTwoValues(maxFlow, edgeElement.Weight);
                }
            }

            await Task.Run(() => ControlPanelProvider.Continue(logger));
            if (ControlPanelProvider.IsReset) return null; // Была нажата кнопка сброса демонстрации алгоритма

            while (await graph.Bfs(rGraph, source, sink, path, logger))
            {
                var pathFlow = int.MaxValue;

                logger?.Info("Начинаем поиск минимального потока у найденного пути.");
                for (var v = sink; v != source; v = path[v])
                {
                    if (v != null)
                    {
                        var currentVertex = path[v];
                        if (currentVertex != null)
                        {
                            var edge = GetEdgeBetweenVertices(v, currentVertex);
                            if (edge != null) pathFlow = Math.Min(pathFlow, rGraph[edge]);
                        }
                    }
                }

                logger?.Info($"Минимальный поток пути определён: {pathFlow}.");
                await Task.Run(() => ControlPanelProvider.Continue(logger));
                if (ControlPanelProvider.IsReset) return null; // Была нажата кнопка сброса демонстрации алгоритма

                logger?.Info("Выполняем уменьшение пропускной способности потока.");
                for (var v = sink; v != source; v = path[v])
                {
                    if (v != null)
                    {
                        var currentVertex = path[v];
                        if (currentVertex != null)
                        {
                            var edge = GetEdgeBetweenVertices(v, currentVertex);
                            if (edge != null)
                            {
                                logger?.Info($"Пропускная способность {rGraph[edge]} уменьшается на {pathFlow}, а максимальный поток увеличен.");
                                rGraph[edge] -= pathFlow;
                                edge.SetDisplayTwoValues(maxFlow + pathFlow, rGraph[edge]);
                                await Task.Run(() => ControlPanelProvider.Continue(logger));
                                if (ControlPanelProvider.IsReset) return null; // Была нажата кнопка сброса демонстрации алгоритма
                            }
                        }
                    }
                }

                maxFlow += pathFlow;
                logger?.Info($"Максимальный поток теперь равен: {maxFlow}.");
            }

            return maxFlow;
        }

        /// <summary> Класс подмножество для алгоритма Union-find</summary>
        private class Subset
        {
            /// <summary> Вершина графа </summary>
            public  VertexElement Parent { get; set; }
            /// <summary> Ранг вершины </summary>
            public int Rank { get; set; }
        }

        /// <summary> Метод выполняющий поиск, сжатия пути </summary>
        /// <param name="subsets"> Словарь хранящий множества Union-find для каждой вершины </param>
        /// <param name="parent"> Родительская вершина </param>
        /// <param name="logger"> Логгер </param>
        private static VertexElement Find(Dictionary<VertexElement, Subset> subsets, VertexElement parent, Logger? logger = null)
        {
            logger?.Info("Рекурсивно выполняем поиск корня. Сжатие пути.");
            if (subsets[parent].Parent != parent)
                subsets[parent].Parent = Find(subsets, subsets[parent].Parent, logger);

            return subsets[parent].Parent;
        }

        /// <summary> Метод объединения вершин для Union-find </summary>
        /// <param name="subsets"> Словарь хранящий множества Union-find для каждой вершины </param>
        /// <param name="vertex1"> Вершина 1 </param>
        /// <param name="vertex2"> Вершина 2 </param>
        /// <param name="logger"> Логгер </param>
        private static void Union(Dictionary<VertexElement, Subset> subsets, VertexElement vertex1, VertexElement vertex2, Logger? logger = null)
        {
            logger?.Info("Выполняем поиск корней обоих вершин.");
            var xroot = Find(subsets, vertex1);
            var yroot = Find(subsets, vertex2);

            logger?.Info("Выполняем сравнение по рангу.");
            if (subsets[xroot].Rank < subsets[yroot].Rank)
            {
                logger?.Info($"Ранг вершины \"{yroot.Data}\" равен: {subsets[yroot].Rank}, что больше ранга вершины \"{xroot.Data}\" ({subsets[xroot].Rank}).");
                logger?.Info($"Устанавливаем вершину \"{yroot.Data}\" как родительскую для вершины \"{xroot.Data}\".");

                subsets[xroot].Parent = yroot;
            }
            else if (subsets[xroot].Rank > subsets[yroot].Rank)
            {
                logger?.Info($"Ранг вершины \"{xroot.Data}\" равен: {subsets[xroot].Rank}, что больше ранга вершины \"{yroot.Data}\" ({subsets[yroot].Rank}).");
                logger?.Info($"Устанавливаем вершину \"{xroot.Data}\" как родительскую для вершины \"{yroot.Data}\".");

                subsets[yroot].Parent = xroot;
            }
            else
            {
                logger?.Info($"Ранги обоих вершин оказались равны: {subsets[yroot].Rank}.");
                logger?.Info($"Устанавливаем вершину \"{xroot.Data}\" как родительскую для вершины \"{yroot.Data}\".");
                logger?.Info($"Увеличиваем ранг: {subsets[xroot].Rank} на единицу.");

                subsets[yroot].Parent = xroot;
                ++subsets[xroot].Rank;
            }
        }

        /// <summary> Выполнить алгоритм Крускала </summary>
        /// <param name="graph"> Граф </param>
        /// <param name="logger"> Логгер </param>
        public static async Task<(List<EdgeElement>, int)> ExecuteKruskal(this DataProvider graph, Logger? logger = null)
        {
            ConsoleHandler.SetIsWriteTitle();
            logger?.Info("Начинается демонстрация работы алгоритма поиск минимального остовного дерева. [Алгоритм Крускала]");

            if (!graph.GetVertexElementsData().HasConnected()) throw new ArgumentException("ОШИБКА! Граф не является связным.");

            var verticesCount = graph.GetVertexElementsData().Count;

            logger.Info($"Минимально остовное дерево имеет {verticesCount - 1} рёбер, что на единицу меньше количества вершин.");

            var edges = graph.GetEdgeElementsData().ToList();

            logger.Info("Инициализируем список для минимального остовного дерева.");
            var result = new List<EdgeElement>();

            var edgesCounter = 0;
            var verticesCounter = 0;
            var minimumCost = 0;

            logger.Info("Выполняем сортировку в порядке неубывания списка рёбер графа.");
            edges.Sort();

            var subsets = new Dictionary<VertexElement, Subset>();

            foreach (var vertexElement in graph.GetVertexElementsData())
            {
                subsets.Add(vertexElement, new Subset() {Parent = vertexElement, Rank = 0});
            }

            logger.Info("Начинаем выполнять проход по отсортированному списку рёбер.");

            while (verticesCounter < verticesCount - 1)
            {
                var nextEdge = edges[edgesCounter++];
                logger?.Info($"Выбрали ребро: \"{nextEdge.Weight}\".");

                logger?.Info("Начинаем выполнять поиск пути для первой вершины текущего ребра.");
                var x = Find(subsets, nextEdge.InitialVertexElement, logger);
                if (nextEdge.DestinationVertexElement != null)
                {
                    logger?.Info("Начинаем выполнять поиск пути для второй вершины текущего ребра.");
                    var y = Find(subsets, nextEdge.DestinationVertexElement);

                    if (x != y)
                    {
                        logger?.Info("Обнаруженные корни оказались разными.");
                        logger?.Info($"Добавляем ребро \"{nextEdge.Weight}\" в минимальное остовное дерево.");
                        result.Add(nextEdge);
                        minimumCost += nextEdge.Weight;
                        logger?.Info($"Минимальная сумма связующего дерева теперь ровна: {minimumCost}.");
                        verticesCounter++;

                        logger?.Info("Устанавливаем ребро как пройденное.");
                        nextEdge.SetVisited();
                        await Task.Run(() => ControlPanelProvider.Continue(logger));

                        logger?.Info($"Начинаем выполнять объединение набора вершин \"{x.Data}\" и \"{y.Data}\" по рангу.");
                        Union(subsets, x, y, logger);
                    }
                }
            }

            return (result, minimumCost);
        }

        /// <summary> Проверка того, что граф связный </summary>
        /// <param name="vertices"> Список вершин графа </param>
        private static bool HasConnected(this IEnumerable<Vertex> vertices) => vertices.All(vertex => vertex.EdgesList.Count != 0);
    }
}
