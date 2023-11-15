using DFD.ViewModel.Interfaces;

namespace DFD.ViewModel;

public class VisualGraph : IVisualGraph
{
    public IVisualGraphEntity VisualGraphRoot { get; set; }
}