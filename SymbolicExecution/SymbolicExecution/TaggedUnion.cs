namespace SymbolicExecution;

public readonly struct TaggedUnion<T1, T2> : IEquatable<TaggedUnion<T1, T2>>
{
	public bool Equals(TaggedUnion<T1, T2> other)
	{
		return IsT1 == other.IsT1 && EqualityComparer<T1>.Default.Equals(T1Value, other.T1Value) && EqualityComparer<T2>.Default.Equals(T2Value, other.T2Value);
	}

	public override bool Equals(object? obj)
	{
		return obj is TaggedUnion<T1, T2> other && Equals(other);
	}

	public override int GetHashCode()
	{
		unchecked
		{
			var hashCode = IsT1.GetHashCode();
			hashCode = (hashCode * 397) ^ EqualityComparer<T1>.Default.GetHashCode(T1Value);
			hashCode = (hashCode * 397) ^ EqualityComparer<T2>.Default.GetHashCode(T2Value);
			return hashCode;
		}
	}

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

	public TResult Match<TResult>(Func<T1, TResult> f1, Func<T2, TResult> f2)
	{
		return IsT1 ? f1(T1Value) : f2(T2Value);
	}

	public static bool operator==(TaggedUnion<T1, T2> left, TaggedUnion<T1, T2> right)
	{
		return left.Equals(right);
	}

	public static bool operator!=(TaggedUnion<T1, T2> left, TaggedUnion<T1, T2> right)
	{
		return !left.Equals(right);
	}
}

public static class TaggedUnionExtensions
{
	public static TaggedUnion<T1, T2> SafeCast<T1, T2, T3, T4>(this TaggedUnion<T3, T4> taggedUnion)
		where T3 : T1
		where T4 : T2
	{
		return taggedUnion.Match(
			t3 => new TaggedUnion<T1, T2>(t3),
			t4 => new TaggedUnion<T1, T2>(t4)
			);
	} 
}