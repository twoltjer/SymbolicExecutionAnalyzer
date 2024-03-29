using System.Numerics;

namespace SymbolicExecution;

/// <summary>
/// Represents an array of integer values. Stores them in BigInteger form to avoid overflow issues.
/// </summary>
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
}