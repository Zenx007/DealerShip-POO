using Dealership.Interfaces;
using Dealership.Models;

namespace Dealership.Services;

public class FileService : IFileService
{
    private readonly string _cartPath = "carrinho.txt";
    private readonly string _salesPath = "vendas.txt";

    public void SaveCart(List<Vehicle> cart)
    {
        using (StreamWriter sw = new StreamWriter(_cartPath))
        {
            foreach (Vehicle v in cart)
            {
                sw.WriteLine($"{v.Type} | {v.Name} - R$ {v.Price:N2} x {v.Quantity}");
            }
        }
    }

    public void ClearCartFile()
    {
        File.WriteAllText(_cartPath, string.Empty);
    }

    public void RegisterSale(List<Vehicle> soldItems, double total)
    {
        using (StreamWriter sw = new StreamWriter(_salesPath, true))
        {
            sw.WriteLine($"Venda realizada em {DateTime.Now}");
            foreach (Vehicle v in soldItems)
            {
                sw.WriteLine($"{v.Type} | {v.Name} - R$ {v.Price:N2} x {v.Quantity} = R$ {(v.Price * v.Quantity):N2}");
            }
            sw.WriteLine($"Total: R$ {total:N2}");
            sw.WriteLine("-----------------------------");
        }
    }

    public void ReadSalesHistory()
    {
        if (!File.Exists(_salesPath))
        {
            Console.WriteLine("Nenhuma venda registrada ainda.");
            return;
        }

        string[] lines = File.ReadAllLines(_salesPath);
        if (lines.Length == 0)
        {
            Console.WriteLine("Nenhuma venda registrada ainda.");
        }
        else
        {
            foreach (string line in lines)
            {
                Console.WriteLine(line);
            }
        }
    }
}