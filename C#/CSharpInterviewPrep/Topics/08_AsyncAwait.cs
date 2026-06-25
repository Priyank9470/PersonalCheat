namespace CSharpInterviewPrep.Topics;

public static class AsyncAwaitDemo
{
    public static void Run()
    {
        Console.WriteLine("=== ASYNC / AWAIT & TASKS ===\n");
        RunAsync().GetAwaiter().GetResult(); // safe in console app — no sync context
    }

    static async Task RunAsync()
    {
        // ── TASK vs THREAD ────────────────────────────────────────────────────
        Console.WriteLine("--- Task vs Thread ---");
        Console.WriteLine("  Thread : OS-level (~1 MB stack), explicit management");
        Console.WriteLine("  Task   : abstraction over ThreadPool, lightweight, composable");
        Console.WriteLine("  Use Task for async I/O; Task.Run for CPU-bound work on background thread\n");

        // ── BASIC ASYNC / AWAIT ───────────────────────────────────────────────
        Console.WriteLine("--- Basic async/await ---");
        string data = await FetchDataAsync("api/products");
        Console.WriteLine($"  Result: {data}");

        int sum = await ComputeAsync(10, 20);
        Console.WriteLine($"  ComputeAsync(10,20) = {sum}");

        // ── TASK.WHENALL (parallel execution) ─────────────────────────────────
        Console.WriteLine("\n--- Task.WhenAll (run concurrently) ---");
        var sw = System.Diagnostics.Stopwatch.StartNew();

        var t1 = SimulateAsync("DB query",    300);
        var t2 = SimulateAsync("API call",    200);
        var t3 = SimulateAsync("File read",   250);

        string[] results = await Task.WhenAll(t1, t2, t3);
        sw.Stop();

        foreach (var r in results) Console.WriteLine($"  {r}");
        Console.WriteLine($"  Total: {sw.ElapsedMilliseconds}ms  (not 750ms — ran in parallel)");

        // ── TASK.WHENANY (first to complete) ──────────────────────────────────
        Console.WriteLine("\n--- Task.WhenAny (first to finish wins) ---");
        var fast = SimulateAsync("Fast cache",  50);
        var slow = SimulateAsync("Slow origin", 500);
        var first = await Task.WhenAny(fast, slow);
        Console.WriteLine($"  Winner: {await first}");

        // ── CANCELLATION TOKEN ─────────────────────────────────────────────────
        Console.WriteLine("\n--- CancellationToken ---");
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(150));
        try
        {
            await LongJobAsync("Export CSV", 1000, cts.Token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("  Job cancelled (timeout exceeded).");
        }

        // Manual cancellation
        using var cts2 = new CancellationTokenSource();
        var jobTask = LongJobAsync("Import data", 2000, cts2.Token);
        await Task.Delay(100); // let it start
        cts2.Cancel();
        try { await jobTask; }
        catch (OperationCanceledException) { Console.WriteLine("  Job cancelled (manual)."); }

        // ── EXCEPTION HANDLING IN ASYNC ────────────────────────────────────────
        Console.WriteLine("\n--- Exception handling in async ---");
        try
        {
            await FailingAsync();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"  Caught: {ex.Message}");
        }

        // AggregateException from Task.WhenAll
        var bad1 = Task.FromException<string>(new Exception("Error A"));
        var bad2 = Task.FromException<string>(new Exception("Error B"));
        var good = Task.FromResult("OK");
        try
        {
            await Task.WhenAll(bad1, bad2, good);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  WhenAll first error: {ex.Message}");
        }

        // ── VALUETASK (performance) ─────────────────────────────────────────────
        Console.WriteLine("\n--- ValueTask<T> (avoids heap alloc for sync-fast paths) ---");
        var r1 = await GetCachedAsync(42);  // returns synchronously from cache
        var r2 = await GetCachedAsync(99);  // actually async
        Console.WriteLine($"  Cached: {r1}   Fetched: {r2}");

        // ── ASYNC VOID — AVOID! ────────────────────────────────────────────────
        Console.WriteLine("\n--- async void (only for event handlers) ---");
        Console.WriteLine("  async void: exceptions are unhandled (crash the process)");
        Console.WriteLine("  async Task: exceptions propagate normally — always prefer this");

        // ── CONFIGUREAWAIT ────────────────────────────────────────────────────
        Console.WriteLine("\n--- ConfigureAwait(false) ---");
        Console.WriteLine("  ConfigureAwait(false): don't resume on the captured sync context.");
        Console.WriteLine("  Use in library code to avoid deadlocks (classic ASP.NET).");
        Console.WriteLine("  Not needed in .NET Core/5+ (no sync context), but still good practice.");
        await LibraryMethodAsync(); // demonstrates ConfigureAwait(false)

        // ── BEST PRACTICES ─────────────────────────────────────────────────────
        Console.WriteLine("\n--- Best Practices ---");
        Console.WriteLine("  ✓ async all the way — don't mix .Result or .Wait()");
        Console.WriteLine("  ✓ Use CancellationToken in all long-running methods");
        Console.WriteLine("  ✓ ConfigureAwait(false) in library / non-UI code");
        Console.WriteLine("  ✓ Use Task.Run only for CPU-bound work");
        Console.WriteLine("  ✗ Never async void (except event handlers)");
        Console.WriteLine("  ✗ Never block with .Result / .Wait() in async context (deadlock risk)");
    }

    // ── HELPERS ───────────────────────────────────────────────────────────────
    static async Task<string> FetchDataAsync(string endpoint)
    {
        await Task.Delay(80);
        return $"[data from {endpoint}]";
    }

    static async Task<int> ComputeAsync(int a, int b)
    {
        await Task.Delay(50);
        return a + b;
    }

    static async Task<string> SimulateAsync(string name, int ms)
    {
        await Task.Delay(ms);
        return $"{name} done ({ms}ms)";
    }

    static async Task LongJobAsync(string name, int ms, CancellationToken ct)
    {
        Console.WriteLine($"  [{name}] Starting...");
        await Task.Delay(ms, ct);
        Console.WriteLine($"  [{name}] Completed.");
    }

    static async Task FailingAsync()
    {
        await Task.Delay(10);
        throw new InvalidOperationException("Async operation failed!");
    }

    static readonly Dictionary<int, string> _cache = new();

    static ValueTask<string> GetCachedAsync(int id)
    {
        // Synchronous fast path — no heap allocation for Task
        if (_cache.TryGetValue(id, out var cached))
            return ValueTask.FromResult(cached);

        return new ValueTask<string>(FetchAndStoreAsync(id));
    }

    static async Task<string> FetchAndStoreAsync(int id)
    {
        await Task.Delay(60);
        var value = $"item-{id}";
        _cache[id] = value;
        return value;
    }

    static async Task LibraryMethodAsync()
    {
        await Task.Delay(10).ConfigureAwait(false);
        Console.WriteLine("  LibraryMethodAsync ran without restoring sync context.");
    }
}
