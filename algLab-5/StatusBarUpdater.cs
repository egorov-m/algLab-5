using algLab_5.Models;
using System.Windows;
using System.Windows.Controls;

namespace algLab_5
{
    /// <summary> Обновитель строки состояния </summary>
    public class StatusBarUpdater
    {
        private readonly TextBlock _tbIsSavedProject;
        private readonly TextBlock _tbCurrentState;
        private readonly TextBlock _tbCoordinates;
        private readonly TextBlock _tbIsHover;

        public StatusBarUpdater(TextBlock tbIsSavedProject, TextBlock tbCurrentState, TextBlock tbCoordinates, TextBlock tbIsHover)
        {
            _tbIsSavedProject = tbIsSavedProject;
            _tbCurrentState = tbCurrentState;
            _tbCoordinates = tbCoordinates;
            _tbIsHover = tbIsHover;
        }

        /// <summary> Полное обновление строки состояния </summary>
        /// <param name="status"> Новый статус инструмента </param>
        /// <param name="coordinates"> Новые координаты </param>
        /// <param name="infoAboutElement"> Новая информация об элементе </param>
        public void Update(StatusTool status, Point? coordinates, string infoAboutElement = null)
        {
            UpdateStatus(status);
            UpdateCoordinates(coordinates);
            UpdateInfo(infoAboutElement);
        }

        /// <summary> Обновление информации о сохранённости </summary>
        /// <param name="status"></param>
        public void UpdateSaveProjectInfo(StatusSaved status)
        {
            _tbIsSavedProject.Text = status.ToString("G");
        }

        /// <summary> Обновление статуса инструмента </summary>
        /// <param name="status"> Новый статус инструмента</param>
        public void UpdateStatus(StatusTool status)
        {
            _tbCurrentState.Text = status.ToString("G");
        }

        /// <summary> Обновление статуса координат </summary>
        /// <param name="coordinates"> Новые координаты </param>
        public void UpdateCoordinates(Point? coordinates)
        {
            if (coordinates != null)
            {
                _tbCoordinates.Text = $"X: {coordinates.Value.X:000.0} Y: {coordinates.Value.Y:000.0}";
            }
        }

        /// <summary> Обновление информации об элементе </summary>
        /// <param name="info">Информация</param>
        public void UpdateInfo(string info)
        {
            _tbIsHover.Text = info;
        }
    }
}
