﻿using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphConverter;

public class CollapsableGraphNode : ICollapsableGraphNode
{
    public bool CanBeCollapsed { get; set; } = true;
    public IGraphNodeData Data { get; set; }

    private bool _childrenCollapsed;
    public bool ChildrenCollapsed
    { 
        get => CanBeCollapsed ? _childrenCollapsed : false;
        set => _childrenCollapsed = CanBeCollapsed ? value : false;
    }
}