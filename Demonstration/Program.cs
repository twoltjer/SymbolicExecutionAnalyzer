class TestClass
{
    public static void Main()
    {
        var testClass = new TestClass();
        testClass.PrintRowTen();
        testClass.PrintRowFifty();
    }
    
    void PrintRowTen()
    {
        PrintRow(10);
    }

    void PrintRowFifty()
    {
        PrintRow(50);
    }

    void PrintRow(int n)
    {
        Console.WriteLine(string.Join(' ', GetRow(n)));
    }

    int[] GetRow(int n)
    {
        var row = new int[n + 1];
        row[0] = 1;
        row[n] = 1;

        if (n <= 1)
            return row;
        
        var previousRow = GetRow(n - 1);

        for (int i = 1; i < n; i++)
        {
            row[i] = previousRow[i - 1] + previousRow[i];
        }
        return row;
    }
}