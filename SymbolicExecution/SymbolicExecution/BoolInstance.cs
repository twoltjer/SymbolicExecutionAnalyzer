namespace SymbolicExecution;

public class BoolInstance : PrimitiveInstance<bool>, IBoolInstance
{
    public BoolInstance(Location location, IValueScope value, int reference) :
        base(location, value, reference)
    {
    }

    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalAndOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("LogicalAndOperator not implemented on BoolInstance", Location);
    }

    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalOrOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("LogicalOrOperator not implemented on BoolInstance", Location);
    }
    
    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalXorOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("LogicalXorOperator not implemented on BoolInstance", Location);
    }
    
    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GreaterThanOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("GreaterThanOperator not implemented on BoolInstance", Location);
    }
    
    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LessThanOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("LessThanOperator not implemented on BoolInstance", Location);
    }
    
    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GreaterThanOrEqualOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("GreaterThanOrEqualOperator not implemented on BoolInstance", Location);
    }
    
    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LessThanOrEqualOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("LessThanOrEqualOperator not implemented on BoolInstance", Location);
    }
    
    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> NotEqualsOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("NotEqualsOperator not implemented on BoolInstance", Location);
    }

    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> AddOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("AddOperator not implemented on BoolInstance", Location);
    }

    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> SubtractOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("SubtractOperator not implemented on BoolInstance", Location);
    }

    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> MultiplyOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("MultiplyOperator not implemented on BoolInstance", Location);
    }

    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> DivideOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("DivideOperator not implemented on BoolInstance", Location);
    }

    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> ModuloOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("ModuloOperator not implemented on BoolInstance", Location);
    }

    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> BitwiseAndOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("BitwiseAndOperator not implemented on BoolInstance", Location);
    }

    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> BitwiseOrOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("BitwiseOrOperator not implemented on BoolInstance", Location);
    }

    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> BitwiseXorOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("BitwiseXorOperator not implemented on BoolInstance", Location);
    }

    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LeftShiftOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("LeftShiftOperator not implemented on BoolInstance", Location);
    }

    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> RightShiftOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("RightShiftOperator not implemented on BoolInstance", Location);
    }

    public override IObjectInstance WithValueScope(IValueScope valueScope)
    {
        return new BoolInstance(Location, valueScope, Reference);
    }

    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> EqualsOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("EqualsOperator not implemented on BoolInstance", Location);
    }
}

public class IntInstance : PrimitiveInstance<int>, IBoolInstance
{
    public IntInstance(Location location, IValueScope value, int reference) :
        base(location, value, reference)
    {
    }

    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalAndOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("LogicalAndOperator not implemented on BoolInstance", Location);
    }

    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalOrOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("LogicalOrOperator not implemented on BoolInstance", Location);
    }
    
    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LogicalXorOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("LogicalXorOperator not implemented on BoolInstance", Location);
    }
    
    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GreaterThanOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("GreaterThanOperator not implemented on BoolInstance", Location);
    }
    
    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LessThanOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("LessThanOperator not implemented on BoolInstance", Location);
    }
    
    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GreaterThanOrEqualOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("GreaterThanOrEqualOperator not implemented on BoolInstance", Location);
    }
    
    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LessThanOrEqualOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("LessThanOrEqualOperator not implemented on BoolInstance", Location);
    }
    
    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> NotEqualsOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("NotEqualsOperator not implemented on BoolInstance", Location);
    }

    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> AddOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("AddOperator not implemented on BoolInstance", Location);
    }

    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> SubtractOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("SubtractOperator not implemented on BoolInstance", Location);
    }

    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> MultiplyOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("MultiplyOperator not implemented on BoolInstance", Location);
    }

    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> DivideOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("DivideOperator not implemented on BoolInstance", Location);
    }

    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> ModuloOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("ModuloOperator not implemented on BoolInstance", Location);
    }

    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> BitwiseAndOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("BitwiseAndOperator not implemented on BoolInstance", Location);
    }

    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> BitwiseOrOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("BitwiseOrOperator not implemented on BoolInstance", Location);
    }

    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> BitwiseXorOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("BitwiseXorOperator not implemented on BoolInstance", Location);
    }

    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> LeftShiftOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("LeftShiftOperator not implemented on BoolInstance", Location);
    }

    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> RightShiftOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("RightShiftOperator not implemented on BoolInstance", Location);
    }

    public override IObjectInstance WithValueScope(IValueScope valueScope)
    {
        return new IntInstance(Location, valueScope, Reference);
    }

    public override TaggedUnion<IEnumerable<(IObjectInstance, IAnalysisState)>, AnalysisFailure> EqualsOperator(IObjectInstance right, IAnalysisState state, bool attemptReverseConversion)
    {
        return new AnalysisFailure("EqualsOperator not implemented on BoolInstance", Location);
    }
}