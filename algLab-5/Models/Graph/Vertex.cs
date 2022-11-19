namespace algLab_5.Models.Graph
{
    /// <summary> Класс вершины графа </summary>
    public class Vertex
    {
        /// <summary> Идентификатор вершины </summary>
        public int Id { get; set; }

        /// <summary> Данные вершины </summary>
        public string Data { get; set; }

        public Vertex(int id, string data)
        {
            Id = id;
            Data = data;
        }

        /// <summary> Установить данные вершины </summary>
        /// <param name="data"> Задаваемые данные </param>
        public virtual void SetData(string data) => Data = data;

        /// <summary> Возвращает True, если данный объект является вершиной графа и имеет тот же id </summary>
        /// <param name="obj"> Объект проверки </param>
        public override bool Equals(object obj) => obj is Vertex && Id == ((Vertex)obj).Id;

        public override string ToString() => $"id: {Id}\nData: {Data}";
    }
}
