using System;

namespace SymbolicExecution.ExceptionHandling;

public abstract class ExceptionHandlerBase<T> where T : Exception
{
    public bool CanHandle(Exception exception) => exception is T;
    
    public abstract void Handle(T exception);
}