using System.Numerics;

namespace DFD.ViewModel.Interfaces;

public interface IVisualText
{
    public string Text { get; }
    public Vector2 Position { get; }
    /// <summary>
    /// Origin of transformation. If set to 0.5, 0.5, then <see cref="Position"/> means the center of the text.
    /// </summary>
    public Vector2 Origin { get; }
    public float FontSize { get; }
    public float Width { get; }
}