namespace SymbolicExecution;

public interface IObjectInstance
{
	IValueScope ValueScope { get; }
	TaggedUnion<ITypeSymbol, Type> Type { get; }
	Location Location { get; }
	int Reference { get; }
	bool IsType(Type type);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> EqualsOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GreaterThanOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LessThanOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalAndOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalOrOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalXorOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GreaterThanOrEqualOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LessThanOrEqualOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> NotEqualsOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> AddOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> SubtractOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> MultiplyOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> DivideOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> ModuloOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> BitwiseAndOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> BitwiseOrOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> BitwiseXorOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LeftShiftOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> RightShiftOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	IObjectInstance WithValueScope(IValueScope valueScope);
}