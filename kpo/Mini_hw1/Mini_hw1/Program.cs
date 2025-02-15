using Microsoft.Extensions.DependencyInjection;

public interface IAlive
{
    int food { get; }
}

public interface IInventory
{
    int number { get; }
}

// Базовые классы
public abstract class Animal : IAlive, IInventory
{
    public int food { get; set; }
    public int number { get; }
    public string name { get; }

    public Animal(string name, int food, int number)
    {
        this.name = name;
        this.food = food;
        this.number = number;
    }
}

public abstract class Herbo : Animal
{
    public int kindness { get; }

    protected Herbo(string name, int food, int number, int kindness) : base(name, food, number)
    {
        this.kindness = kindness;
    }
}

public abstract class Predator : Animal
{
    protected Predator(string name, int food, int number) : base(name, food, number) { }
}

public class Monkey : Herbo
{
    public Monkey(int number) : base("Monkey", 5, number, 6) { }
}

public class Rabbit : Herbo
{
    public Rabbit(int number) : base("Rabbit", 3, number, 7) { }
}

public class Tiger : Predator
{
    public Tiger(int number) : base("Tiger", 10, number) { }
}

public class Wolf : Predator
{
    public Wolf(int number) : base("Wolf", 8, number) { }
}

public class VeterinaryClinic
{
    public bool CheckHealth(Animal animal)
    {
        return new Random().Next(0, 2) == 1;
    }
}

public class Thing : IInventory
{
    public int number { get; }
    public string name { get; }

    public Thing(string name, int number)
    {
        this.name = name;
        this.number = number;
    }
}

public class Table : Thing
{
    public Table(int number) : base("Table", number) { }
}

public class Computer : Thing
{
    public Computer(int number) : base("Computer", number) { }
}

// Зоопарк
public class Zoo
{
    private  VeterinaryClinic _clinic;
    private  List<Animal> _animals = new();
    private  List<IInventory> _inventory = new();

    public Zoo(VeterinaryClinic clinic)
    {
        _clinic = clinic;
    }

    public void AddAnimal(Animal animal)
    {
        if (_clinic.CheckHealth(animal))
        {
            _animals.Add(animal);
            _inventory.Add(animal);
            Console.WriteLine($"{animal.name} прошел медосмотр и принят в зоопарк");
        }
        else
        {
            Console.WriteLine($"{animal.name} не прошел медосмотр");
        }
    }

    public void AddThing(IInventory thing)
    {
        _inventory.Add(thing);
        Console.WriteLine($"{thing.GetType()} добавлен в инвентарь");
    }

    public void PrintFoodReport()
    {
        int totalFood = 0;
        foreach (var a in _animals)
        {
            totalFood += a.food;
        }
        Console.WriteLine($"\n{totalFood}кг. еды потребляют животные в сутки\n");
    }

    public void PrintContactZooAnimals()
    {
        Console.WriteLine("Список травоядных животных пригодных для контактного зоопарка:");
        int count = 1;
        foreach (var a in _animals)
        {
            if (a is Herbo herbo && herbo.kindness > 5)
            {
                Console.WriteLine($"{count++}. {a.name}");
            }
        }
        Console.WriteLine();
    }

    public void PrintInventory()
    {
        Console.WriteLine("Наименование и инвентаризационные номера вещей и животных, стоящих на балансе зоопарка:");
        foreach (var item in _inventory)
        {
            Console.WriteLine($"{item.GetType()}, id: {item.number}");
        }
        Console.WriteLine();
    }
}

class Program
{
    static void Main()
    {
        var services = new ServiceCollection();
        services.AddSingleton<VeterinaryClinic>();
        services.AddSingleton<Zoo>();
        var provider = services.BuildServiceProvider();

        var zoo = provider.GetRequiredService<Zoo>();

        zoo.AddAnimal(new Monkey(0));
        zoo.AddAnimal(new Rabbit(1));
        zoo.AddAnimal(new Tiger(2));
        zoo.AddAnimal(new Wolf(3));
        zoo.AddAnimal(new Monkey(4));
        zoo.AddAnimal(new Rabbit(5));
        zoo.AddAnimal(new Wolf(6));

        zoo.AddThing(new Table(1000));
        zoo.AddThing(new Computer(1001));
        zoo.AddThing(new Computer(1002));

        zoo.PrintFoodReport();
        zoo.PrintContactZooAnimals();
        zoo.PrintInventory();
    }
}
