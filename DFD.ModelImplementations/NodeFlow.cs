using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructure.NamedTree;
using DFD.Model;
using DFD.Model.Interfaces;

namespace DFD.ModelImplementations
{
    public class NodeFlow<T> : INodeFlow<T>
    {
        public ITreeNode<T> Source { get; set; }
        public ITreeNode<T> Target { get; set; }
        public bool BiDirectional { get; set; }
        public string FlowName { get; set; }
    }
}
