using DFD.Model.Interfaces;

namespace DFD.ViewModel.Interfaces
{
    public interface IGraphSymbol
    {
        public IGraphEntity Entity { get; set; }
        public ITransformableSymbol TransformableSymbol { get; set; }
    }
}