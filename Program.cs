using System.Diagnostics;

const string Format = "{0,7:0.000} ";
const int TotalPasses = 25000;
const int Size = 50;
Stopwatch timer = new();

var functionList = new List<Action> { Jagged, Single, SingleStandard, Multi };

Console.WriteLine("{0,5}{1,20}{2,20}{3,20}{4,20}", "Run", "Ticks", "ms", "Ticks/Instance", "ms/Instance");

foreach (var item in functionList)
{
    var warmup = Test(item);
    var run = Test(item);

    Console.WriteLine($"{item.Method.Name}:");
    PrintResult("warmup", warmup);
    PrintResult("run", run);
    Console.WriteLine();
}

static void PrintResult(string name, long ticks)
{
    Console.WriteLine("{0,10}{1,20}{2,20}{3,20}{4,20}", name, ticks, string.Format(Format, (decimal)ticks / TimeSpan.TicksPerMillisecond), (decimal)ticks / TotalPasses, (decimal)ticks / TotalPasses / TimeSpan.TicksPerMillisecond);
}

long Test(Action func)
{
    timer.Restart();
    func();
    timer.Stop();
    return timer.ElapsedTicks;
}

static void Jagged()
{
    for (var passes = 0; passes < TotalPasses; passes++)
    {
        var jagged = new int[Size][][];
        for (var i = 0; i < Size; i++)
        {
            jagged[i] = new int[Size][];
            for (var j = 0; j < Size; j++)
            {
                jagged[i][j] = new int[Size];
                for (var k = 0; k < Size; k++)
                {
                    jagged[i][j][k] = i * j * k;
                }
            }
        }
    }
}

static void Multi()
{
    for (var passes = 0; passes < TotalPasses; passes++)
    {
        var multi = new int[Size, Size, Size];
        for (var i = 0; i < Size; i++)
        {
            for (var j = 0; j < Size; j++)
            {
                for (var k = 0; k < Size; k++)
                {
                    multi[i, j, k] = i * j * k;
                }
            }
        }
    }
}

static void Single()
{
    for (var passes = 0; passes < TotalPasses; passes++)
    {
        var single = new int[Size * Size * Size];
        for (var i = 0; i < Size; i++)
        {
            int iOffset = i * Size * Size;
            for (var j = 0; j < Size; j++)
            {
                var jOffset = iOffset + j * Size;
                for (var k = 0; k < Size; k++)
                {
                    single[jOffset + k] = i * j * k;
                }
            }
        }
    }
}

static void SingleStandard()
{
    for (var passes = 0; passes < TotalPasses; passes++)
    {
        var single = new int[Size * Size * Size];
        for (var i = 0; i < Size; i++)
        {
            for (var j = 0; j < Size; j++)
            {
                for (var k = 0; k < Size; k++)
                {
                    single[i * Size * Size + j * Size + k] = i * j * k;
                }
            }
        }
    }
}
