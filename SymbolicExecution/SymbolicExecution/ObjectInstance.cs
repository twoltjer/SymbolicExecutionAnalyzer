namespace SymbolicExecution;

public struct ObjectInstance : IObjectInstance
{
	public ITypeSymbol ActualTypeSymbol { get; }
	public ITypeSymbol ConvertedTypeSymbol { get; }
	public IValueScope Value { get; }
	public Location Location { get; }
	public bool IsExactType(Type type) => Value.IsExactType(type);

	public ObjectInstance(ITypeSymbol actualTypeSymbol, ITypeSymbol convertedTypeSymbol, Location location, IValueScope value)
	{
		ActualTypeSymbol = actualTypeSymbol;
		ConvertedTypeSymbol = convertedTypeSymbol;
		Location = location;
		Value = value;
	}
}