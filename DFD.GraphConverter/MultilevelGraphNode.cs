using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphConverter;

public class MultilevelGraphNode : IMultilevelGraphNode
{
    public bool IsHiddenAsParent { get; set; }
    public IGraphNodeData Data { get; init; }
    public bool Collapsible { get; set; }

    private bool _collapsed;
    public bool Collapsed
    { 
        get => Collapsible && _collapsed;
        set => _collapsed = value;
    }
}