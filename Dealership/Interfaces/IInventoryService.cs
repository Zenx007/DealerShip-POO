using Dealership.Models;

namespace Dealership.Interfaces;

public interface IInventoryService
{
    List<Vehicle> GetAll();
    Vehicle GetByIndex(int index);
    void AddNewProduct(string type, string name, double price, int quantity);
    void Restock(string name, string type, int quantity);
}