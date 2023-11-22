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
    internal class DiagramLoader
    {
        private Interpreter.Interpreter interpreter = new Interpreter.Interpreter();
        private VisualGraphCreator creator = new VisualGraphCreator();

        public IDiagramModel ReadFromFile(string path)
        {
            DiagramModel model = new DiagramModel();
            
            var dfdString = File.ReadAllText(path);
            IGraph<IGraphNodeData> graph = interpreter.ToDiagram(dfdString);

            model.NodeGraph = creator.GetVisualGraph(graph);
            var pngData = creator.GetPngGraph(graph);

            Texture tex = new Texture(pngData);
            tex.Smooth = false;
            
            model.BgSprite = new Sprite(tex);
            model.BgSprite.Scale = new Vector2f(model.NodeGraph.Size.X / tex.Size.X, model.NodeGraph.Size.Y / tex.Size.Y);

            return model;
        }
    }
}
