namespace CSharpInterviewPrep.Topics;

public static class ModernCSharpDemo
{
    public static void Run()
    {
        Console.WriteLine("=== MODERN C# FEATURES ===\n");

        // ── RECORDS (C# 9) ────────────────────────────────────────────────────
        Console.WriteLine("--- Records (C# 9) ---");
        var p1 = new Point(1, 2);
        var p2 = new Point(1, 2);
        var p3 = new Point(5, 10);
        var p4 = p1 with { Y = 99 };   // non-destructive mutation (new instance)

        Console.WriteLine($"  p1         = {p1}");
        Console.WriteLine($"  p2         = {p2}");
        Console.WriteLine($"  p1 == p2   = {p1 == p2}   (value equality — records compare by value)");
        Console.WriteLine($"  p4 (with)  = {p4}         (copy of p1 with Y changed)");

        // Record class vs record struct (C# 10)
        var r1 = new TemperatureReading(36.6, "Celsius");
        Console.WriteLine($"  Record:    {r1}");

        // ── RECORD vs CLASS vs STRUCT ──────────────────────────────────────────
        Console.WriteLine("\n--- Record vs Class vs Struct ---");
        Console.WriteLine("  Record : reference type, value equality, immutable by default, with-expr");
        Console.WriteLine("  Class  : reference type, reference equality, mutable");
        Console.WriteLine("  Struct : value type,     value equality,     copied on assign, no inherit");

        // ── PATTERN MATCHING ──────────────────────────────────────────────────
        Console.WriteLine("\n--- Pattern Matching (C# 7-11) ---");
        object[] values = { 42, -7, "hello", 3.14, true, null!, new Point(3, 4) };
        foreach (var val in values)
            Console.WriteLine($"  {Describe(val)}");

        // ── SWITCH EXPRESSION (C# 8) ──────────────────────────────────────────
        Console.WriteLine("\n--- Switch Expression (C# 8) ---");
        foreach (var day in Enum.GetValues<DayOfWeek>())
            Console.WriteLine($"  {day,-12} → {ClassifyDay(day)}");

        // ── TUPLES & DECONSTRUCTION ───────────────────────────────────────────
        Console.WriteLine("\n--- Tuples & Deconstruction ---");
        var (min, max, avg) = GetStats(new[] { 3, 1, 4, 1, 5, 9, 2, 6 });
        Console.WriteLine($"  Stats: min={min}  max={max}  avg={avg:F2}");

        // Named tuple
        (string First, string Last) fullName = ("John", "Doe");
        Console.WriteLine($"  Named tuple: {fullName.First} {fullName.Last}");

        // Deconstruct a Point record
        var pt = new Point(7, 8);
        var (x, y) = pt;
        Console.WriteLine($"  Point deconstruct: x={x}, y={y}");

        // Swap without temp variable using tuples
        int a = 10, b = 20;
        (a, b) = (b, a);
        Console.WriteLine($"  Swapped: a={a}, b={b}");

        // ── NULLABLE REFERENCE TYPES (C# 8) ───────────────────────────────────
        Console.WriteLine("\n--- Nullable Reference Types (C# 8) ---");
        string  nonNull = "guaranteed";
        string? nullable = null;
        Console.WriteLine($"  nonNull  = {nonNull}");
        Console.WriteLine($"  nullable = {nullable ?? "(null)"}");
        int safeLen = nullable?.Length ?? 0;
        Console.WriteLine($"  nullable?.Length ?? 0 = {safeLen}");

        // ── INIT-ONLY PROPERTIES (C# 9) ───────────────────────────────────────
        Console.WriteLine("\n--- Init-Only Properties (C# 9) ---");
        var emp = new StaffMember { Name = "Alice", Department = "Engineering" };
        Console.WriteLine($"  {emp.Name} — {emp.Department}");
        // emp.Name = "Bob"; // compile error — init only

        // ── REQUIRED MEMBERS (C# 11) ──────────────────────────────────────────
        Console.WriteLine("\n--- Required Members (C# 11) ---");
        var config = new AppConfiguration { Host = "localhost", Port = 5432 };
        Console.WriteLine($"  Config: {config.Host}:{config.Port}");
        // new AppConfiguration() without Host/Port = compile error

        // ── INDEX & RANGE OPERATORS (C# 8) ────────────────────────────────────
        Console.WriteLine("\n--- Index & Range Operators (C# 8) ---");
        var arr = new[] { "a", "b", "c", "d", "e", "f" };
        Console.WriteLine($"  arr[^1]    = {arr[^1]}             (last element)");
        Console.WriteLine($"  arr[^2]    = {arr[^2]}             (second to last)");
        Console.WriteLine($"  arr[1..4]  = {string.Join(",", arr[1..4])}        (index 1,2,3)");
        Console.WriteLine($"  arr[..3]   = {string.Join(",", arr[..3])}         (first 3)");
        Console.WriteLine($"  arr[3..]   = {string.Join(",", arr[3..])}       (from index 3)");
        Console.WriteLine($"  arr[^2..]  = {string.Join(",", arr[^2..])}       (last 2)");

        // ── SPAN<T> (C# 7.2) ──────────────────────────────────────────────────
        Console.WriteLine("\n--- Span<T> (zero-copy slice, stack-allocated) ---");
        int[] data  = { 10, 20, 30, 40, 50, 60, 70, 80 };
        Span<int> slice = data.AsSpan(2, 4);  // no heap allocation, no copy
        Console.WriteLine($"  Span slice [2..6]: {string.Join(", ", slice.ToArray())}");
        slice[0] = 999;  // modifies original array!
        Console.WriteLine($"  After modify data[2]: {data[2]}  (same memory)");

        // ── LOCAL FUNCTIONS (C# 7) ─────────────────────────────────────────────
        Console.WriteLine("\n--- Local Functions (C# 7) ---");
        Console.WriteLine($"  Fibonacci(10) = {Fibonacci(10)}");

        // ── PRIMARY CONSTRUCTORS (C# 12) ──────────────────────────────────────
        Console.WriteLine("\n--- Primary Constructors (C# 12) ---");
        var svc = new OrderService("https://api.orders.com", 30);
        svc.PrintConfig();

        // ── NULL-COALESCING ASSIGNMENT (C# 8) ──────────────────────────────────
        Console.WriteLine("\n--- Null-Coalescing Assignment (??=) ---");
        string? cached = null;
        cached ??= "computed-value"; // assign only if null
        Console.WriteLine($"  First  ??= : {cached}");
        cached ??= "second-value";   // NOT assigned (already has value)
        Console.WriteLine($"  Second ??= : {cached}");
    }

    // ─── PATTERN MATCHING ──────────────────────────────────────────────────────
    static string Describe(object? obj) => obj switch
    {
        null                    => "null",
        int n when n > 0        => $"positive int: {n}",
        int n when n < 0        => $"negative int: {n}",
        int n                   => $"zero",
        string s when s.Length > 3 => $"long string: '{s}'",
        string s                => $"short string: '{s}'",
        double d                => $"double: {d}",
        bool b                  => $"bool: {b}",
        Point(var px, var py)   => $"Point({px},{py})",
        _                       => $"other: {obj.GetType().Name}"
    };

    static string ClassifyDay(DayOfWeek day) => day switch
    {
        DayOfWeek.Saturday or DayOfWeek.Sunday => "Weekend  ☀",
        DayOfWeek.Monday                        => "Start of week",
        DayOfWeek.Friday                        => "End of week (almost!)",
        _                                       => "Weekday"
    };

    static (int Min, int Max, double Avg) GetStats(int[] nums)
        => (nums.Min(), nums.Max(), nums.Average());

    // LOCAL FUNCTION
    static long Fibonacci(int n)
    {
        if (n <= 1) return n;
        return Fib(n);

        long Fib(int k) => k <= 1 ? k : Fib(k - 1) + Fib(k - 2); // local function
    }
}

// ─── RECORD ───────────────────────────────────────────────────────────────────
public record Point(int X, int Y); // positional record — auto-generates Deconstruct

public record TemperatureReading(double Value, string Unit)
{
    public override string ToString() => $"{Value}° {Unit}";
}

// ─── INIT-ONLY PROPERTIES ─────────────────────────────────────────────────────
public class StaffMember
{
    public string Name       { get; init; } = string.Empty;
    public string Department { get; init; } = string.Empty;
}

// ─── REQUIRED MEMBERS (C# 11) ─────────────────────────────────────────────────
public class AppConfiguration
{
    public required string Host { get; init; }
    public required int    Port { get; init; }
    public int Timeout { get; init; } = 30;
}

// ─── PRIMARY CONSTRUCTOR (C# 12) ──────────────────────────────────────────────
public class OrderService(string baseUrl, int timeoutSeconds)
{
    public void PrintConfig()
        => Console.WriteLine($"  OrderService: {baseUrl}  timeout={timeoutSeconds}s");
}
