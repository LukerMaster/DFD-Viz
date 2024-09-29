using DataStructure.NamedTree;

namespace DFD.Model.Interfaces;

/// <summary>
/// Represents a flow of data between two objects
/// </summary>
/// <typeparam name="T">Type of the object to create flow between</typeparam>
public interface INodeFlow<T>
{
    T Source { get; set; }
    T Target { get; set; }
    bool BiDirectional { get; set; }
    string? FlowName { get; set; }
}