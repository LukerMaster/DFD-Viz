using Avalonia;
using Avalonia.Media;

namespace DFD.AvaloniaEditor.ViewModels.AvaloniaGraph;

internal class AvaloniaPolygon
{
    internal Points Points { get; set; } = new();
    internal Color Color { get; set; } = new();
}