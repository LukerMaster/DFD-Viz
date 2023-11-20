using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure.NamedTree
{
    public class TreeNode<T> : ITreeNode<T>
    {
        public ITreeNode<T>? Parent { get; set; }
        private readonly ICollection<ITreeNode<T>> _children = new List<ITreeNode<T>>();
        public IReadOnlyCollection<ITreeNode<T>> Children => (IReadOnlyCollection<ITreeNode<T>>)_children;
        private object _data = null!;
        private string _entityName;

        public T Data {
            get => (T) _data;
            set => _data = value!;
        }

        public string EntityName
        {
            get => _entityName;
            set
            {
                if (Parent == null)
                {
                    _entityName = value;
                }
                else
                {
                    foreach (var parentChild in Parent.Children)
                    {
                        if (parentChild.EntityName == value && parentChild != this)
                        {
                            throw new SameFullEntityNameException(value);
                        }
                    }
                    _entityName = value;
                }
            }
        }

        private void ConvertAllDownwardsTo<TNew>(Func<T, TNew> dataConversionFunc)
        {
            _data = dataConversionFunc(Data)!;
            foreach (var node in Children)
            {
                ConvertAllDownwardsTo<TNew>(dataConversionFunc);
            }
        }

        public ITreeNode<TNew>? ConvertTreeTo<TNew>(Func<T, TNew> dataConversionFunc)
        {
            if (Parent is not null)
            {
                throw new NodeConversionWithParentException<T>(this);
            }
            ConvertAllDownwardsTo(dataConversionFunc);
            return this as ITreeNode<TNew>;
        }
    }
}
