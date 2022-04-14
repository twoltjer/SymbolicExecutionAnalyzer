using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SymbolicExecution.Analysis.Context
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
				case SyntaxKind.None:
				case SyntaxKind.List:
				case SyntaxKind.TildeToken:
				case SyntaxKind.ExclamationToken:
				case SyntaxKind.DollarToken:
				case SyntaxKind.PercentToken:
				case SyntaxKind.CaretToken:
				case SyntaxKind.AmpersandToken:
				case SyntaxKind.AsteriskToken:
				case SyntaxKind.OpenParenToken:
				case SyntaxKind.CloseParenToken:
				case SyntaxKind.MinusToken:
				case SyntaxKind.PlusToken:
				case SyntaxKind.EqualsToken:
				case SyntaxKind.OpenBraceToken:
				case SyntaxKind.CloseBraceToken:
				case SyntaxKind.OpenBracketToken:
				case SyntaxKind.CloseBracketToken:
				case SyntaxKind.BarToken:
				case SyntaxKind.BackslashToken:
				case SyntaxKind.ColonToken:
				case SyntaxKind.SemicolonToken:
				case SyntaxKind.DoubleQuoteToken:
				case SyntaxKind.SingleQuoteToken:
				case SyntaxKind.LessThanToken:
				case SyntaxKind.CommaToken:
				case SyntaxKind.GreaterThanToken:
				case SyntaxKind.DotToken:
				case SyntaxKind.QuestionToken:
				case SyntaxKind.HashToken:
				case SyntaxKind.SlashToken:
				case SyntaxKind.DotDotToken:
				case SyntaxKind.SlashGreaterThanToken:
				case SyntaxKind.LessThanSlashToken:
				case SyntaxKind.XmlCommentStartToken:
				case SyntaxKind.XmlCommentEndToken:
				case SyntaxKind.XmlCDataStartToken:
				case SyntaxKind.XmlCDataEndToken:
				case SyntaxKind.XmlProcessingInstructionStartToken:
				case SyntaxKind.XmlProcessingInstructionEndToken:
				case SyntaxKind.BarBarToken:
				case SyntaxKind.AmpersandAmpersandToken:
				case SyntaxKind.MinusMinusToken:
				case SyntaxKind.PlusPlusToken:
				case SyntaxKind.ColonColonToken:
				case SyntaxKind.QuestionQuestionToken:
				case SyntaxKind.MinusGreaterThanToken:
				case SyntaxKind.ExclamationEqualsToken:
				case SyntaxKind.EqualsEqualsToken:
				case SyntaxKind.EqualsGreaterThanToken:
				case SyntaxKind.LessThanEqualsToken:
				case SyntaxKind.LessThanLessThanToken:
				case SyntaxKind.LessThanLessThanEqualsToken:
				case SyntaxKind.GreaterThanEqualsToken:
				case SyntaxKind.GreaterThanGreaterThanToken:
				case SyntaxKind.GreaterThanGreaterThanEqualsToken:
				case SyntaxKind.SlashEqualsToken:
				case SyntaxKind.AsteriskEqualsToken:
				case SyntaxKind.BarEqualsToken:
				case SyntaxKind.AmpersandEqualsToken:
				case SyntaxKind.PlusEqualsToken:
				case SyntaxKind.MinusEqualsToken:
				case SyntaxKind.CaretEqualsToken:
				case SyntaxKind.PercentEqualsToken:
				case SyntaxKind.QuestionQuestionEqualsToken:
				case SyntaxKind.BoolKeyword:
				case SyntaxKind.ByteKeyword:
				case SyntaxKind.SByteKeyword:
				case SyntaxKind.ShortKeyword:
				case SyntaxKind.UShortKeyword:
				case SyntaxKind.IntKeyword:
				case SyntaxKind.UIntKeyword:
				case SyntaxKind.LongKeyword:
				case SyntaxKind.ULongKeyword:
				case SyntaxKind.DoubleKeyword:
				case SyntaxKind.FloatKeyword:
				case SyntaxKind.DecimalKeyword:
				case SyntaxKind.StringKeyword:
				case SyntaxKind.CharKeyword:
				case SyntaxKind.VoidKeyword:
				case SyntaxKind.ObjectKeyword:
				case SyntaxKind.TypeOfKeyword:
				case SyntaxKind.SizeOfKeyword:
				case SyntaxKind.NullKeyword:
				case SyntaxKind.TrueKeyword:
				case SyntaxKind.FalseKeyword:
				case SyntaxKind.IfKeyword:
				case SyntaxKind.ElseKeyword:
				case SyntaxKind.WhileKeyword:
				case SyntaxKind.ForKeyword:
				case SyntaxKind.ForEachKeyword:
				case SyntaxKind.DoKeyword:
				case SyntaxKind.SwitchKeyword:
				case SyntaxKind.CaseKeyword:
				case SyntaxKind.DefaultKeyword:
				case SyntaxKind.TryKeyword:
				case SyntaxKind.CatchKeyword:
				case SyntaxKind.FinallyKeyword:
				case SyntaxKind.LockKeyword:
				case SyntaxKind.GotoKeyword:
				case SyntaxKind.BreakKeyword:
				case SyntaxKind.ContinueKeyword:
				case SyntaxKind.ReturnKeyword:
				case SyntaxKind.ThrowKeyword:
				case SyntaxKind.PublicKeyword:
				case SyntaxKind.PrivateKeyword:
				case SyntaxKind.InternalKeyword:
				case SyntaxKind.ProtectedKeyword:
				case SyntaxKind.StaticKeyword:
				case SyntaxKind.ReadOnlyKeyword:
				case SyntaxKind.SealedKeyword:
				case SyntaxKind.ConstKeyword:
				case SyntaxKind.FixedKeyword:
				case SyntaxKind.StackAllocKeyword:
				case SyntaxKind.VolatileKeyword:
				case SyntaxKind.NewKeyword:
				case SyntaxKind.OverrideKeyword:
				case SyntaxKind.AbstractKeyword:
				case SyntaxKind.VirtualKeyword:
				case SyntaxKind.EventKeyword:
				case SyntaxKind.ExternKeyword:
				case SyntaxKind.RefKeyword:
				case SyntaxKind.OutKeyword:
				case SyntaxKind.InKeyword:
				case SyntaxKind.IsKeyword:
				case SyntaxKind.AsKeyword:
				case SyntaxKind.ParamsKeyword:
				case SyntaxKind.ArgListKeyword:
				case SyntaxKind.MakeRefKeyword:
				case SyntaxKind.RefTypeKeyword:
				case SyntaxKind.RefValueKeyword:
				case SyntaxKind.ThisKeyword:
				case SyntaxKind.BaseKeyword:
				case SyntaxKind.NamespaceKeyword:
				case SyntaxKind.UsingKeyword:
				case SyntaxKind.ClassKeyword:
				case SyntaxKind.StructKeyword:
				case SyntaxKind.InterfaceKeyword:
				case SyntaxKind.EnumKeyword:
				case SyntaxKind.DelegateKeyword:
				case SyntaxKind.CheckedKeyword:
				case SyntaxKind.UncheckedKeyword:
				case SyntaxKind.UnsafeKeyword:
				case SyntaxKind.OperatorKeyword:
				case SyntaxKind.ExplicitKeyword:
				case SyntaxKind.ImplicitKeyword:
				case SyntaxKind.YieldKeyword:
				case SyntaxKind.PartialKeyword:
				case SyntaxKind.AliasKeyword:
				case SyntaxKind.GlobalKeyword:
				case SyntaxKind.AssemblyKeyword:
				case SyntaxKind.ModuleKeyword:
				case SyntaxKind.TypeKeyword:
				case SyntaxKind.FieldKeyword:
				case SyntaxKind.MethodKeyword:
				case SyntaxKind.ParamKeyword:
				case SyntaxKind.PropertyKeyword:
				case SyntaxKind.TypeVarKeyword:
				case SyntaxKind.GetKeyword:
				case SyntaxKind.SetKeyword:
				case SyntaxKind.AddKeyword:
				case SyntaxKind.RemoveKeyword:
				case SyntaxKind.WhereKeyword:
				case SyntaxKind.FromKeyword:
				case SyntaxKind.GroupKeyword:
				case SyntaxKind.JoinKeyword:
				case SyntaxKind.IntoKeyword:
				case SyntaxKind.LetKeyword:
				case SyntaxKind.ByKeyword:
				case SyntaxKind.SelectKeyword:
				case SyntaxKind.OrderByKeyword:
				case SyntaxKind.OnKeyword:
				case SyntaxKind.EqualsKeyword:
				case SyntaxKind.AscendingKeyword:
				case SyntaxKind.DescendingKeyword:
				case SyntaxKind.NameOfKeyword:
				case SyntaxKind.AsyncKeyword:
				case SyntaxKind.AwaitKeyword:
				case SyntaxKind.WhenKeyword:
				case SyntaxKind.ElifKeyword:
				case SyntaxKind.EndIfKeyword:
				case SyntaxKind.RegionKeyword:
				case SyntaxKind.EndRegionKeyword:
				case SyntaxKind.DefineKeyword:
				case SyntaxKind.UndefKeyword:
				case SyntaxKind.WarningKeyword:
				case SyntaxKind.ErrorKeyword:
				case SyntaxKind.LineKeyword:
				case SyntaxKind.PragmaKeyword:
				case SyntaxKind.HiddenKeyword:
				case SyntaxKind.ChecksumKeyword:
				case SyntaxKind.DisableKeyword:
				case SyntaxKind.RestoreKeyword:
				case SyntaxKind.ReferenceKeyword:
				case SyntaxKind.InterpolatedStringStartToken:
				case SyntaxKind.InterpolatedStringEndToken:
				case SyntaxKind.InterpolatedVerbatimStringStartToken:
				case SyntaxKind.LoadKeyword:
				case SyntaxKind.NullableKeyword:
				case SyntaxKind.EnableKeyword:
				case SyntaxKind.WarningsKeyword:
				case SyntaxKind.AnnotationsKeyword:
				case SyntaxKind.VarKeyword:
				case SyntaxKind.UnderscoreToken:
				case SyntaxKind.OmittedTypeArgumentToken:
				case SyntaxKind.OmittedArraySizeExpressionToken:
				case SyntaxKind.EndOfDirectiveToken:
				case SyntaxKind.EndOfDocumentationCommentToken:
				case SyntaxKind.EndOfFileToken:
				case SyntaxKind.BadToken:
				case SyntaxKind.IdentifierToken:
				case SyntaxKind.NumericLiteralToken:
				case SyntaxKind.CharacterLiteralToken:
				case SyntaxKind.StringLiteralToken:
				case SyntaxKind.XmlEntityLiteralToken:
				case SyntaxKind.XmlTextLiteralToken:
				case SyntaxKind.XmlTextLiteralNewLineToken:
				case SyntaxKind.InterpolatedStringToken:
				case SyntaxKind.InterpolatedStringTextToken:
				case SyntaxKind.EndOfLineTrivia:
				case SyntaxKind.WhitespaceTrivia:
				case SyntaxKind.SingleLineCommentTrivia:
				case SyntaxKind.MultiLineCommentTrivia:
				case SyntaxKind.DocumentationCommentExteriorTrivia:
				case SyntaxKind.SingleLineDocumentationCommentTrivia:
				case SyntaxKind.MultiLineDocumentationCommentTrivia:
				case SyntaxKind.DisabledTextTrivia:
				case SyntaxKind.PreprocessingMessageTrivia:
				case SyntaxKind.IfDirectiveTrivia:
				case SyntaxKind.ElifDirectiveTrivia:
				case SyntaxKind.ElseDirectiveTrivia:
				case SyntaxKind.EndIfDirectiveTrivia:
				case SyntaxKind.RegionDirectiveTrivia:
				case SyntaxKind.EndRegionDirectiveTrivia:
				case SyntaxKind.DefineDirectiveTrivia:
				case SyntaxKind.UndefDirectiveTrivia:
				case SyntaxKind.ErrorDirectiveTrivia:
				case SyntaxKind.WarningDirectiveTrivia:
				case SyntaxKind.LineDirectiveTrivia:
				case SyntaxKind.PragmaWarningDirectiveTrivia:
				case SyntaxKind.PragmaChecksumDirectiveTrivia:
				case SyntaxKind.ReferenceDirectiveTrivia:
				case SyntaxKind.BadDirectiveTrivia:
				case SyntaxKind.SkippedTokensTrivia:
				case SyntaxKind.ConflictMarkerTrivia:
				case SyntaxKind.XmlElement:
				case SyntaxKind.XmlElementStartTag:
				case SyntaxKind.XmlElementEndTag:
				case SyntaxKind.XmlEmptyElement:
				case SyntaxKind.XmlTextAttribute:
				case SyntaxKind.XmlCrefAttribute:
				case SyntaxKind.XmlNameAttribute:
				case SyntaxKind.XmlName:
				case SyntaxKind.XmlPrefix:
				case SyntaxKind.XmlText:
				case SyntaxKind.XmlCDataSection:
				case SyntaxKind.XmlComment:
				case SyntaxKind.XmlProcessingInstruction:
				case SyntaxKind.TypeCref:
				case SyntaxKind.QualifiedCref:
				case SyntaxKind.NameMemberCref:
				case SyntaxKind.IndexerMemberCref:
				case SyntaxKind.OperatorMemberCref:
				case SyntaxKind.ConversionOperatorMemberCref:
				case SyntaxKind.CrefParameterList:
				case SyntaxKind.CrefBracketedParameterList:
				case SyntaxKind.CrefParameter:
				case SyntaxKind.IdentifierName:
				case SyntaxKind.QualifiedName:
				case SyntaxKind.GenericName:
				case SyntaxKind.TypeArgumentList:
				case SyntaxKind.AliasQualifiedName:
				case SyntaxKind.PredefinedType:
				case SyntaxKind.ArrayType:
				case SyntaxKind.ArrayRankSpecifier:
				case SyntaxKind.PointerType:
				case SyntaxKind.NullableType:
				case SyntaxKind.OmittedTypeArgument:
				case SyntaxKind.ParenthesizedExpression:
				case SyntaxKind.ConditionalExpression:
				case SyntaxKind.InvocationExpression:
				case SyntaxKind.ElementAccessExpression:
				case SyntaxKind.ArgumentList:
				case SyntaxKind.BracketedArgumentList:
				case SyntaxKind.Argument:
				case SyntaxKind.NameColon:
				case SyntaxKind.CastExpression:
				case SyntaxKind.AnonymousMethodExpression:
				case SyntaxKind.SimpleLambdaExpression:
				case SyntaxKind.ParenthesizedLambdaExpression:
				case SyntaxKind.ObjectInitializerExpression:
				case SyntaxKind.CollectionInitializerExpression:
				case SyntaxKind.ArrayInitializerExpression:
				case SyntaxKind.AnonymousObjectMemberDeclarator:
				case SyntaxKind.ComplexElementInitializerExpression:
				case SyntaxKind.ObjectCreationExpression:
				case SyntaxKind.AnonymousObjectCreationExpression:
				case SyntaxKind.ArrayCreationExpression:
				case SyntaxKind.ImplicitArrayCreationExpression:
				case SyntaxKind.StackAllocArrayCreationExpression:
				case SyntaxKind.OmittedArraySizeExpression:
				case SyntaxKind.InterpolatedStringExpression:
				case SyntaxKind.ImplicitElementAccess:
				case SyntaxKind.IsPatternExpression:
				case SyntaxKind.RangeExpression:
				case SyntaxKind.AddExpression:
				case SyntaxKind.SubtractExpression:
				case SyntaxKind.MultiplyExpression:
				case SyntaxKind.DivideExpression:
				case SyntaxKind.ModuloExpression:
				case SyntaxKind.LeftShiftExpression:
				case SyntaxKind.RightShiftExpression:
				case SyntaxKind.LogicalOrExpression:
				case SyntaxKind.LogicalAndExpression:
				case SyntaxKind.BitwiseOrExpression:
				case SyntaxKind.BitwiseAndExpression:
				case SyntaxKind.ExclusiveOrExpression:
				case SyntaxKind.IsExpression:
				case SyntaxKind.AsExpression:
				case SyntaxKind.CoalesceExpression:
				case SyntaxKind.SimpleMemberAccessExpression:
				case SyntaxKind.PointerMemberAccessExpression:
				case SyntaxKind.ConditionalAccessExpression:
				case SyntaxKind.MemberBindingExpression:
				case SyntaxKind.ElementBindingExpression:
				case SyntaxKind.SimpleAssignmentExpression:
				case SyntaxKind.AddAssignmentExpression:
				case SyntaxKind.SubtractAssignmentExpression:
				case SyntaxKind.MultiplyAssignmentExpression:
				case SyntaxKind.DivideAssignmentExpression:
				case SyntaxKind.ModuloAssignmentExpression:
				case SyntaxKind.AndAssignmentExpression:
				case SyntaxKind.ExclusiveOrAssignmentExpression:
				case SyntaxKind.OrAssignmentExpression:
				case SyntaxKind.LeftShiftAssignmentExpression:
				case SyntaxKind.RightShiftAssignmentExpression:
				case SyntaxKind.CoalesceAssignmentExpression:
				case SyntaxKind.UnaryPlusExpression:
				case SyntaxKind.UnaryMinusExpression:
				case SyntaxKind.BitwiseNotExpression:
				case SyntaxKind.LogicalNotExpression:
				case SyntaxKind.PreIncrementExpression:
				case SyntaxKind.PreDecrementExpression:
				case SyntaxKind.PointerIndirectionExpression:
				case SyntaxKind.AddressOfExpression:
				case SyntaxKind.PostIncrementExpression:
				case SyntaxKind.PostDecrementExpression:
				case SyntaxKind.AwaitExpression:
				case SyntaxKind.IndexExpression:
				case SyntaxKind.ThisExpression:
				case SyntaxKind.BaseExpression:
				case SyntaxKind.ArgListExpression:
				case SyntaxKind.NumericLiteralExpression:
				case SyntaxKind.StringLiteralExpression:
				case SyntaxKind.CharacterLiteralExpression:
				case SyntaxKind.TrueLiteralExpression:
				case SyntaxKind.FalseLiteralExpression:
				case SyntaxKind.NullLiteralExpression:
				case SyntaxKind.DefaultLiteralExpression:
				case SyntaxKind.TypeOfExpression:
				case SyntaxKind.SizeOfExpression:
				case SyntaxKind.CheckedExpression:
				case SyntaxKind.UncheckedExpression:
				case SyntaxKind.DefaultExpression:
				case SyntaxKind.MakeRefExpression:
				case SyntaxKind.RefValueExpression:
				case SyntaxKind.RefTypeExpression:
				case SyntaxKind.QueryExpression:
				case SyntaxKind.QueryBody:
				case SyntaxKind.FromClause:
				case SyntaxKind.LetClause:
				case SyntaxKind.JoinClause:
				case SyntaxKind.JoinIntoClause:
				case SyntaxKind.WhereClause:
				case SyntaxKind.OrderByClause:
				case SyntaxKind.AscendingOrdering:
				case SyntaxKind.DescendingOrdering:
				case SyntaxKind.SelectClause:
				case SyntaxKind.GroupClause:
				case SyntaxKind.QueryContinuation:
				case SyntaxKind.Block:
				case SyntaxKind.LocalDeclarationStatement:
				case SyntaxKind.VariableDeclaration:
				case SyntaxKind.VariableDeclarator:
				case SyntaxKind.EqualsValueClause:
				case SyntaxKind.ExpressionStatement:
				case SyntaxKind.EmptyStatement:
				case SyntaxKind.LabeledStatement:
				case SyntaxKind.GotoStatement:
				case SyntaxKind.GotoCaseStatement:
				case SyntaxKind.GotoDefaultStatement:
				case SyntaxKind.BreakStatement:
				case SyntaxKind.ContinueStatement:
				case SyntaxKind.ReturnStatement:
				case SyntaxKind.YieldReturnStatement:
				case SyntaxKind.YieldBreakStatement:
				case SyntaxKind.ThrowStatement:
				case SyntaxKind.WhileStatement:
				case SyntaxKind.DoStatement:
				case SyntaxKind.ForStatement:
				case SyntaxKind.ForEachStatement:
				case SyntaxKind.UsingStatement:
				case SyntaxKind.FixedStatement:
				case SyntaxKind.CheckedStatement:
				case SyntaxKind.UncheckedStatement:
				case SyntaxKind.UnsafeStatement:
				case SyntaxKind.LockStatement:
				case SyntaxKind.IfStatement:
				case SyntaxKind.ElseClause:
				case SyntaxKind.SwitchStatement:
				case SyntaxKind.SwitchSection:
				case SyntaxKind.CaseSwitchLabel:
				case SyntaxKind.DefaultSwitchLabel:
				case SyntaxKind.TryStatement:
				case SyntaxKind.CatchClause:
				case SyntaxKind.CatchDeclaration:
				case SyntaxKind.CatchFilterClause:
				case SyntaxKind.FinallyClause:
				case SyntaxKind.LocalFunctionStatement:
				case SyntaxKind.CompilationUnit:
				case SyntaxKind.GlobalStatement:
				case SyntaxKind.NamespaceDeclaration:
				case SyntaxKind.UsingDirective:
				case SyntaxKind.ExternAliasDirective:
				case SyntaxKind.AttributeList:
				case SyntaxKind.AttributeTargetSpecifier:
				case SyntaxKind.Attribute:
				case SyntaxKind.AttributeArgumentList:
				case SyntaxKind.AttributeArgument:
				case SyntaxKind.NameEquals:
				case SyntaxKind.ClassDeclaration:
				case SyntaxKind.StructDeclaration:
				case SyntaxKind.InterfaceDeclaration:
				case SyntaxKind.EnumDeclaration:
				case SyntaxKind.DelegateDeclaration:
				case SyntaxKind.BaseList:
				case SyntaxKind.SimpleBaseType:
				case SyntaxKind.TypeParameterConstraintClause:
				case SyntaxKind.ConstructorConstraint:
				case SyntaxKind.ClassConstraint:
				case SyntaxKind.StructConstraint:
				case SyntaxKind.TypeConstraint:
				case SyntaxKind.ExplicitInterfaceSpecifier:
				case SyntaxKind.EnumMemberDeclaration:
				case SyntaxKind.FieldDeclaration:
				case SyntaxKind.EventFieldDeclaration:
				case SyntaxKind.MethodDeclaration:
				case SyntaxKind.OperatorDeclaration:
				case SyntaxKind.ConversionOperatorDeclaration:
				case SyntaxKind.ConstructorDeclaration:
				case SyntaxKind.BaseConstructorInitializer:
				case SyntaxKind.ThisConstructorInitializer:
				case SyntaxKind.DestructorDeclaration:
				case SyntaxKind.PropertyDeclaration:
				case SyntaxKind.EventDeclaration:
				case SyntaxKind.IndexerDeclaration:
				case SyntaxKind.AccessorList:
				case SyntaxKind.GetAccessorDeclaration:
				case SyntaxKind.SetAccessorDeclaration:
				case SyntaxKind.AddAccessorDeclaration:
				case SyntaxKind.RemoveAccessorDeclaration:
				case SyntaxKind.UnknownAccessorDeclaration:
				case SyntaxKind.ParameterList:
				case SyntaxKind.BracketedParameterList:
				case SyntaxKind.Parameter:
				case SyntaxKind.TypeParameterList:
				case SyntaxKind.TypeParameter:
				case SyntaxKind.IncompleteMember:
				case SyntaxKind.ArrowExpressionClause:
				case SyntaxKind.Interpolation:
				case SyntaxKind.InterpolatedStringText:
				case SyntaxKind.InterpolationAlignmentClause:
				case SyntaxKind.InterpolationFormatClause:
				case SyntaxKind.ShebangDirectiveTrivia:
				case SyntaxKind.LoadDirectiveTrivia:
				case SyntaxKind.TupleType:
				case SyntaxKind.TupleElement:
				case SyntaxKind.TupleExpression:
				case SyntaxKind.SingleVariableDesignation:
				case SyntaxKind.ParenthesizedVariableDesignation:
				case SyntaxKind.ForEachVariableStatement:
				case SyntaxKind.DeclarationPattern:
				case SyntaxKind.ConstantPattern:
				case SyntaxKind.CasePatternSwitchLabel:
				case SyntaxKind.WhenClause:
				case SyntaxKind.DiscardDesignation:
				case SyntaxKind.RecursivePattern:
				case SyntaxKind.PropertyPatternClause:
				case SyntaxKind.Subpattern:
				case SyntaxKind.PositionalPatternClause:
				case SyntaxKind.DiscardPattern:
				case SyntaxKind.SwitchExpression:
				case SyntaxKind.SwitchExpressionArm:
				case SyntaxKind.VarPattern:
				case SyntaxKind.DeclarationExpression:
				case SyntaxKind.RefExpression:
				case SyntaxKind.RefType:
				case SyntaxKind.ThrowExpression:
				case SyntaxKind.ImplicitStackAllocArrayCreationExpression:
				case SyntaxKind.SuppressNullableWarningExpression:
				case SyntaxKind.NullableDirectiveTrivia:
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
			if (union is null)
			{
				Debug.Fail("Unexpected null union");
				return true;
			}

			return !(union.Equals(leftValueScope) && union.Equals(rightValueScope));
		}


		private IValueScope? GetValueScope(ExpressionSyntax expressionSyntax)
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
			{
				Debug.Fail("Unexpected null value scope");
				return true;
			}

			var intersection = leftValueScope.Intersection(rightValueScope);

			if (intersection is null)
			{
				Debug.Fail("Unexpected null intersection");
				return true;
			}

			return !intersection.Equals(EmptyValueScope.Instance);
		}

		public SymbolicAnalysisContext WithDeclaration(VariableInfo variable)
		{
			return new SymbolicAnalysisContext(Conditions, Variables.Append(variable).ToImmutableArray());
		}

		public ImmutableArray<VariableInfo> Variables { get; }
	}
}