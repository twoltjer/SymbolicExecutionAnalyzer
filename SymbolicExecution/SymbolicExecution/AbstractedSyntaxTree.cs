using System.Diagnostics;

namespace SymbolicExecution;

public class AbstractedSyntaxTree : IAbstractedSyntaxTree
{
	private readonly BlockSyntax _methodBody;
	private IBlockSyntaxAbstraction? _abstraction;

	public AbstractedSyntaxTree(BlockSyntax methodBody)
	{
		_methodBody = methodBody;
	}

	public IBlockSyntaxAbstraction GetRoot()
	{
		_abstraction ??= ProduceAbstraction<BlockSyntaxAbstraction, BlockSyntax>(_methodBody);
		return _abstraction;
	}

	private TAbstractedType ProduceAbstraction<TAbstractedType, TCompileType>(TCompileType node)
		where TAbstractedType : SyntaxNodeAbstraction
		where TCompileType : SyntaxNode
	{
		var children = node.ChildNodes().Select(ProduceAbstraction<SyntaxNodeAbstraction, SyntaxNode>).ToImmutableArray();

		var result = node switch
		{
			BlockSyntax => new BlockSyntaxAbstraction { Children = children } as TAbstractedType,
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

public class BlockSyntaxAbstraction : StatementSyntaxAbstraction, IBlockSyntaxAbstraction
{
}

public class StatementSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IStatementSyntaxAbstraction
{
}

public class CSharpSyntaxNodeAbstraction : SyntaxNodeAbstraction, ICSharpSyntaxNodeAbstraction
{
}

public interface IStatementSyntaxAbstraction : ICSharpSyntaxNodeAbstraction
{
}

public interface ICSharpSyntaxNodeAbstraction : ISyntaxNodeAbstraction
{
}

public class SyntaxNodeAbstraction : ISyntaxNodeAbstraction
{
	public ImmutableArray<SyntaxNodeAbstraction> Children { get; set; }
}