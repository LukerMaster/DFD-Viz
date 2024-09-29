using DFD.DataStructures.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphConverter.Interfaces;

public interface IVisualGraphCreator
{
    IVisualGraph GetVisualGraph(IGraph<ICollapsibleNodeData> logicalGraph);
}