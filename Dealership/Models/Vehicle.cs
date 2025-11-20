namespace Dealership.Models;

public class Vehicle
{
    public string Type { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }

    public Vehicle(string type, string name, double price, int quantity)
    {
        Type = type;
        Name = name;
        Price = price;
        Quantity = quantity;
    }

    public override string ToString()
    {
        return $"{Type} | {Name} - $ {Price:N2}";
    }
}