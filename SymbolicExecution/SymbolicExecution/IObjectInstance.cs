namespace SymbolicExecution;

public interface IObjectInstance : IEquatable<IObjectInstance>
{
	TaggedUnion<ITypeSymbol, Type> ActualTypeSymbol { get; }
	TaggedUnion<ITypeSymbol, Type> ConvertedTypeSymbol { get; }
	Location Location { get; }
	IValueScope Value { get; }
	bool IsExactType(Type type);
}