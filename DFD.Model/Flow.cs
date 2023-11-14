using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFD.Model;
using DFD.Model.Interfaces;

namespace DFD
{
    public class Flow : IFlow
    {
        public IGraphEntity Source { get; init; }
        public IGraphEntity Target { get; init; }
        public string DisplayedText { get; init; }
    }
}
