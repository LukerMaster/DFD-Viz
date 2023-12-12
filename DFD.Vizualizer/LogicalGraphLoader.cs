using DFD.GraphConverter;
using DFD.Model.Interfaces;
using DFD.ViewModel.Interfaces;

namespace DFD.Vizualizer;

public class LogicalGraphLoader
{
    private readonly Parsing.Interpreter _interpreter;
    private readonly MultilevelGraphConverter _converter;

    public LogicalGraphLoader(Parsing.Interpreter interpreter, MultilevelGraphConverter converter)
    {
        _interpreter = interpreter;
        _converter = converter;
    }

    public IGraph<ICollapsableGraphNode> GetLogicalGraph(string filePath)
    {
        var dfdCode = File.ReadAllText(filePath);
        var rawGraph = _interpreter.ToDiagram(dfdCode);
        var collapsableGraph = _converter.ToMultilevelGraph(rawGraph);
        return collapsableGraph;
    }
}