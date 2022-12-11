using algLab_5.Models;
using algLab_5.Models.Utils;
using algLab_5.Tools.Base;
using algLab_5.Views.Graph;
using System.Windows.Input;

namespace algLab_5.Tools
{
    /// <summary> Инструмент добавления элемента </summary>
    public class AddElementTool : Tool
    {
        /// <summary> Выбранный Grid </summary>
        private VertexElement? _element;

        /// <summary> В процессе ли добавление вершины </summary>
        private bool _isProcess;

        public AddElementTool(ToolArgs args) : base(args)
        {
            _args.CanvasBorder.MouseMove += OnMouseMove;
            _args.CanvasBorder.MouseDown += OnMouseDown;

            _args.StatusBarUpdater.UpdateStatus(StatusTool.NewVertex);
        }

        /// <summary> Обработчик события движения мыши </summary>
        /// <param name="sender"> Объект события </param>
        /// <param name="e"> Само событие </param>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var pointCursor = e.GetPosition(_args.Canvas);
            var info = GetInfoHoverElements();

            _args.StatusBarUpdater.Update(StatusTool.NewVertex, pointCursor, info);

            if (_element == null)
            {
                var id = IdentifierSetter.GetId();
                _element = new VertexElement(id, id.ToString());
                _element.Draw(_args.Canvas, pointCursor);
                _isProcess = true;
            }
            else
            {
                _element.Draw(pointCursor);
            }
        }

        /// <summary> Обработчик события нажатия кнопки мыши </summary>
        /// <param name="sender"> Объект события </param>
        /// <param name="e"> Само событие </param>
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (_element != null)
            {
                _isProcess = false;
                _args.DataProvider.AddVertexElement(_element);
                _args.SavedChange(StatusSaved.Unsaved);
                _args.Logger.Info($"Вершина \"{_element.TextBox.Text}\" добавлена и установлена на холст.");
            }

            _element = null;
            _args.MainWindow.DisableTool();
        }

        /// <summary> Разгрузка обработчиков события </summary>
        public override void Unload()
        {
            if (_isProcess) _element?.RemoveDraw(_args.Canvas);
            _args.CanvasBorder.MouseMove -= OnMouseMove;
            _args.CanvasBorder.MouseDown -= OnMouseDown;
            Dispose();
        }
    }
}
