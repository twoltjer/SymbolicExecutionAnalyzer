namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class BlockSyntaxAbstraction : StatementSyntaxAbstraction, IBlockSyntaxAbstraction
{
	public BlockSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		var current = new[] { previous } as IList<IAnalysisState>;
		foreach (var child in Children)
		{
			var nextIteration = new List<IAnalysisState>();
			foreach (var state in current)
			{
				if (state.CurrentException != null)
				{
					nextIteration.Add(state);
				}
				else
				{
					var results = child.AnalyzeNode(state);
					if (results.IsT1)
						nextIteration.AddRange(results.T1Value);
					else
						return results.T2Value;
				}
			}

			current = nextIteration;
		}

		return new TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure>(current);
	}

	public override TaggedUnion<IObjectInstance, AnalysisFailure> GetExpressionResult(IAnalysisState state)
	{
		throw new NotImplementedException();
	}
}