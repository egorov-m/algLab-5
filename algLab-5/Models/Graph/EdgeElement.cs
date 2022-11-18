using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

        /// <summary> Коллекция точек для рисования линии ребра </summary>
        public PointCollection PointCollection { get; set; }

        /// <summary> Текстовый блок ребра </summary>
        public TextBlock TextBlock;
        
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
            TextBlock.Text = weight.ToString();
        }

        /// <summary> Установить элемент вершины </summary>
        private void Set()
        {
            var polyline = ConfiguratorViewElement.GetPolylineEdgeElement();
            var textBlock = ConfiguratorViewElement.GetTextBlockEdgeElement();
            textBlock.Text = Weight.ToString();

            Polyline = polyline;
            TextBlock = textBlock;
        }

        /// <summary> Рисовать ребро от начальной вершины до заданной точки на заданном холсте </summary>
        /// <param name="canvas"> Холст </param>
        /// <param name="point"> Точка </param>
        public void Draw(Canvas canvas, Point point)
        {
            canvas.Children.Add(Polyline);
            Panel.SetZIndex(Polyline, 1);

            canvas.Children.Add(TextBlock);
            Panel.SetZIndex(TextBlock, 2);

            Polyline.Points.Clear();
            PointCollection = ConfiguratorViewElement.GetPointCollectionForConnection(point, InitialVertexElement.Grid, ConnectionType.Default);
            Polyline.Points = PointCollection;

            TextBlock.SetCoordinatesForTextBlock(Polyline.GetDirectPolyLineCenter());
        }

        /// <summary> Перерисовать ребро от начальной вершины до заданной </summary>
        /// <param name="point"> Точка </param>
        public void Draw(Point point)
        {
            Polyline.Points.Clear();
            PointCollection = ConfiguratorViewElement.GetPointCollectionForConnection(point, InitialVertexElement.Grid, ConnectionType.Default);
            Polyline.Points = PointCollection;

            TextBlock.SetCoordinatesForTextBlock(Polyline.GetDirectPolyLineCenter());
        }

        /// <summary> Перерисовать ребро от точки до точки </summary>
        /// <param name="point1"> Точка 1 </param>
        /// <param name="point2"> Точка 2 </param>
        public void Draw(Point point1, Point point2)
        {
            Polyline.Points.Clear();
            PointCollection = ConfiguratorViewElement.GetPointCollectionForConnection(point1, point2, ConnectionType.Default);
            Polyline.Points = PointCollection;
            TextBlock.SetCoordinatesForTextBlock(Polyline.GetDirectPolyLineCenter());
        }

        /// <summary> Перерисовывает ребра с указанным сдвигом </summary>
        /// <param name="diffX"> Разница по оси X </param>
        /// <param name="diffY"> Разница по оси Y </param>
        public void Draw(double diffX, double diffY)
        {
            for (var i = 0; i < PointCollection.Count; i++)
            {
                var newPosition = PointCollection[i];
                newPosition.X += diffX;
                newPosition.Y += diffY;
                PointCollection[i] = newPosition;
            }

            Polyline.Points = PointCollection;
            TextBlock.SetCoordinatesForTextBlock(Polyline.GetDirectPolyLineCenter());
        }

        /// <summary> Рисовать ребро от начальной конечной вершины </summary>
        public void Draw()
        {
            Polyline.Points.Clear();
            if (DestinationVertexElement != null)
            {
                PointCollection = ConfiguratorViewElement.GetPointCollectionForConnection(InitialVertexElement.Grid, DestinationVertexElement.Grid, ConnectionType.Default);
                Polyline.Points = PointCollection;
                TextBlock.SetCoordinatesForTextBlock(Polyline.GetDirectPolyLineCenter());
            }
        }
        /// <summary> Рисовать ребро на заданном холсте </summary>
        /// <param name="canvas"> Холст </param>
        public void Draw(Canvas canvas)
        {
            canvas.Children.Add(Polyline);
            Panel.SetZIndex(Polyline, 1);
            if (DestinationVertexElement != null)
            {
                PointCollection = ConfiguratorViewElement.GetPointCollectionForConnection(InitialVertexElement.Grid, DestinationVertexElement.Grid, ConnectionType.Default);
                Polyline.Points = PointCollection;
                TextBlock.SetCoordinatesForTextBlock(Polyline.GetDirectPolyLineCenter());
            }
        }

        /// <summary> Удалить ребро с холста </summary>
        /// <param name="canvas"> Холст </param>
        public void RemoveDraw(Canvas canvas)
        {
            canvas.Children.Remove(Polyline);
            canvas.Children.Remove(TextBlock);
        }
    }
}
