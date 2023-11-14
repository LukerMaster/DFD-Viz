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
        
        private readonly Dictionary<string, Type> ValidDefinitions = new()
        {
            { "Process", typeof(Process) },
            { "Storage", typeof(Storage) },
            { "IO", typeof(InputOutput) },
        };

        private readonly Dictionary<StatementType, Regex> Regexes = new()
        {
            {
                StatementType.SimpleEntityDeclaration,
                new Regex("^\\s*[a-zA-Z][a-zA-Z0-9]*\\s+[a-zA-Z][a-zA-Z0-9]*(?:\\s+\"[^\"]*\")?$")
            },
            {
                StatementType.FlowDeclaration,
                new Regex("^\\s*[a-zA-Z][a-zA-Z0-9]*\\s+(?:<->|-->) [a-zA-Z][a-zA-Z0-9]*(?:\\s+\"[^\"]*\")?$")
            },
            {
                StatementType.NestedProcessDeclaration,
                new Regex("^\\s*[a-zA-Z][a-zA-Z0-9]*\\s+[a-zA-Z][a-zA-Z0-9]*(?:\\s+\"[^\"]*\")?:$")
            }
        };

        
        public IDiagram ToDiagram(string dfdString)
        {
            var preparedString = StripCommentsAndBlankLines(dfdString);
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
                    newEntity = TryParseEntity(statement, runData.CurrentScopeNode);
                    entities.Add(newEntity);
                    continue;
                }

                // Creation of nested entities.
                if (Regexes[StatementType.NestedProcessDeclaration].Match(statement).Success)
                {
                    newEntity = TryParseEntity(statement.TrimEnd(':'), runData.CurrentScopeNode);
                    runData.RaiseScope(newEntity);
                    entities.Add(newEntity);
                    continue;
                }

                // Creation of flows.
                if (Regexes[StatementType.FlowDeclaration].Match(statement).Success)
                {
                    flows.Add(TryParseFlow(statement, entities));
                    continue;
                }

                // Error if line does not match any valid statements.
                throw new ArgumentException("Invalid statement.");
            }

            return new Diagram(entities, flows);
        }

        private void SetCorrectScopeLevel(ParserRunData runData, string statement)
        {
            var statementScopeLevel = GetScopeLevel(statement);
            if (statementScopeLevel < runData.CurrentScopeLevel)
            {
                var steps = runData.CurrentScopeLevel - statementScopeLevel;
                for (var i = 0; i < steps; i++)
                {
                    runData.CurrentScopeNode = runData.CurrentScopeNode.Parent;
                    runData.CurrentScopeLevel--;
                }
            }
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
                throw new ArgumentException($"Wrong indentation count: {indentations}");

            return indentations / 4;
        }

        string StripCommentsAndBlankLines(string code)
        {
            // Use a regular expression to match and remove comments marked by "#" and entirely blank lines
            string commentlessCode = Regex.Replace(code, "^\\s*#.*", "", RegexOptions.Multiline);

            // Remove leading and trailing whitespaces from each line
            var strippedCode = StripEmptyLines(commentlessCode);
            return strippedCode;
        }

        string StripEmptyLines(string code)
        {
            var lines = code.Replace("\r\n", "\n").Split('\n');
            var newString = String.Empty;
            foreach (var line in lines)
            {
                if (!Regex.Match(line, "^\\s*$", RegexOptions.Singleline).Success)
                {
                    newString = newString + line.TrimEnd() + '\n';
                }
            }

            return newString.TrimEnd();
        }

        private IGraphEntity? TryParseEntity(string line, IGraphEntity currentParent)
        {
            IGraphEntity? entity = null;
            
            try
            {
                // Split by any amount of whitespace
                var definition = SplitByWhitespace(line);

                var typeName =      definition[0];
                var entityName =    SetFullEntityName(definition[1], currentParent);
                var displayedName = definition[2];
                
                if (ValidDefinitions.ContainsKey(typeName))
                {
                    var type = ValidDefinitions[typeName];
                    entity = CreateStandardEntity(type, entityName, displayedName, currentParent);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return entity;
        }

        private string SetFullEntityName(string entityName, IGraphEntity currentParent)
        {
            return currentParent.EntityName + '.' + entityName;
        }

        private static IList<string> SplitByWhitespace(string line)
        {
            return Regex.Split(line, @"\s+").Where(s => s != string.Empty).ToList();
        }

        private IFlow? TryParseFlow(string statement, ICollection<IGraphEntity> declaredEntities)
        {
            IFlow? entity = null;

            try
            {
                var definition = SplitByWhitespace(statement);

                var entityNameA = definition[0];
                var flowType = definition[1];
                var entityNameB = definition[2];

                var displayedName = String.Empty;
                if (definition.Count > 3)
                    displayedName = definition[3];

                entity = new Flow()
                {
                    Source = FindEntityByName(entityNameA, declaredEntities),
                    Target = FindEntityByName(entityNameB, declaredEntities),
                    DisplayedText = displayedName
                };

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return entity;
        }

        private IGraphEntity FindEntityByName(string EntityName, ICollection<IGraphEntity> knownEntities)
        {
            IGraphEntity? found = null;
            foreach (var entity in knownEntities)
            {
                if (HasMatchingEntityNames(entity.EntityName, EntityName))
                {
                    if (found is null)
                        found = entity;
                    else
                        throw new ArgumentException("Ambiguous entity declaration.");
                }
            }

            if (found is null)
                throw new ArgumentException("Entity is not defined.");

            return found;
        }

        private bool HasMatchingEntityNames(string first, string second)
        {
            // Check whether two entities have matching names
            // Example: Namespace.Process.Subprocess
            //                    Process.Subprocess
            //                    ^ Matching

            // Namespace.Process.Subprocess
            //     Other.Process.Subprocess
            //     ^ Not matching

            var firstSections = first.Split('.');
            var secondSections = second.Split('.');

            // Reverse so we check from the innermost
            firstSections = firstSections.Reverse().ToArray();
            secondSections = secondSections.Reverse().ToArray();

            var checkLimit = Math.Min(firstSections.Length, secondSections.Length);

            for (int i = 0; i < checkLimit; i++)
            {
                if (firstSections[i] != secondSections[i])
                    return false;
            }
            return true;
        }

        private ISymbolicEntity CreateStandardEntity(Type type, string name, string displayedName, IGraphEntity parent)
        {
            if (type == typeof(Process))
            {
                return new Process() { EntityName = name, DisplayedText = displayedName, Parent = parent };
            }

            if (type == typeof(Storage))
            {
                return new InputOutput() { EntityName = name, DisplayedText = displayedName, Parent = parent };
            }

            if (type == typeof(InputOutput))
            {
                return new Storage() { EntityName = name, DisplayedText = displayedName, Parent = parent };
            }

            throw new ArgumentException($"Invalid entity type {type.Name}.");
        }
    }

    internal class ParserRunData
    {
        public IGraphEntity CurrentScopeNode { get; set; }
        public int CurrentScopeLevel { get; set; }

        public ParserRunData()
        {
            CurrentScopeNode = new Process()
            {
                EntityName = "top",
                DisplayedText = "System"
            };
        }

        public void RaiseScope(IGraphEntity newChild)
        {
            CurrentScopeNode.Children.Add(newChild);
            CurrentScopeNode = newChild;
            CurrentScopeLevel++;
        }
    }
}