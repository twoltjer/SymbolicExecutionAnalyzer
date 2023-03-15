namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class MethodDeclarationSyntaxAbstraction : BaseMethodDeclarationSyntaxAbstraction, IMethodDeclarationSyntaxAbstraction
{
	public MethodDeclarationSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		return new AnalysisFailure("Method declarations are not supported yet", Location);
	}

	public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
	{
		return new AnalysisFailure("Method declarations are not supported yet", Location);
	}

	public TaggedUnion<IEnumerable<(IObjectInstance? returned, IAnalysisState state)>, AnalysisFailure> AnalyzeMethodCall(IAnalysisState priorState, ImmutableArray<IObjectInstance> parameters)
	{
		var parameterLists = Children
			.OfType<IParameterListSyntaxAbstraction>().ToList();
		
		if (parameterLists.Count != 1)
		{
			return new AnalysisFailure("Method declarations must have exactly one parameter list", Location);
		}
		
		var parameterList = parameterLists[0];
		
		var paramSymbols = new List<IParameterSymbol>();
		
		foreach (var paramListChild in parameterList.Children)
		{
			if (paramListChild is not ParameterSyntaxAbstraction parameter)
			{
				return new AnalysisFailure("Parameter list had non-parameter child", Location);
			}
			
			var parameterSymbol = parameter.Symbol as IParameterSymbol;

			if (parameterSymbol is null)
				return new AnalysisFailure("Parameter had no symbol", Location);
			
			paramSymbols.Add(parameterSymbol);
		}
		
		if (paramSymbols.Count != parameters.Length)
		{
			return new AnalysisFailure("Method call had incorrect number of parameters", Location);
		}

		var paramsTuples = paramSymbols.Zip(parameters, (symbol, value) => (symbol, value)).ToImmutableArray();
		var initialStackCount = priorState.ParametersStack.Count();
		var methodSymbol = Symbol as IMethodSymbol;
		if (methodSymbol is null)
		{
			return new AnalysisFailure("Method declaration had no symbol", Location);
		}
		var methodState = priorState.PushStackFrame(paramsTuples, methodSymbol);
		
		var methodBody = Children.OfType<IBlockSyntaxAbstraction>().ToList();
		
		if (methodBody.Count != 1)
		{
			return new AnalysisFailure("Method declarations must have exactly one body", Location);
		}

		var parameterStackCount = methodState.ParametersStack.Count();
		Debug.Assert(parameterStackCount == initialStackCount + 1);
		var methodBodyResult = methodBody[0].AnalyzeNode(methodState);
		
		if (!methodBodyResult.IsT1)
		{
			return methodBodyResult.T2Value;
		}
		
		var methodBodyResults = methodBodyResult.T1Value.ToList();
		foreach (var result in methodBodyResults)
		{
			var resultCount = result.ParametersStack.Count();
			Debug.Assert(resultCount == parameterStackCount);
		}

		var resultStates = methodBodyResults.Select(x => x.PopStackFrame()).ToList();
		foreach (var (resultState, _) in resultStates)
		{
			var resultCount = resultState.ParametersStack.Count();
			Debug.Assert(resultCount == initialStackCount);
		}
		if (resultStates.Count == 0)
		{
			return new AnalysisFailure("Method body did not return any results", Location);
		}

		return resultStates.Select(x => (x.returnValue, x.state)).ToImmutableArray();
	}
}