using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using algLab_5.Models;
using algLab_5.Services;
using System.Windows.Input;
using algLab_5.Tools.Base;

namespace algLab_5.Tools
{
    /// <summary> Инструмент добавления элемента </summary>
    public class AddElementTool : Tool
    {
        /// <summary> Выбранный Grid </summary>
        private Grid? _element;

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
                _element = ConfiguratorViewElement.GetGrid();
                _args.Canvas.Children.Add(_element);
                Panel.SetZIndex(_element, 5);
            }

            _element.SetCenterEllipseOnGrid(pointCursor);
        }

        /// <summary> Обработчик события нажатия кнопки мыши </summary>
        /// <param name="sender"> Объект события </param>
        /// <param name="e"> Само событие </param>
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            _args.ShapesRepository.AddElement(_element);
            //PersonModelElement personModelElement = new () { View = new VisualElement() };
            //personModelElement.View.Element = _element;

            //#region Добавление данных человека на визуальный элемент

            //var mainStackPanel = ConfiguratorViewElement.GetStackPanelWithDataPerson();

            //personModelElement.View.Element.Children.Add(mainStackPanel);
            //Grid.SetRow(mainStackPanel, 0);
            //Grid.SetColumn(mainStackPanel, 0);

            //#endregion

            //_args.DataProvider.AddPersonElement(personModelElement);

            // Включаем редактор элементов (для того, чтобы не оставлять сырые визуальные данные)
            //var editElementInfoTool = new EditElementInfoTool(_args);
            //editElementInfoTool.LaunchEditorCurrentHoverElement(sender, e);
            //editElementInfoTool.Unload();

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
