using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using algLab_5.Models.Utils;
using algLab_5.Views;

namespace algLab_5.Models.Graph
{
    /// <summary> Класс вершины графа </summary>
    public abstract class Vertex : 
        IDraw, 
        IRemoveDraw,
        IVisited
    {
        /// <summary> Идентификатор вершины </summary>
        public int Id { get; private set; }

        /// <summary> Данные вершины </summary>
        public string Data { get; set; }

        /// <summary> Точка положения вершины </summary>
        public abstract Point Position { get; set; }

        /// <summary> Сетка вершины </summary>
        public abstract Grid Grid { get; set; }

        /// <summary> Текстовое поле вершины </summary>
        public abstract TextBox TextBox { get; set; }

        /// <summary> Список содержащий все рёбра вершины </summary>
        public IList<Edge?> EdgesList { get; protected set; }

        /// <summary> Была ли посещена вершина графа </summary>
        protected bool _isVisited;

        /// <summary> Была ли посещена вершина графа </summary>
        public bool IsVisited => _isVisited;

        protected Vertex(string data)
        {
            Id = IdentifierSetter.GetId();
            Data = data;
            EdgesList = new List<Edge?>();
        }

        protected Vertex(int id, string data)
        {
            Id = id;
            Data = data;
            EdgesList = new List<Edge?>();
        }

        protected Vertex(int id, string data, IList<Edge?> edgesList)
        {
            Id = id;
            Data = data;
            EdgesList = edgesList;
        }

        /// <summary> Установить данные вершины </summary>
        /// <param name="data"> Задаваемые данные </param>
        /// <param name="graph"> Коллекция вершин текущего графа </param>
        public virtual bool SetData(string data, IEnumerable<Vertex> graph)
        {
            var count = graph.Count(graph => graph.Data == data);
            if (count > 0) return false;

            Data = data;
            return true;
        }

        /// <summary> Отобразить данные </summary>
        public abstract void SetData();

        /// <summary> Установить вершину как посещённую </summary>
        public virtual void SetVisited() => _isVisited = true;

        /// <summary> Установить вершину как НЕ посещённую </summary>
        public virtual void SetNoVisited() => _isVisited = false;

        /// <summary> Установить вершину как текущую (визуально) </summary>
        public abstract void SetCurrent();

        /// <summary> Сбросить статус текущей вершины (визуально) </summary>
        public abstract void ResetCurrent();

        public abstract void Draw(Canvas canvas, int canvasHeight, int canvasWidth);
        public abstract void Draw(Canvas canvas, Point point);
        public abstract void Draw(Point point);
        public abstract void Draw(double diffX, double diffY);
        public abstract void Draw(Canvas canvas);
        public abstract void RemoveDraw(Canvas canvas);

        public override string ToString() => $"{Data}";

        void IVisited.SetVisited() => _isVisited = true;

        void IVisited.SetNoVisited() => _isVisited = false;
    }
}
