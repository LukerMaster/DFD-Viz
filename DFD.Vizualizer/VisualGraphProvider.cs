using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DataStructure.NamedTree;
using DFD.GraphConverter;
using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;
using DFD.Vizualizer.Interfaces;

namespace DFD.Vizualizer
{
    public class VisualGraphProvider : IViewDataProvider, IVisualGraphProvider
    {
        private IVisualGraph _visualGraph;
        private IGraph<ICollapsableGraphNode> _logicalGraph;
        private readonly VisualGraphCreator _creator;
        private Dictionary<ICollapsableGraphNode, bool> _previousCollapsedStates = new Dictionary<ICollapsableGraphNode, bool>();
        public VisualGraphProvider(IGraph<ICollapsableGraphNode> logicalGraph, VisualGraphCreator creator)
        {
            _logicalGraph = logicalGraph;
            _creator = creator;

            RegenerateVisualGraph();
            UpdatePreviousCollapsedStates(logicalGraph.Root);
        }

        private void UpdatePreviousCollapsedStates(ITreeNode<ICollapsableGraphNode> node)
        {
            _previousCollapsedStates[node.Data] = node.Data.ChildrenCollapsed;
            foreach (var child in node.Children)
            {
                UpdatePreviousCollapsedStates(child);
            }
        }

        private bool DidAnyNodeChange()
        {
            foreach (var node in _visualGraph.Nodes)
            {
                if (node.Node.ChildrenCollapsed != _previousCollapsedStates[node.Node])
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
