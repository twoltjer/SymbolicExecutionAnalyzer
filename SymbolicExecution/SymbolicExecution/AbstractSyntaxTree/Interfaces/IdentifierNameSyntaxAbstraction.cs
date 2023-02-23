namespace SymbolicExecution.AbstractSyntaxTree.Interfaces;

public class IdentifierNameSyntaxAbstraction : SimpleNameSyntaxAbstraction, IIdentifierNameSyntaxAbstraction
{
	public bool IsVar { get; }

	public IdentifierNameSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location,
		ITypeSymbol? type,
		bool isVar
		) : base(children, symbol, location, type)
	{
		IsVar = isVar;
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		return new[] { previous };
	}

	public override TaggedUnion<ImmutableArray<(int, IAnalysisState)>, AnalysisFailure> GetResults(IAnalysisState state)
	{
		if (Symbol == null)
			return new AnalysisFailure("Cannot get the result of an identifier name without a symbol", Location.None);

		var valueOrFault = state.GetSymbolReferenceOrFailure(Symbol, Location);
		if (!valueOrFault.IsT1)
			return valueOrFault.T2Value;

		return ImmutableArray.Create((valueOrFault.T1Value, analysisState: state));
	}
}