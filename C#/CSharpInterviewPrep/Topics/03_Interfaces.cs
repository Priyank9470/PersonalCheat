namespace CSharpInterviewPrep.Topics;

public static class InterfacesDemo
{
    public static void Run()
    {
        Console.WriteLine("=== INTERFACES & ABSTRACT CLASSES ===\n");

        // ── INTERFACE (contract — what to do) ─────────────────────────────────
        Console.WriteLine("--- Interface ---");
        IShape circle    = new Circle(5);
        IShape rectangle = new Rectangle(4, 6);
        IShape triangle  = new Triangle(3, 4, 5);

        foreach (var shape in new[] { circle, rectangle, triangle })
            Console.WriteLine($"  {shape.ShapeType,-12} Area={shape.Area():F2}  Perim={shape.Perimeter():F2}");

        // ── MULTIPLE INTERFACES ────────────────────────────────────────────────
        Console.WriteLine("\n--- Multiple Interface Implementation ---");
        var duck = new Duck();
        duck.Fly();
        duck.Swim();
        duck.Quack();

        // ── ABSTRACT CLASS (partial implementation) ────────────────────────────
        Console.WriteLine("\n--- Abstract Class ---");
        // Abstract class: CAN have fields, constructors, concrete methods, state
        // Interface: contract only (C# 8+ can have default implementations)
        Vehicle[] vehicles = { new Car("Tesla Model S"), new Bicycle(), new Truck("Volvo") };
        foreach (var v in vehicles)
            v.StartJourney();

        // ── INTERFACE vs ABSTRACT CLASS ────────────────────────────────────────
        Console.WriteLine("\n--- Interface vs Abstract Class (summary) ---");
        Console.WriteLine("  Interface        : no state, multiple allowed, 'can-do' relationship");
        Console.WriteLine("  Abstract class   : can have state/constructors, single inheritance, 'is-a'");
        Console.WriteLine("  Use interface    : when unrelated classes share a capability (IDisposable)");
        Console.WriteLine("  Use abstract     : when classes share code AND have a common base type");

        // ── DEFAULT INTERFACE METHOD (C# 8+) ──────────────────────────────────
        Console.WriteLine("\n--- Default Interface Implementation (C# 8+) ---");
        ILogger consoleLog = new ConsoleLogger();
        ILogger fileLog    = new FileLogger();
        consoleLog.Log("App started");
        consoleLog.LogWarning("Low memory");   // uses default
        fileLog.Log("Writing to file");
        fileLog.LogWarning("Disk almost full"); // FileLogger overrides it

        // ── EXPLICIT INTERFACE IMPLEMENTATION ─────────────────────────────────
        Console.WriteLine("\n--- Explicit Interface Implementation ---");
        var printer = new MultiPrinter();
        ((IColorPrinter)printer).Print();
        ((IMonoPrinter)printer).Print();
        // printer.Print()  ← compile error: ambiguous

        // ── INTERFACE SEGREGATION (ISP from SOLID) ────────────────────────────
        Console.WriteLine("\n--- Interface Segregation Principle ---");
        IReadable  reader  = new FileStore("data.txt");
        IWriteable writer  = new FileStore("data.txt");
        reader.Read();
        writer.Write("Hello");
        // Instead of one fat interface, split into small focused ones
    }
}

// ─── INTERFACES ───────────────────────────────────────────────────────────────
public interface IShape
{
    double Area();
    double Perimeter();
    string ShapeType => GetType().Name;  // default property (C# 8+)
}

public class Circle : IShape
{
    private readonly double _r;
    public Circle(double r) => _r = r;
    public double Area()      => Math.PI * _r * _r;
    public double Perimeter() => 2 * Math.PI * _r;
}

public class Rectangle : IShape
{
    private readonly double _w, _h;
    public Rectangle(double w, double h) { _w = w; _h = h; }
    public double Area()      => _w * _h;
    public double Perimeter() => 2 * (_w + _h);
}

public class Triangle : IShape
{
    private readonly double _a, _b, _c;
    public Triangle(double a, double b, double c) { _a = a; _b = b; _c = c; }
    public double Area()
    {
        double s = (_a + _b + _c) / 2;
        return Math.Sqrt(s * (s - _a) * (s - _b) * (s - _c));
    }
    public double Perimeter() => _a + _b + _c;
}

// ─── MULTIPLE INTERFACES ──────────────────────────────────────────────────────
public interface IFlyable  { void Fly(); }
public interface ISwimmable { void Swim(); }

public class Duck : IFlyable, ISwimmable
{
    public void Fly()   => Console.WriteLine("  Duck is flying.");
    public void Swim()  => Console.WriteLine("  Duck is swimming.");
    public void Quack() => Console.WriteLine("  Duck: Quack!");
}

// ─── ABSTRACT CLASS ───────────────────────────────────────────────────────────
public abstract class Vehicle
{
    protected string Name;
    public int MaxSpeed { get; protected set; }

    protected Vehicle(string name) => Name = name;

    public abstract void Move();      // subclasses MUST override

    public void StartJourney()        // shared concrete logic
    {
        Console.Write($"  {Name,-16} → ");
        Move();
    }
}

public class Car : Vehicle
{
    public Car(string name) : base(name) => MaxSpeed = 200;
    public override void Move() => Console.WriteLine($"Driving at up to {MaxSpeed} km/h.");
}

public class Bicycle : Vehicle
{
    public Bicycle() : base("Bicycle") => MaxSpeed = 30;
    public override void Move() => Console.WriteLine($"Pedaling at up to {MaxSpeed} km/h.");
}

public class Truck : Vehicle
{
    public Truck(string name) : base(name) => MaxSpeed = 120;
    public override void Move() => Console.WriteLine($"Hauling freight at up to {MaxSpeed} km/h.");
}

// ─── DEFAULT INTERFACE METHOD ─────────────────────────────────────────────────
public interface ILogger
{
    void Log(string message);
    void LogWarning(string message) =>    // default implementation
        Console.WriteLine($"  [WARN]  {message}");
    void LogError(string message) =>
        Console.WriteLine($"  [ERROR] {message}");
}

public class ConsoleLogger : ILogger
{
    public void Log(string message) => Console.WriteLine($"  [INFO]  {message}");
    // LogWarning and LogError use defaults
}

public class FileLogger : ILogger
{
    public void Log(string message)     => Console.WriteLine($"  [FILE-INFO] {message}");
    public void LogWarning(string msg)  => Console.WriteLine($"  [FILE-WARN] {msg}");  // override default
}

// ─── EXPLICIT INTERFACE IMPLEMENTATION ────────────────────────────────────────
public interface IColorPrinter { void Print(); }
public interface IMonoPrinter  { void Print(); }

public class MultiPrinter : IColorPrinter, IMonoPrinter
{
    void IColorPrinter.Print() => Console.WriteLine("  Printing in COLOUR.");
    void IMonoPrinter.Print()  => Console.WriteLine("  Printing in BLACK & WHITE.");
}

// ─── INTERFACE SEGREGATION ────────────────────────────────────────────────────
public interface IReadable  { void Read(); }
public interface IWriteable { void Write(string data); }

public class FileStore : IReadable, IWriteable
{
    private readonly string _path;
    public FileStore(string path) => _path = path;
    public void Read()              => Console.WriteLine($"  Reading from {_path}");
    public void Write(string data)  => Console.WriteLine($"  Writing '{data}' to {_path}");
}
