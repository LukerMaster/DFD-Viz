namespace DFD.Model.Interfaces;

public enum NodeType
{
    Process,
    Storage,
    InputOutput
}

public interface IGraphNodeData
{
    public string Name { get; }
    public NodeType Type { get; }
}