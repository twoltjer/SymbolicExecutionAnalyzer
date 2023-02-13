namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class LocalDeclarationStatementSyntaxAbstraction : StatementSyntaxAbstraction, ILocalDeclarationStatementSyntaxAbstraction
{
	public LocalDeclarationStatementSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		if (Children.Length != 1)
		{
			return new AnalysisFailure("Local declaration statement must have exactly one child", Location);
		}

		if (!(Children[0] is IVariableDeclarationSyntaxAbstraction variableDeclarationSyntaxAbstraction))
		{
			return new AnalysisFailure("Local declaration statement must have a variable declaration as its child", Location);
		}

		return variableDeclarationSyntaxAbstraction.AnalyzeNode(previous);
	}

	public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
	{
		return new AnalysisFailure("Cannot get the result of a local declaration statement", Location);
	}
}