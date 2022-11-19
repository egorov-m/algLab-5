using System.Windows.Media;

namespace algLab_5.Views.Utils
{
    public static class Colors
    {
        /// <summary> Внутренний цвет вершины графа </summary>
        public static Color VertexElementInnerColor = Color.FromRgb(111, 111, 111);

        /// <summary> Цвет границы вершины графа </summary>
        public static Color VertexElementBorderColor = Color.FromRgb(214, 214, 214);

        /// <summary> Цвет текста вершины графа </summary>
        public static Color VertexElementTextColor = Color.FromRgb(255, 255, 255);

        /// <summary> Внутренний цвет ребра графа </summary>
        public static Color EdgeElementInnerColor = Color.FromRgb(214, 214, 214);

        /// <summary> Цвет текста ребра графа </summary>
        public static Color EdgeElementTextColor = Color.FromRgb(113, 96, 232);

        /// <summary> Цвет фона элемента контекстного меню </summary>
        public static Color ContextMenuItemBackgroundColor = Color.FromRgb(31, 31, 31);

        /// <summary> Цвет текста элемента контекстного меню </summary>
        public static Color ContextMenuItemForegroundColor = Color.FromRgb(214, 214, 214);

        /// <summary> Цвет границы элемента контекстного меню </summary>
        public static Color ContextMenuItemBorderBrushColor = Color.FromRgb(214, 214, 214);

        /// <summary> Цвет эффекта наведения вершин по умолчанию </summary>
        public static Color DefaultHoverEffectVertexColor = Color.FromRgb(113, 96, 232);

        /// <summary> Цвет эффекта наведения рёбер по умолчанию </summary>
        public static Color DefaultHoverEffectEdgeColor = Color.FromRgb(214, 214, 214);

        public static Color RemoveElementHoverEffectColor = Color.FromRgb(238, 50, 45);
    }
}
