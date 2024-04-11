using Avalonia;
using Avalonia.Media;

namespace DFD.AvaloniaEditor.ViewModels.AvaloniaGraph;

internal class AvaloniaPolygon
{
    internal Points Points { get; set; } = new();
    internal IBrush CurrentColor { get; set; }
    internal Color DefaultColor { get; set; } = new Color(255, 120, 240, 240);

    public AvaloniaPolygon()
    {
        CurrentColor = new SolidColorBrush(DefaultColor);
    }
}