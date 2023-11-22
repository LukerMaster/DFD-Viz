using DataStructure.NamedTree;

namespace DFD.Model.Interfaces;

public interface INodeFlow
{
    string SourceNodeName { get; set; }
    string TargetNodeName { get; set; }
    bool BiDirectional { get; set; }
    string? FlowName { get; set; }
}