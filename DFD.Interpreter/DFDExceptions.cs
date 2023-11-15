namespace DFD.Interpreter;

internal class DFDException : Exception
{
    public DFDException(string message) : base(message)
    {
        
    }
}

internal class InvalidEntityTypeException : DFDException
{
    public InvalidEntityTypeException(string message) : base(message)
    {
    }
}

internal class ProcessWithChildrenConnectedException : DFDException
{
    public ProcessWithChildrenConnectedException(string message) : base(message)
    {

    }
}

internal class InvalidStatementException : DFDException
{
    public InvalidStatementException(string message) : base(message)
    {
    }
}