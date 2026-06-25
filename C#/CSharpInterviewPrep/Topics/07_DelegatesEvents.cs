namespace CSharpInterviewPrep.Topics;

public static class DelegatesEventsDemo
{
    // Custom delegate type (rarely needed — use Func/Action instead)
    delegate int   MathOp(int a, int b);
    delegate void  Logger(string message);

    public static void Run()
    {
        Console.WriteLine("=== DELEGATES, FUNC, ACTION & EVENTS ===\n");

        // ── DELEGATE ──────────────────────────────────────────────────────────
        Console.WriteLine("--- Delegate (type-safe function pointer) ---");
        MathOp add  = (a, b) => a + b;
        MathOp mul  = (a, b) => a * b;
        MathOp mod  = Modulo;           // method group
        Console.WriteLine($"  add(10,3)  = {add(10, 3)}");
        Console.WriteLine($"  mul(10,3)  = {mul(10, 3)}");
        Console.WriteLine($"  mod(10,3)  = {mod(10, 3)}");

        // ── MULTICAST DELEGATE ────────────────────────────────────────────────
        Console.WriteLine("\n--- Multicast Delegate (+=) ---");
        Logger log = msg => Console.WriteLine($"  [Email] {msg}");
        log += msg => Console.WriteLine($"  [SMS]   {msg}");
        log += msg => Console.WriteLine($"  [Slack] {msg}");
        log("Server deployed!");
        // Note: removing anonymous lambda won't work (different instance each time)
        // To remove, hold a reference: Logger slackLog = msg => ...; log += slackLog; log -= slackLog;

        // ── FUNC<T, TResult> ──────────────────────────────────────────────────
        Console.WriteLine("\n--- Func<T, TResult> (returns a value) ---");
        Func<int, int, int>    power    = (b, e) => (int)Math.Pow(b, e);
        Func<string, string>   upper    = s => s.ToUpper();
        Func<int, bool>        isEven   = n => n % 2 == 0;
        Func<int, int, double> divide   = (a, b) => (double)a / b;

        Console.WriteLine($"  power(2,10)  = {power(2, 10)}");
        Console.WriteLine($"  upper('csharp') = {upper("csharp")}");
        Console.WriteLine($"  isEven(7)    = {isEven(7)}");
        Console.WriteLine($"  divide(7,2)  = {divide(7, 2)}");

        // Higher-order function: function that takes/returns a function
        Func<int, Func<int, int>> adder = x => y => x + y;
        var add5 = adder(5);
        Console.WriteLine($"  add5(3) = {add5(3)}  (closure / currying)");

        // ── ACTION<T> ─────────────────────────────────────────────────────────
        Console.WriteLine("\n--- Action<T> (returns void) ---");
        Action<string>        print    = msg => Console.WriteLine($"  >> {msg}");
        Action<string, int>   repeat   = (msg, n) => { for (int i = 0; i < n; i++) Console.WriteLine($"  {i+1}. {msg}"); };
        Action                greet    = () => Console.WriteLine("  Hello!");

        greet();
        print("Action demo");
        repeat("repeat me", 2);

        // ── PREDICATE<T> ──────────────────────────────────────────────────────
        Console.WriteLine("\n--- Predicate<T> (Func<T, bool> shorthand) ---");
        Predicate<int>    isPositive = n => n > 0;
        Predicate<string> isLong     = s => s.Length > 5;
        var nums = new List<int> { -3, -1, 0, 2, 5, 8 };
        var positives = nums.FindAll(isPositive); // List.FindAll takes Predicate
        Console.WriteLine($"  Positives: {string.Join(", ", positives)}");
        Console.WriteLine($"  isLong('Hello'): {isLong("Hello")}");
        Console.WriteLine($"  isLong('CSharp'): {isLong("CSharp")}");

        // ── LAMBDA EXPRESSIONS ────────────────────────────────────────────────
        Console.WriteLine("\n--- Lambda Expressions ---");
        var numbers = Enumerable.Range(1, 10).ToList();
        var evens   = numbers.Where(n => n % 2 == 0).ToList();
        var cubed   = numbers.Select(n => n * n * n).ToList();
        Console.WriteLine($"  Evens : {string.Join(", ", evens)}");
        Console.WriteLine($"  Cubed : {string.Join(", ", cubed)}");

        // Statement lambda (multiple lines)
        Func<int, string> classify = n =>
        {
            if (n < 0)  return "negative";
            if (n == 0) return "zero";
            return "positive";
        };
        Console.WriteLine($"  classify(-5)={classify(-5)}  classify(0)={classify(0)}  classify(3)={classify(3)}");

        // ── CLOSURE ───────────────────────────────────────────────────────────
        Console.WriteLine("\n--- Closure (captures outer variable) ---");
        var funcs = new List<Func<int>>();
        for (int i = 0; i < 4; i++)
        {
            int captured = i; // copy of i — IMPORTANT
            funcs.Add(() => captured);
        }
        Console.WriteLine($"  Closures: {string.Join(", ", funcs.Select(f => f()))}");

        // Without capturing: all would print 4 (the final value of i)
        var funcs2 = new List<Func<int>>();
        int j = 0;
        for (; j < 4; j++) funcs2.Add(() => j); // all capture same j
        Console.WriteLine($"  Without capture (bug): {string.Join(", ", funcs2.Select(f => f()))}");

        // ── EVENTS ────────────────────────────────────────────────────────────
        Console.WriteLine("\n--- Events (publisher-subscriber pattern) ---");
        var order        = new CustomerOrder(201, "MacBook");
        var emailSvc     = new EmailNotifier();
        var smsSvc       = new SmsNotifier();
        var inventorySvc = new InventoryService();

        order.OrderPlaced  += emailSvc.OnOrderPlaced;
        order.OrderPlaced  += smsSvc.OnOrderPlaced;
        order.OrderPlaced  += inventorySvc.OnOrderPlaced;
        order.OrderShipped += emailSvc.OnOrderShipped;

        order.Place();
        Console.WriteLine();
        order.Ship();

        // Unsubscribe
        order.OrderPlaced -= smsSvc.OnOrderPlaced;

        // ── FUNC vs ACTION vs PREDICATE CHEATSHEET ─────────────────────────────
        Console.WriteLine("\n--- Cheatsheet ---");
        Console.WriteLine("  Func<T1,T2,...,TResult>  : takes params, returns TResult");
        Console.WriteLine("  Action<T1,T2,...>         : takes params, returns void");
        Console.WriteLine("  Predicate<T>              : takes T, returns bool (= Func<T,bool>)");
        Console.WriteLine("  Delegate                  : custom function type (rarely needed)");
        Console.WriteLine("  event EventHandler<TArgs> : multicast, subscriber-safe");
    }

    static int Modulo(int a, int b) => a % b;
}

// ─── EVENT PATTERN ────────────────────────────────────────────────────────────
public class OrderEventArgs : EventArgs
{
    public int    OrderId   { get; }
    public string Item      { get; }
    public DateTime PlacedAt { get; } = DateTime.Now;

    public OrderEventArgs(int id, string item) { OrderId = id; Item = item; }
}

public class CustomerOrder
{
    public int    Id   { get; }
    public string Item { get; }
    private bool  _placed;

    public event EventHandler<OrderEventArgs>? OrderPlaced;
    public event EventHandler<OrderEventArgs>? OrderShipped;

    public CustomerOrder(int id, string item) { Id = id; Item = item; }

    public void Place()
    {
        _placed = true;
        Console.WriteLine($"  Order #{Id} ('{Item}') placed.");
        OrderPlaced?.Invoke(this, new OrderEventArgs(Id, Item));
    }

    public void Ship()
    {
        if (!_placed) throw new InvalidOperationException("Cannot ship: order not placed.");
        Console.WriteLine($"  Order #{Id} shipped.");
        OrderShipped?.Invoke(this, new OrderEventArgs(Id, Item));
    }
}

public class EmailNotifier
{
    public void OnOrderPlaced(object? sender, OrderEventArgs e)
        => Console.WriteLine($"    [Email] Order #{e.OrderId} confirmed — '{e.Item}'");
    public void OnOrderShipped(object? sender, OrderEventArgs e)
        => Console.WriteLine($"    [Email] Your '{e.Item}' is on the way!");
}

public class SmsNotifier
{
    public void OnOrderPlaced(object? sender, OrderEventArgs e)
        => Console.WriteLine($"    [SMS]   Order #{e.OrderId} received.");
}

public class InventoryService
{
    public void OnOrderPlaced(object? sender, OrderEventArgs e)
        => Console.WriteLine($"    [INV]   Reserved stock for '{e.Item}'.");
}
