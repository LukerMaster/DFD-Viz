using System.Drawing;

namespace DFD.Model.Interfaces;

public enum DisplayType
{
    Rectangle,
    Circle,
    DbSymbol,
    IOSymbol
}


public interface IVisualSymbol
{
    DisplayType DisplayType { get; }
    Color Color { get; }
    string DisplayedText { get; }
}