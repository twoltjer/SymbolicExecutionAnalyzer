using System.Diagnostics.Contracts;

namespace SymbolicExecution;

public struct TaggedUnion<T1, T2>
{
	public readonly bool IsT1;
	public readonly T1 T1Value = default!;
	public readonly T2 T2Value = default!;

	public TaggedUnion(T1 value)
	{
		IsT1 = true;
		T1Value = value;
		T2Value = default!;
	}

	public TaggedUnion(T2 value)
	{
		IsT1 = false;
		T1Value = default!;
		T2Value = value;
	}

	public static implicit operator TaggedUnion<T1, T2>(T1 value)
	{
		return new TaggedUnion<T1, T2>(value);
	}

	public static implicit operator TaggedUnion<T1, T2>(T2 value)
	{
		return new TaggedUnion<T1, T2>(value);
	}

	[Pure]
	public T Match<T>(Func<T1, T> t1Selector, Func<T2, T> t2Selector)
	{
		return IsT1 ? t1Selector(T1Value) : t2Selector(T2Value);
	}
}