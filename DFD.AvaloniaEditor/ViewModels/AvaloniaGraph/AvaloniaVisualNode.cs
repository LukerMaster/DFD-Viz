using DFD.ViewModel.Interfaces;

namespace DFD.AvaloniaEditor.ViewModels.AvaloniaGraph;

internal class AvaloniaVisualNode
{
    internal AvaloniaPolygon Polygon { get; set; } = new();
    internal IMultilevelGraphNode Node { get; set; }
}