using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFD.ViewModel.Interfaces
{
    public interface IVisualGraph
    {
        IReadOnlyCollection<IVisualGraphNode> Nodes { get; }
        IReadOnlyCollection<IVisualFlow> Flows { get; }
    }
}
