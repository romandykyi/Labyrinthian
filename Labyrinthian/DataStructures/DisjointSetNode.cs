namespace Labyrinthian
{
    internal sealed class DisjointSetNode<T>
    {
        public T Value { get; set; }
        public int Rank { get; set; }
        public DisjointSetNode<T> Parent { get; set; }

        public DisjointSetNode(T value)
        {
            Value = value;
            Parent = this;
            Rank = 0;
        }
    }
}