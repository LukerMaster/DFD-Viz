using DFD.DataStructures.Interfaces;

namespace DFD.Parsing;


public class DfdInterpreterException : Exception
{
    public string Statement { get; init; }
    public int Line { get; init; }
    public Exception Inner { get; init; }
    public DfdInterpreterException(string statement, int line, Exception inner)
    {
        Statement = statement;
        Line = line;
        Inner = inner;
    }
}

internal class DfdException : Exception
{
    public DfdException(string message) : base(message)
    {
        
    }
}

internal class InvalidNodeTypeException : DfdException
{
    public InvalidNodeTypeException(string nodeName) 
        : base($"Invalid node type {nodeName}.")
    {
    }
}
internal class ProcessWithChildrenConnectedException<T> : DfdException
{
    public ProcessWithChildrenConnectedException(INodeRef<T> process) 
        : base($"Cannot create flow from (or to) a process containing subprocesses. {process.FullPath} contains subprocesses.")
    {

    }
}

internal class InvalidStatementException : DfdException
{
    public InvalidStatementException(string statement) 
        : base($"Invalid statement: {statement}.")
    {
    }
}

internal class IndentationTooBigException : DfdException
{
    public IndentationTooBigException(int _is, int shouldBe) 
        : base($"Indentation too big out of nowhere (is {_is}, should be {shouldBe}. Did you forget ':' one line above?")
    {

    }
}

internal class FlowWithAmbiguousNodeException<T> : DfdException
{
    public FlowWithAmbiguousNodeException(string nodeName, INodeRef<T>[] candidates)
        : base($"Flow with ambiguous node: '{nodeName}' declared. Valid candidates are: {string.Join("; ", candidates.Select(c => c.FullPath))}")
    {

    }
}

internal class UndefinedNodeReferencedException : DfdException
{
    public UndefinedNodeReferencedException(string nodeName, string parentNodeName)
        : base($"Could not find '{nodeName}' anywhere inside of '{parentNodeName}' or its children.")
    {

    }
}

internal class IndentationWrongException : DfdException
{
    public IndentationWrongException(int indentations) 
        : base($"Wrong indentation count: {indentations}. Indentations should be divisible by 4. Each 'space' is 1. Each 'tab' is 4.")

    {

    }
}

internal class RedefinitionOfNodeException : DfdException
{
    public RedefinitionOfNodeException(string fullNodeName)
        : base($"Node with same name on the same level is already defined. Node name: {fullNodeName}.")
    {

    }
}