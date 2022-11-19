using System;

namespace algLab_5.Models.Graph
{
    /// <summary> Класс ребра графа </summary>
    public class Edge : IComparable
    {
        /// <summary> Откуда </summary>
        public int InitialVertexId { get; set; }

        /// <summary> Куда </summary>
        public int? DestinationVertexId { get; set; }

        /// <summary> Цена ребра </summary>
        public int Weight { get; set; }

        public Edge(int initialVertexId, int weight)
        {
            InitialVertexId = initialVertexId;
            Weight = weight;
        }

        public Edge(int initialVertexId, int destinationVertexId, int weight)
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
        public virtual void SetWeight(string weight)
        {
            if (int.TryParse(weight, out var intWeight))
            {
                Weight = intWeight;
            }
            else
            {
                throw new ArgumentException("ОШИБКА! Вес вершины должен быть представлен как целое число.");
            }
        }

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
