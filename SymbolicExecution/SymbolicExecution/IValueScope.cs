using System.Numerics;

namespace SymbolicExecution;

public interface IValueScope : IEquatable<IValueScope>
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

	public BigInteger[] Values { get; }
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

	public IValueScope SetElementValue(int index, int value)
	{
		var newValues = new BigInteger[Values.Length];
		Array.Copy(Values, newValues, Values.Length);
		newValues[index] = value;
		return new IntArrayValueScope(newValues);
	}
	
	public override string ToString()
	{
		return $"[{string.Join(", ", Values)}]";
	}

	protected bool Equals(IntArrayValueScope other)
	{
		return Values.SequenceEqual(other.Values);
	}

	public bool Equals(IValueScope other)
	{
		return other is IntArrayValueScope arrayValueScope && Equals(arrayValueScope);
	}

	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != this.GetType()) return false;
		return Equals((IntArrayValueScope)obj);
	}

	public override int GetHashCode()
	{
		return Values.GetHashCode();
	}
}