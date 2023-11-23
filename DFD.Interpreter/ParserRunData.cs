using DataStructure.NamedTree;
using DFD.Model.Interfaces;
using DFD.ModelImplementations;

namespace DFD.Parsing;

internal class ParserRunData
{
    public ITreeNode<IGraphNodeData> CurrentScopeNode { get; set; }
    public int CurrentScopeLevel { get; set; }

    public ParserRunData()
    {
        CurrentScopeNode = new TreeNode<IGraphNodeData>()
        {
            NodeName = "top",
            Data = new GraphNodeData()
            {
                Name = "System",
                Type = NodeType.Process
            }
        };
    }

    public void RaiseScope(ITreeNode<IGraphNodeData> newChild)
    {
        CurrentScopeNode = newChild;
        CurrentScopeLevel++;
    }

    public void LowerScope()
    {
        CurrentScopeNode = CurrentScopeNode.Parent;
        CurrentScopeLevel--;
    }

    public void LowerScopeTo(int level)
    {
        while (CurrentScopeLevel > level)
        {
            LowerScope();
        }
    }
}