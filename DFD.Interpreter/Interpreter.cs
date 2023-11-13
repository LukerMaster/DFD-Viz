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
                new Regex("^[a-zA-Z][a-zA-Z0-9]*\\s+[a-zA-Z][a-zA-Z0-9]*(?:\\s+\"[^\"]*\")?$")
            },
            {
                StatementType.FlowDeclaration,
                new Regex("^[a-zA-Z][a-zA-Z0-9]*\\s+(?:<->|-->) [a-zA-Z][a-zA-Z0-9]*(?:\\s+\"[^\"]*\")?$")
            },
            {
                StatementType.NestedProcessDeclaration,
                new Regex("^[a-zA-Z][a-zA-Z0-9]*\\s+[a-zA-Z][a-zA-Z0-9]*(?:\\s+\"[^\"]*\")?:$")
            }
        };


        public IDiagram ToDiagram(string dfdString)
        {
            var preparedString = StripCommentsAndBlankLines(dfdString);
            List<IGraphEntity> entities = new List<IGraphEntity>();

            string[] lines = preparedString.Split('\n');

            foreach (var statement in lines)
            {
                // Creation of basic entities.
                if (Regexes[StatementType.SimpleEntityDeclaration].Match(statement).Success)
                {
                    IGraphEntity? entity = TryParseEntity(statement);
                    if (entity != null)
                    {
                        entities.Add(entity);
                        continue;
                    }
                }

                if (Regexes[StatementType.NestedProcessDeclaration].Match(statement).Success)
                {
                    // TODO
                    continue;
                }

                if (Regexes[StatementType.FlowDeclaration].Match(statement).Success)
                {
                    // TODO
                    continue;
                }
                
                // Error if line does not match any valid statements.
                throw new ArgumentException($"Invalid statement: {statement}.");
            }

            return new Diagram(entities);
        }

        string StripCommentsAndBlankLines(string code)
        {
            // Use a regular expression to match and remove comments marked by "#" and entirely blank lines
            string pattern = @"(?<!"")#([^""\r\n]*)(?=\r?\n|$)";
            string commentlessCode = Regex.Replace(code, pattern, "", RegexOptions.Multiline);

            // Remove leading and trailing whitespaces from each line
            var strippedCode = StripEmptyLines(commentlessCode);
            return strippedCode;
        }

        string StripEmptyLines(string code)
        {
            var lines = code.Split('\n');
            var newString = String.Empty;
            foreach (var line in lines)
            {
                if (!Regex.Match(line, "^\\s+$", RegexOptions.Singleline).Success)
                {
                    newString = newString + line.TrimEnd() + '\n';
                }
            }

            return newString.TrimEnd();
        }

        private IGraphEntity? TryParseEntity(string line)
        {
            IGraphEntity? entity = null;
            
            try
            {
                var definition = line.Split(" ");

                var typeName =      definition[0];
                var entityName =    definition[1];
                var displayedName = definition[2];

                if (ValidDefinitions.ContainsKey(typeName))
                {
                    var type = ValidDefinitions[typeName];
                    entity = CreateStandardEntity(type, entityName, displayedName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return entity;
        }

        private ISymbolicEntity CreateStandardEntity(Type type, string name, string displayedName)
        {
            if (type == typeof(Process))
            {
                return new Process() { EntityName = name, DisplayedText = displayedName };
            }

            if (type == typeof(Storage))
            {
                return new InputOutput() { EntityName = name, DisplayedText = displayedName };
            }

            if (type == typeof(InputOutput))
            {
                return new Storage() { EntityName = name, DisplayedText = displayedName };
            }

            throw new ArgumentException($"Invalid entity type {type.Name}.");
        }
    }
}