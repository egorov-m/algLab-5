using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using algLab_5.Models;
using algLab_5.Models.Graph;
using algLab_5.Services;
using Colors = algLab_5.Views.Utils.Colors;

namespace algLab_5.Views.Graph
{
    /// <summary> Элемент ребра графа </summary>
    public class EdgeElement : 
        Edge, 
        IDraw, 
        IDrawLine, 
        IRemoveDraw,
        IVisited,
        IComparable
    {
        #region Составные элементы

        /// <summary> Линия ребра </summary>
        public override Polyline Polyline { get; set; }

        /// <summary> Коллекция точек для рисования линии ребра </summary>
        public PointCollection PointCollection { get; set; }

        /// <summary> Панель для содержимого элемента ребра </summary>
        public override StackPanel StackPanel { get; set; }

        /// <summary> Текстовое поле ввода веса ребра </summary>
        public override TextBox TextBox { get; set; }

        #endregion

        public EdgeElement(Vertex initialVertex, Vertex destinationVertex, int weight) : base(initialVertex, destinationVertex, weight)
        {
            //InitialVertexElement = initialVertexElement;
            //DestinationVertexElement = destinationVertexElement;
            Set();
        }

        public EdgeElement(Vertex initialVertex, int weight) : base(initialVertex, weight)
        {
            //InitialVertexElement = initialVertexElement;
            Set();
        }

        /// <summary> Установить отображение двух указанных значений </summary>
        /// <param name="value1"> Значение 1 </param>
        /// <param name="value2"> Значение 2 </param>
        public override void SetDisplayTwoValues(int value1, int value2)
        {
            TextBox.Text = $"{value1} / {value2}";
        }

        /// <summary> Установить вес ребра </summary>
        /// <param name="weight"> Вес ребра </param>
        public override bool SetWeight(int weight)
        {
            if (weight >= 0)
            {
                Weight = weight;
                TextBox.Text = weight.ToString();
                return true;
            }

            return false;
        }

        /// <summary> Установить вес ребра </summary>
        /// <param name="weight"> Вес ребра </param>
        public override bool SetWeight(string weight)
        {
            if (int.TryParse(weight, out var intWeight) && intWeight >= 0)
            {
                Weight = intWeight;
                return true;
            }

            TextBox.Text = Weight.ToString();
            return false;
        }

        /// <summary> Отобразить вес ребра </summary>
        public override void SetWeight() => TextBox.Text = Weight.ToString();

        /// <summary> Установить элемент вершины </summary>
        private void Set()
        {
            var polyline = ConfiguratorViewElement.GetPolylineEdgeElement();

            var stackPanel = ConfiguratorViewElement.GetStackPanelEdgeElement();

            var textBox = ConfiguratorViewElement.GetTextBoxEdgeElement();
            textBox.Text = Weight.ToString();

            stackPanel.Children.Add(textBox);

            Polyline = polyline;
            StackPanel = stackPanel;
            TextBox = textBox;
        }

        public override void SetVisited()
        {
            _isVisited = true;
            Polyline.Stroke = new SolidColorBrush(Colors.VisitedElementColor);
        }

        public override void SetNoVisited()
        {
            _isVisited = false;
            Polyline.Stroke = new SolidColorBrush(Colors.EdgeElementInnerColor);
        }

        /// <summary> Рисовать ребро от начальной вершины до заданной точки на заданном холсте </summary>
        /// <param name="canvas"> Холст </param>
        /// <param name="point"> Точка </param>
        public override void Draw(Canvas canvas, Point point)
        {
            canvas.Children.Add(Polyline);
            Panel.SetZIndex(Polyline, 1);

            canvas.Children.Add(StackPanel);
            Panel.SetZIndex(StackPanel, 2);

            Polyline.Points.Clear();
            PointCollection = ConfiguratorViewElement.GetPointCollectionForConnection(point, InitialVertex.Grid, ConnectionType.Default);
            Polyline.Points = PointCollection;

            StackPanel.SetCoordinatesForStackPanel(Polyline.GetDirectPolyLineCenter());
        }

        /// <summary> Перерисовать ребро от начальной вершины до заданной </summary>
        /// <param name="point"> Точка </param>
        public override void Draw(Point point)
        {
            Polyline.Points.Clear();
            PointCollection = ConfiguratorViewElement.GetPointCollectionForConnection(point, InitialVertex.Grid, ConnectionType.Default);
            Polyline.Points = PointCollection;

            StackPanel.SetCoordinatesForStackPanel(Polyline.GetDirectPolyLineCenter());
        }

        /// <summary> Перерисовать ребро от точки до точки </summary>
        /// <param name="point1"> Точка 1 </param>
        /// <param name="point2"> Точка 2 </param>
        public override void Draw(Point point1, Point point2)
        {
            Polyline.Points.Clear();
            PointCollection = ConfiguratorViewElement.GetPointCollectionForConnection(point1, point2, ConnectionType.Default);
            Polyline.Points = PointCollection;
            StackPanel.SetCoordinatesForStackPanel(Polyline.GetDirectPolyLineCenter());
        }

        /// <summary> Перерисовывает ребра с указанным сдвигом </summary>
        /// <param name="diffX"> Разница по оси X </param>
        /// <param name="diffY"> Разница по оси Y </param>
        public override void Draw(double diffX, double diffY)
        {
            for (var i = 0; i < PointCollection.Count; i++)
            {
                var newPosition = PointCollection[i];
                newPosition.X += diffX;
                newPosition.Y += diffY;
                PointCollection[i] = newPosition;
            }

            Polyline.Points = PointCollection;
            StackPanel.SetCoordinatesForStackPanel(Polyline.GetDirectPolyLineCenter());
        }

        /// <summary> Рисовать ребро от начальной конечной вершины </summary>
        public override void Draw()
        {
            Polyline.Points.Clear();
            if (DestinationVertex != null)
            {
                PointCollection = new PointCollection {InitialVertex.Position, DestinationVertex.Position};
                Polyline.Points = PointCollection;
                StackPanel.SetCoordinatesForStackPanel(Polyline.GetDirectPolyLineCenter());
            }
        }
        /// <summary> Рисовать ребро на заданном холсте </summary>
        /// <param name="canvas"> Холст </param>
        public override void Draw(Canvas canvas)
        {
            canvas.Children.Add(Polyline);
            Panel.SetZIndex(Polyline, 1);
            canvas.Children.Add(StackPanel);
            Panel.SetZIndex(StackPanel, 2);

            if (DestinationVertex != null)
            {
                PointCollection = new PointCollection {InitialVertex.Position, DestinationVertex.Position};
                Polyline.Points = PointCollection;
                StackPanel.SetCoordinatesForStackPanel(Polyline.GetDirectPolyLineCenter());
            }
        }

        /// <summary> Удалить ребро с холста </summary>
        /// <param name="canvas"> Холст </param>
        public override void RemoveDraw(Canvas canvas)
        {
            canvas.Children.Remove(Polyline);
            canvas.Children.Remove(StackPanel);
        }
    }
}
