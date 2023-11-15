using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using DFD.Model;
using DFD.Model.Interfaces;

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

        private readonly Dictionary<StatementType, Regex> Regexes = new()
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

        private readonly CodeSanitizer codeSanitizer = new CodeSanitizer();
        private readonly GraphObjectParser objectParser = new GraphObjectParser();

        public IGraph ToDiagram(string dfdString)
        {
            var preparedString = codeSanitizer.StripCommentsAndBlankLines(dfdString);
            var entities = new List<IGraphEntity>();
            var flows = new List<IFlow>();

            ParserRunData runData = new ParserRunData();

            string[] lines = preparedString.Split('\n');

            foreach (var statement in lines)
            {
                // Setting a correct scope for the statement (correct Parent).
                SetCorrectScopeLevel(runData, statement);

                IGraphEntity? newEntity = null;

                // Creation of basic entities.
                if (Regexes[StatementType.SimpleEntityDeclaration].Match(statement).Success)
                {
                    newEntity = objectParser.TryParseEntity(statement, runData.CurrentScopeNode);
                    entities.Add(newEntity);
                    continue;
                }

                // Creation of nested entities.
                if (Regexes[StatementType.NestedProcessDeclaration].Match(statement).Success)
                {
                    newEntity = objectParser.TryParseEntity(statement.TrimEnd(':'), runData.CurrentScopeNode);
                    runData.RaiseScope(newEntity);
                    entities.Add(newEntity);
                    continue;
                }

                // Creation of flows.
                if (Regexes[StatementType.FlowDeclaration].Match(statement).Success)
                {
                    flows.Add(objectParser.TryParseFlow(statement, runData.CurrentScopeNode));
                    continue;
                }

                // Error if line does not match any valid statements.
                throw new InvalidStatementException(statement);
            }

            return new Graph(entities.First().Root, flows);
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