namespace SymbolicExecution.Test.UnitTests;

public class ConstantValueScopeTests
{
	// [Fact]
	// [Trait("Category", "Unit")]
	// public void TestIsExactType_WithTrueValueAndBooleanTypeSymbol_ReturnsTrue()
	// {
	// 	var mockTypeSymbol = new Mock<ITypeSymbol>(MockBehavior.Strict);
	// 	var namespaceSymbol = new Mock<INamespaceSymbol>(MockBehavior.Strict);
	// 	namespaceSymbol.Setup(symbol => symbol.ToString()).Returns("System");
	// 	mockTypeSymbol.Setup(symbol => symbol.Name).Returns("Boolean");
	// 	mockTypeSymbol.Setup(symbol => symbol.ContainingNamespace).Returns(namespaceSymbol.Object);
	// 	var constantValueScope = new ValueScope(true, new TaggedUnion<ITypeSymbol, Type>(mockTypeSymbol.Object));
	//
	// 	var actual = constantValueScope.IsExactType(typeof(bool));
	//
	// 	actual.Should().BeTrue();
	// }
	//
	// [Fact]
	// [Trait("Category", "Unit")]
	// public void TestIsExactType_WithTrueValueAndStringTypeSymbol_ReturnsFalse()
	// {
	// 	var mockTypeSymbol = new Mock<ITypeSymbol>(MockBehavior.Strict);
	// 	var namespaceSymbol = new Mock<INamespaceSymbol>(MockBehavior.Strict);
	// 	mockTypeSymbol.Setup(symbol => symbol.Name).Returns("String");
	// 	mockTypeSymbol.Setup(symbol => symbol.ContainingNamespace).Returns(namespaceSymbol.Object);
	// 	var constantValueScope = new ValueScope(true, new TaggedUnion<ITypeSymbol, Type>(mockTypeSymbol.Object));
	//
	// 	var actual = constantValueScope.IsExactType(typeof(string));
	//
	// 	actual.Should().BeFalse();
	// }
}