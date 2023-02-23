namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public abstract class BaseParameterSyntaxAbstraction : CSharpSyntaxNodeAbstraction, IBaseParameterSyntaxAbstraction
{
    protected BaseParameterSyntaxAbstraction(ImmutableArray<ISyntaxNodeAbstraction> children, ISymbol? symbol, Location location) : base(children, symbol, location)
    {
    }
}