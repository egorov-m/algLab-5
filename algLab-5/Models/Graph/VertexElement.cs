using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using algLab_5.Models.Utils;
using algLab_5.Services;

namespace algLab_5.Models.Graph
{
    /// <summary> Элемент вершины графа </summary>
    public class VertexElement : Vertex
    {
        /// <summary> Позиция центра вершины на холсте </summary>
        public Point Position { get; set; }

        #region Составные части
        /// <summary> Сетка элемента вершины </summary>
        public Grid Grid;

        /// <summary> Эллипс (снова) элемента вершины </summary>
        private Ellipse _ellipse;

        /// <summary> Панель для содержимого элемента вершины </summary>
        private StackPanel _stackPanel;

        /// <summary> Текстовый блок элемента вершины </summary>
        private TextBlock _textBlock;
        #endregion

        public VertexElement(int id, string data) : base(id, data)
        {
            Set();
        }

        /// <summary> Установить элемент вершины </summary>
        private void Set()
        {
            var grid = ConfiguratorViewElement.GetGridVertexElement();
            var stackPanel =  ConfiguratorViewElement.GetStackPanelVertexElement();

            var textBlock = ConfiguratorViewElement.GetTextBlockVertexElement();
            stackPanel.Children.Add(textBlock);
            grid.Children.Add(stackPanel);

            Grid.SetRow(stackPanel, 0);
            Grid.SetColumn(stackPanel, 0);

            Grid = grid;
            _ellipse = grid.GetEllipse();
            _stackPanel = stackPanel;
            _textBlock = textBlock;
            _textBlock.Text = Data.ToString();
        }

        /// <summary> Установить данные вершины</summary>
        /// <param name="data"> Данные </param>
        public override void SetData(string data)
        {
            Data = data;
            _textBlock.Text = data.ToString();
        }

        public void Draw(Canvas canvas, int canvasHeight, int canvasWidth)
        {
            BoundWithinCanvas(canvasHeight, canvasWidth);
            Draw(canvas);
        }

        /// <summary> Рисовать вершину в заданной точке на заданном холсте </summary>
        /// <param name="canvas"> Холст </param>
        /// <param name="point"> Точка </param>
        public void Draw(Canvas canvas, Point point)
        {
            Position = point;
            Draw(canvas);
        }

        /// <summary> Переместить вершину в новую точку </summary>
        /// <param name="point"> Точка </param>
        public void Draw(Point point)
        {
            Position = point;
            Grid.SetCenterEllipseOnGrid(Position);
        }

        /// <summary> Перерисовывает вершину в новую точку на указанный сдвиг </summary>
        /// <param name="diffX"> Разница по оси X </param>
        /// <param name="diffY"> Разница по оси Y </param>
        public void Draw(double diffX, double diffY)
        {
            var newPosition = Position;
            newPosition.X += diffX;
            newPosition.Y += diffY;
            Position = newPosition;
            Grid.SetCenterEllipseOnGrid(Position);
        }

        /// <summary> Отобразить вершину на холсте </summary>
        /// <param name="canvas"> Холст </param>
        private void Draw(Canvas canvas)
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

        public void RemoveDraw(Canvas canvas)
        {
            canvas.Children.Remove(Grid);
        }
    }
}
