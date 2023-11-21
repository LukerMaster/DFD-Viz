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
    public IReadOnlyCollection<INodeFlow<T>> Flows
    {
        get => (_flows as IReadOnlyCollection<INodeFlow<T>>)!; 
        set => _flows = value;
    }
    public IGraph<TNew> CopyGraphAs<TNew>(Func<T, TNew> dataConversionFunc)
    {
        var newRoot = Root.CopySubtreeAs(dataConversionFunc)!;

        var newFlows = new List<INodeFlow<TNew>>();

        foreach (var flow in Flows)
        {
            newFlows.Add(new NodeFlow<TNew>()
            {
                BiDirectional = flow.BiDirectional,
                FlowName = flow.FlowName,
                Source = newRoot.FindMatchingNode(flow.Source.FullNodeName, leavesOnly: true),
                Target = newRoot.FindMatchingNode(flow.Target.FullNodeName, leavesOnly: true)
            });
        }

        return new Graph<TNew>(newRoot, newFlows);
    }

    public Graph(ITreeNode<T> root, IReadOnlyCollection<INodeFlow<T>> flows)
    {
        _root = root;
        _flows = flows;
    }
}