using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace algLab_5.Models.Utils
{
    public static class Colors
    {
        /// <summary> Внутренний цвет вершины графа </summary>
        public static Color VertexElementInnerColor = Color.FromRgb(111, 111, 111);

        /// <summary> Цвет границы вершины графа </summary>
        public static Color VertexElementBorderColor = Color.FromRgb(214, 214, 214);

        /// <summary> Цвет текста вершины графа </summary>
        public static Color VertexElementTextColor = Color.FromRgb(214, 214, 214);

        /// <summary> Внутренний цвет ребра графа </summary>
        public static Color EdgeElementInnerColor = Color.FromRgb(214, 214, 214);

        /// <summary> Цвет текста ребра графа </summary>
        public static Color EdgeElementTextColor = Color.FromRgb(113, 96, 232);

    }
}
