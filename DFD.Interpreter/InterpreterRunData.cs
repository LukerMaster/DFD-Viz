using DFD.DataStructures.Implementations;
using DFD.DataStructures.Interfaces;
using DFD.Parsing.Interfaces;

namespace DFD.Parsing;

internal class InterpreterRunData
{
    protected INodeDataFactory DataFactory { get; }

    public INodeRef<INodeData> CurrentScopeNode { get; set; }
    public int CurrentScopeLevel { get; set; }

    public INodeRef<INodeData> Root { get; }

    public InterpreterRunData(INodeDataFactory dataFactory)
    {
        DataFactory = dataFactory;
        CurrentScopeNode = new Node<INodeData>()
        {
            Name = "root",
            Data = DataFactory.CreateData("Graph Root", NodeType.Process)
        };
        Root = CurrentScopeNode;
    }

    public void RaiseScope(INodeRef<INodeData> newChild)
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