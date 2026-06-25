using CSharpInterviewPrep.Topics;

while (true)
{
    Console.Clear();
    Console.WriteLine("╔══════════════════════════════════════════════╗");
    Console.WriteLine("║       C# Interview Preparation Guide         ║");
    Console.WriteLine("╚══════════════════════════════════════════════╝");
    Console.WriteLine();
    Console.WriteLine("  1.  Data Types & Variables");
    Console.WriteLine("  2.  OOP — Classes, Inheritance & Polymorphism");
    Console.WriteLine("  3.  Interfaces & Abstract Classes");
    Console.WriteLine("  4.  Generics");
    Console.WriteLine("  5.  Collections");
    Console.WriteLine("  6.  LINQ");
    Console.WriteLine("  7.  Delegates, Func, Action & Events");
    Console.WriteLine("  8.  Async / Await & Tasks");
    Console.WriteLine("  9.  Exception Handling");
    Console.WriteLine("  10. Modern C# (Records, Pattern Matching, Tuples)");
    Console.WriteLine("  0.  Exit");
    Console.WriteLine();
    Console.Write("Enter topic number: ");

    var input = Console.ReadLine();
    Console.Clear();

    switch (input)
    {
        case "1":  DataTypesDemo.Run();         break;
        case "2":  OOPDemo.Run();               break;
        case "3":  InterfacesDemo.Run();        break;
        case "4":  GenericsDemo.Run();          break;
        case "5":  CollectionsDemo.Run();       break;
        case "6":  LinqDemo.Run();              break;
        case "7":  DelegatesEventsDemo.Run();   break;
        case "8":  AsyncAwaitDemo.Run();        break;
        case "9":  ExceptionHandlingDemo.Run(); break;
        case "10": ModernCSharpDemo.Run();      break;
        case "0":  return;
        default:
            Console.WriteLine("Invalid option. Press any key to try again.");
            Console.ReadKey();
            continue;
    }

    Console.WriteLine();
    Console.WriteLine("─────────────────────────────────────────────");
    Console.WriteLine("Press any key to return to menu...");
    Console.ReadKey();
}
