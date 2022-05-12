namespace SymbolicExecution.Analysis.Context;

public class SymbolicAnalysisContext
{
	private SymbolicAnalysisContext(
		ImmutableArray<ExpressionSyntax> conditions,
		ImmutableArray<VariableInfo> variables
		)
	{
		Conditions = conditions;
		Variables = variables;
	}

	public SymbolicAnalysisContext WithCondition(ExpressionSyntax condition)
	{
		return new SymbolicAnalysisContext(Conditions.Append(condition).ToImmutableArray(), Variables);
	}

	private ImmutableArray<ExpressionSyntax> Conditions { get; }
	public static SymbolicAnalysisContext Empty { get; } = new SymbolicAnalysisContext(ImmutableArray<ExpressionSyntax>.Empty, ImmutableArray<VariableInfo>.Empty);

	public bool CanBeTrue()
	{
		foreach (var condition in Conditions)
		{
			switch (condition)
			{
				case LiteralExpressionSyntax literal:
					var token = literal.Token;
					var tokenValue = token.Value;
					if (tokenValue is bool value)
						return value;
					throw new NotImplementedException();
				case BinaryExpressionSyntax binaryExpression:
					return BinaryExpressionCanBeTrue(binaryExpression);
				default:
					throw new NotImplementedException();
			}
		}

		return true;
	}

	private bool BinaryExpressionCanBeTrue(BinaryExpressionSyntax binaryExpression)
	{
		var left = binaryExpression.Left;
		var right = binaryExpression.Right;
		switch (binaryExpression.Kind())
		{
			case SyntaxKind.EqualsExpression:
				return CanBeEqual(left, right);
			case SyntaxKind.NotEqualsExpression:
				return CanBeNotEqual(left, right);
			default:
				throw new UnhandledSyntaxException();
		}
	}

	private bool CanBeNotEqual(ExpressionSyntax left, ExpressionSyntax right)
	{
		var leftValueScope = GetValueScope(left);
		var rightValueScope = GetValueScope(right);
		if (leftValueScope is null || rightValueScope is null)
			throw new UnexpectedValueException();

		var union = leftValueScope.Union(rightValueScope);
		if (union is null)
			throw new UnexpectedValueException();

		return !(union.Equals(leftValueScope) && union.Equals(rightValueScope));
	}


	private IValueScope? GetValueScope(ExpressionSyntax expressionSyntax)
	{
		switch (expressionSyntax)
		{
			case IdentifierNameSyntax identifierName:
				return GetVariableValueScope(identifierName.Identifier.ValueText);
			case LiteralExpressionSyntax literal:
				if (literal.Token.Value.GetType() == typeof(Int32))
				{
					if (int.TryParse(literal.Token.ValueText, out var intValue))
						return new ConcreteValueScope<int>(intValue);
					else
						throw new UnexpectedValueException();
				}
				else
					throw new UnhandledSyntaxException();
			default:
				throw new UnhandledSyntaxException();
		}
	}

	private IValueScope? GetVariableValueScope(string identifierValueText)
	{
		var matchingVariables = Variables.Where(v => v.Name == identifierValueText).ToList();
		if (matchingVariables.Count == 1)
		{
			return matchingVariables.First().ValueScope;
		}
		else
		{
			return null;
		}
	}

	private bool CanBeEqual(ExpressionSyntax left, ExpressionSyntax right)
	{
		var leftValueScope = GetValueScope(left);
		var rightValueScope = GetValueScope(right);
		if (leftValueScope is null || rightValueScope is null)
			throw new UnexpectedValueException();

		var intersection = leftValueScope.Intersection(rightValueScope);

		if (intersection is null)
			throw new UnexpectedValueException();

		return !intersection.Equals(EmptyValueScope.Instance);
	}

	public SymbolicAnalysisContext WithDeclaration(VariableInfo variable)
	{
		return new SymbolicAnalysisContext(Conditions, Variables.Append(variable).ToImmutableArray());
	}

	public ImmutableArray<VariableInfo> Variables { get; }
}