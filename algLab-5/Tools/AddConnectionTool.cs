using algLab_5.Models;
using algLab_5.Tools.Base;
using algLab_5.Views.Graph;
using System;
using System.Windows.Input;
using algLab_5.Models.Graph;

namespace algLab_5.Tools
{
    public class AddConnectionTool : Tool
    {
        /// <summary> Элемент добавляемого ребра графа </summary>
        private Edge? _edgeElement;

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
                if (_countHoverVertexElement == 1)
                {
                    if (_edgeElement != null)
                    {
                        if (_edgeElement.InitialVertex == HoverVertexElements[0])
                        {
                            _isProcess = false;
                            _edgeElement.RemoveDraw(_args.Canvas);
                            _edgeElement = null;
                            _args.MainWindow.DisableTool();
                            throw new Exception("ОШИБКА! Ребро можно создать только между разными вершинами.");
                        }

                        _edgeElement.DestinationVertex = HoverVertexElements[0];
                        _edgeElement.Draw();

                        if (_edgeElement.DestinationVertex != null)
                        {
                            if (!_args.DataProvider.AddEdgeElement(_edgeElement))
                            {
                                throw new ArgumentException("ОШИБКА! Нельзя добавить уже существующее ребро.");
                            }

                            _edgeElement.InitialVertex.EdgesList.Add(_edgeElement);
                            _edgeElement.DestinationVertex.EdgesList.Add(_edgeElement);

                            _args.Logger.Info($"Ребро между вершинами \"{_edgeElement.InitialVertex.TextBox.Text}\" и \"{_edgeElement.DestinationVertex.TextBox.Text}\" добавлено.");

                            _args.SavedChange(StatusSaved.Unsaved);
                            _isProcess = false;
                            _args.MainWindow.DisableTool();
                        }
                    }
                }
            }
            else
            {
                if (_countHoverVertexElement == 1)
                {
                    _edgeElement = new EdgeElement(HoverVertexElements[0], HoverVertexElements[0], 0);
                    var pointCursor = e.GetPosition(_args.Canvas);
                    _edgeElement?.Draw(_args.Canvas, pointCursor);
                    _isProcess = true;
                }
            }
        }

        /// <summary> Разгрузка обработчиков события </summary>
        public override void Unload()
        {
            if (_isProcess) _edgeElement?.RemoveDraw(_args.Canvas);
            _args.CanvasBorder.MouseMove -= OnMouseMove;
            _args.CanvasBorder.MouseDown -= OnMouseDown;
            Dispose();
        }
    }
}
