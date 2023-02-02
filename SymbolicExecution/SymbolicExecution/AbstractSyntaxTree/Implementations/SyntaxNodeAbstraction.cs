namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class SyntaxNodeAbstraction : ISyntaxNodeAbstraction
{
	protected SyntaxNodeAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol, Location location)
	{
		Children = children;
		Symbol = symbol;
		Location = location;
	}

	public ImmutableArray<ISyntaxNodeAbstraction> Children { get; }
	public IEnumerable<ISyntaxNodeAbstraction> GetDescendantNodes(bool includeSelf)
	{
		foreach (var child in Children)
		{
			yield return child;

			foreach (var descendant in child.GetDescendantNodes(includeSelf: false))
			{
				yield return descendant;
			}
		}

		if (includeSelf)
		{
			yield return this;
		}
	}

	public abstract TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous);

	public ISymbol? Symbol { get; }
	public abstract TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state);
	public Location Location { get; }
}