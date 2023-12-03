using DFD.GraphvizConverter;
using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphConverter
{
    public class VisualGraphCreator : IVisualGraphCreator
    {
        private DiagramToDotConverter dotConverter = new DiagramToDotConverter();

        private GraphvizRunner runner = new GraphvizRunner();

        private JsonToGraphParser jsonToGraphParser = new JsonToGraphParser();

        private MultilevelGraphConverter _multilevelGraphConverter = new MultilevelGraphConverter();

        public IVisualGraph GetVisualGraph(IGraph<ICollapsableGraphNode> logicalGraph)
        {
            string dotCode = dotConverter.ToDot(logicalGraph);
            string json = runner.GetGraphAsJson(dotCode);
            IVisualGraph visualGraph = jsonToGraphParser.CreateGraphFrom(json, logicalGraph);
            return visualGraph;
        }
    }
}