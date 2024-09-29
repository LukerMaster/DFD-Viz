using System.Text.RegularExpressions;
using DFD.DataStructures.Implementations;
using DFD.DataStructures.Interfaces;
using DFD.Parsing.Interfaces;

namespace DFD.Parsing
{
    public class Interpreter<T> : IInterpreter<T> where T : INodeData
    {
        protected INodeDataFactory DataFactory { get; }

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
                new Regex("^\\s*[a-zA-Z][a-zA-Z0-9]*\\s+[a-zA-Z][a-zA-Z0-9_]*(?:\\s+\"[^\"]*\")?$")
            },
            {
                StatementType.FlowDeclaration,
                new Regex("^\\s*[a-zA-Z][a-zA-Z0-9_.]*\\s+(?:<->|-->) [a-zA-Z][a-zA-Z0-9_.]*(?:\\s+\"[^\"]*\")?$")
            },
            {
                StatementType.NestedProcessDeclaration,
                new Regex("^\\s*[a-zA-Z][a-zA-Z0-9]*\\s+[a-zA-Z][a-zA-Z0-9_]*(?:\\s+\"[^\"]*\")?:$")
            }
        };

        private readonly CodeSanitizer _codeSanitizer = new CodeSanitizer();
        private readonly GraphObjectParser<T> _objectParser;

        public Interpreter(INodeDataFactory dataFactory)
        {
            DataFactory = dataFactory;
            _objectParser = new GraphObjectParser<T>(DataFactory);
        }

        public IGraph<T> ToDiagram(string dfdString)
        {
            var statements = _codeSanitizer.PrepareAsCode(dfdString);
            var flows = new List<IFlow<T>>();

            InterpreterRunData<T> runData = new(DataFactory);
            
            foreach (var codeLine in statements)
            {
                try
                {
                    SetCorrectScopeLevel(runData, codeLine.Statement);

                    INodeRef<T>? newNode = null;

                    // Creation of basic nodes.
                    if (_regexes[StatementType.SimpleNodeDeclaration].Match(codeLine.Statement).Success)
                    {
                        newNode = _objectParser.TryParseNode(codeLine.Statement, (INode<T>)runData.CurrentScopeNode);
                        continue;
                    }

                    // Creation of nested nodes.
                    if (_regexes[StatementType.NestedProcessDeclaration].Match(codeLine.Statement).Success)
                    {
                        newNode = _objectParser.TryParseNode(codeLine.Statement.TrimEnd(':'), (INode<T>)runData.CurrentScopeNode);
                        runData.RaiseScope(newNode);
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
            return new Graph<T>((INode<T>)runData.Root, flows);
        }

        private void SetCorrectScopeLevel(InterpreterRunData<T> runData, string statement)
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