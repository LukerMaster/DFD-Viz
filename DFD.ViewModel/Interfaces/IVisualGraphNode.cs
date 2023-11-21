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


public interface IVisualGraphNode
{
    ICollapsableGraphNode Node { get; }
    DisplayType Symbol { get; }
    Vector2 Position { get; }
    Vector2 Size { get; }
}