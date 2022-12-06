using algLab_5.Views.Graph;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace algLab_5.Data
{
    /// <summary> Класс поставляющий данные </summary>
    public class DataProvider
    {
        private readonly List<VertexElement> _dataVertexElements;
        private readonly List<EdgeElement?> _dataEdgeElements;

        /// <summary> Инициализация DataProvider без загрузки данных </summary>
        public DataProvider()
        {
            _dataVertexElements = new List<VertexElement>();
            _dataEdgeElements = new List<EdgeElement?>();
        }

        /// <summary> Получить данные проекта от поставщика </summary>
        public List<VertexElement> GetVertexElementsData() => _dataVertexElements;

        public List<EdgeElement?> GetEdgeElementsData() => _dataEdgeElements;

        /// <summary>
        /// Инициализация DataProvider с загрузкой существующих данных
        /// </summary>
        /// <param name="path"> Путь к файлу </param>
        /// <param name="workingDirectory"> Рабочая директория </param>
        /// <param name="formatDataGraph"> Тип файла </param>
        /// <param name="canvas"> Холст </param>
        public DataProvider(string path, string? workingDirectory, FormatDataGraph formatDataGraph, Panel canvas)
        {
            // выполнять запрос данных от пользователя
        }

        /// <summary> Добавить элемент вершины графа </summary>
        /// <param name="vertexElement"> Элемент вершины </param>
        public void AddVertexElement(VertexElement vertexElement)
        {
            _dataVertexElements.Add(vertexElement);
        }

        /// <summary> Добавить элемент ребра графа </summary>
        /// <param name="edgeElement"> Ребро графа </param>
        public bool AddEdgeElement(EdgeElement? edgeElement)
        {
            //_dataEdgeElements.Add(edgeElement);
            var isAdd = !_dataEdgeElements.Any(item =>
                (item.InitialVertexElement == edgeElement.InitialVertexElement &&
                 item.DestinationVertexElement == edgeElement.DestinationVertexElement) ||

                (item.DestinationVertexElement == edgeElement.InitialVertexElement
                 && item.InitialVertexElement == edgeElement.DestinationVertexElement));
            if (!isAdd) return isAdd;

            var unnecessaryEdgeElement = _dataEdgeElements.FirstOrDefault(item =>
                item.InitialVertexElement == edgeElement.DestinationVertexElement &&
                item.DestinationVertexElement == null);
            if (unnecessaryEdgeElement != null) _dataEdgeElements.Remove(unnecessaryEdgeElement);
            _dataEdgeElements.Add(edgeElement);

            return isAdd;
        }
    }
}
