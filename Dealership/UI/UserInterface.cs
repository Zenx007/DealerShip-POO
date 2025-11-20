using Dealership.Interfaces;
using Dealership.Models;

namespace Dealership.UI;

public class UserInterface
    {
        private readonly IInventoryService _inventoryService;
        private readonly ICartService _cartService;
        private readonly IFileService _fileService;

        public UserInterface(IInventoryService inventoryService, ICartService cartService, IFileService fileService)
        {
            _inventoryService = inventoryService;
            _cartService = cartService;
            _fileService = fileService;
        }

        public void ShowMainMenu()
        {
            int option;
            do
            {
                Console.Clear();
                PrintLogo();
                
                Console.WriteLine("\n  Gerenciamento de Vendas");
                Console.WriteLine("  -----------------------");
                PrintOption(1, "Listar Carros");
                PrintOption(2, "Listar Motos");
                PrintOption(3, "Adicionar ao Carrinho");
                PrintOption(4, "Ver Carrinho");
                PrintOption(5, "Finalizar Compra");
                Console.WriteLine("  -----------------------");
                PrintOption(6, "Adicionar Produto (Estoque)");
                PrintOption(7, "Histórico de Vendas");
                PrintOption(8, "Remover do Carrinho");
                Console.WriteLine("  -----------------------");
                PrintOption(0, "Sair");

                Console.WriteLine();
                Console.Write("  >> Escolha uma opção: ");
                
                if (!int.TryParse(Console.ReadLine(), out option))
                {
                    option = -1;
                }

                ProcessOption(option);

                if (option != 0)
                {
                    WriteFooter("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                }

            } while (option != 0);
        }

        private void ProcessOption(int option)
        {
            switch (option)
            {
                case 1:
                    ListVehiclesByType("Carro");
                    break;
                case 2:
                    ListVehiclesByType("Moto");
                    break;
                case 3:
                    MenuAddToCart();
                    break;
                case 4:
                    MenuViewCart();
                    break;
                case 5:
                    CheckoutProcess();
                    break;
                case 6:
                    MenuAddProductToInventory();
                    break;
                case 7:
                    ShowSalesHistory();
                    break;
                case 8:
                    MenuRemoveFromCart();
                    break;
                case 0:
                    WriteSuccess("Obrigado por usar o sistema. Até logo!");
                    Thread.Sleep(1000);
                    break;
                default:
                    WriteError("Opção inválida! Tente novamente.");
                    break;
            }
        }

        private void ListVehiclesByType(string type)
        {
            Console.Clear();
            WriteTitle($"CATÁLOGO DE {type.ToUpper()}S");
            
            List<Vehicle> list = _inventoryService.GetAll();
            bool found = false;
            int i = 1;

            Console.WriteLine($"{"ID",-5} | {"MODELO",-20} | {"PREÇO",-15} | {"ESTOQUE",-10}");
            Console.WriteLine(new string('-', 60));

            foreach (Vehicle v in list)
            {
                if (v.Type == type)
                {
                    // Formatação de tabela alinhada
                    string priceFormatted = $"R$ {v.Price:N2}";
                    Console.WriteLine($"{i,-5} | {v.Name,-20} | {priceFormatted,-15} | {v.Quantity,-10}");
                    found = true;
                }
                i++;
            }

            if (!found) WriteError($"Nenhum(a) {type} encontrado(a) no estoque.");
        }

        private void MenuAddToCart()
        {
            Console.Clear();
            WriteTitle("ADICIONAR AO CARRINHO");
            
            // Reutiliza a visualização do estoque completo
            List<Vehicle> list = _inventoryService.GetAll();
            
            for (int i = 0; i < list.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"[{i + 1}] ");
                Console.ResetColor();
                
                if (list[i].Quantity > 0)
                    Console.WriteLine($"{list[i].Name} (R$ {list[i].Price:N2}) - Restam: {list[i].Quantity}");
                else
                    Console.WriteLine($"{list[i].Name} - [ESGOTADO]");
            }

            Console.WriteLine();
            Console.Write("  Digite o ID do veículo: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= list.Count)
            {
                Vehicle selected = _inventoryService.GetByIndex(choice - 1);

                if (selected.Quantity == 0)
                {
                    WriteError("Este veículo está esgotado!");
                    return;
                }

                Console.Write($"  Quantas unidades de {selected.Name}? ");
                if (int.TryParse(Console.ReadLine(), out int qty) && qty > 0)
                {
                    _cartService.AddItem(selected, qty);
                }
                else
                {
                    WriteError("Quantidade inválida.");
                }
            }
            else
            {
                WriteError("Seleção inválida.");
            }
        }

        private void MenuViewCart()
        {
            Console.Clear();
            WriteTitle("SEU CARRINHO DE COMPRAS");
            
            List<Vehicle> items = _cartService.GetItems();

            if (items.Count == 0)
            {
                Console.WriteLine("  O carrinho está vazio.");
                return;
            }

            double total = 0;
            int i = 1;
            foreach (Vehicle v in items)
            {
                double subtotal = v.Price * v.Quantity;
                Console.WriteLine($"  {i}. {v.Name}");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"     {v.Quantity}x R$ {v.Price:N2} = R$ {subtotal:N2}");
                Console.ResetColor();
                total += subtotal;
                i++;
            }
            
            Console.WriteLine(new string('-', 40));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  TOTAL GERAL: R$ {total:N2}");
            Console.ResetColor();
        }

        private void CheckoutProcess()
        {
            MenuViewCart(); // Mostra o resumo antes
            List<Vehicle> items = _cartService.GetItems();
            
            if (items.Count == 0) return;

            Console.WriteLine();
            Console.Write("  Confirma a compra? (S/N): ");
            string confirm = Console.ReadLine();

            if (confirm?.ToUpper() == "S")
            {
                Console.WriteLine("  Processando pagamento...");
                Thread.Sleep(1500); 
                _cartService.Checkout();
                WriteSuccess("Pagamento aprovado! Nota fiscal gerada.");
            }
            else
            {
                WriteInfo("Compra cancelada. Os itens continuam no carrinho.");
            }
        }

        private void MenuRemoveFromCart()
        {
            Console.Clear();
            WriteTitle("REMOVER ITENS");
            
            List<Vehicle> items = _cartService.GetItems();
            if (items.Count == 0)
            {
                WriteInfo("O carrinho está vazio.");
                return;
            }

            for (int i = 0; i < items.Count; i++)
            {
                Console.WriteLine($"  [{i + 1}] {items[i].Name} (Qtd: {items[i].Quantity})");
            }

            Console.Write("\n  Qual item remover? ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= items.Count)
            {
                Console.Write("  Quantas unidades devolver? ");
                if (int.TryParse(Console.ReadLine(), out int qty) && qty > 0)
                {
                    _cartService.RemoveItem(choice - 1, qty);
                    WriteSuccess("Carrinho atualizado.");
                }
                else
                {
                    WriteError("Quantidade inválida.");
                }
            }
            else
            {
                WriteError("Opção inválida.");
            }
        }

        private void MenuAddProductToInventory()
        {
            Console.Clear();
            WriteTitle("CADASTRAR NOVO PRODUTO");

            Console.Write("  Tipo (Carro/Moto): ");
            string type = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(type) || (type.ToLower() != "carro" && type.ToLower() != "moto"))
            {
                WriteError("Tipo inválido! Use 'Carro' ou 'Moto'.");
                return;
            }
            
            string formattedType = char.ToUpper(type[0]) + type.Substring(1).ToLower();

            Console.Write("  Modelo/Nome: ");
            string name = Console.ReadLine();

            Console.Write("  Preço (R$): ");
            if (!double.TryParse(Console.ReadLine(), out double price) || price <= 0)
            {
                WriteError("Preço inválido.");
                return;
            }

            Console.Write("  Quantidade Inicial: ");
            if (!int.TryParse(Console.ReadLine(), out int qty) || qty < 0)
            {
                WriteError("Quantidade inválida.");
                return;
            }

            _inventoryService.AddNewProduct(formattedType, name, price, qty);
            WriteSuccess("Produto cadastrado com sucesso!");
        }

        private void ShowSalesHistory()
        {
            Console.Clear();
            WriteTitle("HISTÓRICO DE VENDAS");
            Console.ForegroundColor = ConsoleColor.Yellow;
            _fileService.ReadSalesHistory();
            Console.ResetColor();
        }

        // --- Helper Methods (Visuals) ---

        private void PrintLogo()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
   ______  __  __  ____  
  / ____/ /  |/  |/ __ \ 
 / /     / /|_/ // / / / 
/ /___  / /  / // /_/ /  
\____/ /_/  /_//_____/   
    CONCESSIONÁRIA");
            Console.ResetColor();
        }

        private void PrintOption(int number, string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"  {number}. ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        private void WriteTitle(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"=== {title} ===");
            Console.WriteLine();
            Console.ResetColor();
        }

        private void WriteSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n  [SUCESSO] {message}");
            Console.ResetColor();
        }

        private void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n  [ERRO] {message}");
            Console.ResetColor();
        }

        private void WriteInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"\n  [INFO] {message}");
            Console.ResetColor();
        }

        private void WriteFooter(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"\n{message}");
            Console.ResetColor();
        }
    }