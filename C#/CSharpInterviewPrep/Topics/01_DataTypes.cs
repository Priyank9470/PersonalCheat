namespace CSharpInterviewPrep.Topics;

public static class DataTypesDemo
{
    public static void Run()
    {
        Console.WriteLine("=== DATA TYPES & VARIABLES ===\n");

        // ── VALUE TYPES vs REFERENCE TYPES ────────────────────────────────────
        Console.WriteLine("--- Value Types vs Reference Types ---");
        // Value types: stored on the STACK, copied on assignment
        int a = 10;
        int b = a;
        b = 20;
        Console.WriteLine($"Value type  → a={a}, b={b}  (independent copies)");

        // Reference types: stored on the HEAP, assignment copies the pointer
        int[] arr1 = { 1, 2, 3 };
        int[] arr2 = arr1;   // same object on heap
        arr2[0] = 99;
        Console.WriteLine($"Ref type    → arr1[0]={arr1[0]}, arr2[0]={arr2[0]}  (same object)");

        // ── BOXING & UNBOXING ──────────────────────────────────────────────────
        Console.WriteLine("\n--- Boxing & Unboxing ---");
        int num    = 42;
        object box = num;          // Boxing:   value type → object (heap alloc, slow)
        int    unboxed = (int)box; // Unboxing: object → value type (explicit cast)
        Console.WriteLine($"num={num}  boxed={box}  unboxed={unboxed}");
        Console.WriteLine("TIP: Avoid boxing in hot paths — use generics instead.");

        // ── BUILT-IN VALUE TYPES ───────────────────────────────────────────────
        Console.WriteLine("\n--- Built-in Data Types ---");
        bool    flag = true;
        byte    by   = 255;            // 0 – 255
        sbyte   sb   = -128;           // -128 – 127
        short   sh   = 32_767;         // digit separators (C# 7+)
        int     i    = 2_147_483_647;
        long    l    = 9_223_372_036_854_775_807L;
        float   f    = 3.14f;          // ~7 significant digits, suffix f
        double  d    = 3.141592653589793; // ~15 significant digits
        decimal dec  = 99.99m;         // 28-29 significant digits, suffix m (financial)
        char    ch   = 'A';            // UTF-16 character
        string  str  = "Hello C#";     // reference type, immutable

        Console.WriteLine($"bool={flag}  byte={by}  sbyte={sb}  short={sh}");
        Console.WriteLine($"int={i}  long={l}");
        Console.WriteLine($"float={f}  double={d}  decimal={dec}");
        Console.WriteLine($"char={ch}  string={str}");

        // ── NULLABLE TYPES ─────────────────────────────────────────────────────
        Console.WriteLine("\n--- Nullable Types (T?) ---");
        int? nullableInt = null;
        Console.WriteLine($"HasValue={nullableInt.HasValue}  value={nullableInt}");
        nullableInt = 100;
        Console.WriteLine($"HasValue={nullableInt.HasValue}  Value={nullableInt.Value}");

        // Null-coalescing (??) and null-coalescing assignment (??=)
        int result = nullableInt ?? -1;
        Console.WriteLine($"?? operator: {result}");

        string? name = null;
        name ??= "Default";
        Console.WriteLine($"??= operator: {name}");

        // Null-conditional (?.)
        string? maybeNull = null;
        int len = maybeNull?.Length ?? 0;
        Console.WriteLine($"?.Length of null string: {len}");

        // ── CONST vs READONLY ──────────────────────────────────────────────────
        Console.WriteLine("\n--- const vs readonly ---");
        Console.WriteLine($"const  PI      = {Constants.Pi}   (compile-time, implicitly static)");
        Console.WriteLine($"readonly AppName = {AppSettings.AppName}  (set once, runtime)");

        // ── TYPE CONVERSION ────────────────────────────────────────────────────
        Console.WriteLine("\n--- Type Conversion ---");
        // Implicit: no data loss
        int x = 100;
        long y = x;
        Console.WriteLine($"Implicit int→long: {y}");

        // Explicit cast: possible data loss
        double pi    = 3.99;
        int    piInt = (int)pi;  // truncates decimal
        Console.WriteLine($"Explicit cast double→int: {piInt}  (truncated, not rounded)");

        // Convert class: throws on invalid input
        string numStr  = "123";
        int    fromStr = Convert.ToInt32(numStr);
        Console.WriteLine($"Convert.ToInt32(\"123\"): {fromStr}");

        // TryParse: safe, no exception
        bool ok1 = int.TryParse("456abc", out int r1);
        bool ok2 = int.TryParse("789",    out int r2);
        Console.WriteLine($"TryParse(\"456abc\"): success={ok1} value={r1}");
        Console.WriteLine($"TryParse(\"789\")   : success={ok2} value={r2}");

        // ── REF vs OUT ─────────────────────────────────────────────────────────
        Console.WriteLine("\n--- ref vs out ---");
        int refVal = 5;
        DoubleIt(ref refVal);          // must be initialised before passing
        Console.WriteLine($"ref: {refVal}");

        Add(3, 4, out int sum);        // out: no need to initialise
        Console.WriteLine($"out sum: {sum}");

        // ── STRINGS ────────────────────────────────────────────────────────────
        Console.WriteLine("\n--- String (immutable reference type) ---");
        string s = "  Hello, World!  ";
        Console.WriteLine($"Trim        : '{s.Trim()}'");
        Console.WriteLine($"ToUpper     : '{s.Trim().ToUpper()}'");
        Console.WriteLine($"Replace     : '{s.Trim().Replace("World", "C#")}'");
        Console.WriteLine($"Contains    : {s.Contains("World")}");
        Console.WriteLine($"StartsWith  : {s.Trim().StartsWith("Hello")}");
        Console.WriteLine($"Substring   : '{s.Trim().Substring(7, 5)}'");
        Console.WriteLine($"Split       : {string.Join(" | ", s.Trim().Split(','))}");
        Console.WriteLine($"IndexOf 'W' : {s.IndexOf('W')}");
        Console.WriteLine($"Length      : {s.Length}");

        // String == operator compares by value
        string s1 = "hello";
        string s2 = "hello";
        Console.WriteLine($"\n\"hello\" == \"hello\" : {s1 == s2}    (value comparison)");
        Console.WriteLine($"ReferenceEquals    : {ReferenceEquals(s1, s2)}  (may be true due to interning)");

        // StringBuilder: mutable, efficient for repeated concatenation
        Console.WriteLine("\n--- StringBuilder (mutable, O(n) not O(n²)) ---");
        var sb2 = new System.Text.StringBuilder();
        for (int k = 1; k <= 5; k++)
            sb2.Append($"Item{k}");
        sb2.Insert(0, "[").Append("]");
        Console.WriteLine($"StringBuilder: {sb2}");

        // ── VAR ────────────────────────────────────────────────────────────────
        Console.WriteLine("\n--- var (implicit typing, compile-time resolved) ---");
        var number = 42;       // int
        var text   = "hello";  // string
        var items  = new List<int> { 1, 2, 3 };
        Console.WriteLine($"var number={number} ({number.GetType().Name})");
        Console.WriteLine($"var text='{text}' ({text.GetType().Name})");
        Console.WriteLine($"var items={string.Join(",", items)} ({items.GetType().Name})");
    }

    static void DoubleIt(ref int value) => value *= 2;
    static void Add(int a, int b, out int result) => result = a + b;
}

public static class Constants
{
    public const double Pi = 3.14159265358979;
}

public class AppSettings
{
    public static readonly string AppName;
    static AppSettings() => AppName = "CSharpPrepApp v1.0";
}
