using algLab_5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace algLab_5.Data
{
    /// <summary> Класс хранилище фигур </summary>
    [Obsolete("Данный класс не рекомендуется к использованию, структура хранения данных графа переработана.", true)]
    public class ShapesRepository
    {
        /// <summary> Холст </summary>
        private readonly Canvas _canvas;
        /// <summary> Набор элементов информации о соединениях </summary>
        private readonly List<ElementConnectionInfo> _connectingElements = new();

        public ShapesRepository(Canvas canvas)
        {
            _canvas = canvas;
        }

        /// <summary> Добавление нового элемента </summary>
        /// <param name="element"></param>
        public void AddElement(Grid element)
        {
            _connectingElements.Add(new ElementConnectionInfo { Initial = element });
        }

        /// <summary> Добавление новой связи </summary>
        /// <param name="initialElement"> Начальный элемент </param>
        /// <param name="destinationElement"> Элемент назначения </param>
        /// <param name="connection"> Линия соединения </param>
        /// <param name="type"> Тип соединения </param>
        public bool AddConnection(Grid initialElement, Grid? destinationElement, Polyline connection, ConnectionType type)
        {
            var isAdd = !_connectingElements.Any(item => item.Initial == initialElement &&
                item.Destination == destinationElement || item.Destination == initialElement &&
                item.Initial == destinationElement);
            if (!isAdd) return isAdd;
            var unnecessaryInformationConnection = _connectingElements.FirstOrDefault(item =>
                item.Initial == destinationElement && item.ConnectionLine == null);
            if (unnecessaryInformationConnection != null) _connectingElements.Remove(unnecessaryInformationConnection);
            _connectingElements.Add(new ElementConnectionInfo
            {
                Initial = initialElement,
                Destination = destinationElement,
                ConnectionLine = connection,
                ConnectionType = type
            });

            return isAdd;
        }

        /// <summary> Получить связи элемента (линия, сетка) </summary>
        /// <param name="initialElement"> Элемент </param>
        public (List<(Polyline?, ConnectionType, Grid?)> connectionsFromInitial, List<(Polyline?, ConnectionType, Grid?)> connectionsFromDestination) GetConnectionsElement(Grid? initialElement)
        {
            List<(Polyline?, ConnectionType, Grid?)> connectionsFromInitial = new();
            List<(Polyline?, ConnectionType, Grid?)> connectionsFromDestination = new();

            connectionsFromDestination.AddRange(_connectingElements
                .Where(item => item.Initial == initialElement)
                .Select(item => (item.ConnectionLine, item.ConnectionType, item.Destination))
                .ToList());

            connectionsFromInitial.AddRange(_connectingElements
                .Where(item => item.Destination == initialElement)
                .Select(item => (item.ConnectionLine, item.ConnectionType, item.Initial))
                .ToList()!);
            return (connectionsFromInitial, connectionsFromDestination);
        }
    }
}
