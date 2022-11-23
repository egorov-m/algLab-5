using algLab_5.Views.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using Colors = algLab_5.Views.Utils.Colors;

namespace algLab_5.Tools.Base
{
    /// <summary> Базовый класс инструмента </summary>
    public abstract class Tool : IDisposable
    {
        /// <summary> Аргументы инструментов </summary>
        protected ToolArgs _args;
        /// <summary> Количество элементов на холсте </summary>
        protected int _countElementsOnCanvas;
        /// <summary> Элементы вершины под эффектом наведения </summary>
        protected List<VertexElement> HoverVertexElements = new();
        /// <summary> Элемент ребра под эффектом наведения </summary>
        protected List<EdgeElement> HoverEdgeElements = new();
        /// <summary> Число элементов вершин под эффектом наведения </summary>
        protected int _countHoverVertexElement;
        /// <summary> Число элементов рёбер под эффектом наведения</summary>
        protected int _countHoverEdgeElement;
        /// <summary> Цвет эффекта наведения вершин </summary>
        protected Color _hoverEffectVertexColor = Colors.DefaultHoverEffectVertexColor;
        /// <summary> Цвет эффекта наведения рёбер </summary>
        protected Color _hoverEffectEdgeColor = Colors.DefaultHoverEffectEdgeColor;

        protected Tool(ToolArgs args)
        {
            _args = args;
            _countElementsOnCanvas = _args.Canvas.Children.Count;
            _args.CanvasBorder.MouseMove += DefaultMouseMove;
        }

        /// <summary> Обработчик события движения мыши по умолчанию </summary>
        /// <param name="sender"> Объект события </param>
        /// <param name="e"> Событие </param>
        protected void DefaultMouseMove(object sender, MouseEventArgs e)
        {
            (HoverVertexElements, HoverEdgeElements) = GetHoverElements();
            _countHoverVertexElement = HoverVertexElements.Count;
            _countHoverEdgeElement = HoverEdgeElements.Count;
            DisplayHoverEffect(_hoverEffectVertexColor, _hoverEffectEdgeColor);
        }

        /// <summary> Получить выбранные элементы графа </summary>
        protected (List<VertexElement>, List<EdgeElement>) GetHoverElements()
        {
            List<Grid?> selectedShapesVertex = new();
            List<Polyline?> selectedShapesEdge = new();
            List<StackPanel> selectTextBoxEdge = new();
            for (var i = 0; i < _countElementsOnCanvas; i++)
            {
                var shape = _args.Canvas.Children[i];
                if (shape is Grid grid)
                {
                    if (grid.IsMouseOver)
                    {
                        selectedShapesVertex.Add(grid);
                    }
                }
                if (shape is Polyline polyline)
                {
                    if (polyline.IsMouseOver)
                    {
                        selectedShapesEdge.Add(polyline);
                    }
                }
                if (shape is StackPanel stackPanel)
                {
                    if (stackPanel.IsMouseOver)
                    {
                        selectTextBoxEdge.Add(stackPanel);
                    }
                }
            }
            var edgeElements = _args.DataProvider.GetEdgeElementsData().Where(x => selectedShapesEdge.Contains(x.Polyline)).ToHashSet();
            var TextBoxElements = _args.DataProvider.GetEdgeElementsData().Where(x => selectTextBoxEdge.Contains(x.StackPanel));

            foreach (var TextBoxElement in TextBoxElements)
            {
                edgeElements.Add(TextBoxElement);
            }
            return (_args.DataProvider.GetVertexElementsData().Where(x => selectedShapesVertex.Contains(x.Grid)).ToList(),
                edgeElements.ToList());
        }

        /// <summary> Получить информацию о наведённых элементах </summary>
        protected string GetInfoHoverElements()
        {
            if (_countHoverVertexElement > 1)
            {
                return $"{_countHoverVertexElement} {HoverVertexElements[0].GetType().Name}s";
            }

            if (_countHoverEdgeElement > 1)
            {
                return $"{_countHoverEdgeElement} {HoverEdgeElements[0].GetType().Name}s";
            }

            if (_countHoverVertexElement == 1)
            {
                return $"{_countHoverVertexElement} {HoverVertexElements[0].GetType().Name}";
            }

            if (_countHoverEdgeElement == 1)
            {
                return $"{_countHoverEdgeElement} {HoverEdgeElements[0].GetType().Name}";
            }

            return string.Empty;
        }

        /// <summary> Отображение эффекта наведения </summary>
        protected virtual void DisplayHoverEffect(Color colorVertex, Color colorEdge)
        {
            ClearEffects();
            for (var i = 0; i < HoverVertexElements.Count; i++)
            {
                HoverVertexElements[i].Grid.Effect = new DropShadowEffect()
                {
                    Color = colorVertex,
                    ShadowDepth = 0,
                    BlurRadius = 10
                };
                HoverVertexElements[i].Grid.UseLayoutRounding = true;
            }
            for (var i = 0; i < HoverEdgeElements.Count; i++)
            {
                var effect = new DropShadowEffect()
                {
                    Color = colorEdge,
                    ShadowDepth = 0,
                    BlurRadius = 10
                };
                HoverEdgeElements[i].Polyline.Effect = effect;
                HoverEdgeElements[i].Polyline.UseLayoutRounding = true;
            }
        }

        /// <summary> Очистка эффектов </summary>
        protected void ClearEffects()
        {
            for (var i = 0; i < _countElementsOnCanvas; i++)
            {
                var shape = _args.Canvas.Children[i];
                if (shape is Grid or Polyline or TextBox) shape.Effect = null;
            }
        }

        /// <summary> Разгрузка обработчиков события </summary>
        public abstract void Unload();

        /// <summary> Разгрузка обработчика в базовом классе </summary>
        public void Dispose()
        {
            _args.CanvasBorder.MouseMove -= DefaultMouseMove;
        }
    }
}
