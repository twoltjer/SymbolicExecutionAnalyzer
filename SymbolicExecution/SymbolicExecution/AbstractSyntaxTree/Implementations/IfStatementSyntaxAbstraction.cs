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
			if (!stateAfterExpression.IsReachable)
				continue;

			var conditionResult = stateAfterExpression.References[conditionResultReference];
			if (!conditionResult.IsType(typeof(bool)))
				return new AnalysisFailure("Condition must be a boolean", Location);

			var stateWithAppliedTrueConstraintOrFailure =
				conditionResult.ValueScope.ApplyConstraint(new ExactValueConstraint(true));
			if (!stateWithAppliedTrueConstraintOrFailure.IsT1)
				return stateWithAppliedTrueConstraintOrFailure.T2Value;
			if (stateWithAppliedTrueConstraintOrFailure.T1Value.IsReachable)
			{
				 var statementResultsOrFailure = _statement.AnalyzeNode(stateAfterExpression);
				 if (!statementResultsOrFailure.IsT1)
					 return statementResultsOrFailure.T2Value;

				 modifiedStates.AddRange(statementResultsOrFailure.T1Value);
			}

			var stateWithAppliedFalseConstraintOrFailure =
				conditionResult.ValueScope.ApplyConstraint(new ExactValueConstraint(false));
			
			if (!stateWithAppliedFalseConstraintOrFailure.IsT1)
				return stateWithAppliedFalseConstraintOrFailure.T2Value;
			
			if (stateWithAppliedFalseConstraintOrFailure.T1Value.IsReachable)
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
	public ExactValueConstraint(object? value)
	{
	}
}