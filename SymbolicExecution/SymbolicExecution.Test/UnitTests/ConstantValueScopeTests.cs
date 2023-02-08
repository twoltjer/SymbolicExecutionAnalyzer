namespace SymbolicExecution.Test.UnitTests;

public class ConstantValueScopeTests
{
	[Fact]
	[Trait("Category", "Unit")]
	public void TestCanBe_WithNullValueAndNullInput_ReturnsTrue()
	{
		var mockTypeSymbol = new Mock<ITypeSymbol>(MockBehavior.Strict);
		var constantValueScope = new ConstantValueScope(null, mockTypeSymbol.Object);

		var actual = constantValueScope.CanBe(null);

		actual.Should().BeTrue();
	}

	[Theory]
	[Trait("Category", "Unit")]
	[InlineData(1)]
	[InlineData("test")]
	public void TestCanBe_WithNullValueAndNonNullInput_ReturnsFalse(object input)
	{
		var mockTypeSymbol = new Mock<ITypeSymbol>(MockBehavior.Strict);
		var constantValueScope = new ConstantValueScope(null, mockTypeSymbol.Object);

		var actual = constantValueScope.CanBe(input);

		actual.Should().BeFalse();
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestCanBe_WithTrueValueAndNullInput_ReturnsFalse()
	{
		var mockTypeSymbol = new Mock<ITypeSymbol>(MockBehavior.Strict);
		var constantValueScope = new ConstantValueScope(true, mockTypeSymbol.Object);

		var actual = constantValueScope.CanBe(null);

		actual.Should().BeFalse();
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestCanBe_WithTrueValueAndTrueInput_ReturnsTrue()
	{
		var mockTypeSymbol = new Mock<ITypeSymbol>(MockBehavior.Strict);
		var constantValueScope = new ConstantValueScope(true, mockTypeSymbol.Object);

		var actual = constantValueScope.CanBe(true);

		actual.Should().BeTrue();
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestCanBe_WithTrueValueAndFalseInput_ReturnsFalse()
	{
		var mockTypeSymbol = new Mock<ITypeSymbol>(MockBehavior.Strict);
		var constantValueScope = new ConstantValueScope(true, mockTypeSymbol.Object);

		var actual = constantValueScope.CanBe(false);

		actual.Should().BeFalse();
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestCanBe_WithZeroValueAndNullInput_ReturnsFalse()
	{
		var mockTypeSymbol = new Mock<ITypeSymbol>(MockBehavior.Strict);
		var constantValueScope = new ConstantValueScope(0, mockTypeSymbol.Object);

		var actual = constantValueScope.CanBe(null);

		actual.Should().BeFalse();
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestCanBe_WithZeroValueAndZeroInput_ReturnsTrue()
	{
		var mockTypeSymbol = new Mock<ITypeSymbol>(MockBehavior.Strict);
		var constantValueScope = new ConstantValueScope(0, mockTypeSymbol.Object);

		var actual = constantValueScope.CanBe(0);

		actual.Should().BeTrue();
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestCanBe_WithZeroValueAndOneInput_ReturnsFalse()
	{
		var mockTypeSymbol = new Mock<ITypeSymbol>(MockBehavior.Strict);
		var constantValueScope = new ConstantValueScope(0, mockTypeSymbol.Object);

		var actual = constantValueScope.CanBe(1);

		actual.Should().BeFalse();
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestCanBe_WithAStringAndNullInput_ReturnsFalse()
	{
		var mockTypeSymbol = new Mock<ITypeSymbol>(MockBehavior.Strict);
		var constantValueScope = new ConstantValueScope("A", mockTypeSymbol.Object);

		var actual = constantValueScope.CanBe(null);

		actual.Should().BeFalse();
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestCanBe_WithAStringAndAStringInput_ReturnsTrue()
	{
		var mockTypeSymbol = new Mock<ITypeSymbol>(MockBehavior.Strict);
		var constantValueScope = new ConstantValueScope("A", mockTypeSymbol.Object);

		var actual = constantValueScope.CanBe("A");

		actual.Should().BeTrue();
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestCanBe_WithAStringAndAnotherStringInput_ReturnsFalse()
	{
		var mockTypeSymbol = new Mock<ITypeSymbol>(MockBehavior.Strict);
		var constantValueScope = new ConstantValueScope("A", mockTypeSymbol.Object);

		var actual = constantValueScope.CanBe("B");

		actual.Should().BeFalse();
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestCanBe_With1StringAndAnIntInput_ReturnsFalse()
	{
		var mockTypeSymbol = new Mock<ITypeSymbol>(MockBehavior.Strict);
		var constantValueScope = new ConstantValueScope("1", mockTypeSymbol.Object);

		var actual = constantValueScope.CanBe(1);

		actual.Should().BeFalse();
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestCanBe_WithEmptyStringAndNullInput_ReturnsFalse()
	{
		var mockTypeSymbol = new Mock<ITypeSymbol>(MockBehavior.Strict);
		var constantValueScope = new ConstantValueScope(string.Empty, mockTypeSymbol.Object);

		var actual = constantValueScope.CanBe(null);

		actual.Should().BeFalse();
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestIsExactType_WithTrueValueAndBooleanTypeSymbol_ReturnsTrue()
	{
		var mockTypeSymbol = new Mock<ITypeSymbol>(MockBehavior.Strict);
		var namespaceSymbol = new Mock<INamespaceSymbol>(MockBehavior.Strict);
		namespaceSymbol.Setup(symbol => symbol.ToString()).Returns("System");
		mockTypeSymbol.Setup(symbol => symbol.Name).Returns("Boolean");
		mockTypeSymbol.Setup(symbol => symbol.ContainingNamespace).Returns(namespaceSymbol.Object);
		var constantValueScope = new ConstantValueScope(true, mockTypeSymbol.Object);

		var actual = constantValueScope.IsExactType(typeof(bool));

		actual.Should().BeTrue();
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestIsExactType_WithTrueValueAndStringTypeSymbol_ReturnsFalse()
	{
		var mockTypeSymbol = new Mock<ITypeSymbol>(MockBehavior.Strict);
		var namespaceSymbol = new Mock<INamespaceSymbol>(MockBehavior.Strict);
		mockTypeSymbol.Setup(symbol => symbol.Name).Returns("String");
		mockTypeSymbol.Setup(symbol => symbol.ContainingNamespace).Returns(namespaceSymbol.Object);
		var constantValueScope = new ConstantValueScope(true, mockTypeSymbol.Object);

		var actual = constantValueScope.IsExactType(typeof(string));

		actual.Should().BeFalse();
	}
}