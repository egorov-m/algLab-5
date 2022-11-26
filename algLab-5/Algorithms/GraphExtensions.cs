using System.Collections.Generic;
using System.Threading.Tasks;
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
        public static async IAsyncEnumerable<VertexElement> ExecuteDfs(this VertexElement startVertex, Logger? logger = null)
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
    }
}
