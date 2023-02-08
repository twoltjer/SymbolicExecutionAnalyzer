namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class QualifiedNameSyntaxAbstraction : NameSyntaxAbstraction, IQualifiedNameSyntaxAbstraction
{
	public QualifiedNameSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location,
		ITypeSymbol? actualTypeSymbol,
		ITypeSymbol? convertedTypeSymbol
		) : base(children, symbol, location, actualTypeSymbol, convertedTypeSymbol)
	{
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		return new AnalysisFailure("Cannot analyze qualified names", Location);
	}

	public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
	{
		return new AnalysisFailure("Cannot analyze qualified names", Location);
	}
}