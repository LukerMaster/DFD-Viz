﻿using System.Text.RegularExpressions;
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

        flow = new Flow()
        {
            Source = currentParent.FindClosestMatchingLeaf(entityNameA),
            Target = currentParent.FindClosestMatchingLeaf(entityNameB),
            DisplayedText = displayedName
        };

        if (flow.Source.Children.Count > 0 || flow.Target.Children.Count > 0)
            throw new ProcessWithChildrenConnectedException(
                "Cannot create flow from (or to) a process containing subprocesses. Connect it to subprocess instead.");

        return flow;
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


        throw new InvalidEntityTypeException($"Invalid entity type {type.Name}.");
    }
}

