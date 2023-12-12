using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphConverter.Interfaces;

public interface IVisualGraphCreator
{
    IVisualGraph GetVisualGraph(IGraph<ICollapsableGraphNode> logicalGraph);
}