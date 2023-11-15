using DFD.Model.Interfaces;
using DFD.ViewModel;
using DFD.ViewModel.Interfaces;

namespace DFD.GraphvizConverter
{
    public class GraphvizGraphGenerator
    {
        public IVisualGraph ToVisualGraph(IGraph graph)
        {
            IEnumerable<IVisualGraphEntity> visualEntities =
                graph.Entities.Select(e => new VisualGraphEntity()
                {
                    Entity = e,
                    TransformableSymbol = new TransformableSymbol()
                });

            IVisualGraphEntity root = visualEntities.First();

            //while (root.Entity.Parent is not null)
            //{
            //    root = root.Entity.Parent;
            //}

            IVisualGraph visualGraph = new VisualGraph();
            return null;
        }
    }
}