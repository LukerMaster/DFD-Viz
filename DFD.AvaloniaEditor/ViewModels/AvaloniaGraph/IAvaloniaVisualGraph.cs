using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;

namespace DFD.AvaloniaEditor.ViewModels.AvaloniaGraph;

internal interface IAvaloniaVisualGraph
{
    public IReadOnlyCollection<AvaloniaVisualNode> Nodes { get; }
    public IReadOnlyCollection<AvaloniaVisualObject> Flows { get; }
    public IReadOnlyCollection<AvaloniaVisualObject> ArrowHeads { get; }
    public IReadOnlyCollection<AvaloniaCanvasText> TextLabels { get; }
}