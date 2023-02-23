using Microsoft.CodeAnalysis.CSharp;

namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class AssignmentExpressionSyntaxAbstraction : ExpressionSyntaxAbstraction, IAssignmentExpressionSyntaxAbstraction
{
	private readonly SyntaxKind _syntaxKind;

	public AssignmentExpressionSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location,
		ITypeSymbol? actualTypeSymbol,
		SyntaxKind syntaxKind
		) : base(children, symbol, location, actualTypeSymbol)
	{
		_syntaxKind = syntaxKind;
	}

	private TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> PerformAssignment(IAnalysisState state, ILocalSymbol localSymbol, int rightValue)
	{
		if (_syntaxKind == SyntaxKind.SimpleAssignmentExpression)
			return PerformSimpleAssignment(state, localSymbol, rightValue);
		else
			return PerformOperatorAssignment(state, localSymbol, rightValue);
	}

	private TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> PerformSimpleAssignment(IAnalysisState state, ILocalSymbol localSymbol, int rightValue)
	{
		var newStateOrFailure = state.SetSymbolReference(localSymbol, rightValue);
		if (!newStateOrFailure.IsT1)
			return newStateOrFailure.T2Value;

		return new[] { newStateOrFailure.T1Value };
	}

	private TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> PerformOperatorAssignment(IAnalysisState state, ILocalSymbol localSymbol, int rightValue)
	{
		var leftReferenceOrFailure = state.GetSymbolReferenceOrFailure(localSymbol, Location);
		if (!leftReferenceOrFailure.IsT1)
			return leftReferenceOrFailure.T2Value;

		var leftReference = leftReferenceOrFailure.T1Value;

		TaggedUnion<IEnumerable<(int, IAnalysisState)>, AnalysisFailure> operationResultOrFailure = _syntaxKind switch
		{
			// SyntaxKind.AddAssignmentExpression => leftReference.AddOperator(rightValue, state, attemptReverseConversion: true),
			// SyntaxKind.SubtractAssignmentExpression => leftReference.SubtractOperator(rightValue, state, attemptReverseConversion: true),
			// SyntaxKind.MultiplyAssignmentExpression => leftReference.MultiplyOperator(rightValue, state, attemptReverseConversion: true),
			// SyntaxKind.DivideAssignmentExpression => leftReference.DivideOperator(rightValue, state, attemptReverseConversion: true),
			// SyntaxKind.ModuloAssignmentExpression => leftReference.ModuloOperator(rightValue, state, attemptReverseConversion: true),
			// SyntaxKind.AndAssignmentExpression => leftReference.LogicalAndOperator(rightValue, state, attemptReverseConversion: true),
			// SyntaxKind.OrAssignmentExpression => leftReference.LogicalOrOperator(rightValue, state, attemptReverseConversion: true),
			// SyntaxKind.ExclusiveOrAssignmentExpression => leftReference.LogicalXorOperator(rightValue, state, attemptReverseConversion: true),
			// SyntaxKind.LeftShiftAssignmentExpression => leftReference.LeftShiftOperator(rightValue, state, attemptReverseConversion: true),
			// SyntaxKind.RightShiftAssignmentExpression => leftReference.RightShiftOperator(rightValue, state, attemptReverseConversion: true),
			_ => new AnalysisFailure($"Unsupported assignment operator: {_syntaxKind}", Location),
		};

		if (!operationResultOrFailure.IsT1)
			return operationResultOrFailure.T2Value;

		var operationResult = operationResultOrFailure.T1Value;
		var newStates = new List<IAnalysisState>();
		foreach (var (result, modifiedState) in operationResult)
		{
			var newStateOrFailure = modifiedState.SetSymbolReference(localSymbol, result);
			if (!newStateOrFailure.IsT1)
				return newStateOrFailure.T2Value;

			newStates.Add(newStateOrFailure.T1Value);
		}

		return newStates;
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		if (Children.Length != 2)
			return new AnalysisFailure("Assignment expression must have exactly two children", Location);

		if (Children[0] is not IIdentifierNameSyntaxAbstraction identifier)
			return new AnalysisFailure("Assignment expression must have an identifier as its first child", Location);

		if (identifier.Symbol is not ILocalSymbol localSymbol)
			return new AnalysisFailure("Assignment expression must have a local variable as its first child", Location);

		if (Children[1] is not IExpressionSyntaxAbstraction expression)
			return new AnalysisFailure("Assignment expression must have an expression as its second child", Location);

		var resultsOrFailure = expression.GetResults(previous);
		if (!resultsOrFailure.IsT1)
			return resultsOrFailure.T2Value;

		var results = resultsOrFailure.T1Value;
		var returnStates = new List<IAnalysisState>();
		foreach (var (rightValue, preAssignmentOperatorState) in results)
		{
			var assignmentStateOrFailure = PerformAssignment(preAssignmentOperatorState, localSymbol, rightValue);

			if (!assignmentStateOrFailure.IsT1)
				return assignmentStateOrFailure.T2Value;

			returnStates.AddRange(assignmentStateOrFailure.T1Value);
		}

		return returnStates;
	}

	public override TaggedUnion<ImmutableArray<(int, IAnalysisState)>, AnalysisFailure> GetResults(IAnalysisState state)
	{
		return new AnalysisFailure("Cannot get the result of an assignment expression", Location);
	}
}