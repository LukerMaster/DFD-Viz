using Avalonia;
using DataStructure.NamedTree;
using DFD.AvaloniaEditor.Interfaces;
using DFD.GraphConverter.Interfaces;
using DFD.Parsing.Interfaces;
using DFD.ViewModel.Interfaces;
using System.Collections.Generic;
using DFD.Model.Interfaces;

namespace DFD.AvaloniaEditor.Services;

public class VisualGraphGenerationPipeline : IVisualGraphGenerationPipeline
{
    private readonly IVisualGraphCreator _graphCreator;
    private readonly IInterpreter _interpreter;
    private readonly IMultilevelGraphConverter _converter;
    private readonly IDfdCodeStringProvider _dfdCodeProvider;


    private IGraph<ICollapsableGraphNode> _logicalGraph;
    private IVisualGraph _visualGraph;
    
    public VisualGraphGenerationPipeline(IInterpreter interpreter, IMultilevelGraphConverter converter, IVisualGraphCreator graphCreator, IDfdCodeStringProvider dfdCodeProvider)
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
        if (_logicalGraph is not null)
        {
            _visualGraph = _graphCreator.GetVisualGraph(_logicalGraph);
        }
    }

    private void RecompileEntireGraph()
    {
        if (!string.IsNullOrEmpty(_dfdCodeProvider.DfdCode))
        {
            var logicalGraph = _interpreter.ToDiagram(_dfdCodeProvider.DfdCode);
            _logicalGraph = _converter.ToMultilevelGraph(logicalGraph);
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
}