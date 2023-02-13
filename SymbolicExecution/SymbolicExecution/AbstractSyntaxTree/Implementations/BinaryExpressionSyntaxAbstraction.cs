using Microsoft.CodeAnalysis.CSharp;

namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class BinaryExpressionSyntaxAbstraction : ExpressionSyntaxAbstraction, IBinaryExpressionSyntaxAbstraction
{
	private readonly IExpressionSyntaxAbstraction _left;
	private readonly IExpressionSyntaxAbstraction _right;
	private readonly SyntaxKind _syntaxKind;

	public BinaryExpressionSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location,
		ITypeSymbol? actualTypeSymbol,
		IExpressionSyntaxAbstraction left,
		IExpressionSyntaxAbstraction right,
		SyntaxKind syntaxKind
		) : base(children, symbol, location, actualTypeSymbol)
	{
		_left = left;
		_right = right;
		_syntaxKind = syntaxKind;
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		return new AnalysisFailure("Cannot analyze a binary expression", Location);
	}

	public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
	{
		var leftOrFailure = _left.GetExpressionResults(state);
		if (!leftOrFailure.IsT1)
			return leftOrFailure.T2Value;
		
		var leftResults = leftOrFailure.T1Value;
		
		var results = new List<(IObjectInstance, IAnalysisState)>();
		foreach (var (leftResult, leftState) in leftResults)
		{
			var rightOrFailure = _right.GetExpressionResults(leftState);
			if (!rightOrFailure.IsT1)
				return rightOrFailure.T2Value;

			var rightResults = rightOrFailure.T1Value;
			foreach (var (rightResult, leftAndRightState) in rightResults)
			{
				var resultOrFailure = Evaluate(leftResult, rightResult, leftAndRightState);
				if (!resultOrFailure.IsT1)
					return resultOrFailure.T2Value;

				results.AddRange(resultOrFailure.T1Value);
			}
		}

		return results.ToImmutableArray();
	}

	public TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> Evaluate(IObjectInstance left, IObjectInstance right, IAnalysisState state)
	{
		if (_syntaxKind == SyntaxKind.EqualsExpression)
		{
		}
		throw new NotImplementedException();

	}

	public static IBinaryExpressionSyntaxAbstraction? BuildFrom(
		Dictionary<SyntaxNode, ISyntaxNodeAbstraction> abstractionCache,
		ExpressionSyntax left,
		ExpressionSyntax right,
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location,
		ITypeSymbol? actualTypeSymbol,
		SyntaxKind syntaxKind
		)
	{
		var leftAbstraction = abstractionCache.TryGetValue(left, out var leftAbstractionValue)
			? leftAbstractionValue
			: null;

		var rightAbstraction = abstractionCache.TryGetValue(right, out var rightAbstractionValue)
			? rightAbstractionValue
			: null;

		if (leftAbstraction is not ExpressionSyntaxAbstraction leftExpressionAbstraction)
			return null;

		if (rightAbstraction is not ExpressionSyntaxAbstraction rightExpressionAbstraction)
			return null;

		return new BinaryExpressionSyntaxAbstraction(
			children,
			symbol,
			location,
			actualTypeSymbol,
			leftExpressionAbstraction,
			rightExpressionAbstraction,
			syntaxKind
			);
	}
}