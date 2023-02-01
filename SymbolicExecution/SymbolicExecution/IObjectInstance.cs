namespace SymbolicExecution;

public interface IObjectInstance
{
	ITypeSymbol ActualTypeSymbol { get; }
	ITypeSymbol ConvertedTypeSymbol { get; }
	Location Location { get; }
}