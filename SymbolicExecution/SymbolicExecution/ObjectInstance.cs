namespace SymbolicExecution;

public struct ObjectInstance : IObjectInstance
{
	public IValueScope Value { get; }
	public TaggedUnion<ITypeSymbol, Type> Type { get; }
	public Location Location { get; }
	public bool IsExactType(Type type) => Value.IsExactType(type);

	public ObjectInstance(TaggedUnion<ITypeSymbol, Type> type, Location location, IValueScope value)
	{
		Type = type;
		Location = location;
		Value = value;
	}
}