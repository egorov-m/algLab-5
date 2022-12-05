using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using algLab_5.Algorithms;
using algLab_5.Models;
using algLab_5.Services;
using algLab_5.Services.Logger;
using algLab_5.Tools.Base;
using algLab_5.Views.Graph;
using Colors = algLab_5.Views.Utils.Colors;

namespace algLab_5.Tools
{
    public class DemoAlgorithmsSingleChoiceTool : Tool
    {
        /// <summary> Выбранный элемент </summary>
        private VertexElement? _selectedElement;

        private VertexElement? _elementForAlg;

        /// <summary> Текущее положение курсора на холсте </summary>
        private Point _currentCursorPosition;

        /// <summary> Тип демонстрируемого алгоритма </summary>
        private readonly StatusTool _algType;

        /// <summary> В процессе ли выполнения алгоритм </summary>
        private bool _isProcess;

        public DemoAlgorithmsSingleChoiceTool(ToolArgs args, StatusTool algType) : base(args)
        {
            _args.CanvasBorder.MouseDown += OnMouseDown;
            _args.CanvasBorder.MouseMove += OnMouseMove;
            _algType = algType;

            _args.Logger.Info("Выберите любую вершину для начала демонстрации алгоритма.");
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
                    _args.SavedChange(StatusSaved.Unsaved);
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
                    _isProcess = true;
                    _args.ControlPanelProvider.Dispose();
                    _elementForAlg = HoverVertexElements[0];
                    _elementForAlg.Grid.Background = new SolidColorBrush(Colors.SelectedForAlgVertexElement);

                    var tmp = _algType switch
                    {
                        StatusTool.AlgDfs => _elementForAlg.ExecuteDfs(_args.Logger),
                        StatusTool.AlgBfs => _elementForAlg.ExecuteBfs(_args.Logger),
                        _ => null
                    };

                    if (tmp != null)
                    {
                        var list = new List<string>();
                        var count = 0;
                        await foreach (var t in tmp)
                        {
                            list.Add(t.Data);
                            count++;
                        }

                        if (count > 0) _args.Logger.Info($"Обход осуществлялся в порядке: {list.GetArrayForLog()}.");
                    }
                    else
                    {
                        if (_elementForAlg != null) _elementForAlg.Grid.Background = Brushes.Transparent;
                        if (_algType == StatusTool.AlgKruskal)
                        {
                            try
                            {
                                var (edges, minimumCost) = await _args.DataProvider.ExecuteKruskal(_args.Logger);
                                _args.Logger.Info($"Результат, минимальное остовное дерево: {edges.Select(x => x.Weight).ToList().GetArrayForLog()}.");
                                _args.Logger.Info($"Результат, минимальная сумма остовное дерево: {minimumCost}.");
                            }
                            catch (ArgumentException exception)
                            {
                                _args.Logger.Info(exception.Message);
                                _isProcess = false;
                                _args.MainWindow.DisableTool();
                            }
                        }
                    }
                }
            }
        }

        /// <summary> Разгрузка инструмента </summary>
        public override void Unload()
        {
            _args.DataProvider.GetVertexElementsData().ForEach(x => x.SetNoVisited());
            _args.DataProvider.GetEdgeElementsData().ForEach(y => y.SetNoVisited());
            if (_elementForAlg != null) _elementForAlg.Grid.Background = Brushes.Transparent;
            _args.ControlPanelProvider.Dispose();
            _args.CanvasBorder.MouseMove -= OnMouseMove;
            _args.CanvasBorder.MouseDown -= OnMouseDown;
            _args.Logger.Info("Выполнен сброс демонстрации работы алгоритма.");
            Dispose();
        }
    }
}
