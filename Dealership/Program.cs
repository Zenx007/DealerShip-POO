
using Dealership.Interfaces;
using Dealership.Services;
using Dealership.UI;
using Shop.Services;

class Program
{
    static void Main(string[] args)
    {
        IFileService fileService = new FileService();
        IInventoryService inventoryService = new InventoryService();
        
        ICartService cartService = new CartService(fileService, inventoryService);
        
        UserInterface app = new UserInterface(inventoryService, cartService, fileService);

        app.ShowMainMenu();
    }
}