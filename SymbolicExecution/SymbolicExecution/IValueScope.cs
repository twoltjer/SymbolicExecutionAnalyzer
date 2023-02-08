namespace SymbolicExecution;

public interface IValueScope
{
	bool CanBe(object? value);
	bool IsExactType(Type type);
	bool IsAlways(object? value);
}