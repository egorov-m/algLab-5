using System.Windows;
using System.Windows.Controls;

namespace algLab_5.Views
{
    public interface IDraw
    {
        void Draw(Canvas canvas, Point point);

        void Draw(Point point);

        void Draw(double diffX, double diffY);

        void Draw(Canvas canvas);
    }
}
