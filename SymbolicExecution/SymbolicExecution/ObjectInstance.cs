namespace SymbolicExecution;

public struct ObjectInstance
{
    public ITypeSymbol ActualTypeSymbol { get; }
    public ITypeSymbol ConvertedTypeSymbol { get; }
    public Location Location { get; }

    public ObjectInstance(ITypeSymbol actualTypeSymbol, ITypeSymbol convertedTypeSymbol, Location location)
    {
        ActualTypeSymbol = actualTypeSymbol;
        ConvertedTypeSymbol = convertedTypeSymbol;
        Location = location;
    }
}