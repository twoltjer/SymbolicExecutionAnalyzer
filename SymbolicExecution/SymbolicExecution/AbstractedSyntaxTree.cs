using System.Collections.Generic;
using System.Diagnostics;

namespace SymbolicExecution;

public class AbstractedSyntaxTree : IAbstractedSyntaxTree
{
	private readonly SemanticModel _semanticModel;
	private ISyntaxNodeAbstraction? _abstraction;

	public AbstractedSyntaxTree(SemanticModel semanticModel)
	{
		_semanticModel = semanticModel;
	}

	public ISyntaxNodeAbstraction GetRoot()
	{
		_abstraction ??= ProduceAbstraction<SyntaxNodeAbstraction, SyntaxNode>(_semanticModel, _semanticModel.SyntaxTree.GetRoot());
		return _abstraction;
	}

	private TAbstractedType ProduceAbstraction<TAbstractedType, TCompileType>(SemanticModel model, TCompileType node)
		where TAbstractedType : SyntaxNodeAbstraction
		where TCompileType : SyntaxNode
	{
		var children = node.ChildNodes().Select(x => ProduceAbstraction<SyntaxNodeAbstraction, SyntaxNode>(model, x)).ToImmutableArray();

		var symbol = model.GetDeclaredSymbol(node);

		var result = node switch
		{
			BlockSyntax => new BlockSyntaxAbstraction(children, symbol) as TAbstractedType,
			IdentifierNameSyntax => new IdentifierNameSyntaxAbstraction(children, symbol) as TAbstractedType,
			UsingDirectiveSyntax => new UsingDirectiveSyntaxAbstraction(children, symbol) as TAbstractedType,
			CompilationUnitSyntax => new CompilationUnitSyntaxAbstraction(children, symbol) as TAbstractedType,
			QualifiedNameSyntax => new QualifiedNameSyntaxAbstraction(children, symbol) as TAbstractedType,
			AttributeSyntax => new AttributeSyntaxAbstraction(children, symbol) as TAbstractedType,
			AttributeListSyntax => new AttributeListSyntaxAbstraction(children, symbol) as TAbstractedType,
			PredefinedTypeSyntax => new PredefinedTypeSyntaxAbstraction(children, symbol) as TAbstractedType,
			ParameterListSyntax => new ParameterListSyntaxAbstraction(children, symbol) as TAbstractedType,
			ArgumentListSyntax => new ArgumentListSyntaxAbstraction(children, symbol) as TAbstractedType,
			ObjectCreationExpressionSyntax => new ObjectCreationExpressionSyntaxAbstraction(children, symbol) as TAbstractedType,
			ThrowStatementSyntax => new ThrowStatementSyntaxAbstraction(children, symbol) as TAbstractedType,
			MethodDeclarationSyntax methodDeclarationSyntax => new MethodDeclarationSyntaxAbstraction(children, symbol, methodDeclarationSyntax.GetLocation()) as TAbstractedType,
			ClassDeclarationSyntax => new ClassDeclarationSyntaxAbstraction(children, symbol) as TAbstractedType,
			_ => null,
		};

		if (result == null)
		{
			Debug.Fail("Unknown node type or soft cast failed");
			throw new InvalidOperationException("Abstraction result is null");
		}

		return result;
	}
}

public class ThrowStatementSyntaxAbstraction : StatementSyntaxAbstraction, IThrowStatementSyntaxAbstraction
{
	public ThrowStatementSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public interface IThrowStatementSyntaxAbstraction : IStatementSyntaxAbstraction
{
}

public class UsingDirectiveSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IUsingDirectiveSyntaxAbstraction
{
	public UsingDirectiveSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public class ParameterListSyntaxAbstraction : BaseParameterListSyntaxAbstraction, IParameterListSyntaxAbstraction
{
	public ParameterListSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public class MethodDeclarationSyntaxAbstraction : BaseMethodDeclarationSyntaxAbstraction, IMethodDeclarationSyntaxAbstraction
{
	public Location? SourceLocation { get; }

	public MethodDeclarationSyntaxAbstraction(
		ImmutableArray<SyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location? sourceLocation
		) : base(children, symbol)
	{
		SourceLocation = sourceLocation;
	}
}

public interface IMethodDeclarationSyntaxAbstraction : IBaseMethodDeclarationSyntaxAbstraction
{
	Location? SourceLocation { get; }
}

public interface IBaseMethodDeclarationSyntaxAbstraction : IMemberDeclarationSyntaxAbstraction
{
}

public interface IMemberDeclarationSyntaxAbstraction : ICSharpSyntaxNodeAbstraction
{
}

public class ClassDeclarationSyntaxAbstraction : TypeDeclarationSyntaxAbastraction, IClassDeclarationSyntaxAbstraction
{
	public ClassDeclarationSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public interface IClassDeclarationSyntaxAbstraction : ITypeDeclarationSyntaxAbastraction
{
}

public class TypeDeclarationSyntaxAbastraction : BaseTypeDeclarationSyntaxAbstraction, ITypeDeclarationSyntaxAbastraction
{
	public TypeDeclarationSyntaxAbastraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public interface ITypeDeclarationSyntaxAbastraction : IBaseTypeDeclarationSyntaxAbstraction
{
}

public class BaseTypeDeclarationSyntaxAbstraction : MemberDeclarationSyntaxAbstraction, IBaseTypeDeclarationSyntaxAbstraction
{
	public BaseTypeDeclarationSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public interface IBaseTypeDeclarationSyntaxAbstraction : IMemberDeclarationSyntaxAbstraction
{
}

public class BaseMethodDeclarationSyntaxAbstraction : MemberDeclarationSyntaxAbstraction, IBaseMethodDeclarationSyntaxAbstraction
{
	public BaseMethodDeclarationSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public class MemberDeclarationSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IMemberDeclarationSyntaxAbstraction
{
	public MemberDeclarationSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public class ArgumentListSyntaxAbstraction : BaseArgumentListSyntaxAbstraction, IArgumentListSyntaxAbstraction
{
	public ArgumentListSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public interface IArgumentListSyntaxAbstraction : IBaseArgumentListSyntaxAbstraction
{
}

public interface IBaseArgumentListSyntaxAbstraction : ICSharpSyntaxNodeAbstraction
{
}

public class BaseArgumentListSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IBaseArgumentListSyntaxAbstraction
{
	public BaseArgumentListSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public interface IParameterListSyntaxAbstraction : IBaseParameterListSyntaxAbstraction
{
}

public class ObjectCreationExpressionSyntaxAbstraction : BaseObjectCreationExpressionSyntaxAbstraction, IObjectCreationExpressionSyntaxAbstraction
{
	public ObjectCreationExpressionSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public class BaseObjectCreationExpressionSyntaxAbstraction : ExpressionSyntaxAbstraction, IBaseObjectCreationExpressionSyntaxAbstraction
{
	public BaseObjectCreationExpressionSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public interface IBaseObjectCreationExpressionSyntaxAbstraction : IExpressionSyntaxAbstraction
{
}

public interface IObjectCreationExpressionSyntaxAbstraction : IBaseObjectCreationExpressionSyntaxAbstraction
{
}

public class BaseParameterListSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IBaseParameterListSyntaxAbstraction
{
	public BaseParameterListSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public interface IBaseParameterListSyntaxAbstraction : ICSharpSyntaxNodeAbstraction
{
}

public interface IUsingDirectiveSyntaxAbstraction : ICSharpSyntaxNodeAbstraction
{
}

public class PredefinedTypeSyntaxAbstraction : TypeSyntaxAbstraction, IPredefinedTypeSyntaxAbstraction
{
	public PredefinedTypeSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public interface IPredefinedTypeSyntaxAbstraction : ITypeSyntaxAbstraction
{
}

public class QualifiedNameSyntaxAbstraction : NameSyntaxAbstraction, IQualifiedNameSyntaxAbstraction
{
	public QualifiedNameSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public class AttributeListSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IAttributeListSyntaxAbstraction
{
	public AttributeListSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public interface IAttributeListSyntaxAbstraction : ICSharpSyntaxNodeAbstraction
{
}

public class AttributeSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IAttributeSyntaxAbstraction
{
	public AttributeSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public interface IAttributeSyntaxAbstraction : ICSharpSyntaxNodeAbstraction
{
}

public interface IQualifiedNameSyntaxAbstraction : INameSyntaxAbstraction
{
}

public class CompilationUnitSyntaxAbstraction : CSharpSyntaxNodeAbstraction, ICompilationUnitSyntaxAbstraction
{
	public CompilationUnitSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public interface ICompilationUnitSyntaxAbstraction : ICSharpSyntaxNodeAbstraction
{
}

public class BlockSyntaxAbstraction : StatementSyntaxAbstraction, IBlockSyntaxAbstraction
{
	public BlockSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public class StatementSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IStatementSyntaxAbstraction
{
	public StatementSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public class IdentifierNameSyntaxAbstraction : SimpleNameSyntaxAbstraction, IIdentifierNameSyntaxAbstraction
{
	public IdentifierNameSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public interface IIdentifierNameSyntaxAbstraction : ISimpleNameSyntaxAbstraction
{
}

public interface ISimpleNameSyntaxAbstraction : INameSyntaxAbstraction
{
}

public interface INameSyntaxAbstraction : ITypeSyntaxAbstraction
{
}

public interface ITypeSyntaxAbstraction : IExpressionSyntaxAbstraction
{
}

public interface IExpressionSyntaxAbstraction : IExpressionOrPatternSyntaxAbstraction
{
}

public interface IExpressionOrPatternSyntaxAbstraction : ICSharpSyntaxNodeAbstraction
{
}

public class SimpleNameSyntaxAbstraction : NameSyntaxAbstraction, ISimpleNameSyntaxAbstraction
{
	public SimpleNameSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public class NameSyntaxAbstraction : TypeSyntaxAbstraction, INameSyntaxAbstraction
{
	public NameSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public class TypeSyntaxAbstraction : ExpressionSyntaxAbstraction, ITypeSyntaxAbstraction
{
	public TypeSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public class ExpressionSyntaxAbstraction : ExpressionOrPatternSyntaxAbstraction, IExpressionSyntaxAbstraction
{
	public ExpressionSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public class ExpressionOrPatternSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IExpressionOrPatternSyntaxAbstraction
{
	public ExpressionOrPatternSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public class CSharpSyntaxNodeAbstraction : SyntaxNodeAbstraction, ICSharpSyntaxNodeAbstraction
{
	public CSharpSyntaxNodeAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}

public interface IStatementSyntaxAbstraction : ICSharpSyntaxNodeAbstraction
{
}

public interface ICSharpSyntaxNodeAbstraction : ISyntaxNodeAbstraction
{
}

public class SyntaxNodeAbstraction : ISyntaxNodeAbstraction
{
	public SyntaxNodeAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol)
	{
		Children = children;
		Symbol = symbol;
	}

	public ImmutableArray<SyntaxNodeAbstraction> Children { get; }
	public IEnumerable<ISyntaxNodeAbstraction> GetDescendantNodes(bool includeSelf)
	{
		foreach (var child in Children)
		{
			yield return child;

			foreach (var descendant in child.GetDescendantNodes(includeSelf: false))
			{
				yield return descendant;
			}
		}

		if (includeSelf)
		{
			yield return this;
		}
	}

	public ISymbol? Symbol { get; }
}