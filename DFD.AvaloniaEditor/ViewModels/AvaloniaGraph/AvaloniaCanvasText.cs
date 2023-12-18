using Avalonia;
using DFD.ViewModel.Interfaces;

namespace DFD.AvaloniaEditor.ViewModels.AvaloniaGraph;

internal class AvaloniaCanvasText
{
    public string Text { get; set; }
    public Point TopLeft => new Point(Center.X - (Width / 2), Center.Y - (FontSize / 2));
    public Point Center { get; set; }
    public float FontSize { get; set; }
    public float Width { get; set; }

    public AvaloniaCanvasText(IVisualText visualText)
    {
        Text = visualText.Text;
        Center = new Point(visualText.Position.X, visualText.Position.Y);
        FontSize = visualText.FontSize;
        Width = visualText.Width;
    }
}