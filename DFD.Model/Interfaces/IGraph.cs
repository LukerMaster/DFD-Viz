using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFD.Model.Interfaces
{
    public interface IGraph
    {
        public IGraphEntity Root { get; }
        public ICollection<IFlow> Flows { get; }
    }
}
