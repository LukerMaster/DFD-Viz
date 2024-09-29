namespace DFD.DataStructures.Interfaces;

public interface IFlow<T> : IFlowRef<T>
{
    new string Name { get; set; }
    new bool IsBidirectional { get; set; }
}