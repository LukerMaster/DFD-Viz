namespace DFD.DataStructures.Interfaces;

public enum NodeType
{
    Process,
    Storage,
    InputOutput
}

public interface INodeData
{
    public string DisplayedName { get; }
    public NodeType Type { get; }
}