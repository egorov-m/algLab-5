using algLab_5.Models.Graph;
using algLab_5.Services;
using algLab_5.Views.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Colors = algLab_5.Views.Utils.Colors;

namespace algLab_5.Views.Graph
{
    /// <summary> Элемент вершины графа </summary>
    public class VertexElement : 
        Vertex, 
        IDraw, 
        IRemoveDraw,
        IVisited
    {
        /// <summary> Позиция центра вершины на холсте </summary>
        public override Point Position { get; set; }

        #region Составные части
        /// <summary> Сетка элемента вершины </summary>
        public override Grid Grid { get; set; }

        /// <summary> Эллипс (снова) элемента вершины </summary>
        private Ellipse _ellipse;

        /// <summary> Панель для содержимого элемента вершины </summary>
        private StackPanel _stackPanel;

        /// <summary> Текстовое поле ввода элемента вершины </summary>
        public override TextBox TextBox { get; set; }
        #endregion

        public VertexElement(string data) : base(data)
        {
            Set();
        }

        public VertexElement(int id, string data) : base(id, data)
        {
            Set();
        }

        public VertexElement(int id, string data, IList<Edge?> edgesList) : base(id, data, edgesList)
        {
            Set();
        }

        /// <summary> Установить элемент вершины </summary>
        private void Set()
        {
            var grid = ConfiguratorViewElement.GetGridVertexElement();
            var stackPanel = ConfiguratorViewElement.GetStackPanelVertexElement();

            var textBox = ConfiguratorViewElement.GetTextBoxVertexElement();
            stackPanel.Children.Add(textBox);
            grid.Children.Add(stackPanel);

            Grid.SetRow(stackPanel, 0);
            Grid.SetColumn(stackPanel, 0);

            Grid = grid;
            _ellipse = grid.GetEllipse();
            _stackPanel = stackPanel;
            TextBox = textBox;
            TextBox.Text = Data;
        }

        /// <summary> Установить данные вершины </summary>
        /// <param name="data"> Данные </param>
        /// <param name="graphElements"> Коллекция элементов вершин текущего графа (не должно быть повторений) </param>
        public override bool SetData(string data, IEnumerable<Vertex> graphElements)
        {
            if (Data == data) return true;

            var count = graphElements.Count(graphElement => graphElement.Data == data);
            if (count > 0)
            {
                TextBox.Text = Data;
                return false;
            }

            Data = data;
            TextBox.Text = Data;
            return true;
        }

        /// <summary> Отобразить данные </summary>
        public override void SetData() => TextBox.Text = Data;

        /// <summary> Установить вершину как посещённую </summary>
        public override void SetVisited()
        {
            _isVisited = true;
            _ellipse.Fill = new SolidColorBrush(Colors.VisitedElementColor);
        }

        /// <summary> Установить вершину как НЕ посещённую </summary>
        public override void SetNoVisited()
        {
            _isVisited = false;
            _ellipse.Fill = new SolidColorBrush(Colors.VertexElementInnerColor);
        }

        /// <summary> Установить вершину как текущую (визуально) </summary>
        public override void SetCurrent() => _ellipse.Fill = new SolidColorBrush(Colors.CurrentElementColor);

        /// <summary> Сбросить статус текущей вершины (визуально) </summary>
        public override void ResetCurrent()
        {
            _ellipse.Fill = IsVisited ? new SolidColorBrush(Colors.VisitedElementColor) : new SolidColorBrush(Colors.VertexElementInnerColor);
        }

        public override void Draw(Canvas canvas, int canvasHeight, int canvasWidth)
        {
            BoundWithinCanvas(canvasHeight, canvasWidth);
            Draw(canvas);
        }

        /// <summary> Рисовать вершину в заданной точке на заданном холсте </summary>
        /// <param name="canvas"> Холст </param>
        /// <param name="point"> Точка </param>
        public override void Draw(Canvas canvas, Point point)
        {
            Position = point;
            Draw(canvas);
        }

        /// <summary> Переместить вершину в новую точку </summary>
        /// <param name="point"> Точка </param>
        public override void Draw(Point point)
        {
            Position = point;
            Grid.SetCenterEllipseOnGrid(Position);
        }

        /// <summary> Перерисовывает вершину в новую точку на указанный сдвиг </summary>
        /// <param name="diffX"> Разница по оси X </param>
        /// <param name="diffY"> Разница по оси Y </param>
        public override void Draw(double diffX, double diffY)
        {
            var newPosition = Position;
            newPosition.X += diffX;
            newPosition.Y += diffY;
            Position = newPosition;
            Grid.SetCenterEllipseOnGrid(Position);
        }

        /// <summary> Отобразить вершину на холсте </summary>
        /// <param name="canvas"> Холст </param>
        public override void Draw(Canvas canvas)
        {
            canvas.Children.Add(Grid);
            Panel.SetZIndex(Grid, 5);
            Grid.SetCenterEllipseOnGrid(Position);
        }

        public void BoundWithinCanvas(int canvasHeight, int canvasWidth)
        {
            var radius = Params.SizeVertexElement / 2;
            Position = new Point(Math.Max(radius, Math.Min(canvasWidth - radius, Position.X)),
                Math.Max(radius, Math.Min(canvasHeight - radius, Position.Y)));
        }

        public override void RemoveDraw(Canvas canvas)
        {
            canvas.Children.Remove(Grid);
        }
    }
}
