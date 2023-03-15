namespace SymbolicExecution.AbstractSyntaxTree.Interfaces;

public interface IPredefinedTypeSyntaxAbstraction : ITypeSyntaxAbstraction
{
    ISymbol? ActualTypeSymbol { get; }
}