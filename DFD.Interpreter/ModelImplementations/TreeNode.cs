using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructure.NamedTree;
using DFD.Model;
using DFD.Model.Interfaces;

namespace DFD
{
    public class TreeNode<T> : ITreeNode<T>
    {
        public string EntityName { get; init; }
        public T Data { get; set; }
        public ITreeNode<T> Parent { get; set; }
        public ICollection<ITreeNode<T>> Children { get; set; } = new List<ITreeNode<T>>();

    }
}
