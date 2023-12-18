using DFD.ViewModel.Interfaces;

namespace DFD.AvaloniaEditor.ViewModels.AvaloniaGraph;

internal class AvaloniaVisualNode
{
    internal AvaloniaVisualObject VisualObject { get; set; } = new();
    internal ICollapsableGraphNode Node { get; set; }
}