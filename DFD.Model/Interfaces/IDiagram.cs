using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFD.Model.Interfaces
{
    public interface IDiagram
    {
        public ICollection<IGraphEntity> Entities { get; }
    }
}
