using System.Collections.Generic;
using System.Linq;
using algLab_5.Models;
using algLab_5.Tools.Base;
using System.Windows.Input;
using algLab_5.Models.Graph;
using Colors = algLab_5.Models.Utils.Colors;

namespace algLab_5.Tools
{
    /// <summary> Инструмент удаления элементов </summary>
    public class RemoveElementTool : Tool
    {
        public RemoveElementTool(ToolArgs args) : base(args)
        {
            _hoverEffectVertexColor = Colors.RemoveElementHoverEffectColor;
            _hoverEffectEdgeColor = Colors.RemoveElementHoverEffectColor;
            _args.CanvasBorder.MouseUp += OnMouseUp;
            _args.CanvasBorder.MouseMove += OnMouseMove;
        }

        /// <summary> Обработчик события движения мыши </summary>
        /// <param name="sender"> Объект события </param>
        /// <param name="e"> Само событие </param>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var pointCursor = e.GetPosition(_args.Canvas);
            var info = GetInfoHoverElements();
            _args.StatusBarUpdater.Update(StatusTool.RemoveElement, pointCursor, info);
        }

        /// <summary> Обработчик события подъёма кнопки мыши </summary>
        /// <param name="sender"> Объект события </param>
        /// <param name="e"> Само событие</param>
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (_countHoverVertexElement == 1)
            {
                var element = HoverVertexElements[0];
                var removableElements = _args.DataProvider.GetEdgeElementsData()
                    .Where(edgeElement => edgeElement.InitialVertexElement == element || edgeElement.DestinationVertexElement == element).ToList();

                var edgeElementsData = _args.DataProvider.GetEdgeElementsData();
                foreach (var removableElement in removableElements)
                {
                    removableElement.RemoveDraw(_args.Canvas);
                    edgeElementsData.Remove(removableElement);
                }

                element.RemoveDraw(_args.Canvas);
                _args.DataProvider.GetVertexElementsData().Remove(element);
                _args.SavedChange(StatusSaved.Unsaved);
                _args.MainWindow.DisableTool();
                return;
            }

            if (_countHoverEdgeElement == 1)
            {
                var element = HoverEdgeElements[0];
                element.RemoveDraw(_args.Canvas);
                _args.DataProvider.GetEdgeElementsData().Remove(element);
                _args.SavedChange(StatusSaved.Unsaved);
                _args.MainWindow.DisableTool();
                return;
            }
        }

        /// <summary> Разгрузка обработчиков события </summary>
        public override void Unload()
        {
            _args.CanvasBorder.MouseMove -= OnMouseMove;
            _args.CanvasBorder.MouseUp -= OnMouseUp;
            Dispose();
        }
    }
}
