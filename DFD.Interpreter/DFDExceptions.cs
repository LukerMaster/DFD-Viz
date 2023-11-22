using DataStructure.NamedTree;

namespace DFD.Interpreter;

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
    public ProcessWithChildrenConnectedException(ITreeNode<T> process) 
        : base($"Cannot create flow from (or to) a process containing subprocesses. {process.FullNodeName} contains subprocesses.")
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
    public FlowWithAmbiguousNodeException(string nodeName, ITreeNode<T>[] candidates)
        : base($"Flow with ambiguous node: '{nodeName}' declared. Valid candidates are: {string.Join("; ", candidates.Select(c => c.FullNodeName))}")
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