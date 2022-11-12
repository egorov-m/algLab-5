using System.Windows.Controls;
using System.Windows.Shapes;

namespace algLab_5.Models
{
    /// <summary> Элемент информации о соединении </summary>
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
