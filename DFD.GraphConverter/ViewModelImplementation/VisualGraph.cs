using DFD.ViewModel.Interfaces;

namespace DFD.GraphConverter.ViewModelImplementation;

public class VisualGraph : IVisualGraph
{
    public IReadOnlyCollection<IVisualGraphNode> Nodes { get; set; }
    public IReadOnlyCollection<IVisualFlow> Flows { get; set; }
}