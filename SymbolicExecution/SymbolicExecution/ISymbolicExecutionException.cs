namespace SymbolicExecution;

public interface ISymbolicExecutionException
{
    Location Location { get; }
    TaggedUnion<ITypeSymbol, Type> Type { get; }
    IMethodSymbol MethodSymbol { get; }
}