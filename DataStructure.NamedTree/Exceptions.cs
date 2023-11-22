using DataStructure.NamedTree;

public class NodeNotFoundException : Exception
{
    public string NodeName { get; }
    public NodeNotFoundException(string nodeName)
    {
        NodeName = nodeName;
    }
}

public class AmbiguousNodeMatchException<T> : Exception
{
    public string NodeName { get; }
    public ITreeNode<T>[] Candidates { get; }
    public AmbiguousNodeMatchException(string nodeName, params ITreeNode<T>[] candidates)
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
    public ITreeNode<T> NodeAttemptedToConvert { get; set; }
    public NodeConversionWithParentException(ITreeNode<T> treeNode)
    {
        NodeAttemptedToConvert = treeNode;
    }
}