namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class IfStatementSyntaxAbstraction : StatementSyntaxAbstraction, IIfStatementSyntaxAbstraction
{
	private readonly IExpressionSyntaxAbstraction _condition;
	private readonly IStatementSyntaxAbstraction _statement;
	private readonly IStatementSyntaxAbstraction? _elseClause;

	public IfStatementSyntaxAbstraction(
		IExpressionSyntaxAbstraction condition,
		IStatementSyntaxAbstraction statement,
		IStatementSyntaxAbstraction? elseClause,
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		) : base(children, symbol, location)
	{
		_condition = condition;
		_statement = statement;
		_elseClause = elseClause;
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		var conditionResultsOrFailure = _condition.GetResults(previous);
		if (!conditionResultsOrFailure.IsT1)
			return conditionResultsOrFailure.T2Value;

		var conditionResults = conditionResultsOrFailure.T1Value;
		var modifiedStates = new List<IAnalysisState>();
		foreach (var (conditionResultReference, stateAfterExpression) in conditionResults)
		{
			var isReachableOrFailure = stateAfterExpression.GetIsReachable(Location);
			if (!isReachableOrFailure.IsT1)
				return isReachableOrFailure.T2Value;

			if (!isReachableOrFailure.T1Value)
				continue;

			var conditionResult = stateAfterExpression.References[conditionResultReference];
			if (!conditionResult.IsType(typeof(bool)))
				return new AnalysisFailure("Condition must be a boolean", Location);

			var stateWithAppliedTrueConstraintOrFailure = stateAfterExpression.AddConstraint(conditionResult.Reference, new ExactValueConstraint(true), Location);
			if (!stateWithAppliedTrueConstraintOrFailure.IsT1)
				return stateWithAppliedTrueConstraintOrFailure.T2Value;
			var trueStateReachableOrFailure = stateWithAppliedTrueConstraintOrFailure.T1Value.GetIsReachable(Location);
			if (!trueStateReachableOrFailure.IsT1)
				return trueStateReachableOrFailure.T2Value;
			if (trueStateReachableOrFailure.T1Value)
			{
				 var statementResultsOrFailure = _statement.AnalyzeNode(stateAfterExpression);
				 if (!statementResultsOrFailure.IsT1)
					 return statementResultsOrFailure.T2Value;

				 modifiedStates.AddRange(statementResultsOrFailure.T1Value);
			}

			var stateWithAppliedFalseConstraintOrFailure =
				stateAfterExpression.AddConstraint(conditionResult.Reference, new ExactValueConstraint(false), Location);
			
			if (!stateWithAppliedFalseConstraintOrFailure.IsT1)
				return stateWithAppliedFalseConstraintOrFailure.T2Value;
			
			var falseStateReachableOrFailure = stateWithAppliedFalseConstraintOrFailure.T1Value.GetIsReachable(Location);
			if (!falseStateReachableOrFailure.IsT1)
				return falseStateReachableOrFailure.T2Value;
			if (falseStateReachableOrFailure.T1Value)
			{
				modifiedStates.Add(stateAfterExpression);
			}
		}

		return modifiedStates;
	}

	[ExcludeFromCodeCoverage]
	public static IIfStatementSyntaxAbstraction? BuildFrom(
		Dictionary<SyntaxNode, ISyntaxNodeAbstraction> syntaxNodeAbstractions,
		ExpressionSyntax condition,
		StatementSyntax statement,
		ElseClauseSyntax? elseClause,
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location
		)
	{
		if (!syntaxNodeAbstractions.TryGetValue(condition, out var conditionAbstractionAsBase) ||
			conditionAbstractionAsBase is not IExpressionSyntaxAbstraction conditionAbstraction)
		{
			return null;
		}

		if (!syntaxNodeAbstractions.TryGetValue(statement, out var statementAbstractionAsBase) ||
			statementAbstractionAsBase is not IStatementSyntaxAbstraction statementAbstraction)
		{
			return null;
		}

		IStatementSyntaxAbstraction? elseClauseAbstraction = null;
		if (elseClause != null)
		{
			if (!syntaxNodeAbstractions.TryGetValue(elseClause, out var elseClauseAbstractionAsBase) ||
				elseClauseAbstractionAsBase is not IStatementSyntaxAbstraction elseClauseAbstractionAsStatement)
			{
				return null;
			}

			elseClauseAbstraction = elseClauseAbstractionAsStatement;
		}

		return new IfStatementSyntaxAbstraction(
			conditionAbstraction,
			statementAbstraction,
			elseClauseAbstraction,
			children,
			symbol,
			location
			);
	}
}

public class ExactValueConstraint : IConstraint
{
	public object? Value { get; }

	public ExactValueConstraint(object? value)
	{
		Value = value;
	}
}