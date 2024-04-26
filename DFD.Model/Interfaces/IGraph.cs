using DataStructure.NamedTree;

namespace DFD.Model.Interfaces
{
    public interface IGraph<T>
    {
        public ITreeNode<T> Root { get; }
        public IReadOnlyCollection<INodeFlow> Flows { get; }
        /// <summary>
        /// Copies the entire graph and applies <paramref name="dataConversionFunc"/> to all of the nodes
        /// to allow for data conversion from <see cref="T"/> to <see cref="TNew"/>.
        /// </summary>
        /// <typeparam name="TNew"></typeparam>
        /// <param name="dataConversionFunc">Function to convert data.</param>
        /// <returns>New graph with all </returns>
        public IGraph<TNew> CopyGraphAs<TNew>(Func<T, TNew> dataConversionFunc);
    }
}
