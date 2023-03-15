using System.Text;

namespace SymbolicExecution;

public class SymbolicExecutionState : IAnalysisState
{
	private SymbolicExecutionState(
		IExceptionThrownState? currentException,
		IImmutableDictionary<ILocalSymbol, IObjectInstance?> localVariables,
		IImmutableDictionary<IParameterSymbol, IObjectInstance?> parameterVariables,
		IImmutableStack<IImmutableDictionary<ILocalSymbol, IObjectInstance?>> localsStack,
		IImmutableStack<IImmutableDictionary<IParameterSymbol, IObjectInstance?>> parametersStack,
		IImmutableStack<IMethodSymbol?> methodStack,
		IMethodSymbol currentMethod
		)
	{
		CurrentException = currentException;
		LocalVariables = localVariables;
		ParameterVariables = parameterVariables;
		LocalsStack = localsStack;
		ParametersStack = parametersStack;
		MethodStack = methodStack;
		_currentMethod = currentMethod;
	}

	public IExceptionThrownState? CurrentException { get; }

	
	public IImmutableDictionary<ILocalSymbol, IObjectInstance?> LocalVariables { get; }
	public IImmutableDictionary<IParameterSymbol, IObjectInstance?> ParameterVariables { get; }
	
	public IImmutableStack<IImmutableDictionary<ILocalSymbol, IObjectInstance?>> LocalsStack { get; }

	public IImmutableStack<IImmutableDictionary<IParameterSymbol, IObjectInstance?>> ParametersStack { get; }
	
	public IImmutableStack<IMethodSymbol?> MethodStack { get; }

	public bool IsReachable => true;

	public IAnalysisState ThrowException(IObjectInstance exception, Location location)
	{
		var exceptionThrownState = new ExceptionThrownState(exception, location);
		return new SymbolicExecutionState(exceptionThrownState, LocalVariables, ParameterVariables, LocalsStack, ParametersStack, MethodStack, _currentMethod);
	}

	public IAnalysisState AddLocalVariable(ILocalSymbol symbol)
	{
		var newLocalVariables = LocalVariables.Add(symbol, null);
		return new SymbolicExecutionState(CurrentException, newLocalVariables, ParameterVariables, LocalsStack, ParametersStack, MethodStack, _currentMethod);
	}

	public TaggedUnion<IAnalysisState, AnalysisFailure> SetSymbolValue(ISymbol symbol, IObjectInstance value)
	{
		if (symbol is not ILocalSymbol localSymbol)
			return new AnalysisFailure("Cannot set the value of a non-local symbol", Location.None);

		if (LocalVariables.ContainsKey(localSymbol))
		{
			var newLocalVariables = LocalVariables.SetItem(localSymbol, value);
			return new SymbolicExecutionState(CurrentException, newLocalVariables, ParameterVariables, LocalsStack, ParametersStack, MethodStack, _currentMethod);
		}
		else
		{
			return new AnalysisFailure("Symbol missing from list of local variables", Location.None);
		}
	}

	public TaggedUnion<IObjectInstance, AnalysisFailure> GetSymbolValueOrFailure(ISymbol symbol, Location location)
	{
		if (symbol is IParameterSymbol parameterSymbol)
		{
			if (ParameterVariables.TryGetValue(parameterSymbol, out var paramValue))
			{
				if (paramValue == null)
					return new AnalysisFailure("Cannot get the value of a parameter variable that has not been initialized", location);
				else
					return new TaggedUnion<IObjectInstance, AnalysisFailure>(paramValue);
			}
			else
			{
				return new AnalysisFailure("Cannot find symbol", location);
			}
		}
			
			
		if (symbol is not ILocalSymbol localSymbol)
			return new AnalysisFailure("Cannot get the value of a non-local symbol", location);

		if (LocalVariables.TryGetValue(localSymbol, out var value))
		{
			if (value == null)
				return new AnalysisFailure("Cannot get the value of a local variable that has not been initialized", location);
			else
				return new TaggedUnion<IObjectInstance, AnalysisFailure>(value);
		}
		else
		{
			return new AnalysisFailure("Cannot find symbol", location);
		}
	}
	
	private readonly IMethodSymbol _currentMethod;

	public IAnalysisState PushStackFrame(
		ImmutableArray<(IParameterSymbol symbol, IObjectInstance value)> parameterValues,
		IMethodSymbol? methodSymbol
		)
	{
		var newLocalsStack = LocalsStack.Push(LocalVariables);
		var newParametersStack = ParametersStack.Push(ParameterVariables);
		var newLocalVariables = ImmutableDictionary<ILocalSymbol, IObjectInstance?>.Empty;
		var newParameterVariables = ImmutableDictionary<IParameterSymbol, IObjectInstance?>.Empty;
		var newMethodStack = MethodStack;
		newMethodStack = newMethodStack.Push(_currentMethod);

		if (methodSymbol == null)
		{
			Debug.Fail("Method symbol should not be null");
		}
		foreach (var (symbol, value) in parameterValues)
		{
			newParameterVariables = newParameterVariables.Add(symbol, value);
		}
		return new SymbolicExecutionState(CurrentException, newLocalVariables, newParameterVariables, newLocalsStack, newParametersStack, newMethodStack, methodSymbol);
	}
	
	public IAnalysisState PopStackFrame()
	{
		var newLocalsStack = LocalsStack.Pop();
		var newParametersStack = ParametersStack.Pop();
		var newLocalVariables = LocalsStack.Peek();
		var newParameterVariables = ParametersStack.Peek();
		var newMethodStack = MethodStack.Pop();
		var newCurrentMethod = MethodStack.Peek();
		return new SymbolicExecutionState(CurrentException, newLocalVariables, newParameterVariables, newLocalsStack, newParametersStack, newMethodStack, newCurrentMethod);
	}

	public override string ToString()
	{
		var sb = new StringBuilder();
		sb.AppendLine("Current method: " + _currentMethod.Name);
		sb.AppendLine("Current exception: " + CurrentException);
		sb.AppendLine("Local variables:");
		foreach (var (symbol, value) in LocalVariables.Select(x => (x.Key, x.Value)))
		{
			sb.AppendLine($"{symbol.Name} = {value}");
		}
		sb.AppendLine("Parameter variables:");
		foreach (var (symbol, value) in ParameterVariables.Select(x => (x.Key, x.Value)))
		{
			sb.AppendLine($"{symbol.Name} = {value}");
		}
		var str = sb.ToString();
		return str;
	}

	public static IAnalysisState CreateInitialState(IMethodSymbol? methodSymbol)
	{
		if (methodSymbol == null)
			Debug.Fail("Method symbol should not be null");
		return new SymbolicExecutionState(
			currentException: null,
			localVariables: ImmutableDictionary<ILocalSymbol, IObjectInstance?>.Empty,
			parameterVariables: ImmutableDictionary<IParameterSymbol, IObjectInstance?>.Empty,
			localsStack: ImmutableStack<IImmutableDictionary<ILocalSymbol, IObjectInstance?>>.Empty,
			parametersStack: ImmutableStack<IImmutableDictionary<IParameterSymbol, IObjectInstance?>>.Empty,
			methodStack: ImmutableStack<IMethodSymbol?>.Empty,
			currentMethod: methodSymbol
			);
	}
}