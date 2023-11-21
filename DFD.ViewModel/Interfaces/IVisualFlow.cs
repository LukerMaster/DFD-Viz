using System.Numerics;

namespace DFD.ViewModel.Interfaces;

public interface IVisualFlow
{
    string Label { get; }
    ICollection<Vector2> DrawPoints { get; }
}