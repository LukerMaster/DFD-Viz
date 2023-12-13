using Avalonia;
using DataStructure.NamedTree;
using DFD.AvaloniaEditor.Interfaces;
using DFD.GraphConverter.Interfaces;
using DFD.Parsing.Interfaces;
using DFD.ViewModel.Interfaces;
using System.Collections.Generic;
using DFD.Model.Interfaces;

namespace DFD.AvaloniaEditor.Services;

public class VisualGraphProvider : IVisualGraphProvider
{
    private readonly IVisualGraphCreator _graphCreator;
    private readonly IInterpreter _interpreter;
    private readonly IMultilevelGraphConverter _converter;
    private readonly IDfdCodeStringProvider _dfdCodeProvider;


    private IGraph<ICollapsableGraphNode> _logicalGraph;
    private IVisualGraph _visualGraph;

    private string _previousDfdCode;
    private Dictionary<ICollapsableGraphNode, bool> _previousCollapsedStates = new Dictionary<ICollapsableGraphNode, bool>();

    public VisualGraphProvider(IInterpreter interpreter, IMultilevelGraphConverter converter, IVisualGraphCreator graphCreator, IDfdCodeStringProvider dfdCodeProvider)
    {
        _graphCreator = graphCreator;
        _interpreter = interpreter;
        _converter = converter;
        _dfdCodeProvider = dfdCodeProvider;

        if (!string.IsNullOrEmpty(dfdCodeProvider.DfdCode))
        {
            RecompileEntireGraph();
        }
    }

    private void RegenerateVisualGraph()
    {
        _visualGraph = _graphCreator.GetVisualGraph(_logicalGraph);
        UpdatePreviousCollapsedStates(_logicalGraph.Root);
    }

    private void RecompileEntireGraph()
    {
        var logicalGraph = _interpreter.ToDiagram(_dfdCodeProvider.DfdCode);
        _logicalGraph = _converter.ToMultilevelGraph(logicalGraph);
        UpdatePreviousDfdCode();

        RegenerateVisualGraph();
    }

    private void UpdatePreviousDfdCode()
    {
        _previousDfdCode = _dfdCodeProvider.DfdCode;
    }
    private void UpdatePreviousCollapsedStates(ITreeNode<ICollapsableGraphNode> node)
    {
        _previousCollapsedStates[node.Data] = node.Data.ChildrenCollapsed;
        foreach (var child in node.Children)
        {
            UpdatePreviousCollapsedStates(child);
        }
    }


    private bool DidDfdCodeChange()
    {
        return _previousDfdCode != _dfdCodeProvider.DfdCode;
    }
    private bool DidAnyNodeCollapseStateChange()
    {
        foreach (var node in _visualGraph.Nodes)
        {
            if (node.Node.ChildrenCollapsed != _previousCollapsedStates[node.Node])
                return true;
        }

        return false;
    }



    
    public IVisualGraph? VisualGraph {
        get
        {
            if (DidDfdCodeChange())
            {
                RecompileEntireGraph();
            }
            else if (DidAnyNodeCollapseStateChange())
            {
                RegenerateVisualGraph();
            }

            return _visualGraph;
        }
    }
}