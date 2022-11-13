using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using algLab_5.Services;

namespace algLab_5.Models.Graph
{
    /// <summary> Элемент ребра графа </summary>
    public class EdgeElement : Edge
    {
        #region Составные элементы

        /// <summary> Начальная вершина ребра </summary>
        public VertexElement InitialVertexElement { get; set; }

        /// <summary> Конечная вершина ребра </summary>
        public VertexElement? DestinationVertexElement { get; set; }

        /// <summary> Линия ребра </summary>
        public Polyline Polyline { get; set; }

        /// <summary> Текстовый блок ребра </summary>
        private TextBlock _textBlock;
        
        #endregion

        public EdgeElement(VertexElement initialVertexElement, VertexElement destinationVertexElement, int initialVertexId, int destinationVertexId, int weight) : base(initialVertexId, destinationVertexId, weight)
        {
            InitialVertexElement = initialVertexElement;
            DestinationVertexElement = destinationVertexElement;
            Set();
        }

        public EdgeElement(VertexElement initialVertexElement, int initialVertexId, int weight) : base(initialVertexId, weight)
        {
            InitialVertexElement = initialVertexElement;
            Set();
        }

        /// <summary> Установить вес ребра </summary>
        /// <param name="weight"> Вес ребра </param>
        public override void SetWeight(int weight)
        {
            Weight = weight;
            _textBlock.Text = weight.ToString();
        }

        /// <summary> Установить элемент вершины </summary>
        private void Set()
        {
            var polyline = ConfiguratorViewElement.GetPolylineEdgeElement();
            var textBlock = ConfiguratorViewElement.GetTextBlockEdgeElement();

            Polyline = polyline;
            _textBlock = textBlock;
        }

        /// <summary> Рисовать ребро от начальной вершины до заданной точки на заданном холсте </summary>
        /// <param name="canvas"> Холст </param>
        /// <param name="point"> Точка </param>
        public void Draw(Canvas canvas, Point point)
        {
            canvas.Children.Add(Polyline);
            Panel.SetZIndex(Polyline, 1);
            Polyline.Points.Clear();
            Polyline.Points = ConfiguratorViewElement.GetPointCollectionForConnection(point, InitialVertexElement.Grid, ConnectionType.Default);
        }

        /// <summary> Перерисовать ребро от начальной вершины до заданной </summary>
        /// <param name="point"> Точка </param>
        public void Draw(Point point)
        {
            Polyline.Points.Clear();
            Polyline.Points = ConfiguratorViewElement.GetPointCollectionForConnection(point, InitialVertexElement.Grid, ConnectionType.Default);
        }

        /// <summary> Рисовать ребро от начальной конечной вершины </summary>
        public void Draw()
        {
            Polyline.Points.Clear();
            if (DestinationVertexElement != null) Polyline.Points = ConfiguratorViewElement.GetPointCollectionForConnection(InitialVertexElement.Grid, DestinationVertexElement.Grid, ConnectionType.Default);
        }
        /// <summary> Рисовать ребро на заданном холсте </summary>
        /// <param name="canvas"> Холст </param>
        public void Draw(Canvas canvas)
        {
            canvas.Children.Add(Polyline);
            Panel.SetZIndex(Polyline, 1);
            if (DestinationVertexElement != null) Polyline.Points = ConfiguratorViewElement.GetPointCollectionForConnection(InitialVertexElement.Grid, DestinationVertexElement.Grid, ConnectionType.Default);
        }

        /// <summary> Удалить ребро с холста </summary>
        /// <param name="canvas"> Холст </param>
        public void RemoveDraw(Canvas canvas)
        {
            canvas.Children.Remove(Polyline);
        }
    }
}
