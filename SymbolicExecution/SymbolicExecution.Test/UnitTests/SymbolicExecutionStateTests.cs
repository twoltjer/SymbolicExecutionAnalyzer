namespace SymbolicExecution.Test.UnitTests;

public class SymbolicExecutionStateTests
{
	public static IEnumerable<object[]> GetInitialStates()
	{
		var emptyState = SymbolicExecutionState.CreateInitialState(Mock.Of<IMethodSymbol>(MockBehavior.Strict));
		yield return new object[]
		{
			emptyState,
			Array.Empty<(ILocalSymbol, IObjectInstance)>(),
			new Optional<(IObjectInstance, Location)>()
		};
		var exception = Mock.Of<IObjectInstance>(MockBehavior.Strict);
		var exceptionLocation = Mock.Of<Location>(MockBehavior.Strict);
		var stateWithException = emptyState.ThrowException(exception, exceptionLocation);
		yield return new object[]
		{
			stateWithException,
			Array.Empty<(ILocalSymbol, IObjectInstance)>(),
			new Optional<(IObjectInstance, Location)>((exception, exceptionLocation))
		};
		var localVariable1 = Mock.Of<ILocalSymbol>(MockBehavior.Strict);
		var stateWithUndefinedLocalVariable = emptyState.AddLocalVariable(localVariable1);
		yield return new object[]
		{
			stateWithUndefinedLocalVariable,
			new[] { (localVariable1, (IObjectInstance)null) },
			new Optional<(IObjectInstance, Location)>()
		};
		var localValue1 = Mock.Of<IObjectInstance>(MockBehavior.Strict);
		var stateWithDefinedLocalVariable = stateWithUndefinedLocalVariable.SetSymbolValue(localVariable1, localValue1, Location.None).T1Value;
		yield return new object[]
		{
			stateWithDefinedLocalVariable,
			new[] { (localVariable1, localValue1) },
			new Optional<(IObjectInstance, Location)>()
		};
		var localVariable2 = Mock.Of<ILocalSymbol>(MockBehavior.Strict);
		var stateWithTwoLocalVariables = stateWithDefinedLocalVariable.AddLocalVariable(localVariable2);
		yield return new object[]
		{
			stateWithTwoLocalVariables,
			new[] { (localVariable1, localValue1), (localVariable2, (IObjectInstance)null) },
			new Optional<(IObjectInstance, Location)>()
		};
		var localValue2 = Mock.Of<IObjectInstance>(MockBehavior.Strict);
		var stateWithTwoDefinedLocalVariables = stateWithTwoLocalVariables.SetSymbolValue(localVariable2, localValue2, Location.None).T1Value;
		yield return new object[]
		{
			stateWithTwoDefinedLocalVariables,
			new[] { (localVariable1, localValue1), (localVariable2, localValue2) },
			new Optional<(IObjectInstance, Location)>()
		};
		var stateWithExceptionAndTwoDefinedLocalVariables = stateWithTwoDefinedLocalVariables.ThrowException(exception, exceptionLocation);
		yield return new object[]
		{
			stateWithExceptionAndTwoDefinedLocalVariables,
			new[] { (localVariable1, localValue1), (localVariable2, localValue2) },
			new Optional<(IObjectInstance, Location)>((exception, exceptionLocation))
		};
	}

	[Theory]
	[Trait("Category", "Unit")]
	[MemberData(nameof(GetInitialStates))]
	public void TestSymbolicExecutionState(
		SymbolicExecutionState state,
		(ILocalSymbol symbol, IObjectInstance location)[] symbolsAndValues,
		Optional<(IObjectInstance value, Location location)> exceptionAndLocation
		)
	{
		if (exceptionAndLocation.HasValue)
		{
			state.CurrentException.Should().NotBeNull();
			state.CurrentException!.Exception.Should().BeSameAs(exceptionAndLocation.Value.value);
			state.CurrentException!.Location.Should().BeSameAs(exceptionAndLocation.Value.location);
		}
		else
		{
			state.CurrentException.Should().BeNull();
		}
		foreach (var (symbol, value) in symbolsAndValues)
		{
			var location = Mock.Of<Location>(MockBehavior.Strict);
			var getSymbolResult = state.GetSymbolValueOrFailure(symbol, location);
			if (value == null)
			{
				getSymbolResult.IsT1.Should().BeFalse();
				getSymbolResult.T2Value.Should().NotBeNull();
				getSymbolResult.T2Value!.Location.Should().BeSameAs(location);
				getSymbolResult.T2Value!.Reason.Should().Be("Cannot get the value of a local variable that has not been initialized");
			}
			else
			{
				getSymbolResult.IsT1.Should().BeTrue();
				getSymbolResult.T1Value.Should().BeSameAs(value);
			}
		}

		var failLocation = Mock.Of<Location>(MockBehavior.Strict);
		var failure = state.GetSymbolValueOrFailure(Mock.Of<ILocalSymbol>(), failLocation).T2Value;
		failure.Should().NotBeNull();
		failure.Location.Should().BeSameAs(failLocation);
		failure.Reason.Should().Be("Cannot find symbol");

		failure = state.GetSymbolValueOrFailure(Mock.Of<ISymbol>(MockBehavior.Strict), failLocation).T2Value;
		failure.Should().NotBeNull();
		failure.Location.Should().BeSameAs(failLocation);
		failure.Reason.Should().Be("Cannot get the value of a non-local symbol");

		// Try to set the value of a non-local symbol
		var nonLocalSymbol = Mock.Of<ISymbol>(MockBehavior.Strict);
		var nonLocalSymbolValue = Mock.Of<IObjectInstance>(MockBehavior.Strict);
		failure = state.SetSymbolValue(nonLocalSymbol, nonLocalSymbolValue, Location.None).T2Value;
		failure.Should().NotBeNull();
		failure.Location.Should().BeSameAs(Location.None);
		failure.Reason.Should().Be("Cannot set the value of a non-local symbol");

		// Try to set the value of an undeclared local variable
		var undeclaredLocalVariable = Mock.Of<ILocalSymbol>(MockBehavior.Strict);
		var undeclaredLocalVariableValue = Mock.Of<IObjectInstance>(MockBehavior.Strict);
		failure = state.SetSymbolValue(undeclaredLocalVariable, undeclaredLocalVariableValue, Location.None).T2Value;
		failure.Should().NotBeNull();
		failure.Location.Should().BeSameAs(Location.None);
		failure.Reason.Should().Be("Symbol missing from list of local variables");
	}
}