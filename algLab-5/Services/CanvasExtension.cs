using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace algLab_5.Services
{
    public static class CanvasExtension
    {
        /// <summary> Индекс главного прямоугольника на Grid'е </summary>
        private const int IndexMainEllipseOnGrid = 0;

        /// <summary> Получение центра эллипса на сетке </summary>
        /// <param name="element"> Сетка </param>
        public static Point GetCenterEllipseOnGrid(this Grid? element)
        {
            var rectangle = (Ellipse)element.Children[IndexMainEllipseOnGrid];
            return new Point(element.Margin.Left + rectangle.Width / 2,
                element.Margin.Top + rectangle.Height / 2);
        }

        /// <summary> Установить центр эллипса на сетке </summary>
        /// <param name="element"> Сетка </param>
        /// <param name="point"> Точка центра эллипса </param>
        public static void SetCenterEllipseOnGrid(this Grid? element, Point point)
        {
            var ellipse = (Ellipse)element.Children[IndexMainEllipseOnGrid];
            element.Margin = new Thickness(point.X - ellipse.Width / 2, point.Y - ellipse.Height / 2, 0, 0);
        }

        /// <summary> Установить координаты текстового блока на холсте </summary>
        /// <param name="textBox"> Текстовый блок </param>
        /// <param name="point"> Точка установки координат </param>
        public static void SetCoordinatesForTextBox(this TextBox textBox, Point point)
        {
            Canvas.SetLeft(textBox, point.X);
            Canvas.SetTop(textBox, point.Y);
        }

        /// <summary> Получить центр прямой линии </summary>
        /// <param name="points"> Точки линии </param>
        public static Point GetDirectPolyLineCenter(this PointCollection points)
        {
            var point1 = points[0];
            var point2 = points[^1];

            var centerPoint = new Point();

            if (point1.X >= point2.X)
            {
                centerPoint.X = point2.X + (point1.X - point2.X) / 2;
            }
            else
            {
                centerPoint.X = point1.X + (point2.X - point1.X) / 2;
            }

            if (point1.Y >= point2.Y)
            {
                centerPoint.Y = point2.Y + (point1.Y - point2.Y) / 2;
            }
            else
            {
                centerPoint.Y = point1.Y + (point2.Y - point1.Y) / 2;
            }

            return centerPoint;
        }

        /// <summary> Получить центр прямой линии </summary>
        /// <param name="polyline"> Линия </param>
        public static Point GetDirectPolyLineCenter(this Polyline polyline) => polyline.Points.GetDirectPolyLineCenter();
    }
}
