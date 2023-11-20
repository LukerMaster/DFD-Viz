using DataStructure.NamedTree;

public class EntityNotFoundException : Exception
{
    public string EntityName { get; }
    public EntityNotFoundException(string entityName)
    {
        EntityName = entityName;
    }
}

public class AmbiguousEntityMatchException<T> : Exception
{
    public string EntityName { get; }
    public ITreeNode<T>[] Candidates { get; }
    public AmbiguousEntityMatchException(string entityName, params ITreeNode<T>[] candidates)
    {
        EntityName = entityName;
        Candidates = candidates;
    }
}