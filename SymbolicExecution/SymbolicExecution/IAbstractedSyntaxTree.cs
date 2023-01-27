namespace SymbolicExecution;

public interface IAbstractedSyntaxTree
{
	TaggedUnion<ISyntaxNodeAbstraction, AnalysisFailure> GetRoot();
}