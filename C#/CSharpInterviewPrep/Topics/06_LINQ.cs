namespace CSharpInterviewPrep.Topics;

public static class LinqDemo
{
    record EmpRecord(int Id, string Name, string Dept, decimal Salary, int Age);

    static readonly List<EmpRecord> Employees = new()
    {
        new(1, "Alice",  "Engineering", 95000,  30),
        new(2, "Bob",    "Marketing",   60000,  25),
        new(3, "Carol",  "Engineering", 110000, 35),
        new(4, "Dave",   "HR",          55000,  28),
        new(5, "Eve",    "Engineering", 105000, 32),
        new(6, "Frank",  "Marketing",   65000,  27),
        new(7, "Grace",  "HR",          58000,  31),
        new(8, "Heidi",  "Engineering", 120000, 40),
    };

    public static void Run()
    {
        Console.WriteLine("=== LINQ (Language Integrated Query) ===\n");

        // ── WHERE + SELECT ─────────────────────────────────────────────────────
        Console.WriteLine("--- Where + Select + OrderBy ---");
        var engineers = Employees
            .Where(e => e.Dept == "Engineering")
            .Select(e => new { e.Name, e.Salary })
            .OrderByDescending(e => e.Salary);

        foreach (var e in engineers)
            Console.WriteLine($"  {e.Name,-8}: ${e.Salary:N0}");

        // ── AGGREGATE ─────────────────────────────────────────────────────────
        Console.WriteLine("\n--- Aggregate Functions ---");
        Console.WriteLine($"  Count       : {Employees.Count()}");
        Console.WriteLine($"  Average Sal : ${Employees.Average(e => e.Salary):N0}");
        Console.WriteLine($"  Max Salary  : ${Employees.Max(e => e.Salary):N0}");
        Console.WriteLine($"  Min Salary  : ${Employees.Min(e => e.Salary):N0}");
        Console.WriteLine($"  Total Salary: ${Employees.Sum(e => e.Salary):N0}");
        Console.WriteLine($"  Custom Agg  : {Employees.Aggregate(0, (acc, e) => acc + (int)(e.Salary / 1000))}k total");

        // ── GROUP BY ──────────────────────────────────────────────────────────
        Console.WriteLine("\n--- GroupBy ---");
        var byDept = Employees
            .GroupBy(e => e.Dept)
            .Select(g => new { Dept = g.Key, Count = g.Count(), AvgSal = g.Average(e => e.Salary) })
            .OrderByDescending(g => g.AvgSal);

        foreach (var g in byDept)
            Console.WriteLine($"  {g.Dept,-15} Count={g.Count}  AvgSal=${g.AvgSal:N0}");

        // ── FIRST / SINGLE / ANY / ALL ────────────────────────────────────────
        Console.WriteLine("\n--- First / FirstOrDefault / Single / Any / All ---");
        var firstHR = Employees.First(e => e.Dept == "HR");
        Console.WriteLine($"  First HR           : {firstHR.Name}");

        var maybeZara = Employees.FirstOrDefault(e => e.Name == "Zara");
        Console.WriteLine($"  FirstOrDefault Zara: {maybeZara?.Name ?? "(null — not found)"}");

        // Single: throws if 0 or >1 results
        var uniqueHeidi = Employees.Single(e => e.Name == "Heidi");
        Console.WriteLine($"  Single('Heidi')    : {uniqueHeidi.Name}");

        Console.WriteLine($"  Any salary >100k   : {Employees.Any(e => e.Salary > 100000)}");
        Console.WriteLine($"  All salary >0      : {Employees.All(e => e.Salary > 0)}");
        Console.WriteLine($"  Count salary >90k  : {Employees.Count(e => e.Salary > 90000)}");

        // ── SELECT MANY (flatten) ─────────────────────────────────────────────
        Console.WriteLine("\n--- SelectMany (flatten nested collections) ---");
        var teams = new List<(string Team, string[] Members)>
        {
            ("Alpha", new[] { "Alice", "Bob" }),
            ("Beta",  new[] { "Carol", "Dave", "Eve" }),
        };
        var allMembers = teams.SelectMany(t => t.Members);
        Console.WriteLine($"  All members: {string.Join(", ", allMembers)}");

        var teamMember = teams.SelectMany(t => t.Members, (t, m) => $"{m}({t.Team})");
        Console.WriteLine($"  With team  : {string.Join(", ", teamMember)}");

        // ── JOIN ──────────────────────────────────────────────────────────────
        Console.WriteLine("\n--- Join ---");
        var deptBudgets = new[] {
            (Dept: "Engineering", Budget: 500_000m),
            (Dept: "Marketing",   Budget: 200_000m),
            (Dept: "HR",          Budget: 100_000m),
        };

        var joined = Employees
            .Join(deptBudgets,
                  e => e.Dept,
                  d => d.Dept,
                  (e, d) => $"{e.Name,-8} [{e.Dept}] Budget=${d.Budget:N0}")
            .Take(4);

        foreach (var j in joined) Console.WriteLine($"  {j}");

        // ── SKIP + TAKE (pagination) ──────────────────────────────────────────
        Console.WriteLine("\n--- Skip + Take (pagination) ---");
        int page = 1, pageSize = 3;
        var paged = Employees.OrderBy(e => e.Name).Skip((page - 1) * pageSize).Take(pageSize);
        Console.WriteLine($"  Page {page}, size {pageSize}: {string.Join(", ", paged.Select(e => e.Name))}");

        // ── SET OPERATIONS ────────────────────────────────────────────────────
        Console.WriteLine("\n--- Set Operations ---");
        int[] a = { 1, 2, 3, 4, 5 };
        int[] b = { 3, 4, 5, 6, 7 };
        Console.WriteLine($"  Distinct(1,2,2,3)  : {string.Join(",", new[]{1,2,2,3}.Distinct())}");
        Console.WriteLine($"  Union              : {string.Join(",", a.Union(b))}");
        Console.WriteLine($"  Intersect          : {string.Join(",", a.Intersect(b))}");
        Console.WriteLine($"  Except(a - b)      : {string.Join(",", a.Except(b))}");

        // ── QUERY SYNTAX vs METHOD SYNTAX ─────────────────────────────────────
        Console.WriteLine("\n--- Query Syntax vs Method Syntax ---");
        // Method syntax (preferred in practice)
        var method = Employees.Where(e => e.Salary > 90000).Select(e => e.Name);

        // Query syntax (SQL-like, compiles to same IL)
        var query  = from e in Employees
                     where e.Salary > 90000
                     select e.Name;

        Console.WriteLine($"  High earners: {string.Join(", ", method)}");

        // ── DEFERRED vs IMMEDIATE EXECUTION ───────────────────────────────────
        Console.WriteLine("\n--- Deferred vs Immediate Execution ---");
        var deferred = Employees.Where(e => e.Age > 30);     // NOT executed yet
        Employees.Add(new EmpRecord(9, "Ivy", "Engineering", 88000, 33)); // add after

        Console.WriteLine($"  Deferred query sees new record: {deferred.Count()} employees > 30");
        // immediate: ToList(), ToArray(), Count(), First(), Sum() — executes NOW
        var immediate = Employees.Where(e => e.Age > 30).ToList(); // snapshot
        Employees.RemoveAt(Employees.Count - 1); // remove Ivy
        Console.WriteLine($"  Immediate snapshot count: {immediate.Count} (captured before remove)");

        // ── ZIP ──────────────────────────────────────────────────────────────
        Console.WriteLine("\n--- Zip (pair two sequences) ---");
        var names  = new[] { "Alice", "Bob", "Carol" };
        var grades = new[] { "A",     "B",   "A+" };
        var zipped = names.Zip(grades, (n, g) => $"{n}:{g}");
        Console.WriteLine($"  {string.Join(", ", zipped)}");

        // ── TOLOOKUP ─────────────────────────────────────────────────────────
        Console.WriteLine("\n--- ToLookup (like GroupBy but immediate) ---");
        var lookup = Employees.ToLookup(e => e.Dept);
        foreach (var group in lookup)
            Console.WriteLine($"  {group.Key}: {string.Join(", ", group.Select(e => e.Name))}");
    }
}
