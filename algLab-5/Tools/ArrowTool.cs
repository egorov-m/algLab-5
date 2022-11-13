using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using algLab_5.Models;
using algLab_5.Models.Graph;
using algLab_5.Services;
using algLab_5.Tools.Base;

namespace algLab_5.Tools
{
    public class ArrowTool : Tool
    {
        /// <summary> Выбранный элемент </summary>
        private VertexElement? _selectedElement;

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
            if (HoverElements.Count > 0)
            {
                _selectedElement = HoverElements[0];
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
            var pointCursor = e.GetPosition(_args.Canvas);
            var info = GetInfoHoverElements();
            _args.StatusBarUpdater.Update(StatusTool.None, pointCursor, info);

            if (_selectedElement != null && e.LeftButton == MouseButtonState.Pressed)
            {
                _args.SavedChange(StatusSaved.Unsaved);
                _args.StatusBarUpdater.UpdateStatus(StatusTool.SelectingVertex);
                _selectedElement.Draw(pointCursor);

                var (connectionsFromInitial, connectionsFromDestination) = _args.ShapesRepository.GetConnectionsElement(_selectedElement.Grid);
                foreach (var connection in connectionsFromInitial.Where(connection => connection.Item1 != null && connection.Item3 != null))
                {
                    connection.Item1.Points = ConfiguratorViewElement.GetPointCollectionForConnection(_selectedElement.Grid, connection.Item3, connection.Item2);
                }

                foreach (var connection in connectionsFromDestination.Where(connection => connection.Item1 != null && connection.Item3 != null))
                {
                    connection.Item1.Points = ConfiguratorViewElement.GetPointCollectionForConnection(_selectedElement.Grid, connection.Item3, connection.Item2);
                }
            }
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
