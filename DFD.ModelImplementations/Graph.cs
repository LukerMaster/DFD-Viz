using DataStructure.NamedTree;
using DFD.Model.Interfaces;

namespace DFD.ModelImplementations;

public class Graph<T> : IGraph<T>
{
    private object _root;
    public ITreeNode<T> Root
    {
        get => (_root as ITreeNode<T>)!; 
        set => _root = value;
    }

    private object _flows;
    public IReadOnlyCollection<INodeFlow> Flows
    {
        get => (_flows as IReadOnlyCollection<INodeFlow>)!; 
        set => _flows = value;
    }
    public IGraph<TNew> CopyGraphAs<TNew>(Func<T, TNew> dataConversionFunc)
    {
        var newRoot = Root.CopySubtreeAs(dataConversionFunc)!;

        var newFlows = new List<INodeFlow>();

        foreach (var flow in Flows)
        {
            newFlows.Add(new NodeFlow()
            {
                BiDirectional = flow.BiDirectional,
                FlowName = flow.FlowName,
                SourceNodeName = flow.Source,
                TargetNodeName = flow.Target
            });
        }

        return new Graph<TNew>(newRoot, newFlows);
    }

    public Graph(ITreeNode<T> root, IReadOnlyCollection<INodeFlow> flows)
    {
        _root = root;
        _flows = flows;
    }
}