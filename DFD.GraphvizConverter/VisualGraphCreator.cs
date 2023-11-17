using DataStructure.NamedTree;
using DFD.Model.Interfaces;
using DFD.ModelImplementations;
using DFD.ViewModel;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphvizConverter
{
    public class VisualGraphCreator
    {
        private DiagramToDotConverter dotConverter = new DiagramToDotConverter();

        private GraphvizRunner runner = new GraphvizRunner();

        public IGraph<IVisualGraphNode>? GetVisualGraph(IGraph<IGraphNodeData> codeGraph)
        {


            IGraph<ICollapsableGraphNode> multilevelGraph =
                codeGraph.ConvertTo<ICollapsableGraphNode,
                    TreeNode<ICollapsableGraphNode>,
                    Flow<ICollapsableGraphNode>,
                    Graph<ICollapsableGraphNode>>(data => new CollapsableGraphNode()
                {
                    ChildrenCollapsed = false,
                    Data = data
                });

            string dotCode = dotConverter.ToDot(multilevelGraph);

            string json = runner.GetGraphAsJson(dotCode);

            Console.WriteLine(json);
            return null;
        }
    }
}