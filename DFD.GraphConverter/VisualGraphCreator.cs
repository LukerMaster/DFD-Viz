﻿using DFD.DataStructures.Interfaces;
using DFD.GraphConverter.Interfaces;
using DFD.GraphvizConverter;
using DFD.GraphvizConverter.API;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphConverter
{
    public class VisualGraphCreator : IVisualGraphCreator
    {
        private DiagramToDotConverter dotConverter = new DiagramToDotConverter();

        private IGraphvizRunner runner;

        private JsonToGraphParser jsonToGraphParser = new JsonToGraphParser();

        private MultilevelGraphPreparator _multilevelGraphPreparator = new MultilevelGraphPreparator();

        public VisualGraphCreator(IGraphvizRunner runner)
        {
            this.runner = runner;
        }

        public IVisualGraph GetVisualGraph(IGraph<ICollapsibleNodeData> logicalGraph)
        {
            string dotCode = dotConverter.ToDot(logicalGraph);
            string json = runner.GetGraphAsJson(dotCode);
            IVisualGraph visualGraph = jsonToGraphParser.CreateGraphFrom(json, logicalGraph);
            return visualGraph;
        }
    }
}