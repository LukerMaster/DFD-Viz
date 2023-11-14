using System.Text.RegularExpressions;
using DFD.Model.Interfaces;

namespace DFD.Interpreter;

internal class GraphObjectParser
{
    private readonly Dictionary<string, Type> ValidDefinitions = new()
    {
        { "Process", typeof(Process) },
        { "Storage", typeof(Storage) },
        { "IO", typeof(InputOutput) },
    };

    public IGraphEntity? TryParseEntity(string line, IGraphEntity currentParent)
    {
        IGraphEntity? entity = null;

        // Split by any amount of whitespace
        var definition = SplitByWhitespace(line);

        var typeName = definition[0];
        var entityName = definition[1];
        var displayedName = definition[2];

        if (ValidDefinitions.ContainsKey(typeName))
        {
            var type = ValidDefinitions[typeName];
            entity = CreateStandardEntity(type, entityName, displayedName, currentParent);
            currentParent.Children.Add(entity);
        }
        
        return entity;
    }


    private static IList<string> SplitByWhitespace(string line)
    {
        return Regex.Split(line, @"\s+").Where(s => s != string.Empty).ToList();
    }

    public IFlow? TryParseFlow(string statement, ICollection<IGraphEntity> declaredEntities)
    {
        IFlow? entity = null;

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

        return entity;
    }

    private IGraphEntity FindEntityByName(string EntityName, ICollection<IGraphEntity> knownEntities)
    {
        IGraphEntity? found = null;
        foreach (var entity in knownEntities)
        {
            if (entity.CanNameBeThisEntity(EntityName))
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