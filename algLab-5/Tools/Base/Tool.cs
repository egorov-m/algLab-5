using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Windows.Media;
using algLab_5.Models.Graph;

namespace algLab_5.Tools.Base
{
    /// <summary> Базовый класс инструмента </summary>
    public abstract class Tool : IDisposable
    {
        /// <summary> Аргументы инструментов </summary>
        protected ToolArgs _args;
        /// <summary> Количество элементов на холсте </summary>
        protected int _countElementsOnCanvas;
        /// <summary> Элементы под эффектом наведения </summary>
        protected List<VertexElement> HoverElements = new ();
        /// <summary> Число элементов под эффектом наведения </summary>
        protected int _countHoverElement;

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
            HoverElements = GetHoverElements();
            _countHoverElement = HoverElements.Count;
            DisplayHoverEffect();
        }

        /// <summary>
        /// Получить выбранные сетки
        /// </summary>
        /// <returns> список выбранных гридов </returns>
        protected List<VertexElement> GetHoverElements()
        {
            List<Grid?> selectedShapes = new ();
            for (var i = 0; i < _countElementsOnCanvas; i++)
            {
                var shape = _args.Canvas.Children[i];
                if (shape is Grid grid)
                {
                    if (grid.IsMouseOver)
                    {
                        selectedShapes.Add(grid);
                    }
                }
            }

            return _args.DataProvider.GetVertexElementstData().Where(x => selectedShapes.Contains(x.Grid)).ToList();
        }

        /// <summary> Получить информацию о наведённых элементах </summary>
        protected string GetInfoHoverElements()
        {
            if (_countHoverElement > 1)
            {
                return $"{_countHoverElement} {HoverElements[0]?.GetType().Name + 's'}";
            }
            else
            {
                if (_countHoverElement > 0)
                {
                    return $"{_countHoverElement} {HoverElements[0]?.GetType().Name}";
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary> Отображение эффекта наведения </summary>
        protected void DisplayHoverEffect()
        {
            ClearEffects();
            for (var i = 0; i < _countHoverElement; i++)
            {
                HoverElements[i]!.Grid.Effect =  new DropShadowEffect()
                {
                    Color = Color.FromRgb(113, 96, 232), // #7160e8
                    ShadowDepth = 0,
                    BlurRadius = 10
                };
                HoverElements[i]!.Grid.UseLayoutRounding = true;
            }
        }

        /// <summary> Очистка эффектов </summary>
        protected void ClearEffects()
        {
            for (var i = 0; i < _countElementsOnCanvas; i++)
            {
                var shape = _args.Canvas.Children[i];
                if (shape is Grid) shape.Effect = null;
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
