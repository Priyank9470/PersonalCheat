namespace CSharpInterviewPrep.Topics;

public static class OOPDemo
{
    public static void Run()
    {
        Console.WriteLine("=== OOP — CLASSES, INHERITANCE & POLYMORPHISM ===\n");

        // ── ENCAPSULATION ──────────────────────────────────────────────────────
        Console.WriteLine("--- Encapsulation ---");
        var person = new Person("Alice", 30);
        Console.WriteLine(person);
        person.Age = 31;   // validated through property
        Console.WriteLine($"Updated: {person}");

        // ── INHERITANCE ────────────────────────────────────────────────────────
        Console.WriteLine("\n--- Inheritance ---");
        var dog = new Dog("Rex", "Golden Retriever");
        dog.Eat();         // inherited from Animal
        dog.Speak();       // overridden in Dog
        Console.WriteLine(dog.Describe());

        var cat = new Cat("Whiskers");
        cat.Eat();
        cat.Speak();

        // ── POLYMORPHISM (runtime) ─────────────────────────────────────────────
        Console.WriteLine("\n--- Polymorphism (runtime dispatch) ---");
        Animal[] animals = { new Dog("Buddy", "Labrador"), new Cat("Luna"), new Dog("Max", "Poodle") };
        foreach (var animal in animals)
        {
            Console.Write($"  {animal.Name,-10} → ");
            animal.Speak();   // correct override called at runtime
        }

        // ── METHOD HIDING (new) vs OVERRIDING (override) ──────────────────────
        Console.WriteLine("\n--- Method Hiding (new) vs Overriding (override) ---");
        Base   baseRef  = new Derived();
        Derived derRef  = new Derived();
        Console.WriteLine($"override via Base ref : {baseRef.OverriddenMethod()}");
        Console.WriteLine($"override via Derived  : {derRef.OverriddenMethod()}");
        Console.WriteLine($"hidden   via Base ref : {baseRef.HiddenMethod()}");   // calls Base version!
        Console.WriteLine($"hidden   via Derived  : {derRef.HiddenMethod()}");    // calls Derived version

        // ── IS / AS ────────────────────────────────────────────────────────────
        Console.WriteLine("\n--- is / as ---");
        Animal a = new Dog("Fido", "Beagle");
        if (a is Dog d)
            Console.WriteLine($"  'a' is Dog → Name={d.Name}, Breed={d.Breed}");

        Cat? c = a as Cat;
        Console.WriteLine($"  'a' as Cat → {(c == null ? "null (not a Cat)" : c.Name)}");

        // ── CONSTRUCTOR CHAINING ───────────────────────────────────────────────
        Console.WriteLine("\n--- Constructor Chaining (:this / :base) ---");
        var e1 = new Employee("Bob");
        var e2 = new Employee("Carol", "Engineering");
        var e3 = new Employee("Dave",  "Marketing", 85000);
        Console.WriteLine($"  {e1}");
        Console.WriteLine($"  {e2}");
        Console.WriteLine($"  {e3}");

        // ── SEALED ────────────────────────────────────────────────────────────
        Console.WriteLine("\n--- sealed class (cannot be inherited) ---");
        var s1 = AppSingleton.Instance;
        var s2 = AppSingleton.Instance;
        Console.WriteLine($"  Same instance: {ReferenceEquals(s1, s2)}  Value={s1.Config}");

        // ── STATIC ───────────────────────────────────────────────────────────
        Console.WriteLine("\n--- static class ---");
        Console.WriteLine($"  CircleArea(5)      = {GeometryHelper.CircleArea(5):F4}");
        Console.WriteLine($"  HypotenusLength(3,4) = {GeometryHelper.Hypotenuse(3, 4):F4}");

        // ── OBJECT METHODS ─────────────────────────────────────────────────────
        Console.WriteLine("\n--- Object base methods (ToString, Equals, GetHashCode) ---");
        var p1 = new PersonValue("Alice", 30);
        var p2 = new PersonValue("Alice", 30);
        var p3 = new PersonValue("Bob",   25);
        Console.WriteLine($"  p1.ToString()       = {p1}");
        Console.WriteLine($"  p1.Equals(p2)       = {p1.Equals(p2)}  (overridden)");
        Console.WriteLine($"  p1.Equals(p3)       = {p1.Equals(p3)}");
        Console.WriteLine($"  p1.GetHashCode()    = {p1.GetHashCode()}");
        Console.WriteLine($"  p2.GetHashCode()    = {p2.GetHashCode()}  (same as p1)");

        // ── IDISPOSABLE & USING ────────────────────────────────────────────────
        Console.WriteLine("\n--- IDisposable & using ---");
        using (var resource = new ManagedResource("Connection"))
        {
            resource.DoWork();
        } // Dispose() called automatically here

        using var resource2 = new ManagedResource("FileHandle"); // C# 8 using declaration
        resource2.DoWork();
        // Dispose() called at end of enclosing scope
    }
}

// ─── ENCAPSULATION ───────────────────────────────────────────────────────────
public class Person
{
    private string _name;
    private int    _age;

    public Person(string name, int age) { _name = name; _age = age; }

    public string Name
    {
        get => _name;
        set => _name = string.IsNullOrWhiteSpace(value)
                       ? throw new ArgumentException("Name cannot be empty.")
                       : value;
    }

    public int Age
    {
        get => _age;
        set => _age = (value < 0 || value > 150)
                      ? throw new ArgumentOutOfRangeException(nameof(value), "Age must be 0-150.")
                      : value;
    }

    public override string ToString() => $"Person({_name}, {_age})";
}

// ─── INHERITANCE & POLYMORPHISM ───────────────────────────────────────────────
public abstract class Animal
{
    public string Name { get; }
    public int    Legs { get; protected set; }

    protected Animal(string name) => Name = name;

    public void Eat() => Console.WriteLine($"  {Name} is eating.");

    public abstract void Speak();                    // subclasses MUST implement
    public virtual string Describe() => $"I am {Name} with {Legs} legs.";
}

public class Dog : Animal
{
    public string Breed { get; }

    public Dog(string name, string breed) : base(name)
    {
        Breed = breed;
        Legs  = 4;
    }

    public override void Speak()         => Console.WriteLine($"  {Name} says: Woof!");
    public override string Describe()    => base.Describe() + $" Breed: {Breed}.";
}

public class Cat : Animal
{
    public Cat(string name) : base(name) => Legs = 4;
    public override void Speak() => Console.WriteLine($"  {Name} says: Meow!");
}

// ─── METHOD HIDING ────────────────────────────────────────────────────────────
public class Base
{
    public virtual string OverriddenMethod() => "Base.OverriddenMethod";
    public string         HiddenMethod()     => "Base.HiddenMethod";
}

public class Derived : Base
{
    public override string OverriddenMethod() => "Derived.OverriddenMethod";
    public new string      HiddenMethod()     => "Derived.HiddenMethod";
}

// ─── CONSTRUCTOR CHAINING ─────────────────────────────────────────────────────
public class Employee
{
    public string Name       { get; }
    public string Department { get; }
    public decimal Salary    { get; }

    public Employee(string name) : this(name, "Unassigned") { }
    public Employee(string name, string dept) : this(name, dept, 50000) { }
    public Employee(string name, string dept, decimal salary)
    {
        Name       = name;
        Department = dept;
        Salary     = salary;
    }

    public override string ToString() => $"Employee({Name}, {Department}, ${Salary:N0})";
}

// ─── SINGLETON (sealed) ───────────────────────────────────────────────────────
public sealed class AppSingleton
{
    private static AppSingleton? _instance;
    private static readonly object _lock = new();

    public string Config { get; } = "Production";

    private AppSingleton() { }

    public static AppSingleton Instance
    {
        get
        {
            if (_instance is null)
                lock (_lock)
                    _instance ??= new AppSingleton();
            return _instance;
        }
    }
}

// ─── STATIC CLASS ─────────────────────────────────────────────────────────────
public static class GeometryHelper
{
    public static double CircleArea(double r)       => Math.PI * r * r;
    public static double Hypotenuse(double a, double b) => Math.Sqrt(a * a + b * b);
}

// ─── OBJECT OVERRIDES ─────────────────────────────────────────────────────────
public class PersonValue
{
    public string Name { get; }
    public int    Age  { get; }

    public PersonValue(string name, int age) { Name = name; Age = age; }

    public override string ToString()   => $"PersonValue({Name}, {Age})";
    public override bool   Equals(object? obj)
        => obj is PersonValue other && Name == other.Name && Age == other.Age;
    public override int GetHashCode() => HashCode.Combine(Name, Age);
}

// ─── IDISPOSABLE ──────────────────────────────────────────────────────────────
public class ManagedResource : IDisposable
{
    private readonly string _name;
    private bool _disposed;

    public ManagedResource(string name)
    {
        _name = name;
        Console.WriteLine($"  [{_name}] Opened.");
    }

    public void DoWork()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        Console.WriteLine($"  [{_name}] Working...");
    }

    public void Dispose()
    {
        if (_disposed) return;
        Console.WriteLine($"  [{_name}] Disposed.");
        _disposed = true;
    }
}
