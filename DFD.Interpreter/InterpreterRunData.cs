using DFD.DataStructures.Implementations;
using DFD.DataStructures.Interfaces;
using DFD.Parsing.Interfaces;

namespace DFD.Parsing;

internal class InterpreterRunData<T> where T : INodeData
{
    protected INodeDataFactory DataFactory { get; }

    public INodeRef<T> CurrentScopeNode { get; set; }
    public int CurrentScopeLevel { get; set; }

    public INodeRef<T> Root { get; }

    public InterpreterRunData(INodeDataFactory dataFactory)
    {
        DataFactory = dataFactory;
        CurrentScopeNode = new Node<T>()
        {
            Name = "root",
            Data = (T)DataFactory.CreateData("Graph Root", NodeType.Process)
        };
        Root = CurrentScopeNode;
    }

    public void RaiseScope(INodeRef<T> newChild)
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