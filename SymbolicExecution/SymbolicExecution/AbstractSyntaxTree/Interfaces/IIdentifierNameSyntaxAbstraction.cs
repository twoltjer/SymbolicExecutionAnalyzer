namespace SymbolicExecution.AbstractSyntaxTree.Interfaces;

public interface IIdentifierNameSyntaxAbstraction : ISimpleNameSyntaxAbstraction
{
	bool IsVar { get; }
}