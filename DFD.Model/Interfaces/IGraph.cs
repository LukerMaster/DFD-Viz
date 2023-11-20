using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructure.NamedTree;

namespace DFD.Model.Interfaces
{
    public interface IGraph<T>
    {
        public ITreeNode<T> Root { get; protected set; }
        public ICollection<INodeFlow<T>> Flows { get; protected set; }
        public IGraph<TNew> ConvertGraphTo<TNew>(Func<T, TNew> dataConversionFunc)
        {
            Root.ConvertTreeTo(dataConversionFunc);
            return (this as IGraph<TNew>)!;
        }
    }
}
