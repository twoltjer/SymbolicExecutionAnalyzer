namespace SymbolicExecution;

public interface ISymbolicExecutionException
{
    Location Location { get; }
    ITypeSymbol Type { get; }
}