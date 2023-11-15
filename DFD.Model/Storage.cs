using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFD.Model.Interfaces;

namespace DFD
{
    public class Storage : IGraphEntity
    {
        public string EntityName { get; init; }
        public string DisplayedName { get; set; }
        public IGraphEntity Parent { get; set; }
        public ICollection<IGraphEntity> Children { get; set; } = new List<IGraphEntity>();
    }
}
