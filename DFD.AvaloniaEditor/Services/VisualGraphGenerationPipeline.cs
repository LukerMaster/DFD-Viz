using System;
using DFD.AvaloniaEditor.Interfaces;
using DFD.GraphConverter.Interfaces;
using DFD.Parsing.Interfaces;
using DFD.ViewModel.Interfaces;
using System.Collections.Generic;
using DFD.DataStructures.Interfaces;
using ICollapsibleNodeData = DFD.DataStructures.Interfaces.ICollapsibleNodeData;

namespace DFD.AvaloniaEditor.Services;

public class VisualGraphGenerationPipeline : IVisualGraphGenerationPipeline
{
    private readonly IVisualGraphCreator _graphCreator;
    private readonly IInterpreter<ICollapsibleNodeData> _interpreter;
    private readonly IMultilevelGraphPreparator _preparator;
    private readonly IDfdCodeStringProvider _dfdCodeProvider;


    private IGraph<ICollapsibleNodeData> _logicalGraph;
    private IVisualGraph _visualGraph;
    
    public VisualGraphGenerationPipeline(IInterpreter<ICollapsibleNodeData> interpreter, IMultilevelGraphPreparator preparator, IVisualGraphCreator graphCreator, IDfdCodeStringProvider dfdCodeProvider)
    {
        _graphCreator = graphCreator;
        _interpreter = interpreter;
        _preparator = preparator;
        _dfdCodeProvider = dfdCodeProvider;

        if (!string.IsNullOrEmpty(dfdCodeProvider.DfdCode))
        {
            RecompileEntireGraph();
        }
    }

    private void RegenerateVisualGraph()
    {
        if (_logicalGraph is not null)
        {
            _visualGraph = _graphCreator.GetVisualGraph(_logicalGraph);
        }
    }

    private void RecompileEntireGraph()
    {
        if (!string.IsNullOrEmpty(_dfdCodeProvider.DfdCode))
        {
            _logicalGraph = _interpreter.ToDiagram(_dfdCodeProvider.DfdCode);
            _preparator.TweakCollapsability(_logicalGraph);
            RegenerateVisualGraph();
        }
    }

    public IVisualGraph? VisualGraph {
        get
        {
            return _visualGraph;
        }
    }

    public IVisualGraph RecompiledGraph
    {
        get
        {
            RecompileEntireGraph();
            return VisualGraph;
        }
    }

    public IVisualGraph RefreshedGraph
    {
        get
        {
            RegenerateVisualGraph();
            return VisualGraph;
        }
    }

    public void ExecuteOnNode(string nodeName, Action<ICollapsibleNodeData> command)
    {
        Queue<INodeRef<ICollapsibleNodeData>> nodes = new();
        
        nodes.Enqueue(_logicalGraph.Root);
        while (nodes.Count > 0)
        {
            var node = nodes.Dequeue();
            if (node.FullPath == nodeName)
            {
                command(node.Data);
                break;
            }

            foreach (var nodeChild in node.Children)
            {
                nodes.Enqueue(nodeChild);
            }
        }
    }

}