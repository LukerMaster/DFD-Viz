using System.Drawing;
using System.Numerics;

namespace DFD.ViewModel.Interfaces;

public enum DisplayType
{
    Rectangle,
    Circle,
    DbSymbol,
    IOSymbol
}


public interface ITransformableSymbol
{
    bool HasChildrenCollapsed { get; set; }
    Vector2 Position { get; set; }
    Vector2 Size { get; set; }
}

class TransformableSymbol : ITransformableSymbol
{
    public bool HasChildrenCollapsed { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Size { get; set; }
}