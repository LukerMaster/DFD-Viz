using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using DataStructure.NamedTree;
using DFD.Model;
using DFD.Model.Interfaces;
using DFD.ModelImplementations;

namespace DFD.Interpreter
{
    public class Interpreter
    {
        private enum StatementType
        {
            SimpleEntityDeclaration,
            NestedProcessDeclaration,
            FlowDeclaration
        }

        private readonly Dictionary<StatementType, Regex> _regexes = new()
        {
            {
                StatementType.SimpleEntityDeclaration,
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
            var preparedString = _codeSanitizer.StripCommentsAndBlankLines(dfdString);
            var entities = new List<ITreeNode<IGraphNodeData>>();
            var flows = new List<IFlow<IGraphNodeData>>();

            ParserRunData runData = new ParserRunData();

            string[] lines = preparedString.Split('\n');

            foreach (var statement in lines)
            {
                // Setting a correct scope for the statement (correct Parent).
                SetCorrectScopeLevel(runData, statement);

                ITreeNode<IGraphNodeData>? newEntity = null;

                // Creation of basic entities.
                if (_regexes[StatementType.SimpleEntityDeclaration].Match(statement).Success)
                {
                    newEntity = _objectParser.TryParseEntity(statement, runData.CurrentScopeNode);
                    entities.Add(newEntity);
                    continue;
                }

                // Creation of nested entities.
                if (_regexes[StatementType.NestedProcessDeclaration].Match(statement).Success)
                {
                    newEntity = _objectParser.TryParseEntity(statement.TrimEnd(':'), runData.CurrentScopeNode);
                    runData.RaiseScope(newEntity);
                    entities.Add(newEntity);
                    continue;
                }

                // Creation of flows.
                if (_regexes[StatementType.FlowDeclaration].Match(statement).Success)
                {
                    flows.Add(_objectParser.TryParseFlow(statement, runData.CurrentScopeNode));
                    continue;
                }

                // Error if line does not match any valid statements.
                throw new InvalidStatementException(statement);
            }

            return new Graph<IGraphNodeData>(entities.First().Root, flows);
        }

        private void SetCorrectScopeLevel(ParserRunData runData, string statement)
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