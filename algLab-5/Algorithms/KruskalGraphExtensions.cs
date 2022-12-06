using algLab_5.Data;
using algLab_5.Models.Graph;
using algLab_5.Services.Logger;
using algLab_5.Views.Graph;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using algLab_5.Services;

namespace algLab_5.Algorithms
{
    public static class KruskalGraphExtensions
    {
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
