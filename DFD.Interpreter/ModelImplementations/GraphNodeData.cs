namespace DFD.Interpreter.ModelImplementations;

public enum NodeType
{
    Process,
    Storage,
    InputOutput
}

public class GraphNodeData
{
    public string Name { get; set; }
    public NodeType Type { get; set; }
}