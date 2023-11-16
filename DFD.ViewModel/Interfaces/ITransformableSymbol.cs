using System.Drawing;
using System.Numerics;

namespace DFD.ViewModel.Interfaces;

public enum DisplayType
{
    Rectangle,
    Circle,
    DbSymbol,
    IoSymbol
}


public interface ITransformableSymbol
{
    bool HasChildrenCollapsed { get; set; }
    Vector2 Position { get; set; }
    Vector2 Size { get; set; }
}