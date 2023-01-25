namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class BlockSyntaxAbstraction : StatementSyntaxAbstraction, IBlockSyntaxAbstraction
{
	public BlockSyntaxAbstraction(ImmutableArray<SyntaxNodeAbstraction> children, ISymbol? symbol) : base(children, symbol)
	{
	}
}