using System.Diagnostics.Contracts;

namespace SymbolicExecution;

/// <summary>
/// A tagged union of two types.
/// </summary>
/// <typeparam name="T1">The first type</typeparam>
/// <typeparam name="T2">The second type</typeparam>
public struct TaggedUnion<T1, T2>
{
	/// <summary>
	/// True if the union is of type T1, false if it is of type T2
	/// </summary>
	public readonly bool IsT1;
	/// <summary>
	/// The value of the union if it is of type T1. Always default if IsT1 is false.
	/// </summary>
	public readonly T1 T1Value = default!;
	/// <summary>
	/// The value of the union if it is of type T2. Always default if IsT1 is true.
	/// </summary>
	public readonly T2 T2Value = default!;

	/// <summary>
	/// Creates a new tagged union of type T1
	/// </summary>
	/// <param name="value">The value of the union</param>
	public TaggedUnion(T1 value)
	{
		IsT1 = true;
		T1Value = value;
		T2Value = default!;
	}

	/// <summary>
	/// Creates a new tagged union of type T2
	/// </summary>
	/// <param name="value">The value of the union</param>
	public TaggedUnion(T2 value)
	{
		IsT1 = false;
		T1Value = default!;
		T2Value = value;
	}

	/// <summary>
	/// Implicitly converts a T1 to a tagged union of T1 and T2. Nice when returning a tagged union from a method.
	/// </summary>
	/// <param name="value">The value to convert</param>
	/// <returns>A tagged union of T1 and T2</returns>
	public static implicit operator TaggedUnion<T1, T2>(T1 value)
	{
		return new TaggedUnion<T1, T2>(value);
	}

	/// <summary>
	/// Implicitly converts a T2 to a tagged union of T1 and T2. Nice when returning a tagged union from a method.
	/// </summary>
	/// <param name="value">The value to convert</param>
	/// <returns>A tagged union of T1 and T2</returns>
	public static implicit operator TaggedUnion<T1, T2>(T2 value)
	{
		return new TaggedUnion<T1, T2>(value);
	}

	/// <summary>
	/// Matches the tagged union to a value of type T
	/// </summary>
	/// <param name="t1Selector">A function that takes a T1 and returns a T</param>
	/// <param name="t2Selector">A function that takes a T2 and returns a T</param>
	/// <typeparam name="T">The type to match to</typeparam>
	/// <returns>A value of type T that is the result of the selector function</returns>
	[Pure]
	public T Match<T>(Func<T1, T> t1Selector, Func<T2, T> t2Selector)
	{
		return IsT1 ? t1Selector(T1Value) : t2Selector(T2Value);
	}
}