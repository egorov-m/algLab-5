using algLab_5.Services.Logger;
using algLab_5.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using algLab_5.Models.Graph;

namespace algLab_5.Algorithms
{
    public static class DijkstraGraphExtensions
    {
        /// <summary> Класс данных для алгоритма Дейкстры </summary>
        private class DijkstraData
        {
            /// <summary> Стоимость </summary>
            public int Price { get; set; }
            /// <summary> Предыдущая вершина </summary>
            public Vertex? Previous { get; set; }
        }

        /// <summary> Выполнить алгоритм Дейкстры </summary>
        /// <param name="graph"> Граф (список всех вершин) </param>
        /// <param name="start"> Стартовая вершина </param>
        /// <param name="end"> Конечна вершина </param>
        /// <param name="logger"> Логгер </param>
        public static async Task<List<Vertex>?> ExecuteDijkstra(this List<Vertex> graph, Vertex start, Vertex end, Logger? logger = null)
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
            var track = new Dictionary<Vertex, DijkstraData>();

            logger?.Info("Складываем начальную вершину в трекинг. Начальная цена равна нулю.");
            track[start] = new DijkstraData {Previous = null, Price = 0 };
            start.SetVisited();
            start.TextBox.Text = "0";

            await Task.Run(() => ControlPanelProvider.Continue(logger));
            if (ControlPanelProvider.IsReset) return null; // Была нажата кнопка сброса демонстрации алгоритма

            logger?.Info("Запускаем вечный цикл.");
            while (true)
            {
                Vertex? toOpen = null;
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
                    logger?.Info($"Выполняем проход по ребру \"{edge.Weight}\".");
                    edge.SetVisited();

                    await Task.Run(() => ControlPanelProvider.Continue(logger));
                    if (ControlPanelProvider.IsReset)
                    {
                        isFinal = false;
                        return null; // Была нажата кнопка сброса демонстрации алгоритма
                    }

                    var currentPrice = track[toOpen].Price + edge.Weight;
                    logger?.Info($"Вычисляем текущую цену (цена раскрываемой вершины + вес ребра) = {track[toOpen].Price} + {edge.Weight} = {currentPrice}.");

                    var nextVertex = edge.InitialVertex == toOpen ? edge.DestinationVertex: edge.InitialVertex;
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

                logger?.Info($"Устанавливаем вершину \"{toOpen.Data}\" как посещённую.");
                noVisited.Remove(toOpen);
            }

            var result = new List<Vertex>();
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
    }
}
