using System.Collections.Generic;
using algLab_5.Models;
using algLab_5.Tools.Base;
using System.Windows.Input;
using algLab_5.Views.Graph;
using Colors = algLab_5.Views.Utils.Colors;

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
                var edgeElementsData = _args.DataProvider.GetEdgeElementsData();
                var listForRemove = new List<EdgeElement?>();
                foreach (var removableEdge in element.EdgesList)
                {
                    if (removableEdge is EdgeElement removableEdgeElement)
                    {
                        removableEdgeElement.RemoveDraw(_args.Canvas);
                        listForRemove.Add(removableEdgeElement);

                        _args.Logger.Info($"Ребро \"{removableEdgeElement.TextBox.Text}\" входящее/исходящее в/из вершины \"{element.TextBox.Text}\" успешно удалено.");
                    }
                }

                listForRemove.ForEach(x =>
                {
                    x.InitialVertexElement.EdgesList.Remove(x);
                    x.DestinationVertexElement?.EdgesList.Remove(x);
                    edgeElementsData.Remove(x);
                });

                element.RemoveDraw(_args.Canvas);
                _args.DataProvider.GetVertexElementsData().Remove(element);

                _args.Logger.Info($"Вершина \"{element.TextBox.Text}\" успешно удалена.");

                _args.SavedChange(StatusSaved.Unsaved);
                _args.MainWindow.DisableTool();
                return;
            }

            if (_countHoverEdgeElement == 1)
            {
                var element = HoverEdgeElements[0];

                element.InitialVertexElement.EdgesList.Remove(element);
                element.DestinationVertexElement?.EdgesList.Remove(element);

                element.RemoveDraw(_args.Canvas);
                _args.DataProvider.GetEdgeElementsData().Remove(element);

                _args.Logger.Info($"Ребро \"{element.TextBox.Text}\" между вершинами \"{element.InitialVertexElement.TextBox.Text}\" и \"{element.DestinationVertexElement?.TextBox.Text}\" успешно удалено.");

                _args.SavedChange(StatusSaved.Unsaved);
                _args.MainWindow.DisableTool();
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
