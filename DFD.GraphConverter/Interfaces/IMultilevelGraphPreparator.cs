using DFD.DataStructures.Interfaces;

namespace DFD.GraphConverter.Interfaces;

public interface IMultilevelGraphPreparator
{
    void TweakCollapsability(IGraph<ICollapsibleNodeData> graph);
}