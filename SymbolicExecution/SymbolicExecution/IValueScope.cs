namespace SymbolicExecution;

public interface IValueScope
{
	TaggedUnion<IValueScope, AnalysisFailure> AddConstraint(
		IConstraint constraint,
		Location location,
		SymbolicExecutionState symbolicExecutionState
		);
	TaggedUnion<bool, AnalysisFailure> GetIsReachable(Location location);
}

public interface IConstraint
{
}