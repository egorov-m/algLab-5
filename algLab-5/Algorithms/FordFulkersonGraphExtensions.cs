using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using algLab_5.Data;
using algLab_5.Models.Graph;
using algLab_5.Services;
using algLab_5.Services.Logger;

namespace algLab_5.Algorithms
{
    public static class FordFulkersonGraphExtensions
    {
        /// <summary> Получить ребро между двумя переданными вершинами </summary>
        /// <param name="x"> Первая вершина </param>
        /// <param name="y"> Вторая вершина </param>
        private static Edge? GetEdgeBetweenVertices(Vertex x, Vertex y)
        {
            foreach (var edgeX in x.EdgesList)
            {
                foreach (var edgeY in y.EdgesList)
                {
                    if (edgeX == edgeY) return edgeX;
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
        private static async Task<bool> Bfs(this DataProvider graph, Dictionary<Edge, int> rGraph, Vertex source, Vertex sink, Dictionary<Vertex, Vertex?> path, Logger? logger = null)
        {
            logger?.Info("Начинаем выполнять обход граф в ширину.");
            logger?.Info("Инициализируем очереди для добавления вершин графа.");
            var queue = new Queue<Vertex>();

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
                    var v = currentVertex == edge?.InitialVertex ? edge.DestinationVertex : edge.InitialVertex;

                    if (v != null)
                    {
                        if (!v.IsVisited && rGraph[edge] > 0)
                        {
                            logger?.Info($"Выполняем проход по ребру \"{edge.Weight}\".");

                            if (v == sink)
                            {
                                edge.SetVisited();
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
                            edge.SetVisited();
                            v.SetVisited();
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
        public static async Task<int?> ExecuteFordFulkerson(this DataProvider graph, Vertex source, Vertex sink, Logger? logger = null)
        {
            ConsoleHandler.SetIsWriteTitle();
            logger?.Info("Начинается демонстрация работы алгоритма поиск максимального потока между истоком и стоком. [Форда — Фалкерсона]");
            logger?.Info("!!! В процессе выполнения алгоритма на рёбрах графа будут указаны текущий максимальный поток и пропускная способность.");

            var maxFlow = 0;
            var path = new Dictionary<Vertex, Vertex?>();
            var rGraph = new Dictionary<Edge, int>();

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
    }
}
