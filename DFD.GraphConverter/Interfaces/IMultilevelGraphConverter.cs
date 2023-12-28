using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphConverter.Interfaces;

public interface IMultilevelGraphConverter
{
    IGraph<IEditableGraphNode> ToMultilevelGraph(IGraph<IGraphNodeData> graph);
}