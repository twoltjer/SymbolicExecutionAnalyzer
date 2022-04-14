﻿using System.Diagnostics;
using Microsoft.CodeAnalysis.Diagnostics;
using SymbolicExecution.Analysis.Context;

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
		Debug.Fail("No handler found for value " + value.GetType().Name);
		return analysisContext;
	}
}