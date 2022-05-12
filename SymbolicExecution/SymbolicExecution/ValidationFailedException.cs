namespace SymbolicExecution;

internal class ValidationFailedException : Exception
{
	public ValidationFailedException(string message) : base(message)
	{
	}
}