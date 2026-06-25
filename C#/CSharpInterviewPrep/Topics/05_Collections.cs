namespace CSharpInterviewPrep.Topics;

public static class CollectionsDemo
{
    public static void Run()
    {
        Console.WriteLine("=== COLLECTIONS ===\n");

        // ── LIST<T> ────────────────────────────────────────────────────────────
        Console.WriteLine("--- List<T> (dynamic array, O(1) access) ---");
        var list = new List<int> { 5, 3, 1, 4, 2 };
        list.Add(6);
        list.AddRange(new[] { 7, 8 });
        list.Remove(3);            // removes first occurrence
        list.Sort();
        Console.WriteLine($"  Sorted   : {string.Join(", ", list)}");
        Console.WriteLine($"  Contains 4: {list.Contains(4)}   IndexOf 4: {list.IndexOf(4)}");
        Console.WriteLine($"  Count={list.Count}  Capacity={list.Capacity}");

        list.RemoveAt(0);
        Console.WriteLine($"  After RemoveAt(0): {string.Join(", ", list)}");

        // ── DICTIONARY<K,V> ───────────────────────────────────────────────────
        Console.WriteLine("\n--- Dictionary<K,V> (hash table, O(1) average) ---");
        var scores = new Dictionary<string, int>
        {
            ["Alice"] = 90,
            ["Bob"]   = 85,
            ["Carol"] = 92,
        };
        scores["Dave"] = 88;
        scores["Alice"] = 95; // update

        foreach (var (name, score) in scores)
            Console.WriteLine($"  {name,-8}: {score}");

        // Safe read
        if (scores.TryGetValue("Bob", out int bobScore))
            Console.WriteLine($"  Bob's score: {bobScore}");

        // GetValueOrDefault
        int eve = scores.GetValueOrDefault("Eve", -1);
        Console.WriteLine($"  Eve's score (default): {eve}");

        Console.WriteLine($"  Keys: {string.Join(", ", scores.Keys)}");

        // ── HASHSET<T> ─────────────────────────────────────────────────────────
        Console.WriteLine("\n--- HashSet<T> (unique values, O(1) add/contains) ---");
        var set1 = new HashSet<int> { 1, 2, 3, 4, 5 };
        var set2 = new HashSet<int> { 3, 4, 5, 6, 7 };
        set1.Add(2); // no duplicate
        Console.WriteLine($"  Set1     : {string.Join(", ", set1)}");
        Console.WriteLine($"  Set2     : {string.Join(", ", set2)}");

        var union = new HashSet<int>(set1); union.UnionWith(set2);
        var inter = new HashSet<int>(set1); inter.IntersectWith(set2);
        var diff  = new HashSet<int>(set1); diff.ExceptWith(set2);

        Console.WriteLine($"  Union    : {string.Join(", ", union)}");
        Console.WriteLine($"  Intersect: {string.Join(", ", inter)}");
        Console.WriteLine($"  Except   : {string.Join(", ", diff)}");

        // ── QUEUE<T> (FIFO) ────────────────────────────────────────────────────
        Console.WriteLine("\n--- Queue<T> (FIFO — first in, first out) ---");
        var queue = new Queue<string>();
        queue.Enqueue("First");
        queue.Enqueue("Second");
        queue.Enqueue("Third");
        Console.WriteLine($"  Peek    : {queue.Peek()}");
        Console.WriteLine($"  Dequeue : {queue.Dequeue()}");
        Console.WriteLine($"  Count   : {queue.Count}");

        // ── STACK<T> (LIFO) ────────────────────────────────────────────────────
        Console.WriteLine("\n--- Stack<T> (LIFO — last in, first out) ---");
        var stack = new Stack<string>();
        stack.Push("Bottom");
        stack.Push("Middle");
        stack.Push("Top");
        Console.WriteLine($"  Peek    : {stack.Peek()}");
        Console.WriteLine($"  Pop     : {stack.Pop()}");
        Console.WriteLine($"  Count   : {stack.Count}");

        // ── LINKEDLIST<T> ──────────────────────────────────────────────────────
        Console.WriteLine("\n--- LinkedList<T> (O(1) add/remove at head/tail) ---");
        var linked = new LinkedList<int>(new[] { 1, 2, 3, 4, 5 });
        linked.AddFirst(0);
        linked.AddLast(6);
        linked.Remove(3);
        Console.WriteLine($"  List: {string.Join(", ", linked)}");

        // ── SORTEDDICTIONARY ───────────────────────────────────────────────────
        Console.WriteLine("\n--- SortedDictionary<K,V> (always sorted by key, O(log n)) ---");
        var sorted = new SortedDictionary<string, int>
        {
            ["Zebra"] = 1, ["Apple"] = 2, ["Mango"] = 3
        };
        sorted["Banana"] = 4;
        foreach (var kv in sorted)
            Console.WriteLine($"  {kv.Key}: {kv.Value}");

        // ── CONCURRENTDICTIONARY ───────────────────────────────────────────────
        Console.WriteLine("\n--- ConcurrentDictionary (thread-safe) ---");
        var concurrent = new System.Collections.Concurrent.ConcurrentDictionary<string, int>();
        concurrent.TryAdd("x", 1);
        concurrent.AddOrUpdate("x", 1, (key, old) => old + 10);
        Console.WriteLine($"  x = {concurrent["x"]}");

        // ── YIELD RETURN ───────────────────────────────────────────────────────
        Console.WriteLine("\n--- yield return (lazy IEnumerable) ---");
        foreach (var n in EvenNumbers(10))
            Console.Write($"{n} ");
        Console.WriteLine();

        // ── INTERFACE HIERARCHY ─────────────────────────────────────────────────
        Console.WriteLine("\n--- Collection Interface Hierarchy ---");
        Console.WriteLine("  IEnumerable<T>   : foreach only (read-only, lazy)");
        Console.WriteLine("  ICollection<T>   : + Count, Add, Remove, Contains");
        Console.WriteLine("  IList<T>         : + indexer [i], Insert, IndexOf");
        Console.WriteLine("  IDictionary<K,V> : + key-based access");
        Console.WriteLine();
        Console.WriteLine("  Array vs List<T>:");
        Console.WriteLine("    Array  : fixed size, faster iteration, multi-dimensional");
        Console.WriteLine("    List<T>: dynamic size, more methods, slight overhead");

        // ── ARRAY TRICKS ───────────────────────────────────────────────────────
        Console.WriteLine("\n--- Array tricks ---");
        int[] arr = { 5, 2, 8, 1, 9, 3 };
        Array.Sort(arr);
        Console.WriteLine($"  Sorted: {string.Join(", ", arr)}");
        int idx = Array.BinarySearch(arr, 8);
        Console.WriteLine($"  BinarySearch(8): index={idx}");
        Array.Reverse(arr);
        Console.WriteLine($"  Reversed: {string.Join(", ", arr)}");
    }

    static IEnumerable<int> EvenNumbers(int upTo)
    {
        for (int i = 2; i <= upTo; i += 2)
            yield return i;   // execution pauses here until next MoveNext()
    }
}
