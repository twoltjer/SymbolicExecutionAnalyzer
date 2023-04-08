namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ArgumentSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IArgumentSyntaxAbstraction
{
	public ArgumentSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		return new AnalysisFailure("Cannot analyze argument expressions", Location);
	}

	public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
	{
		if (Children.Length != 1)
			return new AnalysisFailure("ArgumentSyntaxAbstraction must have exactly one child", Location);
		
		return Children[0].GetExpressionResults(state);
	}
}