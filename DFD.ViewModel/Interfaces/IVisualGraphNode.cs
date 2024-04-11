using System.Numerics;

namespace DFD.ViewModel.Interfaces;


public interface IVisualGraphNode
{
    IMultilevelGraphNode Node { get; }
    IVisualObject VisualObject { get; }
}