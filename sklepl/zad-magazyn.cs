using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

class Product
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public double UnitPrice { get; set; }

    public Product(string name, int quantity, double unitPrice)
    {
        Name = name;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public double GetValue() => Quantity * UnitPrice;
}

class Program
{
    private static List<Product> products = new List<Product>();
    private static string filePath = "products.json";

    static void Main()
    {
        LoadProducts();
        bool isRunning = true;

        while (isRunning)
        {
            Console.WriteLine("\nSystem zarządzania magazynem");
            Console.WriteLine("1. Dodaj produkt");
            Console.WriteLine("2. Usuń produkt");
            Console.WriteLine("3. Wyświetl listę produktów");
            Console.WriteLine("4. Aktualizuj produkt");
            Console.WriteLine("5. Oblicz wartość magazynu");
            Console.WriteLine("6. Wyjście");
            Console.Write("\nWybierz opcję: ");
            
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1: AddProduct(); break;
                    case 2: RemoveProduct(); break;
                    case 3: DisplayProducts(); break;
                    case 4: UpdateProduct(); break;
                    case 5: CalculateWarehouseValue(); break;
                    case 6: isRunning = false; SaveProducts(); break;
                    default: Console.WriteLine("Nieprawidłowa opcja!"); break;
                }
            }
            else
            {
                Console.WriteLine("Nieprawidłowa opcja! Wprowadź liczbę.");
            }
        }
    }

    static void AddProduct()
    {
        Console.Write("Podaj nazwę produktu: ");
        string name = Console.ReadLine();

        Console.Write("Podaj ilość: ");
        if (!int.TryParse(Console.ReadLine(), out int quantity))
        {
            Console.WriteLine("Błąd: Nieprawidłowa ilość.");
            return;
        }

        Console.Write("Podaj cenę jednostkową: ");
        if (!double.TryParse(Console.ReadLine(), out double unitPrice))
        {
            Console.WriteLine("Błąd: Nieprawidłowa cena.");
            return;
        }

        products.Add(new Product(name, quantity, unitPrice));
        Console.WriteLine("Produkt został dodany.");
    }

    static void RemoveProduct()
    {
        Console.Write("Podaj nazwę produktu do usunięcia: ");
        string name = Console.ReadLine();
        Product productToRemove = products.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        
        if (productToRemove != null)
        {
            products.Remove(productToRemove);
            Console.WriteLine("Produkt został usunięty.");
        }
        else Console.WriteLine("Nie znaleziono produktu.");
    }

    static void DisplayProducts()
    {
        if (products.Count == 0)
        {
            Console.WriteLine("Magazyn jest pusty.");
            return;
        }

        Console.WriteLine("\nLista produktów:");
        foreach (var product in products)
        {
            Console.WriteLine($"{product.Name}, Ilość: {product.Quantity}, Cena: {product.UnitPrice:C}");
        }
    }

    static void UpdateProduct()
    {
        Console.Write("Podaj nazwę produktu do aktualizacji: ");
        string name = Console.ReadLine();
        Product productToUpdate = products.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (productToUpdate != null)
        {
            Console.Write("Podaj nową ilość: ");
            if (!int.TryParse(Console.ReadLine(), out int newQuantity))
            {
                Console.WriteLine("Błąd: Nieprawidłowa ilość.");
                return;
            }
            productToUpdate.Quantity = newQuantity;

            Console.Write("Podaj nową cenę: ");
            if (!double.TryParse(Console.ReadLine(), out double newPrice))
            {
                Console.WriteLine("Błąd: Nieprawidłowa cena.");
                return;
            }
            productToUpdate.UnitPrice = newPrice;

            Console.WriteLine("Produkt zaktualizowany.");
        }
        else Console.WriteLine("Nie znaleziono produktu.");
    }

    static void CalculateWarehouseValue()
    {
        double totalValue = products.Sum(p => p.GetValue());
        Console.WriteLine($"Całkowita wartość magazynu: {totalValue:C}");
    }

    static void SaveProducts()
    {
        string json = JsonSerializer.Serialize(products);
        File.WriteAllText(filePath, json);
    }

    static void LoadProducts()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            products = JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
        }
    }
}
