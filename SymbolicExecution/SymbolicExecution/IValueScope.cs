namespace SymbolicExecution;

public interface IValueScope
{
	bool CanBe(object? value);
	bool IsExactType(Type type);
	bool IsAlways(object? value);
	TaggedUnion<IEnumerable<(IValueScope, IAnalysisState)>, AnalysisFailure> EqualsOperator(IValueScope other, IAnalysisState state, Location location);
	TaggedUnion<IEnumerable<(IValueScope, IAnalysisState)>, AnalysisFailure> GreaterThanOperator(IValueScope other, IAnalysisState state, Location location);
	TaggedUnion<IEnumerable<(IValueScope, IAnalysisState)>, AnalysisFailure> LessThanOperator(IObjectInstance right, IAnalysisState state, Location location);
	TaggedUnion<IEnumerable<(IValueScope, IAnalysisState)>, AnalysisFailure> LogicalAndOperator(IObjectInstance right, IAnalysisState state, Location location);
	TaggedUnion<IEnumerable<(IValueScope, IAnalysisState)>, AnalysisFailure> LogicalOrOperator(IObjectInstance right, IAnalysisState state, Location location);
	TaggedUnion<IEnumerable<(IValueScope, IAnalysisState)>, AnalysisFailure> GreaterThanOrEqualOperator(IObjectInstance right, IAnalysisState state, Location location);
	TaggedUnion<IEnumerable<(IValueScope, IAnalysisState)>, AnalysisFailure> LessThanOrEqualOperator(IObjectInstance right, IAnalysisState state, Location location);
	TaggedUnion<IEnumerable<(IValueScope, IAnalysisState)>, AnalysisFailure> NotEqualsOperator(IObjectInstance right, IAnalysisState state, Location location);
	TaggedUnion<IEnumerable<(IValueScope, IAnalysisState)>, AnalysisFailure> AddOperator(IObjectInstance right, IAnalysisState state, Location location);
	TaggedUnion<IEnumerable<(IValueScope, IAnalysisState)>, AnalysisFailure> SubtractOperator(IObjectInstance right, IAnalysisState state, Location location);
	TaggedUnion<IEnumerable<(IValueScope, IAnalysisState)>, AnalysisFailure> MultiplyOperator(IObjectInstance right, IAnalysisState state, Location location);
	TaggedUnion<IEnumerable<(IValueScope, IAnalysisState)>, AnalysisFailure> DivideOperator(IObjectInstance right, IAnalysisState state, Location location);
	TaggedUnion<IEnumerable<(IValueScope, IAnalysisState)>, AnalysisFailure> ExclusiveOrOperator(IObjectInstance right, IAnalysisState state, Location location);
}