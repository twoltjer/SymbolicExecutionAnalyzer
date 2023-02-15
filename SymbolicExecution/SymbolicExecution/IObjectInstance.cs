namespace SymbolicExecution;

public interface IObjectInstance
{
	IValueScope Value { get; }
	TaggedUnion<ITypeSymbol, Type> Type { get; }
	Location Location { get; }
	bool IsExactType(Type type);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> EqualsOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GreaterThanOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LessThanOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalAndOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalOrOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GreaterThanOrEqualOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LessThanOrEqualOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion);
}

public interface IValueTypeInstance : IObjectInstance
{
}

public interface IBoolInstance : IValueTypeInstance
{
}

public interface IIntInstance : IValueTypeInstance
{
}

public interface ILongInstance : IValueTypeInstance
{
}

public interface IFloatInstance : IValueTypeInstance
{
}

public interface IDoubleInstance : IValueTypeInstance
{
}

public interface IDecimalInstance : IValueTypeInstance
{
}

public interface ICharInstance : IValueTypeInstance
{
}

public interface IByteInstance : IValueTypeInstance
{
}

public interface IShortInstance : IValueTypeInstance
{
}

public interface IUIntInstance : IValueTypeInstance
{
}

public interface IULongInstance : IValueTypeInstance
{
}

public interface IUShortInstance : IValueTypeInstance
{
}

public interface ISByteInstance : IValueTypeInstance
{
}

