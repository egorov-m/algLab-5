using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using algLab_5.Models;
using System.Windows.Controls;
using System.Windows.Shapes;
using algLab_5.Tools.Base;
using algLab_5.Services;
using System.Windows.Input;
using System.Windows.Media;

namespace algLab_5.Tools
{
    public class AddConnectionTool : Tool
    {
        /// <summary> Начальный Grid </summary>
        private Grid? _initialGrid;
        /// <summary> Конечный Grid </summary>
        private Grid? _destinationGrid;
        /// <summary> Линия связи </summary>
        private Polyline? _connection;
        /// <summary> В процессе ли добавление связи </summary>
        private bool _isProcess = false;
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
                if (_initialGrid != null && _connection == null)
                {
                    _connection = ConfiguratorViewElement.GetPolyline(_connectionType);
                    _args.Canvas.Children.Add(_connection);
                    Panel.SetZIndex(_connection, 1);
                }
                _connection?.Points.Clear();
                if (_connection != null)
                    _connection.Points =
                        ConfiguratorViewElement.GetPointCollectionForConnection(pointCursor, _initialGrid, _connectionType);
            }
        }

        /// <summary> Обработчик события нажатия кнопки мыши </summary>
        /// <param name="sender"> Объект события </param>
        /// <param name="e"> Само событие</param>
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (_isProcess)
            {
                if (_initialGrid != null && _hoverElements.Count == 1)
                {
                    _destinationGrid = _hoverElements[0];

                    if (_initialGrid == _destinationGrid)
                    {
                        _args.MainWindow.DisableTool();
                        _isProcess = false;
                        _initialGrid = null;
                        _destinationGrid = null;
                        if (_connection != null) _connection.Stroke = Brushes.Transparent;
                        _connection = null;
                        throw new Exception("ОШИБКА! Ребро можно создать только между разными вершинами.");
                    }

                    if (_connection != null)
                    {
                        //var personModelElementInitial = _args.DataProvider.GetPersonElement(_initialGrid);
                        //var personModelElementDestination = _args.DataProvider.GetPersonElement(_destinationGrid);
                        //if (personModelElementInitial != null && personModelElementDestination != null)
                        //{
                        //    switch (_connectionType)
                        //    {
                        //        case ConnectionType.ParentChild:
                        //            personModelElementDestination.Data?.AddParent(personModelElementInitial.Data);
                        //            break;
                        //        case ConnectionType.CurrentSpouses:
                        //            personModelElementInitial.Data?.AddCurrentSpouse(personModelElementDestination.Data);
                        //            personModelElementDestination.Data?.AddCurrentSpouse(personModelElementInitial.Data);
                        //            break;
                        //        case ConnectionType.FormerSpouses:
                        //            personModelElementInitial.Data?.AddFormerSpouse(personModelElementDestination.Data);
                        //            personModelElementDestination.Data?.AddFormerSpouse(personModelElementInitial.Data);
                        //            break;
                        //        default:
                        //            throw new ArgumentException("ОШИБКА! Недопустимы тип родственной связи.");
                        //    }
                        //}
                        
                        var point = _connection.Points[3]; // начальная точка линии связи
                        _connection.Points.Clear();
                        _connection.Points = ConfiguratorViewElement.GetPointCollectionForConnection(point, _destinationGrid, _connectionType);

                        if (!_args.ShapesRepository.AddConnection(_initialGrid, _destinationGrid, _connection,
                                _connectionType)) throw new ArgumentException("ОШИБКА! Нельзя добавить уже существующее ребро.");
                        _args.SavedChange(StatusSaved.Unsaved);
                        _args.MainWindow.DisableTool();
                        _isProcess = false;
                        _initialGrid = null;
                        _destinationGrid = null;
                        _connection = null;
                    }
                }
            }
            else
            {
                if (_hoverElements.Count == 1)
                {
                    _initialGrid = _hoverElements[0];
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
