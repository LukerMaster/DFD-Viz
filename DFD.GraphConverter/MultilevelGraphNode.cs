using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphConverter;

public class MultilevelGraphNode : IMultilevelGraphNode
{
    public bool IsHiddenAsParent { get; set; }
    public bool LockedFromCollapsing { get; set; } = false;
    public IGraphNodeData Data { get; init; }

    private bool _collapsed;
    public bool Collapsed
    { 
        get => !LockedFromCollapsing && Collapsable && _collapsed;
        set => _collapsed = value;
    }

    public bool Collapsable { get; set; }
}