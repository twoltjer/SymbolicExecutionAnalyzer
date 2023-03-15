namespace SymbolicExecution;

public struct ObjectInstance : IObjectInstance
{
	public TaggedUnion<ITypeSymbol, Type> ActualTypeSymbol { get; }
	public TaggedUnion<ITypeSymbol, Type> ConvertedTypeSymbol { get; }
	public IValueScope Value { get; }
	public Location Location { get; }
	public bool IsExactType(Type type) => Value.IsExactType(type);

	public ObjectInstance(TaggedUnion<ITypeSymbol, Type> actualTypeSymbol, TaggedUnion<ITypeSymbol, Type> convertedTypeSymbol, Location location, IValueScope value)
	{
		ActualTypeSymbol = actualTypeSymbol;
		ConvertedTypeSymbol = convertedTypeSymbol;
		Location = location;
		Value = value;
	}
}