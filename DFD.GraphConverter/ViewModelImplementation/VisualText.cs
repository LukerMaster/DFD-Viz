using System.Numerics;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphConverter.ViewModelImplementation;

public class VisualText : IVisualText
{
    public string Text { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Origin { get; set; }
    public float FontSize { get; set; }
    public float Width { get; set; }
}