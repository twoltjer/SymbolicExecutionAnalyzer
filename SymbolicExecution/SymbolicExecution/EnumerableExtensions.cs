using System.Collections.Generic;

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
}