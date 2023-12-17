using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using DFD.ViewModel.Interfaces;

namespace DFD.AvaloniaEditor.ViewModels;

public interface IAvaloniaVisualGraph
{
    public IReadOnlyCollection<Polygon> Nodes { get; }
    public IReadOnlyCollection<Polyline> Flows { get; }
    public IReadOnlyCollection<Polygon> ArrowHeads { get; }
    public IReadOnlyCollection<AvaloniaCanvasText> TextLabels { get; }
}

class AvaloniaVisualGraph : IAvaloniaVisualGraph
{
    public IReadOnlyCollection<Polygon> Nodes { get; set; }
    public IReadOnlyCollection<Polyline> Flows { get; set; }
    public IReadOnlyCollection<Polygon> ArrowHeads { get; set; }
    public IReadOnlyCollection<AvaloniaCanvasText> TextLabels { get; set; }

    public AvaloniaVisualGraph(IVisualGraph visualGraph)
    {
        // TODO: UZUPEŁNIJ
    }
}

public class AvaloniaCanvasText
{
    public string Text { get; }
    public Point Position { get; }
    public float FontSize { get; }

    public AvaloniaCanvasText(IVisualText visualText)
    {
        Text = visualText.Text;
        Position = new Point(visualText.Position.X, visualText.Position.Y);
        FontSize = visualText.FontSize;
    }
}