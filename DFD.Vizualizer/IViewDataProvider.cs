using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DFD.Vizualizer
{
    public interface IViewDataProvider
    {
        Vector2 Center { get; }
        Vector2 Size { get; }
    }
}
