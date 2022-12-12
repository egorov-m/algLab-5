using System.Windows;
using algLab_5.Models;
using algLab_5.Tools.Base;
using System.Windows.Input;
using System;
using algLab_5.Models.Graph;

namespace algLab_5.Tools
{
    public class EditDataTool : Tool
    {
        /// <summary> В процессе ли редактирование графа </summary>
        private bool _isProcess;

        /// <summary> Выбранный для редактирования элемент вершины </summary>
        private Vertex? _selectedVertexElement;

        /// <summary> Выбранный для редактирования элемент ребра </summary>
        private Edge? _selectedEdgeElement;

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
                    HoverVertexElements[0].TextBox.BorderThickness = new Thickness(1, 1, 1, 1);
                    _selectedVertexElement = HoverVertexElements[0];
                }
                else
                {
                    if (_countHoverEdgeElement == 1)
                    {
                        _isProcess = true;
                        HoverEdgeElements[0]!.TextBox.IsEnabled = true;
                        HoverEdgeElements[0]!.TextBox.BorderThickness = new Thickness(1, 1, 1, 1);

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
                        if (!_selectedVertexElement.SetData(_selectedVertexElement.TextBox.Text, _args.DataProvider.GetVertexElementsData()))
                        {
                            _args.MainWindow.DisableTool();
                            throw new ArgumentException("ОШИБКА! Вершины графа должны отличаться.");
                        }

                        _args.Logger.Info($"Вершина \"{_selectedVertexElement.TextBox.Text}\" успешно отредактирована.");
                    }

                    if (_selectedEdgeElement != null)
                    {
                        if (!_selectedEdgeElement.SetWeight(_selectedEdgeElement.TextBox.Text))
                        {
                            _args.MainWindow.DisableTool();
                            throw new ArgumentException("ОШИБКА! Вес вершины должен быть представлен как целое неотрицательное число.");
                        }

                        _args.Logger.Info($"Вес ребра \"{_selectedEdgeElement.TextBox.Text}\" успешно отредактирован.");
                    }

                    _args.SavedChange(StatusSaved.Unsaved);
                    _args.MainWindow.DisableTool();
                }
            }
        }

        public override void Unload()
        {
            if (_selectedVertexElement != null)
            {
                _selectedVertexElement.TextBox.IsEnabled = false;
                _selectedVertexElement.TextBox.BorderThickness = new Thickness(0, 0, 0, 0);
            }

            if (_selectedEdgeElement != null)
            {
                _selectedEdgeElement.TextBox.IsEnabled = false;
                _selectedEdgeElement.TextBox.BorderThickness = new Thickness(0, 0, 0, 0);
            }

            Keyboard.ClearFocus();

            _args.CanvasBorder.MouseMove -= OnMouseMove;
            _args.CanvasBorder.MouseDown -= OnMouseDown;
            Dispose();
        }
    }
}
