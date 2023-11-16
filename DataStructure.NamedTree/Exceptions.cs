using DataStructure.NamedTree;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string message) : base(message)
    {

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