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

    public IFlow? TryParseFlow(string statement, IGraphEntity currentParent)
    {
        IFlow? flow = null;

        var definition = SplitByWhitespace(statement);

        var entityNameA = definition[0];
        var flowType = definition[1]; // TODO: do something with this value.
        var entityNameB = definition[2];

        var displayedName = String.Empty;
        if (definition.Count > 3)
            displayedName = definition[3];

        try
        {
            flow = new Flow()
            {
                Source = currentParent.FindClosestMatchingLeaf(entityNameA),
                Target = currentParent.FindClosestMatchingLeaf(entityNameB),
                DisplayedText = displayedName
            };
        }
        catch (AmbiguousEntityMatchException e)
        {
            throw new FlowWithAmbiguousEntityException(e.EntityName, e.Candidates);
        }

        if (flow.Source.Children.Count > 0)
            throw new ProcessWithChildrenConnectedException(flow.Source);
        if (flow.Target.Children.Count > 0)
            throw new ProcessWithChildrenConnectedException(flow.Target);

        return flow;
    }

    private IGraphEntity CreateStandardEntity(Type type, string name, string displayedName, IGraphEntity parent)
    {
        if (type == typeof(Process))
        {
            return new Process() { EntityName = name, DisplayedName = displayedName, Parent = parent };
        }

        if (type == typeof(Storage))
        {
            return new InputOutput() { EntityName = name, DisplayedName = displayedName, Parent = parent };
        }

        if (type == typeof(InputOutput))
        {
            return new Storage() { EntityName = name, DisplayedName = displayedName, Parent = parent };
        }


        throw new InvalidEntityTypeException(type.Name);
    }
}

