using System.Windows.Controls;
using System.Windows;
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
            var rectangle = (Ellipse)element.Children[IndexMainEllipseOnGrid];
            element.Margin = new Thickness(point.X - rectangle.Width / 2, point.Y - rectangle.Height / 2, 0, 0);
        }
    }
}
