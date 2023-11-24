using System.Numerics;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphConverter.ViewModelImplementation;

public class VisualObject : IVisualObject
{
    public IReadOnlyList<Vector2> Points { get; set; }
    public DrawTechnique DrawTechnique { get; set; }
    public bool IsClosed { get; set; }
    public int DrawOrder { get; set; }
}