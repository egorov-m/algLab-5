using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Windows.Media;

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
        protected List<Grid?> _hoverElements = new ();
        /// <summary> Число элементов под эффектом наведения </summary>
        protected int _countHoverElement;

        protected Tool(ToolArgs args)
        {
            _args = args;
            _countElementsOnCanvas = _args.Canvas.Children.Count;
            _args.Canvas.MouseMove += DefaultMouseMove;
        }

        /// <summary> Обработчик события движения мыши по умолчанию </summary>
        /// <param name="sender"> Объект события </param>
        /// <param name="e"> Событие </param>
        protected void DefaultMouseMove(object sender, MouseEventArgs e)
        {
            _hoverElements = GetHoverElements();
            _countHoverElement = _hoverElements.Count;
            DisplayHoverEffect();
        }

        /// <summary>
        /// Получить выбранные сетки
        /// </summary>
        /// <returns> список выбранных гридов </returns>
        protected List<Grid?> GetHoverElements()
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
            return selectedShapes;
        }

        /// <summary> Получить информацию о наведённых элементах </summary>
        protected string GetInfoHoverElements()
        {
            if (_countHoverElement > 1)
            {
                return $"{_countHoverElement} {_hoverElements[0]?.GetType().Name + 's'}";
            }
            else
            {
                if (_countHoverElement > 0)
                {
                    return $"{_countHoverElement} {_hoverElements[0]?.GetType().Name}";
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
                _hoverElements[i]!.Effect =  new DropShadowEffect()
                {
                    Color = Color.FromRgb(113, 96, 232), // #7160e8
                    ShadowDepth = 0,
                    BlurRadius = 10
                };
                _hoverElements[i]!.UseLayoutRounding = true;
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
            _args.Canvas.MouseMove -= DefaultMouseMove;
        }
    }
}
