using DFD.DataStructures.Interfaces;
using DFD.GraphConverter;

namespace DFD.Vizualizer;

public class LogicalGraphLoader
{
    private readonly Parsing.Interpreter<ICollapsibleNodeData> _interpreter;
    private readonly MultilevelGraphPreparator _preparator;

    public LogicalGraphLoader(Parsing.Interpreter<ICollapsibleNodeData> interpreter, MultilevelGraphPreparator preparator)
    {
        _interpreter = interpreter;
        _preparator = preparator;
    }

    public IGraph<ICollapsibleNodeData> GetLogicalGraph(string filePath)
    {
        var dfdCode = File.ReadAllText(filePath);
        var graph = _interpreter.ToDiagram(dfdCode);
        _preparator.TweakCollapsability(graph);
        return graph;
    }
}