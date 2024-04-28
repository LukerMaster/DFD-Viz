using DFD.ViewModel.Interfaces;

namespace DFD.Vizualizer.Interfaces;

public interface IVisualGraphProvider
{
    IVisualGraph VisualGraph { get; }
}