namespace SymbolicExecution.Architecture.Handling;

public interface IHandler<in TValue, TResult>
{
	bool CanHandle(TValue value);
	Task<TResult> HandleAsync(TValue value, CancellationToken token);
}