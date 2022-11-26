﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using algLab_5.Algorithms;
using algLab_5.Models;
using algLab_5.Services;
using algLab_5.Services.Logger;
using algLab_5.Tools.Base;
using algLab_5.Views.Graph;

namespace algLab_5.Tools
{
    public class DemoAlgorithmsSingleChoiceTool : Tool
    {
        /// <summary> Выбранный элемент </summary>
        private VertexElement? _selectedElement;

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
                    _args.Logger.Info("Выполнен сброс демонстрации работы алгоритма.");
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

                    var tmp = HoverVertexElements[0].ExecuteDfs(_args.Logger);
                    var list = new List<string>();
                    var count = 0;
                    await foreach (var t in tmp)
                    {
                        list.Add(t.Data);
                        count++;
                    }

                    if (count > 0) _args.Logger.Info($"Обход осуществлялся в порядке: {list.GetArrayForLog()}.");
                }
            }
        }

        /// <summary> Разгрузка инструмента </summary>
        public override void Unload()
        {
            _args.DataProvider.GetVertexElementsData().ForEach(x => x.SetNoVisited());
            _args.DataProvider.GetEdgeElementsData().ForEach(y => y.SetNoVisited());
            _args.ControlPanelProvider.Dispose();
            _args.CanvasBorder.MouseMove -= OnMouseMove;
            _args.CanvasBorder.MouseDown -= OnMouseDown;
            Dispose();
        }
    }
}