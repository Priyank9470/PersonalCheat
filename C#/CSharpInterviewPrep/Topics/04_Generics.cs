namespace CSharpInterviewPrep.Topics;

public static class GenericsDemo
{
    public static void Run()
    {
        Console.WriteLine("=== GENERICS ===\n");

        // ── GENERIC METHOD ─────────────────────────────────────────────────────
        Console.WriteLine("--- Generic Method ---");
        Console.WriteLine($"  Swap(1, 2)     = {Swap(1, 2)}");
        Console.WriteLine($"  Swap('A','B')  = {Swap('A', 'B')}");
        Console.WriteLine($"  Swap(\"hi\",\"bye\") = {Swap("hi", "bye")}");

        // ── GENERIC CLASS ──────────────────────────────────────────────────────
        Console.WriteLine("\n--- Generic Stack<T> ---");
        var intStack = new GenericStack<int>();
        intStack.Push(10); intStack.Push(20); intStack.Push(30);
        Console.WriteLine($"  Pop:   {intStack.Pop()}");
        Console.WriteLine($"  Peek:  {intStack.Peek()}");
        Console.WriteLine($"  Count: {intStack.Count}");

        var strStack = new GenericStack<string>();
        strStack.Push("alpha"); strStack.Push("beta");
        Console.WriteLine($"  String stack peek: {strStack.Peek()}");

        // ── CONSTRAINTS (where T : ...) ────────────────────────────────────────
        Console.WriteLine("\n--- Generic Constraints ---");
        int[]    nums  = { 3, 1, 4, 1, 5, 9, 2, 6 };
        string[] words = { "banana", "apple", "cherry", "date" };
        Console.WriteLine($"  Max int   : {FindMax(nums)}");
        Console.WriteLine($"  Max string: {FindMax(words)}");

        var minMax = MinMax(nums);
        Console.WriteLine($"  Min={minMax.Min}  Max={minMax.Max}");

        // ── GENERIC REPOSITORY (common interview pattern) ──────────────────────
        Console.WriteLine("\n--- Generic Repository Pattern ---");
        var repo = new InMemoryRepository<Product>();
        repo.Add(new Product(1, "Laptop",   999.99m));
        repo.Add(new Product(2, "Mouse",     29.99m));
        repo.Add(new Product(3, "Keyboard",  79.99m));

        Console.WriteLine("  All products:");
        foreach (var p in repo.GetAll())
            Console.WriteLine($"    {p}");

        var found = repo.GetById(2);
        Console.WriteLine($"  GetById(2) → {found}");

        repo.Delete(2);
        Console.WriteLine($"  After delete: {repo.Count} products.");

        // ── COVARIANCE (out T) & CONTRAVARIANCE (in T) ────────────────────────
        Console.WriteLine("\n--- Covariance (out) & Contravariance (in) ---");

        // Covariance: IProducer<Dog> assignable to IProducer<Animal> (Dog IS-A Animal)
        IProducer<Dog> dogProducer = new DogProducer();
        IProducer<Animal> animalProducer = dogProducer; // works because out T
        Console.WriteLine($"  Covariant: {animalProducer.Produce().Name}");

        // Contravariance: IConsumer<Animal> assignable to IConsumer<Dog>
        IConsumer<Animal> animalConsumer = new AnimalPrinter();
        IConsumer<Dog>    dogConsumer    = animalConsumer; // works because in T
        dogConsumer.Consume(new Dog("Rex", "Lab"));

        // ── DEFAULT(T) ─────────────────────────────────────────────────────────
        Console.WriteLine("\n--- default(T) ---");
        Console.WriteLine($"  default(int)    = {default(int)}");
        Console.WriteLine($"  default(bool)   = {default(bool)}");
        Console.WriteLine($"  default(string) = {default(string) ?? "(null)"}");
        Console.WriteLine($"  default(Dog)    = {default(Dog) ?? (object)"(null)"}");
    }

    static string Swap<T>(T a, T b) => $"({b}, {a})";

    static T FindMax<T>(T[] items) where T : IComparable<T>
    {
        T max = items[0];
        foreach (var item in items)
            if (item.CompareTo(max) > 0) max = item;
        return max;
    }

    static (T Min, T Max) MinMax<T>(T[] items) where T : IComparable<T>
    {
        T min = items[0], max = items[0];
        foreach (var item in items)
        {
            if (item.CompareTo(min) < 0) min = item;
            if (item.CompareTo(max) > 0) max = item;
        }
        return (min, max);
    }
}

// ─── GENERIC STACK ────────────────────────────────────────────────────────────
public class GenericStack<T>
{
    private readonly List<T> _items = new();
    public int Count => _items.Count;

    public void Push(T item) => _items.Add(item);

    public T Pop()
    {
        if (_items.Count == 0) throw new InvalidOperationException("Stack is empty.");
        var top = _items[^1];
        _items.RemoveAt(_items.Count - 1);
        return top;
    }

    public T Peek() => _items.Count == 0
        ? throw new InvalidOperationException("Stack is empty.")
        : _items[^1];
}

// ─── GENERIC REPOSITORY ───────────────────────────────────────────────────────
public interface IEntity { int Id { get; } }

public interface IRepository<T> where T : class, IEntity
{
    void Add(T entity);
    T?   GetById(int id);
    void Delete(int id);
    IEnumerable<T> GetAll();
    int Count { get; }
}

public class InMemoryRepository<T> : IRepository<T> where T : class, IEntity
{
    private readonly List<T> _store = new();

    public int Count => _store.Count;
    public void Add(T entity) => _store.Add(entity);
    public T?   GetById(int id) => _store.FirstOrDefault(x => x.Id == id);
    public void Delete(int id) => _store.RemoveAll(x => x.Id == id);
    public IEnumerable<T> GetAll() => _store;
}

public record Product(int Id, string Name, decimal Price) : IEntity
{
    public override string ToString() => $"[{Id}] {Name} — ${Price:F2}";
}

// ─── COVARIANCE & CONTRAVARIANCE ──────────────────────────────────────────────
public interface IProducer<out T> { T Produce(); }    // out = covariant (read)
public interface IConsumer<in T>  { void Consume(T item); }  // in = contravariant (write)

public class DogProducer : IProducer<Dog>
{
    public Dog Produce() => new Dog("Produced Dog", "Poodle");
}

public class AnimalPrinter : IConsumer<Animal>
{
    public void Consume(Animal a) => Console.WriteLine($"  Consumed: {a.Name}");
}
