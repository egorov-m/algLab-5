namespace algLab_5.Models.Graph
{
    public interface IVisited
    {
        bool IsVisited { get; }
        void SetVisited();
        void SetNoVisited();
    }
}
