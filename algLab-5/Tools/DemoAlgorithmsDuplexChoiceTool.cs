using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using algLab_5.Algorithms;
using algLab_5.Models;
using algLab_5.Services;
using algLab_5.Services.Logger;
using algLab_5.Tools.Base;
using algLab_5.Views.Graph;
using Microsoft.VisualBasic;
using Colors = algLab_5.Views.Utils.Colors;

namespace algLab_5.Tools
{
    public class DemoAlgorithmsDuplexChoiceTool : Tool
    {
        /// <summary> Начальный элемент </summary>
        private VertexElement? _sourceElement;

        /// <summary> Конечный элемент </summary>
        private VertexElement? _destElement;

        /// <summary> Выбранный элемент </summary>
        private VertexElement? _selectedElement;

        /// <summary> Текущее положение курсора на холсте </summary>
        private Point _currentCursorPosition;

        /// <summary> Тип демонстрируемого алгоритма </summary>
        private readonly StatusTool _algType;

        /// <summary> В процессе ли выполнения алгоритм </summary>
        private bool _isProcess;

        public DemoAlgorithmsDuplexChoiceTool(ToolArgs args, StatusTool algType) : base(args)
        {
            _args.CanvasBorder.MouseDown += OnMouseDown;
            _args.CanvasBorder.MouseMove += OnMouseMove;
            _algType = algType;

            _args.Logger.Info("Выберите начальную вершину для демонстрации алгоритма.");
        }

        /// <summary> Обработчик события движения мыши </summary>
        /// <param name="sender"> Объект события </param>
        /// <param name="e"> Само событие </param>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var cursorPosition = e.GetPosition(_args.Canvas);
            var info = GetInfoHoverElements();
            _args.StatusBarUpdater.Update(_algType, cursorPosition, info);

            if (_isProcess)
            {
                if (ControlPanelProvider.IsReset)
                {
                    ControlPanelProvider.IsReset = false;
                    _isProcess = false;
                    _args.MainWindow.DisableTool();
                    return;
                }
                if (_selectedElement != null && e.LeftButton == MouseButtonState.Pressed)
                {
                    //_args.SavedChange(StatusSaved.Unsaved);
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
        }

        /// <summary> Обработчик события нажатия кнопки мыши </summary>
        /// <param name="sender"> Объект события </param>
        /// <param name="e"> Само событие</param>
        private async void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (_isProcess)
            {
                _selectedElement = HoverVertexElements.Count > 0 ? HoverVertexElements[0] : null;
            }
            else
            {
                if (_countHoverVertexElement == 1)
                {
                    if (_sourceElement == null)
                    {
                        _sourceElement = HoverVertexElements[0];
                        _sourceElement.Grid.Background = new SolidColorBrush(Colors.SelectedForAlgVertexElement);
                    }
                    else
                    {
                        _destElement = HoverVertexElements[0];
                        _destElement.Grid.Background = new SolidColorBrush(Colors.SelectedForAlgVertexElement);

                        _isProcess = true;
                        _args.ControlPanelProvider.Dispose();

                        switch (_algType)
                        {
                            case StatusTool.AlgFindMaxFlow:
                                var maxFlow = await _args.DataProvider.ExecuteFordFulkerson(_sourceElement, _destElement, _args.Logger);
                                if (maxFlow != null) _args.Logger.Info($"Итоговый максимальный поток равен: {maxFlow}.");
                                break;
                            case StatusTool.AlgDijkstra:
                                var minDist = await _args.DataProvider.GetVertexElementsData().ExecuteDijkstra(_sourceElement, _destElement, _args.Logger);
                                if (minDist != null && minDist.Count > 0)
                                {
                                    _args.Logger.Info($"Кротчайший путь между двумя вершинами: {minDist.Select(x => x.Data).ToList().GetArrayForLog()}.");
                                }
                                break;
                        }
                    }
                }
            }
        }

        /// <summary> Разгрузка инструмента </summary>
        public override void Unload()
        {
            if (_sourceElement != null) _sourceElement.Grid.Background = Brushes.Transparent;
            if (_destElement != null) _destElement.Grid.Background = Brushes.Transparent;
            _args.DataProvider.GetVertexElementsData().ForEach(x => x.SetData());
            _args.DataProvider.GetEdgeElementsData().ForEach(x => x?.SetWeight());
            _args.DataProvider.GetVertexElementsData().ForEach(x => x.SetNoVisited());
            _args.DataProvider.GetEdgeElementsData().ForEach(y => y.SetNoVisited());

            _args.ControlPanelProvider.Dispose();
            _args.CanvasBorder.MouseMove -= OnMouseMove;
            _args.CanvasBorder.MouseDown -= OnMouseDown;
            _args.Logger.Info("Выполнен сброс демонстрации работы алгоритма.");
            Dispose();
        }
    }
}
