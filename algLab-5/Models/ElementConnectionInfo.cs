using System;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace algLab_5.Models
{
    /// <summary> Элемент информации о соединении </summary>
    [Obsolete("Данный класс не рекомендуется к использованию, структура хранения данных графа переработана.", true)]
    public class ElementConnectionInfo
    {
        /// <summary> Начальный элемент </summary>
        public Grid Initial { get; set; }
        /// <summary> Удалённый элемент </summary>
        public Grid? Destination { get; set; }
        /// <summary> Связь элементов </summary>
        public Polyline? ConnectionLine { get; set; }
        /// <summary> Тип связи элементов </summary>
        public ConnectionType ConnectionType { get; set; }
    }
}
