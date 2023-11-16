using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructure.NamedTree;
using DFD.Model;
using DFD.Model.Interfaces;

namespace DFD
{
    public class Flow<T> : IFlow<T>
    {
        public ITreeNode<T> Source { get; init; }
        public ITreeNode<T> Target { get; init; }
        public string DisplayedText { get; init; }
    }
}
