namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ParameterListSyntaxAbstraction : BaseParameterListSyntaxAbstraction, IParameterListSyntaxAbstraction
{
	public ParameterListSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		var previousStates = new[] { previous } as IList<IAnalysisState>;
		var nextStates = new List<IAnalysisState>();
		foreach (var child in Children)
		{
			foreach (var state in previousStates)
			{
				var resultsOrFailure = child.AnalyzeNode(state);
				if (!resultsOrFailure.IsT1)
					return resultsOrFailure.T2Value;
				foreach (var result in resultsOrFailure.T1Value)
				{
					var isReachableOrFailure = result.GetIsReachable(Location);
					if (!isReachableOrFailure.IsT1)
						return isReachableOrFailure.T2Value;
					if (isReachableOrFailure.T1Value)
						nextStates.Add(result);
				}
			}
			previousStates = nextStates.ToArray();
			nextStates.Clear();
		}

		return new TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure>(previousStates);
	}
}