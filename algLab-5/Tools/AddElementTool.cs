using algLab_5.Models;
using System.Windows.Input;
using algLab_5.Models.Graph;
using algLab_5.Models.Utils;
using algLab_5.Tools.Base;

namespace algLab_5.Tools
{
    /// <summary> Инструмент добавления элемента </summary>
    public class AddElementTool : Tool
    {
        /// <summary> Выбранный Grid </summary>
        private VertexElement? _element;

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
            _args.SavedChange(StatusSaved.Unsaved);

            if (_element == null)
            {
                var id = IdentifierSetter.GetId();
                _element = new VertexElement(id, id.ToString());
                _element.Draw(_args.Canvas, pointCursor);
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
                _args.DataProvider.AddVertexElement(_element);
            }

            _element = null;
            _args.MainWindow.DisableTool();
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
