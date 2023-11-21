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

        private JsonToGraphParser jsonToGraphParser = new JsonToGraphParser();

        public IVisualGraph GetVisualGraph(IGraph<IGraphNodeData> codeGraph)
        {
            IGraph<ICollapsableGraphNode> multilevelGraph =
                codeGraph.CopyGraphAs<ICollapsableGraphNode>(data => new CollapsableGraphNode()
                {
                    Data = data,
                    ChildrenCollapsed = false
                });

            string dotCode = dotConverter.ToDot(multilevelGraph);
            
            string json = runner.GetGraphAsJson(dotCode);

            IVisualGraph visualGraph = jsonToGraphParser.CreateGraphFrom(json, multilevelGraph);
            //Console.WriteLine(json);
            return visualGraph;
        }
    }
}