using System;
using algLab_5.Models;
using algLab_5.Tools.Base;
using System.Windows.Input;
using algLab_5.Models.Graph;

namespace algLab_5.Tools
{
    public class AddConnectionTool : Tool
    {
        private EdgeElement? _edgeElement;

        /// <summary> В процессе ли добавление связи </summary>
        private bool _isProcess;
        /// <summary> Тип связи </summary>
        private readonly ConnectionType _connectionType;

        public AddConnectionTool(ToolArgs args, ConnectionType connectionType) : base(args)
        {
            _args.CanvasBorder.MouseDown += OnMouseDown;
            _args.CanvasBorder.MouseMove += OnMouseMove;
            _connectionType = connectionType;
        }

        /// <summary> Обработчик события движения мыши </summary>
        /// <param name="sender"> Объект события </param>
        /// <param name="e"> Само событие </param>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var pointCursor = e.GetPosition(_args.Canvas);
            var info = GetInfoHoverElements();
            _args.StatusBarUpdater.Update(StatusTool.NewEdge, pointCursor, info);
            if (_isProcess)
            {
                _edgeElement?.Draw(pointCursor);
            }
        }

        /// <summary> Обработчик события нажатия кнопки мыши </summary>
        /// <param name="sender"> Объект события </param>
        /// <param name="e"> Само событие</param>
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (_isProcess)
            {
                if (HoverElements.Count == 1)
                {
                    if (_edgeElement != null)
                    {
                        if (_edgeElement.InitialVertexElement == HoverElements[0])
                        {
                            _isProcess = false;
                            _edgeElement.RemoveDraw(_args.Canvas);
                            _edgeElement = null;
                            _args.MainWindow.DisableTool();
                            throw new Exception("ОШИБКА! Ребро можно создать только между разными вершинами.");
                        }

                        _edgeElement.DestinationVertexElement = HoverElements[0];
                        _edgeElement.Draw();

                        if (_edgeElement.DestinationVertexElement != null)
                        {
                            if (!_args.DataProvider.AddEdgeElement(_edgeElement))
                            {
                                throw new ArgumentException("ОШИБКА! Нельзя добавить уже существующее ребро.");
                            }

                            _args.SavedChange(StatusSaved.Unsaved);
                            _args.MainWindow.DisableTool();
                            _isProcess = false;
                            _args.DataProvider.AddEdgeElement(_edgeElement);
                        }
                    }
                }
            }
            else
            {
                if (HoverElements.Count == 1)
                {
                    _edgeElement = new EdgeElement(HoverElements[0], HoverElements[0].Id, 0);
                    var pointCursor = e.GetPosition(_args.Canvas);
                    _edgeElement?.Draw(_args.Canvas, pointCursor);
                    _isProcess = true;
                }
            }
        }

        /// <summary> Разгрузка обработчиков события </summary>
        public override void Unload()
        {
            _args.CanvasBorder.MouseMove -= OnMouseMove;
            _args.CanvasBorder.MouseDown -= OnMouseDown;
            Dispose();
        }
    }
}
