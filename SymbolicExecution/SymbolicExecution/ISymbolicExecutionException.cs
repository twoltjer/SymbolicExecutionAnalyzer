namespace SymbolicExecution;

public interface ISymbolicExecutionException : IEquatable<ISymbolicExecutionException>
{
    Location Location { get; }
    TaggedUnion<ITypeSymbol, Type> Type { get; }
    IMethodSymbol MethodSymbol { get; }
}