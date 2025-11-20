using Dealership.Interfaces;
using Dealership.Models;

namespace Dealership.Services;

public class CartService : ICartService
    {
        private List<Vehicle> _cart;
        private readonly IFileService _fileService;
        private readonly IInventoryService _inventoryService;

        public CartService(IFileService fileService, IInventoryService inventoryService)
        {
            _cart = new List<Vehicle>();
            _fileService = fileService;
            _inventoryService = inventoryService;
        }

        public List<Vehicle> GetItems()
        {
            return _cart;
        }

        public void AddItem(Vehicle inventoryVehicle, int desiredQuantity)
        {
            if (inventoryVehicle.Quantity >= desiredQuantity)
            {
                _cart.Add(new Vehicle(inventoryVehicle.Type, inventoryVehicle.Name, inventoryVehicle.Price, desiredQuantity));
                inventoryVehicle.Quantity -= desiredQuantity;
                
                Console.WriteLine($"{desiredQuantity} unidade(s) de {inventoryVehicle.Name} adicionadas ao carrinho.");
                _fileService.SaveCart(_cart);
            }
            else
            {
                Console.WriteLine("Estoque insuficiente!");
            }
        }

        public void RemoveItem(int index, int quantityToRemove)
        {
            Vehicle cartItem = _cart[index];

            if (quantityToRemove >= cartItem.Quantity)
            {
                _inventoryService.Restock(cartItem.Name, cartItem.Type, cartItem.Quantity);
                _cart.RemoveAt(index);
                Console.WriteLine($"{cartItem.Name} removido completamente do carrinho.");
            }
            else
            {
                cartItem.Quantity -= quantityToRemove;
                _inventoryService.Restock(cartItem.Name, cartItem.Type, quantityToRemove);
                Console.WriteLine($"{quantityToRemove} unidade(s) de {cartItem.Name} removidas do carrinho.");
            }

            _fileService.SaveCart(_cart);
        }

        public void Checkout()
        {
            if (_cart.Count == 0)
            {
                Console.WriteLine("Carrinho vazio, impossível finalizar!");
                return;
            }

            double total = 0;
            foreach (Vehicle v in _cart)
            {
                Console.WriteLine($"{v.Type} | {v.Name} - R$ {v.Price:N2} x {v.Quantity} = R$ {(v.Price * v.Quantity):N2}");
                total += v.Price * v.Quantity;
            }

            Console.WriteLine($"\nTotal: R$ {total:N2}");
            Console.WriteLine("Compra finalizada com sucesso!");

            _fileService.RegisterSale(_cart, total);
            _cart.Clear();
            _fileService.ClearCartFile();
        }
    }