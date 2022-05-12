namespace SymbolicExecution.Analysis;

public abstract class HandlerMediatorBase<T> where T : notnull
{
	public IHandler<T>[] Handlers { get; }

	protected HandlerMediatorBase(params IHandler<T>[] handlers)
	{
		Handlers = handlers;
	}

	public SymbolicAnalysisContext Handle(T value, SymbolicAnalysisContext analysisContext, CodeBlockAnalysisContext codeBlockContext)
	{
		foreach (var handler in Handlers)
		{
			if (!handler.CanHandle(value))
				continue;

			return handler.Handle(value, analysisContext, codeBlockContext);
		}

		throw new UnhandledSyntaxException();
	}
}