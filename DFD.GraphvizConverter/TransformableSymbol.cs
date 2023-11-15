using System.Numerics;
using DFD.ViewModel.Interfaces;

namespace DFD.ViewModel;

public class TransformableSymbol : ITransformableSymbol
{
    public bool HasChildrenCollapsed { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Size { get; set; }
}