namespace SymbolicExecution;

public sealed class UninitializedValueScope : IValueScope
{
	public static UninitializedValueScope Instance { get; } = new UninitializedValueScope();

	public Result<IValueScope> Union(IValueScope other) => new Result<IValueScope>(
		new AnalysisErrorInfo($"{nameof(UninitializedValueScope)}.{nameof(Union)}: Invalid operation", default)
		);

	public Result<IValueScope> Intersection(IValueScope other) => new Result<IValueScope>(
		new AnalysisErrorInfo($"{nameof(UninitializedValueScope)}.{nameof(Intersection)}: Invalid operation", default)
		);

	public bool Equals(IValueScope other) => other is UninitializedValueScope;
}