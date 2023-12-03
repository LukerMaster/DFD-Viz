using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphConverter;

public interface IVisualGraphCreator
{
    IVisualGraph GetVisualGraph(IGraph<ICollapsableGraphNode> logicalGraph);
}