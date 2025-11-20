using Dealership.Interfaces;
using Dealership.Models;

namespace Shop.Services
{
    public class InventoryService : IInventoryService
    {
        private List<Vehicle> _inventory;

        public InventoryService()
        {
            _inventory = new List<Vehicle>();
            InitializeDefaultInventory();
        }

        private void InitializeDefaultInventory()
        {
            _inventory.Add(new Vehicle("Carro", "Chevrolet Onix", 75000, 5));
            _inventory.Add(new Vehicle("Carro", "Toyota Corolla", 130000, 3));
            _inventory.Add(new Vehicle("Carro", "Honda Civic", 120000, 4));
            _inventory.Add(new Vehicle("Moto", "Honda CG 160", 15000, 10));
            _inventory.Add(new Vehicle("Moto", "Yamaha Fazer 250", 23000, 7));
            _inventory.Add(new Vehicle("Moto", "BMW GS 850", 62000, 2));
        }

        public List<Vehicle> GetAll()
        {
            return _inventory;
        }

        public Vehicle GetByIndex(int index)
        {
            if (index >= 0 && index < _inventory.Count)
            {
                return _inventory[index];
            }
            return null;
        }

        public void AddNewProduct(string type, string name, double price, int quantity)
        {
            _inventory.Add(new Vehicle(type, name, price, quantity));
        }

        public void Restock(string name, string type, int quantity)
        {
            Vehicle originalItem = _inventory.Find(v => v.Name == name && v.Type == type);
            if (originalItem != null)
            {
                originalItem.Quantity += quantity;
            }
        }
    }
}