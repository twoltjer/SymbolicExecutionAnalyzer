namespace SymbolicExecution;

/// <summary>
/// Stores information on what a value can be. This is used to store information on the values of variables and
/// parameters, as well as the values of expressions.
/// </summary>
public interface IValueScope
{
	bool CanBe(object? value);
	bool IsExactType(Type type);
	bool IsAlways(object? value);
}