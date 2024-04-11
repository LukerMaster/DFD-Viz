using DFD.ViewModel.Interfaces;

namespace DFD.AvaloniaEditor.ViewModels.AvaloniaGraph;

internal class AvaloniaVisualNode
{
    internal AvaloniaPolygon Polygon { get; set; } = new();
    internal IEditableGraphNode Node { get; set; }
}