using DataStructure.NamedTree;
using DFD.Model.Interfaces;
using DFD.ModelImplementations;
using DFD.ViewModel;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphvizConverter
{
    public class VisualGraphCreator
    {
        public ITreeNode<IGraphNodeData> ConvertToCollapsableGraph(ITreeNode<IGraphNodeData> root)
        {
            var newRoot = new TreeNode<ICollapsableGraphNode>()
            {
                Children = root.Children,
                Data = new CollapsableGraphNode()
                {
                    Data = root.Data,
                    ChildrenCollapsed = false,
                },
                EntityName = root.EntityName,
                Parent = root.Parent,
            };
            foreach (var child in root.Children)
            {
                
            }
        }
    }
}