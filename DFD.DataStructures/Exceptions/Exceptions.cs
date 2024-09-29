using DFD.DataStructures.Interfaces;

public class NodeNotFoundException : Exception
{
    public string NodeName { get; }
    public string ParentNodeName { get; }
    public NodeNotFoundException(string nodeName, string parentNodeName)
    {
        NodeName = nodeName;
        ParentNodeName = parentNodeName;
    }
}

public class AmbiguousNodeMatchException<T> : Exception
{
    public string NodeName { get; }
    public INodeRef<T>[] Candidates { get; }
    public AmbiguousNodeMatchException(string nodeName, params INodeRef<T>[] candidates)
    {
        NodeName = nodeName;
        Candidates = candidates;
    }
}

public class SameFullNodeNameException : Exception
{
    public string NodeName { get; set; }
    public SameFullNodeNameException(string nodeName)
    {
        NodeName = nodeName;
    }
}

public class NodeConversionWithParentException<T> : Exception
{
    public INodeRef<T> NodeAttemptedToConvert { get; set; }
    public NodeConversionWithParentException(INodeRef<T> treeNode)
    {
        NodeAttemptedToConvert = treeNode;
    }
}