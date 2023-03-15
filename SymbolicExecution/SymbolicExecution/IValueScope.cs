using System.Numerics;

namespace SymbolicExecution;

public interface IValueScope
{
	bool CanBe(object? value);
	bool IsExactType(Type type);
	bool IsAlways(object? value);
}

public class IntArrayValueScope : IValueScope
{
	public IntArrayValueScope(BigInteger[] values)
	{
		Values = values;
	}

	public BigInteger[] Values { get; set; }
	public bool CanBe(object? value)
	{
		throw new NotImplementedException();
	}

	public bool IsExactType(Type type)
	{
		throw new NotImplementedException();
	}

	public bool IsAlways(object? value)
	{
		throw new NotImplementedException();
	}
}