using DFD.ViewModel.Interfaces;

namespace DFD.Vizualizer;

public interface IVisualGraphProvider
{
    IVisualGraph VisualGraph { get; }
}