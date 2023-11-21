using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFD.ModelImplementations;

namespace DataStructure.NamedTree
{
    public class TreeNode<T> : IModifiableTreeNode<T>
    {
        private readonly ICollection<ITreeNode<T>> _children = new List<ITreeNode<T>>();
        // TreeNode
        public IReadOnlyCollection<ITreeNode<T>> Children
        {
            get => (IReadOnlyCollection<ITreeNode<T>>)_children;
            init => _children = (ICollection<ITreeNode<T>>)value;
        }
        // ModifiableTreeNode
        ICollection<ITreeNode<T>> IModifiableTreeNode<T>.Children => _children;


        public ITreeNode<T>? Parent { get; set; }

        private object _data = null!;
        public T Data {
            get => (T) _data;
            set => _data = value!;
        }

        private string _nodeName;
        public string NodeName
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
                            throw new SameFullEntityNameException(value);
                        }
                    }
                    _nodeName = value;
                }
            }
        }

        public ITreeNode<TNew> CopySubtreeAs<TNew>(Func<T, TNew> dataConversionFunc, ITreeNode<TNew> newParent)
        {
            TreeNode<TNew> newNode = new TreeNode<TNew>()
            {
                Parent = newParent,
                Data = dataConversionFunc(Data),
                NodeName = NodeName,
            };
            foreach (var child in Children)
            {
                newNode._children.Add(child.CopySubtreeAs(dataConversionFunc, newNode));
            }
            return newNode;
        }
    }
}
