using algLab_5.Models;
using algLab_5.Tools.Base;
using algLab_5.Views.Graph;
using System.Windows;
using System.Windows.Input;

namespace algLab_5.Tools
{
    public class ArrowTool : Tool
    {
        /// <summary> Выбранный элемент </summary>
        private VertexElement? _selectedElement;

        private Point _currentCursorPosition;

        public ArrowTool(ToolArgs args) : base(args)
        {
            _args.CanvasBorder.MouseMove += OnMouseMove;
            _args.CanvasBorder.MouseDown += OnMouseDown;
            _args.CanvasBorder.MouseUp += OnMouseUp;
        }

        /// <summary> Обработчик события нажатия кнопки мыши </summary>
        /// <param name="sender"> Объект события </param>
        /// <param name="e"> Само событие </param>
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (HoverVertexElements.Count > 0)
            {
                _selectedElement = HoverVertexElements[0];
            }
            else
            {
                _selectedElement = null;
            }
        }

        /// <summary> Обработчик события отпускания кнопки мыши </summary>
        /// <param name="sender"> Объект события </param>
        /// <param name="e"> Само событие </param>
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            _selectedElement = null;
        }

        /// <summary> Обработчик события движения мыши </summary>
        /// <param name="sender"> Объект события </param>
        /// <param name="e"> Само событие </param>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var cursorPosition = e.GetPosition(_args.Canvas);
            var info = GetInfoHoverElements();
            _args.StatusBarUpdater.Update(StatusTool.None, cursorPosition, info);

            if (_selectedElement != null && e.LeftButton == MouseButtonState.Pressed)
            {
                _args.SavedChange(StatusSaved.Unsaved);
                _args.StatusBarUpdater.UpdateStatus(StatusTool.SelectingVertex);
                _selectedElement.Draw(cursorPosition);

                var edgeElements = _args.DataProvider.GetEdgeElementsData();
                foreach (var edgeElement in edgeElements)
                {
                    edgeElement.Draw();
                }
            }
            else
            {
                // Имитация движения всего холста
                if (_countHoverEdgeElement + _countHoverVertexElement < 1 && e.LeftButton == MouseButtonState.Pressed)
                {
                    var diffX = cursorPosition.X - _currentCursorPosition.X;
                    var diffY = cursorPosition.Y - _currentCursorPosition.Y;

                    foreach (var vertexElement in _args.DataProvider.GetVertexElementsData())
                    {
                        vertexElement.Draw(diffX, diffY);
                    }

                    foreach (var edgeElement in _args.DataProvider.GetEdgeElementsData())
                    {
                        edgeElement.Draw(diffX, diffY);
                    }
                }
            }

            _currentCursorPosition = cursorPosition;
        }

        /// <summary> Разгрузка обработчиков события </summary>
        public override void Unload()
        {
            _args.CanvasBorder.MouseMove -= OnMouseMove;
            _args.CanvasBorder.MouseDown -= OnMouseDown;
            _args.CanvasBorder.MouseUp -= OnMouseUp;
            Dispose();
        }
    }
}
