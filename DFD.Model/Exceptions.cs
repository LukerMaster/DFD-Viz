using DFD.Model.Interfaces;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string message) : base(message)
    {

    }
}

public class AmbiguousEntityMatchException : Exception
{
    public string EntityName { get; }
    public IGraphEntity[] Candidates { get; }
    public AmbiguousEntityMatchException(string entityName, params IGraphEntity[] candidates)
    {
        EntityName = entityName;
        Candidates = candidates;
    }
}