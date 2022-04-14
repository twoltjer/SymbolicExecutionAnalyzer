using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SymbolicExecution
{
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
						switch (literal.Token.ValueText)
						{
							case "true":
								return true;
							case "false":
								return false;
							default:
								Debug.Fail("Unexpected literal value: " + literal.Token.ValueText);
								return true;
						}
					case BinaryExpressionSyntax binaryExpression:
						return BinaryExpressionCanBeTrue(binaryExpression);
					default:
						Debug.Fail("Unhandled condition type");
						break;
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
				case SyntaxKind.GreaterThanExpression:
				case SyntaxKind.GreaterThanOrEqualExpression:
				case SyntaxKind.LessThanExpression:
				case SyntaxKind.LessThanOrEqualExpression:
				default:
					Debug.Fail("Unexpected binary expression: " + binaryExpression.Kind());
					return true;
			}
		}

		private bool CanBeNotEqual(ExpressionSyntax left, ExpressionSyntax right)
		{
			var leftValueScope = GetValueScope(left);
			var rightValueScope = GetValueScope(right);
			if (leftValueScope is null || rightValueScope is null)
			{
				Debug.Fail("Unexpected null value scope");
				return true;
			}

			var union = leftValueScope.Union(rightValueScope);
			
			return !(union.Equals(leftValueScope) && union.Equals(rightValueScope));
		}


		private IValueScope GetValueScope(ExpressionSyntax expressionSyntax)
		{
			switch (expressionSyntax)
			{
				case IdentifierNameSyntax identifierName:
					return GetVariableValueScope(identifierName.Identifier.ValueText);
				case LiteralExpressionSyntax literal:
					if (int.TryParse(literal.Token.ValueText, out var intValue))
					{
						return new ConcreteValueScope<int>(intValue);
					}
					else
					{
						Debug.Fail("Unexpected literal value: " + literal.Token.ValueText);
						return new ConcreteValueScope<int>(0);
					}
				default:
					Debug.Fail("Unexpected expression type: " + expressionSyntax.Kind());
					return null;
			}
		}

		private IValueScope GetLiteralValueScope(string valueText)
		{
			throw new NotImplementedException();
		}

		private IValueScope GetVariableValueScope(string identifierValueText)
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
			{
				Debug.Fail("Unexpected null value scope");
				return true;
			}

			var intersection = leftValueScope.Intersection(rightValueScope);

			return !(intersection.Equals(EmptyValueScope.Instance));
		}

		public SymbolicAnalysisContext WithDeclaration(VariableInfo variable)
		{
			return new SymbolicAnalysisContext(Conditions, Variables.Append(variable).ToImmutableArray());
		}

		public ImmutableArray<VariableInfo> Variables { get; }
	}
}