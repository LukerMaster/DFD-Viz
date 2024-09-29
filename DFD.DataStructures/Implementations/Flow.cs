using DFD.DataStructures.Interfaces;

namespace DFD.DataStructures.Implementations;

public class Flow<T> : IFlow<T>
{
    public required INodeRef<T> Source { get; set; }
    public required INodeRef<T> Target { get; set; }
    public required string Name { get; set; }
    public bool IsBidirectional { get; set; }
}