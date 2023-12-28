using DFD.ViewModel.Interfaces;

namespace DFD.AvaloniaEditor.ViewModels.AvaloniaGraph;

internal class AvaloniaVisualNode
{
    internal AvaloniaVisualObject VisualObject { get; set; } = new();
    internal IEditableGraphNode Node { get; set; }
}