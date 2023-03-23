namespace SymbolicExecution;

/// <summary>
/// Represents an instance of an object or value
/// </summary>
public interface IObjectInstance
{
	/// <summary>
	/// The actual type of the object, as determined by the compiler
	/// </summary>
	TaggedUnion<ITypeSymbol, Type> ActualTypeSymbol { get; }
	/// <summary>
	/// The type of the object as known to the current context (e.g. BaseClass a = new DerivedClass())
	/// </summary>
	TaggedUnion<ITypeSymbol, Type> ConvertedTypeSymbol { get; }
	/// <summary>
	/// Location that the object was created
	/// </summary>
	Location Location { get; }
	/// <summary>
	/// Current value scope of the object
	/// </summary>
	IValueScope Value { get; }
	/// <summary>
	/// Checks if the object is of the exact type specified (note, does not check for inheritance)
	/// </summary>
	/// <param name="type">A runtime and/or system type</param>
	/// <returns>True if the object is of the exact type specified</returns>
	bool IsExactType(Type type);
}