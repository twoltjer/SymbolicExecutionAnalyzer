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