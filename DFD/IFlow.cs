namespace DFD;

public interface IFlow : IGraphEntity
{
    IGraphEntity Source { get; }
    IGraphEntity Target { get; }
}