namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class LiteralExpressionSyntaxAbstraction : ExpressionSyntaxAbstraction, ILiteralExpressionSyntaxAbstraction
{
	private readonly Optional<object?> _constantValue;

	public LiteralExpressionSyntaxAbstraction(
		ImmutableArray<ISyntaxNodeAbstraction> children,
		ISymbol? symbol,
		Location location,
		Optional<object?> constantValue,
		ITypeSymbol? type
		) : base(children, symbol, location, type)
	{
		_constantValue = constantValue;
	}

	public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
	{
		return new AnalysisFailure("Cannot analyze literal expressions", Location);
	}

	public override TaggedUnion<ImmutableArray<(int, IAnalysisState)>, AnalysisFailure> GetResults(IAnalysisState state)
	{
		if (!_constantValue.HasValue)
			return new AnalysisFailure("Cannot analyze literal expressions without constant values", Location);

		if (_type == null)
			return new AnalysisFailure("Cannot analyze literal expressions without actual type symbols", Location);

		var scope = new ConstantValueScope(_constantValue.Value, new TaggedUnion<ITypeSymbol, Type>(_type));
		var isBool = scope.IsExactType(typeof(bool));
		var isInt = scope.IsExactType(typeof(int));
		var isLong = scope.IsExactType(typeof(long));
		var isFloat = scope.IsExactType(typeof(float));
		var isDouble = scope.IsExactType(typeof(double));
		var isUInt = scope.IsExactType(typeof(uint));
		var isULong = scope.IsExactType(typeof(ulong));
		var isDecimal = scope.IsExactType(typeof(decimal));
		var isByte = scope.IsExactType(typeof(byte));
		var isSByte = scope.IsExactType(typeof(sbyte));
		var isShort = scope.IsExactType(typeof(short));
		var isUShort = scope.IsExactType(typeof(ushort));
		var isChar = scope.IsExactType(typeof(char));

		ObjectInstance result;
		if (isBool)
		{
			result = new BoolInstance(Location, scope, ObjectInstance.GetNextReferenceId());
		}
		else if (isInt)
		{
			result = new IntInstance(Location, scope, ObjectInstance.GetNextReferenceId());
		}
		else if (isLong)
		{
			result = new LongInstance(Location, scope, ObjectInstance.GetNextReferenceId());
		}
		else if (isFloat)
		{
			result = new FloatInstance(Location, scope, ObjectInstance.GetNextReferenceId());
		}
		else if (isDouble)
		{
			result = new DoubleInstance(Location, scope, ObjectInstance.GetNextReferenceId());
		}
		else if (isUInt)
		{
			result = new UIntInstance(Location, scope, ObjectInstance.GetNextReferenceId());
		}
		else if (isULong)
		{
			result = new ULongInstance(Location, scope, ObjectInstance.GetNextReferenceId());
		}
		else if (isDecimal)
		{
			result = new DecimalInstance(Location, scope, ObjectInstance.GetNextReferenceId());
		}
		else if (isByte)
		{
			result = new ByteInstance(Location, scope, ObjectInstance.GetNextReferenceId());
		}
		else if (isSByte)
		{
			result = new SByteInstance(Location, scope, ObjectInstance.GetNextReferenceId());
		}
		else if (isShort)
		{
			result = new ShortInstance(Location, scope, ObjectInstance.GetNextReferenceId());
		}
		else if (isUShort)
		{
			result = new UShortInstance(Location, scope, ObjectInstance.GetNextReferenceId());
		}
		else if (isChar)
		{
			result = new CharInstance(Location, scope, ObjectInstance.GetNextReferenceId());
		}
		else
		{
			return new AnalysisFailure("Literal expressions not a known type", Location);
		}

		state = state.AddReference(result.Reference, result);
		return ImmutableArray.Create((result.Reference, state));
	}
}