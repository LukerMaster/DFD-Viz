using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;
using DFD.DataStructures.Interfaces;

namespace DFD.DataStructures.Implementations
{
    public class Node<T> : INode<T>
    {
        private readonly ICollection<INodeRef<T>> _children = new List<INodeRef<T>>();

        public INodeRef<T>? Parent { get; set; }

        ICollection<INodeRef<T>> INode<T>.Children => _children;

        public IReadOnlyCollection<INodeRef<T>> Children => (IReadOnlyCollection<INodeRef<T>>)_children;

        public string FullPath
        {
            get
            {
                var name = Name;
                var currentParent = Parent;
                while (currentParent is not null)
                {
                    name = currentParent.Name + '.' + name;
                    currentParent = currentParent.Parent;
                }
                return name;
            }
        }

        public string HexHash
        {
            get
            {
                var endoded = Encoding.UTF8.GetBytes(FullPath);
                var hashed = SHA512.HashData(endoded);
                var hexed = Convert.ToHexString(hashed);
                return hexed;
            }
        }

        public INodeRef<T> AddChild(T data, string name)
        {
            Node<T> new_node = new Node<T>()
            {
                Data = data,
                Name = name,
                Parent = this,
            };
            _children.Add(new_node);
            return new_node;
        }

        private object _data = null!;
        public T Data {
            get => (T) _data;
            set => _data = value!;
        }

        private string _nodeName = String.Empty;

        public string Name
        {
            get => _nodeName;
            set
            {
                if (AreThereSiblingsWithSameName(value))
                {
                    throw new SameFullNodeNameException();
                }
                else
                {
                    _nodeName = value;
                }
            }
        }

        private bool AreThereSiblingsWithSameName(string name_to_check)
        {
            if (Parent == null)
            {
                return false;
            }
            else
            {
                foreach (var parentChild in Parent.Children)
                {
                    if (parentChild.Name == name_to_check && parentChild != this)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public INodeRef<TNew> CopySubtreeAs<TNew>(Func<T, TNew> dataConversionFunc, INodeRef<TNew> newParent)
        {
            Node<TNew> newNode = new Node<TNew>()
            {
                Parent = newParent,
                Data = dataConversionFunc(Data),
                Name = Name,
            };
            foreach (var child in Children)
            {
                newNode._children.Add((INode<TNew>)child.CopySubtreeAs(dataConversionFunc, newNode));
            }
            return newNode;
        }
    }
}
