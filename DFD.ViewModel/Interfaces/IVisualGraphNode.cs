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
    string DisplayedText { get; set; }
    DisplayType Symbol { get; set; }
    bool HasChildrenCollapsed { get; set; }
    Vector2 Position { get; set; }
    Vector2 Size { get; set; }
}