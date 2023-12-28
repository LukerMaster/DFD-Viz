using System.Numerics;

namespace DFD.ViewModel.Interfaces;


public interface IVisualGraphNode
{
    IEditableGraphNode Node { get; }
    IVisualObject VisualObject { get; }
}