using System.Numerics;

namespace DFD.ViewModel.Interfaces;


public interface IVisualGraphNode
{
    ICollapsableGraphNode Node { get; }
    IVisualObject VisualObject { get; }
}