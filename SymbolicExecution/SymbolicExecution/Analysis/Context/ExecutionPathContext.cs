namespace SymbolicExecution.Analysis.Context;

public readonly struct ExecutionPath
{
	private ExecutionPath(
		ImmutableArray<ExpressionSyntax> conditions,
		ImmutableArray<VariableInfo> variables
		)
	{
		Conditions = conditions;
		Variables = variables;
	}

	public ExecutionPath WithCondition(ExpressionSyntax condition)
	{
		return new ExecutionPath(Conditions.Append(condition).ToImmutableArray(), Variables);
	}

	private ImmutableArray<ExpressionSyntax> Conditions { get; }
	public static ExecutionPath Empty { get; } = new ExecutionPath(ImmutableArray<ExpressionSyntax>.Empty, ImmutableArray<VariableInfo>.Empty);

	public Result<bool> IsValid()
	{
		foreach (var condition in Conditions)
		{
			switch (condition)
			{
				case LiteralExpressionSyntax literal:
					var token = literal.Token;
					var tokenValue = token.Value;
					if (tokenValue is bool value)
						return new Result<bool>(value);

					return new Result<bool>(new AnalysisErrorInfo($"Unexpected {nameof(LiteralExpressionSyntax)}.{nameof(literal.Token)}.{nameof(token.Value)}: {tokenValue}. Only boolean literals are supported.", literal.GetLocation()));
				case BinaryExpressionSyntax binaryExpression:
					return BinaryExpressionCanBeTrue(binaryExpression);
				default:
					throw new NotImplementedException();
			}
		}

		return new Result<bool>(true);
	}

	public Result<bool> CanBeFalse()
	{
		foreach (var condition in Conditions)
		{
			switch (condition)
			{
				case LiteralExpressionSyntax literal:
					var token = literal.Token;
					var tokenValue = token.Value;
					if (tokenValue is bool value)
						return new Result<bool>(!value);

					return new Result<bool>(new AnalysisErrorInfo($"Unexpected {nameof(LiteralExpressionSyntax)}.{nameof(literal.Token)}.{nameof(token.Value)}: {tokenValue}. Only boolean literals are supported.", literal.GetLocation()));
				case BinaryExpressionSyntax binaryExpression:
					return BinaryExpressionCanBeFalse(binaryExpression);
				default:
					throw new NotImplementedException();
			}
		}

		return new Result<bool>(new AnalysisErrorInfo("Unexpected stuff happening here", default));
	}

	private Result<bool> BinaryExpressionCanBeTrue(BinaryExpressionSyntax binaryExpression)
	{
		return binaryExpression.Kind() switch
		{
			SyntaxKind.EqualsExpression => CanBeEqual(binaryExpression),
			SyntaxKind.NotEqualsExpression => CanBeNotEqual(binaryExpression),
			var other => new Result<bool>(
				new AnalysisErrorInfo(
					$"Unexpected result from {nameof(binaryExpression)}.{nameof(binaryExpression.Kind)}: {other}",
					binaryExpression.GetLocation()
					)
				),
		};
	}

	private Result<bool> BinaryExpressionCanBeFalse(BinaryExpressionSyntax binaryExpression)
	{
		return binaryExpression.Kind() switch
		{
			SyntaxKind.EqualsExpression => CanBeNotEqual(binaryExpression),
			SyntaxKind.NotEqualsExpression => CanBeEqual(binaryExpression),
			var other => new Result<bool>(
				new AnalysisErrorInfo(
					$"Unexpected result from {nameof(binaryExpression)}.{nameof(binaryExpression.Kind)}: {other}",
					binaryExpression.GetLocation()
					)
				),
		};
	}

	private Result<bool> CanBeNotEqual(BinaryExpressionSyntax binaryExpressionSyntax)
	{
		var left = binaryExpressionSyntax.Left;
		var right = binaryExpressionSyntax.Right;
		var leftValueScope = GetValueScope(left);
		var rightValueScope = GetValueScope(right);
		if (leftValueScope.IsFaulted)
			return new Result<bool>(leftValueScope.ErrorInfo);

		if (rightValueScope.IsFaulted)
			return new Result<bool>(rightValueScope.ErrorInfo);

		var union = leftValueScope.Value.Union(rightValueScope.Value);
		if (union.IsFaulted)
			return new Result<bool>(union.ErrorInfo);

		return new Result<bool>(!(union.Value.Equals(leftValueScope.Value) && union.Value.Equals(rightValueScope.Value)));
	}


	private Result<IValueScope> GetValueScope(ExpressionSyntax expressionSyntax)
	{
		switch (expressionSyntax)
		{
			case IdentifierNameSyntax identifierName:
				var variableScope = GetVariableValueScope(
					identifierName.Identifier.ValueText,
					expressionSyntax.GetLocation()
					);
				return variableScope;
			case LiteralExpressionSyntax literal:
				if (literal.Token.Value is int value)
				{
					return new Result<IValueScope>(new ConcreteValueScope<int>(value));
				}
				else
					return new Result<IValueScope>(
						new AnalysisErrorInfo(
							$"{nameof(LiteralExpressionSyntax)}.{nameof(literal.Token)}.{nameof(literal.Token.Value)} is not an integer",
							literal.GetLocation()
							)
						);
			default:
				return new Result<IValueScope>(
					new AnalysisErrorInfo(
						$"Not implemented expression syntax: {expressionSyntax.GetType().Name}",
						expressionSyntax.GetLocation()
						)
					);
		}
	}

	private Result<IValueScope> GetVariableValueScope(string identifierValueText, Location locationForError)
	{
		var matchingVariables = Variables.Where(v => v.Name == identifierValueText).ToList();
		if (matchingVariables.Count == 1)
		{
			return new Result<IValueScope>(matchingVariables.First().ValueScope);
		}
		else
		{
			return new Result<IValueScope>(new AnalysisErrorInfo($"Variables matching identifier \"{identifierValueText}\" were not 1", locationForError));
		}
	}

	private Result<bool> CanBeEqual(BinaryExpressionSyntax binaryExpression)
	{
		var left = binaryExpression.Left;
		var right = binaryExpression.Right;

		var leftValueScope = GetValueScope(left);
		var rightValueScope = GetValueScope(right);
		if (leftValueScope.IsFaulted)
			return new Result<bool>(leftValueScope.ErrorInfo);
		if (rightValueScope.IsFaulted)
			return new Result<bool>(rightValueScope.ErrorInfo);

		var intersection = leftValueScope.Value.Intersection(rightValueScope.Value);

		if (intersection.IsFaulted)
			return new Result<bool>(intersection.ErrorInfo);

		return new Result<bool>(!intersection.Value.Equals(EmptyValueScope.Instance));
	}

	public ExecutionPath WithDeclaration(VariableInfo variable)
	{
		return new ExecutionPath(Conditions, Variables.Append(variable).ToImmutableArray());
	}

	public ImmutableArray<VariableInfo> Variables { get; }
}