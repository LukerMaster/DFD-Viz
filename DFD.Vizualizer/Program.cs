using DFD.GraphConverter;
using DFD.Model.Interfaces;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace DFD.Vizualizer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Interpreter.Interpreter interpreter = new Interpreter.Interpreter();

            VisualGraphCreator creator = new VisualGraphCreator();

            var dfdString = File.ReadAllText("example-ml.dfd");

            IGraph<IGraphNodeData> graph = interpreter.ToDiagram(dfdString);

            var visualGraph = creator.GetVisualGraph(graph);
            var pngData = creator.GetPngGraph(graph);
            
            SFML.Graphics.Texture tex = new Texture(pngData);
            SFML.Graphics.Sprite graphSprite = new Sprite(tex);

            SFML.Graphics.RenderWindow w = new RenderWindow(new VideoMode(1200, 800), "DFD-Viz", Styles.Default);
            w.SetVerticalSyncEnabled(true);
            while (true)
            {
                w.Clear(Color.Black);
                w.DispatchEvents();
                w.Draw(graphSprite);
                foreach (var node in visualGraph.Nodes)
                {
                    SFML.Graphics.ConvexShape shape = new ConvexShape((uint)node.DrawPoints.Count);

                    for (int i = 0; i < node.DrawPoints.Count; i++)
                    {
                        shape.SetPoint((uint)i, new Vector2f(node.DrawPoints[i].X, node.DrawPoints[i].Y));
                    }
                    shape.FillColor = new Color(255, (byte)(node.DrawOrder * 70), (byte)(node.DrawOrder * 50));
                    w.Draw(shape);
                }

                w.Display();
            }
        }
    }
}