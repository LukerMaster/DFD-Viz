﻿using DFD.DataStructures.Interfaces;
using DFD.GraphConverter;
using DFD.GraphConverter.Interfaces;
using DFD.GraphvizConverter;
using DFD.Parsing;
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

            Parsing.Interpreter<ICollapsibleNodeData> interpreter = new(new NodeDataFactory());
            MultilevelGraphPreparator preparator = new MultilevelGraphPreparator();
            LogicalGraphLoader loader = new LogicalGraphLoader(interpreter, preparator);

            IGraph<ICollapsibleNodeData> logicalGraph = null;
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

            IVisualGraphCreator creator = new VisualGraphCreator(new GraphvizRunnerFactory(Environment.OSVersion.Platform).CreateRunner());

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