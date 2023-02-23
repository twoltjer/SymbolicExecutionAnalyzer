namespace SymbolicExecution;

public interface IValueScope
{
	bool CouldBe(object? value);
	bool IsAlways(object? value);
	TaggedUnion<IAnalysisState, AnalysisFailure> ApplyConstraint(IConstraint exactValueConstraint);
}

public interface IConstraint
{
}