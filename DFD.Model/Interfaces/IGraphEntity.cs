﻿using System.Collections.Generic;

namespace DFD.Model.Interfaces;

public interface IGraphEntity
{
    IGraphEntity Parent { get; set; }
    ICollection<IGraphEntity> Children { get; set; }
    public string EntityName { get; }
    public string FullEntityName
    {
        get
        {
            var name = EntityName;
            var currentParent = Parent;
            while (currentParent is not null)
            {
                name = currentParent.EntityName + '.' + name;
                currentParent = currentParent.Parent;
            }
            return name;
        }
    }

    public bool CanNameBeThisEntity(string entityName)
    {
        // Check whether two entities have matching names
        // Example: Namespace.Process.Subprocess
        //                    Process.Subprocess
        //                    ^ Matching

        // Namespace.Process.Subprocess
        //     Other.Process.Subprocess
        //     ^ Not matching

        var firstSections = this.FullEntityName.Split('.');
        var secondSections = entityName.Split('.');

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
}