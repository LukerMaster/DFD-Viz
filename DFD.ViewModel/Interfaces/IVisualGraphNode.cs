using DFD.DataStructures.Interfaces;

namespace DFD.ViewModel.Interfaces;


public interface IVisualGraphNode
{
    ICollapsibleNodeData Node { get; }
    IVisualObject VisualObject { get; }
}