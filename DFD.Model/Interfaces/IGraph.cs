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
        public ITreeNode<T> Root { get; }
        public ICollection<IFlow<T>> Flows { get; }
    }
}
