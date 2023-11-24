using System.Numerics;

namespace DFD.ViewModel.Interfaces;

public interface IVisualObject
{
    IReadOnlyList<Vector2> Points { get; }
    DrawTechnique DrawTechnique { get; }
    /// <summary>
    /// If true - points forms a loop, effectively creating a shape, if false - it's a line.
    /// </summary>
    bool IsClosed { get; }
}

public enum DrawTechnique
{
    Straight,
    Bezier
}