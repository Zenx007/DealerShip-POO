using Dealership.Models;

namespace Dealership.Interfaces;

public interface ICartService
{
    List<Vehicle> GetItems();
    void AddItem(Vehicle inventoryVehicle, int desiredQuantity);
    void RemoveItem(int index, int quantityToRemove);
    void Checkout();
}