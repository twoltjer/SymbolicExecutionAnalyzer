namespace SymbolicExecution.AbstractSyntaxTree.Interfaces;

public interface IMethodDeclarationSyntaxAbstraction : IBaseMethodDeclarationSyntaxAbstraction
{
	Location? SourceLocation { get; }
}