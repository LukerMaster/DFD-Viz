using System.Diagnostics;
using DataStructure.NamedTree;
using DFD.Model.Interfaces;
using DFD.ModelImplementations;
using DFD.Parsing;
using DFD.Parsing.Interfaces;
using System.Xml.Linq;

namespace DFD.Tests.Parsing
{
    public class InterpreterTests
    {

        public IInterpreter tested = new Interpreter();

        public class NodePair
        {
            public NodePair(ITreeNode<IGraphNodeData> expected, ITreeNode<IGraphNodeData> generated)
            {
                this.generated = generated;
                this.expected = expected;
            }

            public ITreeNode<IGraphNodeData> generated;
            public ITreeNode<IGraphNodeData> expected;
        }

        public static ITreeNode<IGraphNodeData> MakeNode(string NodeName, string DisplayName, NodeType type)
        {
            return new TreeNode<IGraphNodeData>()
            {
                NodeName = NodeName,
                Data = new GraphNodeData()
                {
                    Name = DisplayName,
                    Type = type
                },
            };
        }

        public class TestCases : TheoryData<string, IGraph<IGraphNodeData>>
        {
            public TestCases()
            {
                Add("Process A", CreateMostBasicExample());
                Add("Process A\nProcess B", CreateExampleWithTwoProcesses());
                Add("Process A:\n\tProcess B", CreateSimpleMultilevelExample());
            }

            #region TestCaseBuilding

            private static IGraph<IGraphNodeData> CreateMostBasicExample()
            {
                ITreeNode<IGraphNodeData> root = MakeNode("root", "Graph Root", NodeType.Process);
                ITreeNode<IGraphNodeData> nodeA = MakeNode("A", "A", NodeType.Process);

                (root as IModifiableTreeNode<IGraphNodeData>).Children.Add(nodeA);
                IGraph<IGraphNodeData> graphExample = new Graph<IGraphNodeData>(root, new List<INodeFlow>());
                return graphExample;
            }
            private static IGraph<IGraphNodeData> CreateExampleWithTwoProcesses()
            {
                ITreeNode<IGraphNodeData> root = MakeNode("root", "Graph Root", NodeType.Process);
                ITreeNode<IGraphNodeData> nodeA = MakeNode("A", "A", NodeType.Process);
                ITreeNode<IGraphNodeData> nodeB = MakeNode("A", "B", NodeType.Process);
                (root as IModifiableTreeNode<IGraphNodeData>).Children.Add(nodeA);
                (root as IModifiableTreeNode<IGraphNodeData>).Children.Add(nodeB);


                IGraph<IGraphNodeData> graphExample = new Graph<IGraphNodeData>(root, new List<INodeFlow>());
                return graphExample;
            }
            private static IGraph<IGraphNodeData> CreateSimpleMultilevelExample()
            {
                ITreeNode<IGraphNodeData> root = MakeNode("root", "Graph Root", NodeType.Process);
                ITreeNode<IGraphNodeData> nodeA = MakeNode("A", "A", NodeType.Process);
                ITreeNode<IGraphNodeData> nodeB = MakeNode("B", "B", NodeType.Process);
                (root as IModifiableTreeNode<IGraphNodeData>).Children.Add(nodeA);
                (nodeA as IModifiableTreeNode<IGraphNodeData>).Children.Add(nodeB);


                IGraph<IGraphNodeData> graphExample = new Graph<IGraphNodeData>(root, new List<INodeFlow>());
                return graphExample;
            }

            #endregion

        }

        public class NodeComparator : IEqualityComparer<ITreeNode<IGraphNodeData>>
        {
            public bool Equals(ITreeNode<IGraphNodeData>? node1, ITreeNode<IGraphNodeData>? node2)
            {
                if (node1.FullNodeName != node2.FullNodeName) return false;
                if (node1.Data.Name != node2.Data.Name) return false;
                if (node1.Data.Type > node2.Data.Type) return false;
                return true;
            }

            public int GetHashCode(ITreeNode<IGraphNodeData> obj)
            {
                throw new NotImplementedException();
            }
        }
        

        [Theory]
        [ClassData(typeof(TestCases))]
        public void ShouldCreateGraphOutOfCorrectCode(string code, IGraph<IGraphNodeData> expected)
        {
            // Arrange
            // Act
            var generated = tested.ToDiagram(code);
            // Assert
            var nodePairs = new Queue<NodePair>();

            nodePairs.Enqueue(new NodePair(expected.Root, generated.Root));

            while (nodePairs.Count > 0)
            {
                var pair = nodePairs.Dequeue();

                Assert.Equal(pair.expected, pair.generated, new NodeComparator());
                Assert.Equal(pair.generated.Children.Count, pair.generated.Children.Count);

                var childPairs = pair.expected.Children.Zip(pair.generated.Children, (n1, n2) => new NodePair(n1, n2));
                foreach (var childPair in childPairs)
                {
                    nodePairs.Enqueue(childPair);
                }
            }
        }
    }
}