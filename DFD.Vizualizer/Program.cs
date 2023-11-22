using DFD.GraphConverter;
using DFD.Model.Interfaces;
using DFD.Vizualizer.Model;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace DFD.Vizualizer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IDiagramModel model = new DiagramLoader().ReadFromFile(args[0]);

            SFML.Graphics.RenderWindow w = new RenderWindow(new VideoMode(640, 480), "DFD-Viz", Styles.Default);
            w.SetVerticalSyncEnabled(true);

            bool shouldRun = true;

            w.Closed += (sender, eventArgs) => shouldRun = false;
            
            DiagramUI ui = new DiagramUI(w, model);

            while (shouldRun)
            {
                w.DispatchEvents();
                ui.Process();
            }
        }
    }
}