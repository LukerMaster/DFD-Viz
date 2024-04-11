using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphConverter.Interfaces;

public interface IMultilevelGraphConverter
{
    IGraph<IMultilevelGraphNode> ToMultilevelGraph(IGraph<IGraphNodeData> graph);
}