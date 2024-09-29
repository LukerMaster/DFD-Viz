using System.Numerics;
using DFD.DataStructures.Interfaces;
using DFD.GraphConverter.Interfaces;
using DFD.ViewModel.Interfaces;
using DFD.Vizualizer.Interfaces;

namespace DFD.Vizualizer
{
    public class VisualGraphProvider : IVisualGraphProvider, IViewDataProvider
    {
        private IVisualGraph _visualGraph;
        private IGraph<ICollapsibleNodeData> _logicalGraph;
        private readonly IVisualGraphCreator _creator;
        private Dictionary<ICollapsibleNodeData, bool> _previousCollapsedStates = new();
        public VisualGraphProvider(IGraph<ICollapsibleNodeData> logicalGraph, IVisualGraphCreator creator)
        {
            _logicalGraph = logicalGraph;
            _creator = creator;

            RegenerateVisualGraph();
            UpdatePreviousCollapsedStates(logicalGraph.Root);
        }

        private void UpdatePreviousCollapsedStates(INodeRef<ICollapsibleNodeData> node)
        {
            _previousCollapsedStates[node.Data] = node.Data.Collapsed;
            foreach (var child in node.Children)
            {
                UpdatePreviousCollapsedStates(child);
            }
        }

        private bool DidAnyNodeChange()
        {
            foreach (var node in _visualGraph.Nodes)
            {
                if (node.Node.Collapsed != _previousCollapsedStates[node.Node])
                    return true;
            }

            return false;
        }

        private void RegenerateVisualGraph()
        {
            _visualGraph = _creator.GetVisualGraph(_logicalGraph);
        }
        public IVisualGraph VisualGraph
        {
            get
            {
                if (DidAnyNodeChange())
                {
                    RegenerateVisualGraph();
                    UpdatePreviousCollapsedStates(_logicalGraph.Root);
                }

                return _visualGraph;
            }
        }

        public Vector2 Center { get => VisualGraph.Size / 2; }
        public Vector2 Size { get => VisualGraph.Size; }
    }
}
