namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class ThrowStatementSyntaxAbstraction : StatementSyntaxAbstraction, IThrowStatementSyntaxAbstraction
{
	private readonly Location _location;

	public ThrowStatementSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol,
		Location location) : base(children, symbol)
	{
		_location = location;
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		if (Children.Length != 1)
			return new AnalysisFailure("Throw statement expected to have exactly one child", _location);

		var expressionResultOrFailure = Children[0].GetExpressionResult(previous);
		if (!expressionResultOrFailure.IsT1)
			return expressionResultOrFailure.T2Value;

		var thrownState = previous.ThrowException(expressionResultOrFailure.T1Value, _location);
		return new[] { thrownState };
	}

	public override TaggedUnion<ObjectInstance, AnalysisFailure> GetExpressionResult(IAnalysisState state)
	{
		throw new NotImplementedException();
	}
}