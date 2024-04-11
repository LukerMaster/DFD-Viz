using DFD.GraphConverter;
using DFD.GraphConverter.Interfaces;
using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;
using DFD.Vizualizer.Interfaces;
using SFML.Graphics;
using SFML.Window;

namespace DFD.Vizualizer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SFML.Graphics.RenderWindow window = new RenderWindow(new VideoMode(640, 480), "DFD-Viz", Styles.Default);
            window.SetVerticalSyncEnabled(true);
            bool shouldRun = true;
            window.Closed += (sender, eventArgs) => shouldRun = false;

            Parsing.Interpreter interpreter = new Parsing.Interpreter();
            MultilevelGraphConverter converter = new MultilevelGraphConverter();
            LogicalGraphLoader loader = new LogicalGraphLoader(interpreter, converter);

            IGraph<IMultilevelGraphNode> logicalGraph = null;
            IProgramUI ui = null;

            try
            {
                logicalGraph = loader.GetLogicalGraph(args[0]);
            }
            catch (AggregateException e)
            {
                var errorData = new ErrorViewData(e);
                var viewManipulator = new WindowViewManipulator(window, errorData);
                ui = new ErrorUI(window, errorData, viewManipulator);

                while (shouldRun)
                {
                    window.DispatchEvents();
                    ui.Process();
                }

                return;
            }

            IVisualGraphCreator creator = new VisualGraphCreator();

            VisualGraphProvider provider = new VisualGraphProvider(logicalGraph, creator);

            WindowViewManipulator windowViewManipulator = new WindowViewManipulator(window, provider);

            IDiagramPresenter presenter = new DiagramPresenter(provider, window, windowViewManipulator);

            ui = new ProgramUI(window, provider, presenter);

            while (shouldRun)
            {
                window.DispatchEvents();
                ui.Process();
            }
        }
    }
}