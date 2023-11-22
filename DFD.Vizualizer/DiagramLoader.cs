using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFD.GraphConverter;
using DFD.Interpreter;
using DFD.Model.Interfaces;
using DFD.Vizualizer.Model;
using SFML.Graphics;
using SFML.System;

namespace DFD.Vizualizer
{
    public class DiagramLoader
    {
        private Interpreter.Interpreter interpreter = new Interpreter.Interpreter();
        private MultilevelGraphConverter _multilevelGraphConverter = new MultilevelGraphConverter();
        private VisualGraphCreator creator = new VisualGraphCreator();

        public void ReloadModelDataBasedOn(IDiagramModel model)
        {
            model.NodeGraph = creator.GetVisualGraph(model.NodeGraph.LogicalGraph);
            model.BgSprite.Texture = new Texture(creator.GetPngGraph(model.NodeGraph.LogicalGraph));
        }

        public IDiagramModel ReadFromFile(string path)
        {
            DiagramModel model = new DiagramModel();
            
            var dfdString = File.ReadAllText(path);
            IGraph<IGraphNodeData> graph = interpreter.ToDiagram(dfdString);
            var multilevelGraph = _multilevelGraphConverter.CreateMultiLevelGraphOutOf(graph);
            model.NodeGraph = creator.GetVisualGraph(multilevelGraph);
            var pngData = creator.GetPngGraph(multilevelGraph);

            Texture tex = new Texture(pngData);
            tex.Smooth = false;
            
            model.BgSprite = new Sprite(tex);
            model.BgSprite.Scale = new Vector2f(model.NodeGraph.Size.X / tex.Size.X, model.NodeGraph.Size.Y / tex.Size.Y);

            return model;
        }
    }
}
