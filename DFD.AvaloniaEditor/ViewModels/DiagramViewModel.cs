using Avalonia.Controls;
using DFD.AvaloniaEditor.Interfaces;
using DFD.AvaloniaEditor.ViewModels.AvaloniaGraph;

namespace DFD.AvaloniaEditor.ViewModels;

internal class DiagramViewModel : ViewModelBase
{
    private readonly IVisualGraphGenerationPipeline _diagramGenerationPipeline;

    private AvaloniaVisualGraph _visualGraph;
    public AvaloniaVisualGraph VisualGraph {
        get => _visualGraph;
        set
        {
            if (_visualGraph != value)
            {
                _visualGraph = value;
                OnPropertyChanged();
            }
        }
    }

    public Panel MainPanel { get; set; }

    public DiagramViewModel(IVisualGraphGenerationPipeline diagramGenerationPipeline)
    {
        _diagramGenerationPipeline = diagramGenerationPipeline;
        VisualGraph = new AvaloniaVisualGraph(diagramGenerationPipeline.RecompiledGraph);
    }

    public void ReconstructGraph()
    {
        VisualGraph = new AvaloniaVisualGraph(_diagramGenerationPipeline.RefreshedGraph);
    }
    public void RecompileGraph()
    {
        VisualGraph = new AvaloniaVisualGraph(_diagramGenerationPipeline.RecompiledGraph);
    }

    public void ToggleVisibility(string nodeName)
    {
        if (VisualGraph.Nodes.Count > 0)
            _diagramGenerationPipeline.ExecuteOnNode(nodeName, node => node.IsHiddenAsParent = !node.IsHiddenAsParent);
        ReconstructGraph();
    }
}