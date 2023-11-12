using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFD
{
    public class Flow : IFlow
    {
        public string Name { get; init; }
        public IGraphEntity Source { get; init; }
        public IGraphEntity Target { get; init; }
    }
}
