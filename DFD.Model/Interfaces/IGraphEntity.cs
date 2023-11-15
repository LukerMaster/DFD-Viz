using System.Collections.Generic;

namespace DFD.Model.Interfaces;

public interface IGraphEntity
{
    IGraphEntity? Parent { get; set; }
    ICollection<IGraphEntity> Children { get; set; }
    public string DisplayedName { get; set; }
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

    public IGraphEntity FindClosestMatchingLeaf(string leafEntityPath)
    {
        List<IGraphEntity> candidates = new();


        Queue<IGraphEntity> queue = new Queue<IGraphEntity>();
        queue.Enqueue(this);

        while (queue.Count > 0)
        {
            IGraphEntity currentNode = queue.Dequeue();

            // Check if the current node contains the target value
            if (currentNode.CanNameBeThisEntity(leafEntityPath) && currentNode.Children.Count == 0)
            {
                candidates.Add(currentNode);
            }

            // If we already found matching node, do not enqueue lower levels
            // We just need to check whether there is any other node
            // matching on THE SAME level, so we can throw ambiguity error.
            if (candidates.Count == 0)
            {
                // Enqueue children for further exploration
                foreach (IGraphEntity child in currentNode.Children)
                {
                    queue.Enqueue(child);
                }
            }

        }

        if (candidates.Count == 1)
            return candidates[0];
        if (candidates.Count > 1)
            throw new AmbiguousEntityMatchException(leafEntityPath, candidates.ToArray());
        // Target value not found in the tree
        throw new EntityNotFoundException("Entity could not be found within scope.");
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

