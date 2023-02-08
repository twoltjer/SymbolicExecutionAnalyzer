namespace SymbolicExecution;

public interface IObjectInstance
{
	ITypeSymbol ActualTypeSymbol { get; }
	ITypeSymbol ConvertedTypeSymbol { get; }
	Location Location { get; }
	IValueScope Value { get; }
	bool IsExactType(Type type);
}