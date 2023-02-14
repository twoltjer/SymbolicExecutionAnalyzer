namespace SymbolicExecution;

public interface IObjectInstance
{
	TaggedUnion<ITypeSymbol, Type> Type { get; }
	Location Location { get; }
	IValueScope Value { get; }
	bool IsExactType(Type type);
	TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> EqualsOperator(IObjectInstance right, IAnalysisState state);
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