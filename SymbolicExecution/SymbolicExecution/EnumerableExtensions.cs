namespace SymbolicExecution;

public static class EnumerableExtensions
{
	public static bool TryGetSingle<T>(this IEnumerable<T> enumerable, out T single)
	{
		using var enumerator = enumerable.GetEnumerator();
		if (!enumerator.MoveNext())
		{
			single = default!;
			return false;
		}

		single = enumerator.Current;
		return !enumerator.MoveNext();
	}

	[ExcludeFromCodeCoverage]
	public static T? FirstOrNull<T>(this IEnumerable<T> enumerable, Predicate<T>? predicate = null) where T : struct
	{
		foreach (var item in enumerable)
		{
			if (predicate == null || predicate(item))
			{
				return item;
			}
		}

		return null;
	}

	public static T? FirstOrNull<T>(this ImmutableArray<T> array, Predicate<T>? predicate = null) where T : struct
	{
		foreach (var item in array)
		{
			if (predicate == null || predicate(item))
			{
				return item;
			}
		}

		return null;
	}
}