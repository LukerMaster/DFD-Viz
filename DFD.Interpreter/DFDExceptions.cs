using DFD.Model.Interfaces;

namespace DFD.Interpreter;

internal class DFDException : Exception
{
    public DFDException(string message) : base(message)
    {
        
    }
}

internal class InvalidEntityTypeException : DFDException
{
    public InvalidEntityTypeException(string entityName) 
        : base($"Invalid entity type {entityName}.")
    {
    }
}

internal class ProcessWithChildrenConnectedException : DFDException
{
    public ProcessWithChildrenConnectedException(IGraphEntity process) 
        : base($"Cannot create flow from (or to) a process containing subprocesses. {process.FullEntityName} contains subprocesses.")
    {

    }
}

internal class InvalidStatementException : DFDException
{
    public InvalidStatementException(string statement) 
        : base($"Invalid statement: {statement}.")
    {
    }
}

internal class IndentationTooBigException : DFDException
{
    public IndentationTooBigException(int _is, int _shouldBe) 
        : base($"Indentation too big out of nowhere (is {_is}, should be {_shouldBe}. Did you forget ':' one line above?")
    {

    }
}

internal class FlowWithAmbiguousEntityException : DFDException
{
    public FlowWithAmbiguousEntityException(string entityName, IGraphEntity[] candidates)
        : base($"Flow with ambiguous entity: '{entityName}' declared. Valid candidates are: {string.Join("; ", candidates.Select(c => c.FullEntityName))}")
    {

    }
}

internal class IndentationWrongException : DFDException
{
    public IndentationWrongException(int indentations) 
        : base($"Wrong indentation count: {indentations}. Indentations should be divisible by 4. Each 'space' is 1. Each 'tab' is 4.")

    {

    }
}