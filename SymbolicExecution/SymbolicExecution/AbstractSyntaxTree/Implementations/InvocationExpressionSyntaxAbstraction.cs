namespace SymbolicExecution.AbstractSyntaxTree.Implementations;

public class InvocationExpressionSyntaxAbstraction : ExpressionSyntaxAbstraction
{
    private readonly SymbolInfo _symbolInfo;

    public InvocationExpressionSyntaxAbstraction(
        ImmutableArray<ISyntaxNodeAbstraction> children,
        ISymbol? symbol,
        Location location,
        ITypeSymbol? actualTypeSymbol,
        ITypeSymbol? convertedTypeSymbol,
        SymbolInfo symbolInfo
        ) : base(children, symbol, location, actualTypeSymbol, convertedTypeSymbol)
    {
        _symbolInfo = symbolInfo;
    }

    public override TaggedUnion<IEnumerable<IAnalysisState>, AnalysisFailure> AnalyzeNode(IAnalysisState previous)
    {
        // Find class and method
        var targetTypeSymbolOrFailure = _symbolInfo.Symbol?.ContainingType is INamedTypeSymbol targetType
                ? new TaggedUnion<INamedTypeSymbol, AnalysisFailure>(targetType)
                : new AnalysisFailure("Cannot find the target type of the invocation expression", Location);
        
        if (!targetTypeSymbolOrFailure.IsT1)
        {
            return targetTypeSymbolOrFailure.T2Value;
        }
        
        var targetTypeSymbol = targetTypeSymbolOrFailure.T1Value;
        
        var classNodeAbstractionOrFailure = FindTypeDeclarationNodeAbstraction(targetTypeSymbol);
        
        if (!classNodeAbstractionOrFailure.IsT1)
        {
            return classNodeAbstractionOrFailure.T2Value;
        }
        
        var classNodeAbstraction = classNodeAbstractionOrFailure.T1Value;
        
        var targetMethodSymbolOrFailure = _symbolInfo.Symbol is IMethodSymbol targetMethod
            ? new TaggedUnion<IMethodSymbol, AnalysisFailure>(targetMethod)
            : new AnalysisFailure("Cannot find the target method of the invocation expression", Location);

        if (!targetMethodSymbolOrFailure.IsT1)
        {
            return targetMethodSymbolOrFailure.T2Value;
        }
        
        var targetMethodSymbol = targetMethodSymbolOrFailure.T1Value;
        
        var methodNodeAbstractionOrFailure = FindMethodDeclarationNodeAbstraction(classNodeAbstraction, targetMethodSymbol);
        
        if (!methodNodeAbstractionOrFailure.IsT1)
        {
            return methodNodeAbstractionOrFailure.T2Value;
        }
        
        var methodNodeAbstraction = methodNodeAbstractionOrFailure.T1Value;
        
        // Analyze arguments
        if (Children.Length != 2)
        {
            return new AnalysisFailure("Invocation expression must have exactly two children", Location);
        }
        
        var argumentListSyntaxAbstraction = Children[1] as ArgumentListSyntaxAbstraction;
        
if (argumentListSyntaxAbstraction == null)
        {
            return new AnalysisFailure("Invocation expression must have an argument list as its second child", Location);
        }

        var priorStatesWithResultRefs = new List<(IAnalysisState, ImmutableArray<IObjectInstance>)> { (previous, ImmutableArray<IObjectInstance>.Empty)}.ToImmutableArray();
        var currentStatesWithResultRefs = new List<(IAnalysisState, List<IObjectInstance>)>();
        foreach (var argumentSyntaxAbstraction in argumentListSyntaxAbstraction.Children)
        {
            if (argumentSyntaxAbstraction is ArgumentSyntaxAbstraction argument)
            {
                if (argument.Children.Length != 1)
                {
                    return new AnalysisFailure("Invocation expression must have an argument as its child", Location);
                }
                
                if (argument.Children[0] is ExpressionSyntaxAbstraction argumentExpression)
                {
                    foreach (var (priorState, priorResultRefs) in priorStatesWithResultRefs)
                    {
                        var argumentResultOrFailure = argumentExpression.GetExpressionResults(priorState);
                        if (!argumentResultOrFailure.IsT1)
                        {
                            return argumentResultOrFailure.T2Value;
                        }

                        foreach (var (argumentResult, argumentState) in argumentResultOrFailure.T1Value)
                        {
                            var currentResultRefs = new List<IObjectInstance>(priorResultRefs);
                            currentResultRefs.Add(argumentResult);
                            currentStatesWithResultRefs.Add((argumentState, currentResultRefs));
                        }
                    }
                    priorStatesWithResultRefs = currentStatesWithResultRefs.Select(x => (x.Item1, x.Item2.ToImmutableArray())).ToImmutableArray();
                }
                else
                {
                    return new AnalysisFailure("Invocation expression must have an argument as its child", Location);
                }
            }
            else
            {
                return new AnalysisFailure("Invocation expression must have an argument as its child", Location);
            }
        }
        
        var results = new List<IAnalysisState>();
        foreach (var (priorState, priorResultRefs) in priorStatesWithResultRefs)
        {
            var methodResultOrFailure = methodNodeAbstraction.AnalyzeMethodCall(priorState, priorResultRefs);
            if (!methodResultOrFailure.IsT1)
            {
                return methodResultOrFailure.T2Value;
            }
            
            results.AddRange(methodResultOrFailure.T1Value);
        }
        
        if (results.Count > 0)
            return results.ToImmutableArray();
        
            
        return new AnalysisFailure("Cannot analyze an invocation expression", Location);
    }

    private TaggedUnion<ITypeDeclarationSyntaxAbstraction, AnalysisFailure> FindTypeDeclarationNodeAbstraction(
        INamedTypeSymbol targetTypeSymbol
        )
    {
        // Go up to the root of the tree, then do a depth-first search for the class declaration
        ISyntaxNodeAbstraction root = this;
        while (root.ParentResolver?.Invoke() is ISyntaxNodeAbstraction parent)
        {
            root = parent;
        }
        
        if (!TryRecursivelyFindDeclaration(root, targetTypeSymbol, out var classNodeAbstractionOrFailure))
        {
            return new AnalysisFailure("Cannot find the type declaration of the target type of the invocation expression", Location);
        }
        
        return classNodeAbstractionOrFailure == null 
            ? new AnalysisFailure("Cannot find the type declaration of the target type of the invocation expression", Location)
            : new TaggedUnion<ITypeDeclarationSyntaxAbstraction, AnalysisFailure>(classNodeAbstractionOrFailure);
    }
    
    private TaggedUnion<IMethodDeclarationSyntaxAbstraction, AnalysisFailure> FindMethodDeclarationNodeAbstraction(
        ITypeDeclarationSyntaxAbstraction typeNodeAbstraction,
        IMethodSymbol targetMethodSymbol
        )
    {
        if (!TryRecursivelyFindDeclaration(typeNodeAbstraction, targetMethodSymbol, out var methodNodeAbstractionOrNull))
        {
            return new AnalysisFailure("Cannot find the method declaration of the target method of the invocation expression", Location);
        }
        
        return methodNodeAbstractionOrNull == null 
            ? new AnalysisFailure("Cannot find the method declaration of the target method of the invocation expression", Location)
            : new TaggedUnion<IMethodDeclarationSyntaxAbstraction, AnalysisFailure>(methodNodeAbstractionOrNull);
    }
    
    private bool TryRecursivelyFindDeclaration(
        ISyntaxNodeAbstraction node,
        INamedTypeSymbol targetTypeSymbol,
        out ITypeDeclarationSyntaxAbstraction? classNodeAbstractionOrNull
        )
    {
        if (node is ITypeDeclarationSyntaxAbstraction classNodeAbstraction)
        {
            if (classNodeAbstraction.Symbol!.Equals(targetTypeSymbol))
            {
                classNodeAbstractionOrNull = classNodeAbstraction;
                return true;
            }
        }

        foreach (var child in node.Children)
        {
            if (TryRecursivelyFindDeclaration(child, targetTypeSymbol, out classNodeAbstractionOrNull))
            {
                return true;
            }
        }

        classNodeAbstractionOrNull = null;
        return false;
    }
    
    private bool TryRecursivelyFindDeclaration(
        ISyntaxNodeAbstraction node,
        IMethodSymbol targetMethodSymbol,
        out IMethodDeclarationSyntaxAbstraction? methodNodeAbstractionOrNull
        )
    {
        if (node is IMethodDeclarationSyntaxAbstraction methodNodeAbstraction)
        {
            if (methodNodeAbstraction.Symbol!.Equals(targetMethodSymbol))
            {
                methodNodeAbstractionOrNull = methodNodeAbstraction;
                return true;
            }
        }

        foreach (var child in node.Children)
        {
            if (TryRecursivelyFindDeclaration(child, targetMethodSymbol, out methodNodeAbstractionOrNull))
            {
                return true;
            }
        }

        methodNodeAbstractionOrNull = null;
        return false;
    }

    public override TaggedUnion<ImmutableArray<(IObjectInstance, IAnalysisState)>, AnalysisFailure> GetExpressionResults(IAnalysisState state)
    {
        return new AnalysisFailure("Cannot get the result of an invocation expression", Location);
    }
}