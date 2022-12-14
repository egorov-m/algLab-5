using algLab_5.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Colors = algLab_5.Views.Utils.Colors;
using algLab_5.Views.Utils;

namespace algLab_5.Services
{
    /// <summary> Класс для получения настроенных визуальных элементов </summary>
    public static class ConfiguratorViewElement
    {
        /// <summary> Индекс эллипса на сетки элемента вершины </summary>
        public static int IndexEllipseOnGridVertexElement = 0;

        /// <summary> Получить элемент контекстного меню </summary>
        /// <param name="text"> Текст заголовка </param>
        private static MenuItem GetMenuItem(string text)
        {
            return new MenuItem()
            {
                Header = text,
                Background = new SolidColorBrush(Colors.ContextMenuItemBackgroundColor),
                Foreground = new SolidColorBrush(Colors.ContextMenuItemForegroundColor),
                BorderBrush = new SolidColorBrush(Colors.ContextMenuItemBorderBrushColor),
                BorderThickness = new Thickness(1, 1, 1, 1)
            };
        }

        /// <summary> Получить Grid с добавленным в него эллипса для элемента вершины графа </summary>
        public static Grid GetGridVertexElement()
        {
            var element = new Grid();
            var ellipse = new Ellipse()
            {
                Width = Params.SizeVertexElement,
                Height = Params.SizeVertexElement,
                StrokeThickness = Params.BorderWidthVertexElement,
                Stroke = new SolidColorBrush(Colors.VertexElementBorderColor),
                Fill = new SolidColorBrush(Colors.VertexElementInnerColor)
            };
                
            //element.ContextMenu = ContextMenu;

            element.Children.Add(ellipse);

            return element;
        }

        /// <summary> Получить StackPanel для элемента вершины графа </summary>
        public static StackPanel GetStackPanelVertexElement()
        {
            var stackPanel = new StackPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            return stackPanel;
        }

        /// <summary> Получить StackPanel для элемента ребра графа </summary>
        public static StackPanel GetStackPanelEdgeElement()
        {
            var stackPanel = new StackPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            return stackPanel;
        }

        /// <summary> Получить TextBox для элемента вершины графа </summary>
        public static TextBox GetTextBoxVertexElement()
        {
            var textBox = new TextBox()
            {
                IsEnabled = false,
                CaretBrush = new SolidColorBrush(Colors.VertexElementTextColor),
                Background = Brushes.Transparent,
                BorderBrush = new SolidColorBrush(Colors.VertexElementTextColor),
                BorderThickness = new Thickness(0, 0, 0, 0),
                FontSize = Params.FontSizeVertexElement,
                FontWeight = FontWeight.FromOpenTypeWeight(Params.FontWeightVertexElement),
                Foreground = new SolidColorBrush(Colors.VertexElementTextColor),
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center,
            };

            return textBox;
        }

        /// <summary> Получить эллипс элемента вершины графа из сетки вершины </summary>
        /// <param name="grid"> Сетка </param>
        public static Ellipse GetEllipse(this Grid grid) => grid.Children[IndexEllipseOnGridVertexElement] as Ellipse ?? throw new ArgumentNullException("ОШИБКА! Сетка не соответствует схеме.");

        /// <summary> Получить коллекцию точек для соединения </summary>
        /// <param name="gridOne"> Первый Grid </param>
        /// <param name="gridTwo"> Второй Grid </param>
        /// <param name="connectionType"> Тип соединения </param>
        public static PointCollection GetPointCollectionForConnection(Grid gridOne, Grid gridTwo, ConnectionType connectionType)
        {
            var point = gridOne.GetCenterEllipseOnGrid();
            //switch (connectionType)
            //{
            //    case ConnectionType.ParentChild:
            //        point = !isGridTwoDestination ? gridOne.GetCenterTopSideRectangleOnGrid() : gridOne.GetCenterBottomSideRectangleOnGrid();
            //        break;
            //    case (ConnectionType.CurrentSpouses or ConnectionType.FormerSpouses):
            //        point = point.X >= gridTwo.GetCenterRectangleOnGrid().X ? gridOne.GetCenterLeftRectangleOnGrid() : gridOne.GetCenterRightRectangleOnGrid();
            //        break;
            //}
            return GetPointCollectionForConnection(point, gridTwo, connectionType);
        }

        /// <summary> Получить набор точек создания линии связи </summary>
        /// <param name="point"> Точка (начало/ конец) </param>
        /// <param name="grid"> Grid (начало/ конец) </param>
        /// <param name="connectionType"> Тип родственного соединения</param>
        public static PointCollection GetPointCollectionForConnection(Point point, Grid? grid, ConnectionType connectionType)
        {
            Point pointGrid;

            //switch (connectionType)
            //{
            //    case ConnectionType.ParentChild:
            //        pointGrid = isGridDestination ? grid.GetCenterTopSideRectangleOnGrid() : grid.GetCenterBottomSideRectangleOnGrid();
            //        var transformation = (isGridDestination ? 1 : -1);
            //        bendingLength = !isGridDestination ? (point.Y - pointGrid.Y) / 2 : (pointGrid.Y - point.Y) / 2;
            //        double additionalBendingLength;
            //        if (bendingLength < 0)
            //        {
            //            bendingLength = 10 * transformation;
            //            var isAdditionalSide = point.X >= pointGrid.X;
            //            additionalBendingLength = Math.Abs(point.X - pointGrid.X) / 2 * (isAdditionalSide ? 1 : -1);
            //            return new PointCollection() { point, new (point.X, point.Y + bendingLength), new (point.X - additionalBendingLength, point.Y + bendingLength), new (pointGrid.X + additionalBendingLength, pointGrid.Y - bendingLength), new (pointGrid.X, pointGrid.Y - bendingLength), pointGrid };
            //        }
            //        else
            //        {
            //            bendingLength *= transformation;
            //            return new PointCollection() { point, new (point.X, point.Y + bendingLength), new (pointGrid.X, pointGrid.Y - bendingLength), pointGrid };
            //        }
            //    case (ConnectionType.CurrentSpouses or ConnectionType.FormerSpouses):
            //    {
            //        pointGrid = grid.GetCenterEllipseOnGrid();
            //        var isSide = point.X >= pointGrid.X;
            //        pointGrid = pointGrid.X >= point.X ? grid.GetCenterLeftRectangleOnGrid() : grid.GetCenterRightRectangleOnGrid();
            //        bendingLength = Math.Abs(point.X - pointGrid.X) / 2 * (isSide ?  -1 : 1);
            //        return new PointCollection() { point, new (point.X + bendingLength, point.Y), new (pointGrid.X - bendingLength, pointGrid.Y), pointGrid };
            //    }
            //    default:
            //        throw new ArgumentException("ОШИБКА! Недопустимы тип родственной связи.");
            //}

            pointGrid = grid.GetCenterEllipseOnGrid();
            return new PointCollection() { point, pointGrid };
        }

        /// <summary> Получить набор точек создания линии связи </summary>
        /// <param name="point1"> Точка 1 </param>
        /// <param name="point2"> Точка 2 </param>
        /// <param name="connectionType"> тип соединения </param>
        public static PointCollection GetPointCollectionForConnection(Point point1, Point point2, ConnectionType connectionType)
        {
            return new PointCollection() { point1,  point2 };
        }

        /// <summary> Получить линию связи в соответствии с типом </summary>
        /// <param name="connectionType"> Тип связи </param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Polyline GetPolylineEdgeElement(ConnectionType connectionType = ConnectionType.Default)
        {
            return connectionType switch
            {
                ConnectionType.Default => new Polyline() {Stroke = new SolidColorBrush(Colors.EdgeElementInnerColor)},
                _ => throw new ArgumentException("ОШИБКА! Недопустимый тип связи.")
            };
        }

        /// <summary> Получить текстовый блок Вершины элемента </summary>
        public static TextBox GetTextBoxEdgeElement()
        {
            var textBox = new TextBox()
            {
                IsEnabled = false,
                CaretBrush = new SolidColorBrush(Colors.EdgeElementTextColor),
                Background = Brushes.Transparent,
                BorderBrush = new SolidColorBrush(Colors.EdgeElementTextColor),
                BorderThickness = new Thickness(0, 0, 0, 0),
                FontSize = Params.FontSizeEdgeElement,
                FontWeight = FontWeight.FromOpenTypeWeight(Params.FontWeightEdgeElement),
                Foreground = new SolidColorBrush(Colors.EdgeElementTextColor),
            };

            return textBox;
        }
    }
}
