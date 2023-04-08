namespace SymbolicExecution.Test.UnitTests;

public class ExceptionThrownStateTests
{
	[Fact]
	[Trait("Category", "Unit")]
	public void TestEquals_ReturnsTrue_ForSameObject()
	{
		var exception = Mock.Of<IObjectInstance>(MockBehavior.Strict);
		var location = Location.Create(
			"TestFile.cs",
			new TextSpan(0, 10),
			new LinePositionSpan(new LinePosition(10, 10), new LinePosition(10, 12))
			);
		var symbol = Mock.Of<IMethodSymbol>(MockBehavior.Strict);
		var subject = new ExceptionThrownState(exception, location, symbol);
		subject.Equals(subject).Should().BeTrue();
		subject.Equals(subject as object).Should().BeTrue();
		subject.GetHashCode().Should().Be(subject.GetHashCode());
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestEquals_SameLocationAndExceptionObject_ReturnsTrue()
	{
		var exception = Mock.Of<IObjectInstance>(MockBehavior.Strict);
		var location = Location.Create(
			"TestFile.cs",
			new TextSpan(0, 10),
			new LinePositionSpan(new LinePosition(10, 10), new LinePosition(10, 12))
			);
		var symbol = Mock.Of<IMethodSymbol>(MockBehavior.Strict);
		var subject = new ExceptionThrownState(exception, location, symbol);
		var other = new ExceptionThrownState(exception, location, symbol);
		subject.Equals(other).Should().BeTrue();
		subject.Equals(other as object).Should().BeTrue();
		subject.GetHashCode().Should().Be(other.GetHashCode());
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestEquals_DifferentLocation_ReturnsFalse()
	{
		var exception = Mock.Of<IObjectInstance>(MockBehavior.Strict);
		var location = Location.Create(
			"TestFile.cs",
			new TextSpan(0, 10),
			new LinePositionSpan(new LinePosition(10, 10), new LinePosition(10, 12))
			);
		var symbol = Mock.Of<IMethodSymbol>(MockBehavior.Strict);
		var subject = new ExceptionThrownState(exception, location, symbol);
		var differentLocation = Location.Create(
			"TestFile.cs",
			new TextSpan(0, 10),
			new LinePositionSpan(new LinePosition(10, 10), new LinePosition(10, 13))
			);
		var other = new ExceptionThrownState(exception, differentLocation, symbol);
		subject.Equals(other).Should().BeFalse();
		subject.Equals(other as object).Should().BeFalse();
		subject.GetHashCode().Should().NotBe(other.GetHashCode());
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestEquals_DifferentExceptionObject_ReturnsFalse()
	{
		var exception = Mock.Of<IObjectInstance>(MockBehavior.Strict);
		var differentException = Mock.Of<IObjectInstance>(MockBehavior.Strict);
		var symbol = Mock.Of<IMethodSymbol>(MockBehavior.Strict);
		var location = Location.Create(
			"TestFile.cs",
			new TextSpan(0, 10),
			new LinePositionSpan(new LinePosition(10, 10), new LinePosition(10, 12))
			);
		var subject = new ExceptionThrownState(exception, location, symbol);
		var other = new ExceptionThrownState(differentException, location, symbol);
		subject.Equals(other).Should().BeFalse();
		subject.Equals(other as object).Should().BeFalse();
		subject.GetHashCode().Should().NotBe(other.GetHashCode());
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestEquals_WithDifferentObjectType_ReturnsFalse()
	{
		var exception = Mock.Of<IObjectInstance>(MockBehavior.Strict);
		var symbol = Mock.Of<IMethodSymbol>(MockBehavior.Strict);
		var location = Location.Create(
			"TestFile.cs",
			new TextSpan(0, 10),
			new LinePositionSpan(new LinePosition(10, 10), new LinePosition(10, 12))
			);
		var subject = new ExceptionThrownState(exception, location, symbol);
		subject.Equals(new object()).Should().BeFalse();
	}
}