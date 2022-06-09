using System.Threading;

namespace SymbolicExecution.Architecture.Handling;

public abstract class HandlerMediatorBase<TValue, TResult> where TValue : notnull where TResult : notnull
{
	public IHandler<TValue, TResult>[] Handlers { get; }

	protected HandlerMediatorBase(params IHandler<TValue, TResult>[] handlers)
	{
		Handlers = handlers;
	}

	public async Task<TResult> HandleAsync(TValue value, CancellationToken token)
	{
		foreach (var handler in Handlers)
		{
			if (token.IsCancellationRequested)
				break;

			if (!handler.CanHandle(value))
				continue;

			return await handler.HandleAsync(value, token);
		}

		return HandleUnhandledValue(value);
	}

	protected abstract TResult HandleUnhandledValue(TValue value);
}