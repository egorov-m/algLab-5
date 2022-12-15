using System.Windows.Media;

namespace algLab_5.Views.Utils
{
    public static class Colors
    {
        /// <summary> Внутренний цвет вершины графа </summary>
        public static Color VertexElementInnerColor { get; } = Color.FromRgb(111, 111, 111);

        /// <summary> Цвет границы вершины графа </summary>
        public static Color VertexElementBorderColor { get; } = Color.FromRgb(214, 214, 214);

        /// <summary> Цвет текста вершины графа </summary>
        public static Color VertexElementTextColor { get; } = Color.FromRgb(255, 255, 255);

        /// <summary> Внутренний цвет ребра графа </summary>
        public static Color EdgeElementInnerColor { get; } = Color.FromRgb(214, 214, 214);

        /// <summary> Цвет текста ребра графа </summary>
        public static Color EdgeElementTextColor { get; } = Color.FromRgb(113, 96, 232);

        /// <summary> Цвет фона элемента контекстного меню </summary>
        public static Color ContextMenuItemBackgroundColor { get; } = Color.FromRgb(31, 31, 31);

        /// <summary> Цвет текста элемента контекстного меню </summary>
        public static Color ContextMenuItemForegroundColor { get; } = Color.FromRgb(214, 214, 214);

        /// <summary> Цвет границы элемента контекстного меню </summary>
        public static Color ContextMenuItemBorderBrushColor { get; } = Color.FromRgb(214, 214, 214);

        /// <summary> Цвет эффекта наведения вершин по умолчанию </summary>
        public static Color DefaultHoverEffectVertexColor { get; } = Color.FromRgb(113, 96, 232);

        /// <summary> Цвет эффекта наведения рёбер по умолчанию </summary>
        public static Color DefaultHoverEffectEdgeColor { get; } = Color.FromRgb(214, 214, 214);

        /// <summary> Цвет эффекта наведения удаляемого элемента </summary>
        public static Color RemoveElementHoverEffectColor { get; } = Color.FromRgb(238, 50, 45);

        /// <summary> Цвет посещённого элемента </summary>
        public static Color VisitedElementColor { get; } = Color.FromRgb(134, 27, 45);

        public static Color CurrentElementColor { get; } = Color.FromRgb(27, 72, 134);

        /// <summary> Цвет выделения элементов выбранных для алгоритма </summary>
        public static Color SelectedForAlgVertexElement = Color.FromRgb(85, 177, 85);
    }
}
