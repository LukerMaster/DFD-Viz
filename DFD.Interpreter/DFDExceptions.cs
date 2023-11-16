using DataStructure.NamedTree;
using DFD.Model.Interfaces;

namespace DFD.Interpreter;

internal class DfdException : Exception
{
    public DfdException(string message) : base(message)
    {
        
    }
}

internal class InvalidEntityTypeException : DfdException
{
    public InvalidEntityTypeException(string entityName) 
        : base($"Invalid entity type {entityName}.")
    {
    }
}

internal class ProcessWithChildrenConnectedException<T> : DfdException
{
    public ProcessWithChildrenConnectedException(ITreeNode<T> process) 
        : base($"Cannot create flow from (or to) a process containing subprocesses. {process.FullEntityName} contains subprocesses.")
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

internal class FlowWithAmbiguousEntityException<T> : DfdException
{
    public FlowWithAmbiguousEntityException(string entityName, ITreeNode<T>[] candidates)
        : base($"Flow with ambiguous entity: '{entityName}' declared. Valid candidates are: {string.Join("; ", candidates.Select(c => c.FullEntityName))}")
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