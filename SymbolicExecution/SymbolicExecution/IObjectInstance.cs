namespace SymbolicExecution;

public interface IObjectInstance
{
	TaggedUnion<ITypeSymbol, Type> Type { get; }
	Location Location { get; }
	IValueScope Value { get; }
	bool IsExactType(Type type);
}