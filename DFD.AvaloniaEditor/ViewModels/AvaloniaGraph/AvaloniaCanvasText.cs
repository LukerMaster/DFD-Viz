using Avalonia;
using DFD.ViewModel.Interfaces;

namespace DFD.AvaloniaEditor.ViewModels.AvaloniaGraph;

internal class AvaloniaCanvasText
{
    public string Text { get; set; }
    public Point Position { get; set; }
    public float FontSize { get; set; }

    public AvaloniaCanvasText(IVisualText visualText)
    {
        Text = visualText.Text;
        Position = new Point(visualText.Position.X, visualText.Position.Y);
        FontSize = visualText.FontSize;
    }
}