using DFD.DataStructures.Interfaces;

namespace DFD.DataStructures.Implementations
{
    public class Node<T> : INode<T>
    {
        private readonly ICollection<INodeRef<T>> _children = new List<INodeRef<T>>();
        public INodeRef<T>? Parent { get; set; }

        public IReadOnlyCollection<INodeRef<T>> Children => (IReadOnlyCollection<INodeRef<T>>)_children;
        public string FullPath { get; set; }
        public void RemoveChild(INodeRef<T> node)
        {
            _children.Remove(node);
            node.P
        }

        public void AddChild(T data, string name)
        {
            throw new NotImplementedException();
        }



        private object _data = null!;
        public T Data {
            get => (T) _data;
            set => _data = value!;
        }

        private string _nodeName;
        public string Name
        {
            get => _nodeName;
            set
            {
                if (Parent == null)
                {
                    _nodeName = value;
                }
                else
                {
                    foreach (var parentChild in Parent.Children)
                    {
                        if (parentChild.NodeName == value && parentChild != this)
                        {
                            throw new SameFullNodeNameException(value);
                        }
                    }
                    _nodeName = value;
                }
            }
        }

        public INodeRef<TNew> CopySubtreeAs<TNew>(Func<T, TNew> dataConversionFunc, INodeRef<TNew> newParent)
        {
            Node<TNew> newNode = new Node<TNew>()
            {
                Parent = newParent,
                Data = dataConversionFunc(Data),
                NodeName = NodeName,
            };
            foreach (var child in Children)
            {
                newNode._children.Add((INode<TNew>)child.CopySubtreeAs(dataConversionFunc, newNode));
            }
            return newNode;
        }
    }
}
