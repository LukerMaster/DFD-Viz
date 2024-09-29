using System.Numerics;

namespace DFD.Vizualizer.Interfaces
{
    public interface IViewDataProvider
    {
        Vector2 Center { get; }
        Vector2 Size { get; }
    }
}
