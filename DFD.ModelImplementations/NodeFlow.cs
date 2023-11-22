using DataStructure.NamedTree;
using DFD.Model.Interfaces;

namespace DFD.ModelImplementations
{
    public class NodeFlow : INodeFlow
    {
        public string SourceNodeName { get; set; }
        public string TargetNodeName { get; set; }
        public bool BiDirectional { get; set; }
        public string FlowName { get; set; }
    }
}
