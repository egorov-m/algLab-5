using algLab_5.Models;
using algLab_5.Tools.Base;
using System.Windows.Input;
using algLab_5.Views.Graph;
using System.Windows.Media;

namespace algLab_5.Tools
{
    public class EditDataTool : Tool
    {
        /// <summary> В процессе ли редактирование графа </summary>
        private bool _isProcess;

        /// <summary> Выбранный для редактирования элемент вершины </summary>
        private VertexElement? _selectedVertexElement;

        /// <summary> Выбранный для редактирования элемент ребра </summary>
        private EdgeElement? _selectedEdgeElement;

        public EditDataTool(ToolArgs args) : base(args)
        {
            _args.CanvasBorder.MouseDown += OnMouseDown;
            _args.CanvasBorder.MouseMove += OnMouseMove;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var pointCursor = e.GetPosition(_args.Canvas);
            var info = GetInfoHoverElements();
            _args.StatusBarUpdater.Update(StatusTool.EditData, pointCursor, info);
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (!_isProcess)
            {
                if (_countHoverVertexElement == 1)
                {
                    _isProcess = true;
                    HoverVertexElements[0].TextBox.IsEnabled = true;
                    _selectedVertexElement = HoverVertexElements[0];
                }
                else
                {
                    if (_countHoverEdgeElement == 1)
                    {
                        _isProcess = true;
                        HoverEdgeElements[0].TextBox.IsReadOnly = false;
                        HoverEdgeElements[0].TextBox.SelectionBrush = null;
                        HoverEdgeElements[0].TextBox.Cursor = null;
                        _selectedEdgeElement = HoverEdgeElements[0];
                    }
                }
            }
            else
            {
                if (_countHoverEdgeElement + _countHoverVertexElement < 1)
                {
                    _isProcess = false;

                    if (_selectedVertexElement != null)
                    {
                        _selectedVertexElement.SetData(_selectedVertexElement.TextBox.Text);

                        _selectedVertexElement.TextBox.IsEnabled = false;
                    }

                    if (_selectedEdgeElement != null)
                    {
                        _selectedEdgeElement.SetWeight(_selectedEdgeElement.TextBox.Text);

                        _selectedEdgeElement.TextBox.IsReadOnly = true;
                        _selectedEdgeElement.TextBox.SelectionBrush = Brushes.Transparent;
                        _selectedEdgeElement.TextBox.Cursor = Cursors.Arrow;
                    }

                    _args.SavedChange(StatusSaved.Unsaved);
                    _args.MainWindow.DisableTool();
                }
            }
            
        }

        public override void Unload()
        {
            _args.CanvasBorder.MouseMove -= OnMouseMove;
            _args.CanvasBorder.MouseDown -= OnMouseDown;
            Dispose();
        }
    }
}
