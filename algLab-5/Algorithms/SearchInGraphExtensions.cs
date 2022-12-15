using algLab_5.Services.Logger;
using algLab_5.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using algLab_5.Models.Graph;
using System.Collections;

namespace algLab_5.Algorithms
{
    public static class SearchInGraphExtensions
    {
        /// <summary> Выполнить обход графа в глубину </summary>
        /// <param name="startVertex"> Стартовая вершина </param>
        /// <param name="logger"> Логгер </param>
        public static async IAsyncEnumerable<Vertex>? ExecuteDfs(this Vertex startVertex, Logger? logger = null)
        {
            ConsoleHandler.SetIsWriteTitle();
            logger?.Info("Начинается демонстрация работы алгоритма поиск в глубину (DFS). [реализация на стеке]");
            logger?.Info("Инициализируем стек для добавления вершин графа.");
            var stack = new Stack<Vertex>();

            logger?.Info($"Добавляем стартовую вершину \"{startVertex.Data}\" в стек.");
            stack.Push(startVertex);

            logger?.Info($"Текущее состояние стека: {stack.ToList().GetArrayForLog()}");

            var isFinal = true;

            logger?.Info("Пока созданный стек не пуст будем выполнять итерации цикла.");
            while (stack.Count > 0)
            {
                var currentVertex = stack.Pop();
                if (currentVertex.IsVisited) continue;
                logger?.Info($"Посещаем элемент \"{currentVertex.Data}\".");
                currentVertex.SetVisited();
                currentVertex.SetCurrent();

                logger?.Info($"Текущее состояние стека: {stack.ToList().GetArrayForLog()}");

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
                        var el = edge.InitialVertex == currentVertex ? edge.DestinationVertex : edge.InitialVertex;
                        if (el != null)
                        {
                            logger?.Info($"Добавляем вершину \"{el.Data}\" в стек.");
                            stack.Push(el);

                            logger?.Info($"Текущее состояние стека: {stack.ToList().GetArrayForLog()}");
                        }
                    }
                }

                await Task.Run(() => ControlPanelProvider.Continue(logger));
                if (ControlPanelProvider.IsReset) // Была нажата кнопка сброса демонстрации алгоритма
                {
                    isFinal = false;
                    break;
                }

                currentVertex.ResetCurrent();
            }

            if (isFinal) logger?.Info("Обход графа завершён.");
        }

        /// <summary> Выполнить обход графа в ширину </summary>
        /// <param name="startVertex"> Стартовая вершина </param>
        /// <param name="logger"> Логгер </param>
        public static async IAsyncEnumerable<Vertex> ExecuteBfs(this Vertex startVertex, Logger? logger = null)
        {
            ConsoleHandler.SetIsWriteTitle();
            logger?.Info("Начинается демонстрация работы алгоритма поиск в ширину (BFS). [реализация на очереди]");
            logger?.Info("Инициализируем очереди для добавления вершин графа.");
            var queue = new Queue<Vertex>();

            logger?.Info($"Добавляем стартовую вершину \"{startVertex.Data}\" в очередь.");
            queue.Enqueue(startVertex);

            logger?.Info($"Текущее состояние очереди: {queue.ToList().GetArrayForLog()}");

            var isFinal = true;

            logger?.Info("Пока созданная очередь не пуста будем выполнять итерации цикла.");
            while (queue.Count > 0)
            {
                var currentVertex = queue.Dequeue();
                if (currentVertex.IsVisited) continue;
                logger?.Info($"Посещаем элемент \"{currentVertex.Data}\".");
                currentVertex.SetVisited();
                currentVertex.SetCurrent();

                logger?.Info($"Текущее состояние очереди: {queue.ToList().GetArrayForLog()}");

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
                        var el = edge.InitialVertex == currentVertex ? edge.DestinationVertex : edge.InitialVertex;
                        if (el != null)
                        {
                            logger?.Info($"Добавляем вершину \"{el.Data}\" в очередь.");
                            queue.Enqueue(el);

                            logger?.Info($"Текущее состояние очереди: {queue.ToList().GetArrayForLog()}");
                        }
                    }
                }

                await Task.Run(() => ControlPanelProvider.Continue(logger));
                if (ControlPanelProvider.IsReset) // Была нажата кнопка сброса демонстрации алгоритма
                {
                    isFinal = false;
                    break;
                }

                currentVertex.ResetCurrent();
            }

            if (isFinal) logger?.Info("Обход графа завершён.");
        }
    }
}
