namespace DFD.Model.Interfaces;

public interface IFlow
{
    IGraphEntity Source { get; }
    IGraphEntity Target { get; }
}