using algLab_5.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace algLab_5.Services
{
    /// <summary> Класс для получения настроенных визуальных элементов </summary>
    public static class ConfiguratorViewElement
    {
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
            return new PointCollection() { point, new (point.X, point.Y), new (pointGrid.X, pointGrid.Y), pointGrid };
        }
    }
}
