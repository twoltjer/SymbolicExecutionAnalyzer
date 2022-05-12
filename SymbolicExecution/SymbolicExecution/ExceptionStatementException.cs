namespace SymbolicExecution;

internal class ExceptionStatementException : Exception
{
	public ExceptionStatementException(ThrowStatementSyntax throwStatement)
	{
		ThrowStatement = throwStatement;
	}

	public ThrowStatementSyntax ThrowStatement { get; }
}