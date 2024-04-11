using System.Collections.Generic;
using System.Numerics;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Vector = Avalonia.Vector;

namespace DFD.AvaloniaEditor.ViewModels.AvaloniaGraph;

internal interface IAvaloniaVisualGraph
{
    public IReadOnlyCollection<AvaloniaVisualNode> Nodes { get; }
    public IReadOnlyCollection<AvaloniaPolygon> Flows { get; }
    public IReadOnlyCollection<AvaloniaPolygon> ArrowHeads { get; }
    public IReadOnlyCollection<AvaloniaCanvasText> TextLabels { get; }
    public Vector Size { get; }
}