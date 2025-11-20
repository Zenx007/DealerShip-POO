using Dealership.Models;

namespace Dealership.Interfaces;

public interface IFileService
{
    void SaveCart(List<Vehicle> cart);
    void ClearCartFile();
    void RegisterSale(List<Vehicle> soldItems, double total);
    void ReadSalesHistory();
}