using algLab_5.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace algLab_5.Models.Graph
{
    /// <summary> Класс ребра графа </summary>
    public abstract class Edge : 
        IComparable,
        IDraw,
        IDrawLine,
        IRemoveDraw,
        IVisited
    {
        /// <summary> Откуда </summary>
        public Vertex InitialVertex { get; set; }

        /// <summary> Куда </summary>
        public Vertex? DestinationVertex { get; set; }

        /// <summary> Цена ребра </summary>
        public int Weight { get; set; }

        /// <summary> Линия ребра </summary>
        public abstract Polyline Polyline { get; set; }

        /// <summary> Панель для содержимого элемента ребра </summary>
        public abstract StackPanel StackPanel { get; set; }

        /// <summary> Текстовое поле ввода веса ребра </summary>
        public abstract TextBox TextBox { get; set; }

        /// <summary> Было ли посещено ребро графа </summary>
        protected bool _isVisited;

        /// <summary> Было ли посещено ребро графа </summary>
        public bool IsVisited => _isVisited;

        protected Edge(Vertex initialVertex, int weight)
        {
            InitialVertex = initialVertex;
            Weight = weight;
        }

        protected Edge(Vertex initialVertex, Vertex destinationVertex, int weight)
        {
            InitialVertex = initialVertex;
            DestinationVertex = destinationVertex;
            Weight = weight;
        }

        /// <summary> Установить вес ребра </summary>
        /// <param name="weight"> Вес </param>
        public virtual bool SetWeight(int weight)
        {
            if (weight >= 0)
            {
                Weight = weight;
                return true;
            }

            return false;
        }

        /// <summary> Установить вес ребра </summary>
        /// <param name="weight"> Вес </param>
        public virtual bool SetWeight(string weight)
        {
            if (int.TryParse(weight, out var intWeight) && intWeight >= 0)
            {
                Weight = intWeight;
                return true;
            }

            return false;
        }

        /// <summary> Отобразить вес ребра </summary>
        public abstract void SetWeight();

        /// <summary> Установить отображение двух указанных значений </summary>
        /// <param name="value1"> Значение 1 </param>
        /// <param name="value2"> Значение 2 </param>
        public abstract void SetDisplayTwoValues(int value1, int value2);

        /// <summary> Установить ребро как посещённое </summary>
        public virtual void SetVisited() => _isVisited = true;

        /// <summary> Установить ребро как НЕ посещённое </summary>
        public virtual void SetNoVisited() => _isVisited = false;

        public abstract void Draw(Canvas canvas, Point point);
        public abstract void Draw(Point point);
        public abstract void Draw(Point point1, Point point2);
        public abstract void Draw(double diffX, double diffY);
        public abstract void Draw();
        public abstract void Draw(Canvas canvas);
        public abstract void RemoveDraw(Canvas canvas);

        public override bool Equals(object obj)
        {
            if (obj is not Edge edge) return false;
            return InitialVertex == edge.InitialVertex && DestinationVertex == edge.DestinationVertex && Weight == edge.Weight;
        }

        /// <summary> Сравнивает только стоимости рёбер </summary>
        /// <param name="obj"></param>
        public int CompareTo(object? obj) => Weight - ((obj as Edge)!).Weight;
    }
}
