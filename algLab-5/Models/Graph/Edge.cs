using algLab_5.Views;
using System;
using System.Windows;
using System.Windows.Controls;

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
        public int InitialVertexId { get; set; }

        /// <summary> Куда </summary>
        public int? DestinationVertexId { get; set; }

        /// <summary> Цена ребра </summary>
        public int Weight { get; set; }

        /// <summary> Было ли посещено ребро графа </summary>
        protected bool _isVisited;

        /// <summary> Было ли посещено ребро графа </summary>
        public bool IsVisited => _isVisited;

        protected Edge(int initialVertexId, int weight)
        {
            InitialVertexId = initialVertexId;
            Weight = weight;
        }

        protected Edge(int initialVertexId, int destinationVertexId, int weight)
        {
            InitialVertexId = initialVertexId;
            DestinationVertexId = destinationVertexId;
            Weight = weight;
        }

        /// <summary> Установить вес ребра </summary>
        /// <param name="weight"> Вес </param>
        public virtual void SetWeight(int weight) => Weight = weight;

        /// <summary> Установить вес ребра </summary>
        /// <param name="weight"> Вес </param>
        public virtual bool SetWeight(string weight)
        {
            if (int.TryParse(weight, out var intWeight))
            {
                Weight = intWeight;
                return true;
            }

            return false;
        }

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
            return InitialVertexId == edge.InitialVertexId && DestinationVertexId == edge.DestinationVertexId && Weight == edge.Weight;
        }

        /// <summary> Сравнивает только стоимости рёбер </summary>
        /// <param name="obj"></param>
        public int CompareTo(object? obj) => Weight - ((obj as Edge)!).Weight;
    }
}
