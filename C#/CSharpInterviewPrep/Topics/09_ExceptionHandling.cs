namespace CSharpInterviewPrep.Topics;

public static class ExceptionHandlingDemo
{
    public static void Run()
    {
        Console.WriteLine("=== EXCEPTION HANDLING ===\n");

        // ── TRY / CATCH / FINALLY ─────────────────────────────────────────────
        Console.WriteLine("--- try / catch / finally ---");
        try
        {
            int[] arr = { 1, 2, 3 };
            Console.WriteLine(arr[10]);       // IndexOutOfRangeException
        }
        catch (IndexOutOfRangeException ex)
        {
            Console.WriteLine($"  Caught: {ex.GetType().Name} — {ex.Message}");
        }
        finally
        {
            Console.WriteLine("  Finally: always runs (cleanup, close connections)");
        }

        // ── MULTIPLE CATCH ────────────────────────────────────────────────────
        Console.WriteLine("\n--- Multiple catch blocks (most specific first) ---");
        SafeParse("123");
        SafeParse("abc");
        SafeParse(null!);

        // ── CUSTOM EXCEPTION ──────────────────────────────────────────────────
        Console.WriteLine("\n--- Custom Exception ---");
        try { ValidateAge(-5); }
        catch (InvalidAgeException ex)
        {
            Console.WriteLine($"  Custom: {ex.Message}  Age={ex.Age}");
        }

        try { ValidateAge(200); }
        catch (InvalidAgeException ex)
        {
            Console.WriteLine($"  Custom: {ex.Message}  Age={ex.Age}");
        }

        // ── EXCEPTION FILTER (when) ────────────────────────────────────────────
        Console.WriteLine("\n--- Exception Filter (when clause) ---");
        foreach (int code in new[] { 404, 503, 401 })
        {
            try
            {
                ThrowHttp(code);
            }
            catch (AppHttpException ex) when (ex.StatusCode == 404)
            {
                Console.WriteLine($"  Not Found (404): {ex.Message}");
            }
            catch (AppHttpException ex) when (ex.StatusCode >= 500)
            {
                Console.WriteLine($"  Server Error ({ex.StatusCode}): {ex.Message}");
            }
            catch (AppHttpException ex)
            {
                Console.WriteLine($"  HTTP {ex.StatusCode}: {ex.Message}");
            }
        }

        // ── INNER EXCEPTION & RETHROW ──────────────────────────────────────────
        Console.WriteLine("\n--- Inner Exception & Re-throw ---");
        try { LoadConfiguration(); }
        catch (ConfigurationException ex)
        {
            Console.WriteLine($"  Outer: {ex.Message}");
            Console.WriteLine($"  Inner: {ex.InnerException?.Message}");
        }

        // throw vs throw ex
        Console.WriteLine("\n--- throw vs throw ex ---");
        Console.WriteLine("  throw    : rethrows, PRESERVES original stack trace (preferred)");
        Console.WriteLine("  throw ex : rethrows, RESETS stack trace to this line (loses info)");

        // ── USING / IDISPOSABLE ────────────────────────────────────────────────
        Console.WriteLine("\n--- using statement (auto-dispose) ---");
        try
        {
            using var conn = new FakeDbConnection("Server=mydb");
            conn.Open();
            conn.ExecuteQuery("SELECT * FROM Users");
            throw new InvalidOperationException("Query failed mid-way");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"  Caught: {ex.Message}");
        }
        // Dispose() was still called on FakeDbConnection even though exception was thrown

        // ── WHEN TO USE EXCEPTIONS ─────────────────────────────────────────────
        Console.WriteLine("\n--- When to use exceptions ---");
        Console.WriteLine("  ✓ Unexpected / unrecoverable errors (file not found, DB down)");
        Console.WriteLine("  ✓ Programmer errors (null arg, out-of-range)");
        Console.WriteLine("  ✗ Control flow (don't use for expected conditions like 'user not found')");
        Console.WriteLine("  ✗ Performance-critical paths (exceptions are expensive)");
        Console.WriteLine();
        Console.WriteLine("  Prefer TryParse over Parse, FirstOrDefault over First,");
        Console.WriteLine("  TryGetValue over direct dictionary access.");

        // ── EXCEPTION HIERARCHY ────────────────────────────────────────────────
        Console.WriteLine("\n--- Exception Hierarchy ---");
        Console.WriteLine("  object");
        Console.WriteLine("  └─ Exception");
        Console.WriteLine("     ├─ SystemException");
        Console.WriteLine("     │  ├─ ArgumentException");
        Console.WriteLine("     │  │  ├─ ArgumentNullException");
        Console.WriteLine("     │  │  └─ ArgumentOutOfRangeException");
        Console.WriteLine("     │  ├─ InvalidOperationException");
        Console.WriteLine("     │  ├─ NullReferenceException");
        Console.WriteLine("     │  ├─ IndexOutOfRangeException");
        Console.WriteLine("     │  ├─ OverflowException");
        Console.WriteLine("     │  ├─ NotImplementedException");
        Console.WriteLine("     │  ├─ NotSupportedException");
        Console.WriteLine("     │  └─ IOException");
        Console.WriteLine("     │     ├─ FileNotFoundException");
        Console.WriteLine("     │     └─ DirectoryNotFoundException");
        Console.WriteLine("     └─ ApplicationException  (for app-specific exceptions)");
    }

    static void SafeParse(string input)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(input, nameof(input));
            int value = int.Parse(input);
            Console.WriteLine($"  Parsed: {value}");
        }
        catch (ArgumentNullException)
        {
            Console.WriteLine("  Input was null.");
        }
        catch (FormatException)
        {
            Console.WriteLine($"  '{input}' is not a valid number.");
        }
        catch (OverflowException)
        {
            Console.WriteLine($"  '{input}' is out of int range.");
        }
    }

    static void ValidateAge(int age)
    {
        if (age < 0 || age > 150)
            throw new InvalidAgeException(age, $"Age {age} is out of valid range (0-150).");
    }

    static void ThrowHttp(int code)
        => throw new AppHttpException(code, $"HTTP error occurred.");

    static void LoadConfiguration()
    {
        try
        {
            throw new FileNotFoundException("appsettings.json not found.");
        }
        catch (FileNotFoundException ex)
        {
            // Wrap with context, preserve original as InnerException
            throw new ConfigurationException("Failed to load app configuration.", ex);
        }
    }
}

// ─── CUSTOM EXCEPTIONS ────────────────────────────────────────────────────────
public class InvalidAgeException : Exception
{
    public int Age { get; }

    public InvalidAgeException(int age, string message) : base(message)
        => Age = age;

    public InvalidAgeException(int age, string message, Exception inner)
        : base(message, inner) => Age = age;
}

public class AppHttpException : Exception
{
    public int StatusCode { get; }
    public AppHttpException(int code, string message) : base(message) => StatusCode = code;
}

public class ConfigurationException : Exception
{
    public ConfigurationException(string message, Exception inner) : base(message, inner) { }
}

// ─── FAKE DB CONNECTION (IDisposable demo) ────────────────────────────────────
public class FakeDbConnection : IDisposable
{
    private readonly string _connStr;
    private bool _open;
    private bool _disposed;

    public FakeDbConnection(string connStr) => _connStr = connStr;

    public void Open()
    {
        _open = true;
        Console.WriteLine($"  [DB] Connection opened ({_connStr})");
    }

    public void ExecuteQuery(string sql)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        if (!_open) throw new InvalidOperationException("Connection not open.");
        Console.WriteLine($"  [DB] Executing: {sql}");
    }

    public void Dispose()
    {
        if (_disposed) return;
        Console.WriteLine("  [DB] Connection disposed.");
        _open     = false;
        _disposed = true;
    }
}
