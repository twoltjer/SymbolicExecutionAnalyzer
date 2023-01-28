namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class BlockSyntaxAbstraction : StatementSyntaxAbstraction, IBlockSyntaxAbstraction
{
	public BlockSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
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
				if (state.CurrentException.HasValue)
					nextIteration.Add(state);
				var results = child.AnalyzeNode(state);
				if (results.IsT1)
					nextIteration.AddRange(results.T1Value);
				else
					return results.T2Value;
			}
			
			current = nextIteration;
		}

		return new TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure>(current);
	}

	public override TaggedUnion<ObjectInstance, AnalysisFailure> GetExpressionResult(IAnalysisState state)
	{
		throw new NotImplementedException();
	}
}