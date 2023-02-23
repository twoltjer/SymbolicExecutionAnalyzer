namespace SymbolicExecution.Test.UnitTests;

public class ReferenceTypeScopeTests
{
	[Theory]
	[Trait("Category", "Unit")]
	[InlineData(null)]
	[InlineData(0)]
	[InlineData(2)]
	[InlineData("string")]
	[InlineData(false)]
	[InlineData(true)]
	public void TestCanBe_Always_ReturnsFalse(object input)
	{
		var typeSymbol = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
		var subject = new ReferenceTypeScope(typeSymbol);
		subject.CouldBe(input).Should().BeFalse();
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestIsExactType_WithMatchingType_ReturnsTrue()
	{
		var typeSymbol = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
		Mock.Get(typeSymbol)
			.Setup(x => x.Name)
			.Returns("String");
		Mock.Get(typeSymbol)
			.Setup(x => x.ContainingNamespace.ToString())
			.Returns("System");
		var subject = new ReferenceTypeScope(typeSymbol);
		subject.IsExactType(typeof(string)).Should().BeTrue();
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestIsExactType_WithNonMatchingType_ReturnsFalse()
	{
		var typeSymbol = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
		Mock.Get(typeSymbol)
			.Setup(x => x.Name)
			.Returns("String");
		Mock.Get(typeSymbol)
			.Setup(x => x.ContainingNamespace.ToString())
			.Returns("System");
		var subject = new ReferenceTypeScope(typeSymbol);
		subject.IsExactType(typeof(int)).Should().BeFalse();
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestIsExactType_WithNonMatchingNamespace_ReturnsFalse()
	{
		var typeSymbol = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
		Mock.Get(typeSymbol)
			.Setup(x => x.Name)
			.Returns("String");
		Mock.Get(typeSymbol)
			.Setup(x => x.ContainingNamespace.ToString())
			.Returns("System.Collections");
		var subject = new ReferenceTypeScope(typeSymbol);
		subject.IsExactType(typeof(string)).Should().BeFalse();
	}

	[Fact]
	[Trait("Category", "Unit")]
	public void TestIsExactType_WithNonMatchingName_ReturnsFalse()
	{
		var typeSymbol = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
		Mock.Get(typeSymbol)
			.Setup(x => x.Name)
			.Returns("String");
		Mock.Get(typeSymbol)
			.Setup(x => x.ContainingNamespace.ToString())
			.Returns("System");
		var subject = new ReferenceTypeScope(typeSymbol);
		subject.IsExactType(typeof(int)).Should().BeFalse();
	}

	[Theory]
	[Trait("Category", "Unit")]
	[InlineData(null)]
	[InlineData(0)]
	[InlineData(2)]
	[InlineData("string")]
	[InlineData(false)]
	[InlineData(true)]
	public void TestIsAlways_Always_ReturnsFalse(object input)
	{
		var typeSymbol = Mock.Of<ITypeSymbol>(MockBehavior.Strict);
		var subject = new ReferenceTypeScope(typeSymbol);
		subject.IsAlways(input).Should().BeFalse();
	}
}