using System.Text.RegularExpressions;
using DataStructure.NamedTree;
using DFD.Model.Interfaces;
using DFD.ModelImplementations;
using DFD.Parsing.Interfaces;

namespace DFD.Parsing
{
    public class Interpreter : IInterpreter
    {
        private enum StatementType
        {
            SimpleNodeDeclaration,
            NestedProcessDeclaration,
            FlowDeclaration
        }

        private readonly Dictionary<StatementType, Regex> _regexes = new()
        {
            {
                StatementType.SimpleNodeDeclaration,
                new Regex("^\\s*[a-zA-Z][a-zA-Z0-9]*\\s+[a-zA-Z][a-zA-Z0-9]*(?:\\s+\"[^\"]*\")?$")
            },
            {
                StatementType.FlowDeclaration,
                new Regex("^\\s*[a-zA-Z][a-zA-Z0-9.]*\\s+(?:<->|-->) [a-zA-Z][a-zA-Z0-9.]*(?:\\s+\"[^\"]*\")?$")
            },
            {
                StatementType.NestedProcessDeclaration,
                new Regex("^\\s*[a-zA-Z][a-zA-Z0-9]*\\s+[a-zA-Z][a-zA-Z0-9]*(?:\\s+\"[^\"]*\")?:$")
            }
        };

        private readonly CodeSanitizer _codeSanitizer = new CodeSanitizer();
        private readonly GraphObjectParser _objectParser = new GraphObjectParser();

        public IGraph<IGraphNodeData> ToDiagram(string dfdString)
        {
            var statements = _codeSanitizer.PrepareAsCode(dfdString);
            var nodes = new List<ITreeNode<IGraphNodeData>>();
            var flows = new List<INodeFlow>();

            InterpreterRunData runData = new InterpreterRunData();
            

            foreach (var codeLine in statements)
            {
                try
                {
                    SetCorrectScopeLevel(runData, codeLine.Statement);

                    ITreeNode<IGraphNodeData>? newNode = null;

                    // Creation of basic nodes.
                    if (_regexes[StatementType.SimpleNodeDeclaration].Match(codeLine.Statement).Success)
                    {
                        newNode = _objectParser.TryParseNode(codeLine.Statement,
                            (runData.CurrentScopeNode as IModifiableTreeNode<IGraphNodeData>)!);
                        nodes.Add(newNode);
                        continue;
                    }

                    // Creation of nested nodes.
                    if (_regexes[StatementType.NestedProcessDeclaration].Match(codeLine.Statement).Success)
                    {
                        newNode = _objectParser.TryParseNode(codeLine.Statement.TrimEnd(':'),
                            (runData.CurrentScopeNode as IModifiableTreeNode<IGraphNodeData>)!);
                        runData.RaiseScope(newNode);
                        nodes.Add(newNode);
                        continue;
                    }

                    // Creation of flows.
                    if (_regexes[StatementType.FlowDeclaration].Match(codeLine.Statement).Success)
                    {
                        flows.Add(_objectParser.TryParseFlow(codeLine.Statement, runData.CurrentScopeNode));
                        continue;
                    }

                    // Error if line does not match any valid statements.
                    throw new InvalidStatementException(codeLine.Statement);
                }
                catch (DfdException e)
                {
                    throw new DfdInterpreterException(codeLine.Statement, codeLine.LineNumber, e);
                }
                catch (Exception e)
                {
                    throw new DfdInterpreterException(codeLine.Statement, codeLine.LineNumber, new Exception($"Unknown error occured. Inner exception:\n{e.Message}"));
                }
            }
            return new Graph<IGraphNodeData>(nodes.First().Root, flows);
        }

        private void SetCorrectScopeLevel(InterpreterRunData runData, string statement)
        {
            var statementScopeLevel = GetScopeLevel(statement);
            if (statementScopeLevel < runData.CurrentScopeLevel)
            {
                runData.LowerScopeTo(statementScopeLevel);
            }

            if (statementScopeLevel > runData.CurrentScopeLevel)
                throw new IndentationTooBigException(runData.CurrentScopeLevel, statementScopeLevel);
        }

        int GetScopeLevel(string line)
        {
            int indentations = 0;
            foreach (var character in line)
            {
                if (character == ' ')
                    indentations++;
                else if (character == '\t')
                    indentations += 4;
                else
                    break;
            }

            if (indentations % 4 != 0)
                throw new IndentationWrongException(indentations);

            return indentations / 4;
        }

    }

    
}