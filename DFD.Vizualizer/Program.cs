using DFD.GraphvizConverter;
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

            var dfdString = File.ReadAllText("documentation.dfd");

            IGraph<IGraphNodeData> graph = interpreter.ToDiagram(dfdString);

            var visualGraph = new VisualGraphCreator().GetVisualGraph(graph);

            SFML.Graphics.RenderWindow w = new RenderWindow(new VideoMode(1200, 800), "DFD-Viz", Styles.Default);
            w.SetVerticalSyncEnabled(true);
            while (true)
            {
                w.Clear(Color.Black);
                w.DispatchEvents();
                
                foreach (var node in visualGraph.Nodes)
                {
                    SFML.Graphics.ConvexShape shape = new ConvexShape((uint)node.DrawPoints.Count);

                    for (int i = 0; i < node.DrawPoints.Count; i++)
                    {
                        shape.SetPoint((uint)i, new Vector2f(node.DrawPoints[i].X, node.DrawPoints[i].Y));
                    }
                    shape.FillColor = node.Node.Data.Type == NodeType.Process ? Color.Green : Color.Cyan;
                    w.Draw(shape);
                }

                w.Display();
            }
        }
    }
}