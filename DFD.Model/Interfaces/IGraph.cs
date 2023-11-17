using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructure.NamedTree;

namespace DFD.Model.Interfaces
{
    public interface IGraph<T>
    {
        public ITreeNode<T> Root { get; protected set; }
        public ICollection<IFlow<T>> Flows { get; protected set; }

        public IGraph<TNew> ConvertTo<TNew, TTreeNodeImpl, TFlowImpl, TGraphImpl>(Func<T, TNew> dataConversionFunc)
            where TTreeNodeImpl : ITreeNode<TNew>, new()
            where TFlowImpl : IFlow<TNew>, new()
            where TGraphImpl : IGraph<TNew>, new()
        {
            var newRoot = Root.ConvertTo<TNew, TTreeNodeImpl>(dataConversionFunc);

            List<IFlow<TNew>> newFlows = new List<IFlow<TNew>>(Flows.Count);

            foreach (var flow in Flows)
            {
                var newFlow = new TFlowImpl();

                newFlow.Source = newRoot.FindClosestMatchingLeaf(flow.Source.FullEntityName);
                newFlow.Target = newRoot.FindClosestMatchingLeaf(flow.Target.FullEntityName);
                newFlow.BiDirectional = flow.BiDirectional;
                newFlow.FlowName = flow.FlowName;
                newFlows.Add(newFlow);
            }

            var newGraph = new TGraphImpl();
            newGraph.Flows = newFlows;
            newGraph.Root = newRoot;

            return newGraph;
        }
    }
}
