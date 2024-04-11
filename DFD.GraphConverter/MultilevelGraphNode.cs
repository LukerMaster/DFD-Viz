using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphConverter;

public class MultilevelGraphNode : IMultilevelGraphNode
{
    public bool IsHiddenAsParent { get; set; }
    public bool LockedFromCollapsing { get; set; } = true;
    public IGraphNodeData Data { get; set; }

    private bool _childrenCollapsed;
    public bool Collapsed
    { 
        get => LockedFromCollapsing ? false : _childrenCollapsed;
        set => _childrenCollapsed = LockedFromCollapsing ? false : value;
    }

    public bool Collapsable { get; set; }
}