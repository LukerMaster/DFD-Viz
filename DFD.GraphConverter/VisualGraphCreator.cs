using DFD.GraphvizConverter;
using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphConverter
{
    public class VisualGraphCreator
    {
        private DiagramToDotConverter dotConverter = new DiagramToDotConverter();

        private GraphvizRunner runner = new GraphvizRunner();

        private JsonToGraphParser jsonToGraphParser = new JsonToGraphParser();

        private MultilevelGraphCreator multilevelGraphCreator = new MultilevelGraphCreator();

        public IVisualGraph GetVisualGraph(IGraph<IGraphNodeData> codeGraph)
        {
            var multilevelGraph = multilevelGraphCreator.CreateMultiLevelGraphOutOf(codeGraph);
            string dotCode = dotConverter.ToDot(multilevelGraph);
            string json = runner.GetGraphAsJson(dotCode);
            IVisualGraph visualGraph = jsonToGraphParser.CreateGraphFrom(json, multilevelGraph);
            return visualGraph;
        }

        public byte[] GetPngGraph(IGraph<IGraphNodeData> codeGraph)
        {
            var multilevelGraph = multilevelGraphCreator.CreateMultiLevelGraphOutOf(codeGraph);
            string dotCode = dotConverter.ToDot(multilevelGraph);
            return runner.GetGraphAsPng(dotCode);
        }
    }
}