using System.Windows;

namespace algLab_5.Views
{
    public interface IDrawLine : IDraw
    {
        void Draw(Point point1, Point point2);

        void Draw();
    }
}
