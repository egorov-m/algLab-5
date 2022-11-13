using System;

namespace algLab_5.Models.Graph
{
    /// <summary> Класс вершины графа </summary>
    public class Vertex : IComparable
    {
        /// <summary> Идентификатор вершины </summary>
        public int Id { get; set; }

        /// <summary> Данные вершины </summary>
        public int Data { get; set; }

        public Vertex(int id, int data)
        {
            Id = id;
            Data = data;
        }

        /// <summary> Установить данные вершины </summary>
        /// <param name="data"> Задаваемые данные </param>
        public virtual void SetData(int data) => Data = data;

        /// <summary> Возвращает True, если данный объект является вершиной графа и имеет тот же id </summary>
        /// <param name="obj"> Объект проверки </param>
        public override bool Equals(object obj) => obj is Vertex && Id == ((Vertex)obj).Id;

        /// <summary> Сравнивает только данные </summary>
        /// <param name="obj"></param>
        public int CompareTo(object? obj) => Data - ((Vertex)obj).Data;

        public override string ToString() => $"id: {Id}\nData: {Data}";
    }
}
