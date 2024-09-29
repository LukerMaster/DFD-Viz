using DataStructure.NamedTree;
using DFD.Model.Interfaces;

namespace DFD.ModelImplementations
{
    public class NodeFlow<T> : INodeFlow
    {
        public T SourceNodeName { get; set; }
        public T TargetNodeName { get; set; }
        public bool BiDirectional { get; set; }
        public string FlowName { get; set; }
    }
}
