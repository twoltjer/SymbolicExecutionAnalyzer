using System.Diagnostics;

namespace SymbolicExecution;

public readonly struct Result<T>
{
	public AnalysisErrorInfo ErrorInfo { get; }

	private readonly T? _value;
	public T Value
	{
		get
		{
			if (_value is null)
				throw new NullReferenceException();

			return _value;
		}
	}

	public bool IsFaulted { get; }

	public Result(T value)
	{
		IsFaulted = false;
		_value = value;
		ErrorInfo = default;
	}

	public Result(AnalysisErrorInfo errorInfo)
	{
		ErrorInfo = errorInfo;
		_value = default;
		IsFaulted = true;
	}
}