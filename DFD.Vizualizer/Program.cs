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
            Parsing.Interpreter interpreter = new Parsing.Interpreter();
            MultilevelGraphConverter converter = new MultilevelGraphConverter();

            LogicalGraphLoader loader = new LogicalGraphLoader(interpreter, converter);

            var logicalGraph = loader.GetLogicalGraph(args[0]);

            VisualGraphCreator creator = new VisualGraphCreator();

            VisualGraphProvider provider = new VisualGraphProvider(logicalGraph, creator);


            SFML.Graphics.RenderWindow window = new RenderWindow(new VideoMode(640, 480), "DFD-Viz", Styles.Default);
            window.SetVerticalSyncEnabled(true);

            WindowManipulationHandler windowManipulationHandler = new WindowManipulationHandler(window);

            DiagramPresenter presenter = new DiagramPresenter(provider, window, windowManipulationHandler);

            bool shouldRun = true;

            window.Closed += (sender, eventArgs) => shouldRun = false;
            
            DiagramUI ui = new DiagramUI(window, provider, presenter);

            while (shouldRun)
            {
                window.DispatchEvents();
                ui.Process();
            }
        }
    }
}