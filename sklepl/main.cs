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

    public double GetValue()
    {
        return Quantity * UnitPrice;
    }
}

class Transaction
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public double TotalPrice { get; set; }
    public string DiscountCode { get; set; }

    public Transaction(int id, string productName, int quantity, double totalPrice, string discountCode)
    {
        Id = id;
        ProductName = productName;
        Quantity = quantity;
        TotalPrice = totalPrice;
        DiscountCode = discountCode;
    }
}

class Program
{
    private static List<Product> products = new List<Product>();
    private static List<Transaction> transactions = new List<Transaction>();
    private static string filePath = "products.json";
    private static int transactionCounter = 1;

    static void Main(string[] args)
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
            Console.WriteLine("6. Sprzedaj produkt");
            Console.WriteLine("7. Zwrot produktu");
            Console.WriteLine("8. Dostawa");
            Console.WriteLine("9. Wyjście");
            Console.Write("\nWybierz opcję: ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int choice))
            {
                if (choice == 1) AddProduct();
                else if (choice == 2) RemoveProduct();
                else if (choice == 3) DisplayProducts();
                else if (choice == 4) UpdateProduct();
                else if (choice == 5) CalculateWarehouseValue();
                else if (choice == 6) SellProduct();
                else if (choice == 7) ReturnProduct();
                else if (choice == 8) Delivery();
                else if (choice == 9) { isRunning = false; SaveProducts(); }
                else Console.WriteLine("Nieprawidłowa opcja!");
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
        int quantity = int.Parse(Console.ReadLine());

        Console.Write("Podaj cenę jednostkową: ");
        double unitPrice = double.Parse(Console.ReadLine());

        products.Add(new Product(name, quantity, unitPrice));
        Console.WriteLine("Produkt został dodany.");
    }

    static void RemoveProduct()
    {
        Console.Write("Podaj nazwę produktu do usunięcia: ");
        string name = Console.ReadLine();
        Product productToRemove = products.Find(p => p.Name == name);
        if (productToRemove != null)
        {
            products.Remove(productToRemove);
            Console.WriteLine("Produkt został usunięty.");
        }
        else Console.WriteLine("Nie znaleziono produktu.");
    }

    static void DisplayProducts()
    {
        foreach (var product in products)
        {
            Console.WriteLine($"{product.Name}, Ilość: {product.Quantity}, Cena: {product.UnitPrice}");
        }
    }

    static void UpdateProduct()
    {
        Console.Write("Podaj nazwę produktu do aktualizacji: ");
        string name = Console.ReadLine();
        Product productToUpdate = products.Find(p => p.Name == name);
        if (productToUpdate != null)
        {
            Console.Write("Podaj nową ilość: ");
            productToUpdate.Quantity = int.Parse(Console.ReadLine());

            Console.Write("Podaj nową cenę: ");
            productToUpdate.UnitPrice = double.Parse(Console.ReadLine());

            Console.WriteLine("Produkt zaktualizowany.");
        }
        else Console.WriteLine("Nie znaleziono produktu.");
    }

    static void CalculateWarehouseValue()
    {
        double totalValue = products.Sum(p => p.GetValue());
        Console.WriteLine($"Całkowita wartość magazynu: {totalValue}");
    }

    static void SellProduct()
    {
        Console.WriteLine("Opcja sprzedaży jeszcze nie jest dostępna.");
    }

    static void ReturnProduct()
    {
        Console.WriteLine("Opcja zwrotu jeszcze nie jest dostępna.");
    }

    static void Delivery()
    {
        Console.WriteLine("Opcja dostawy jeszcze nie jest dostępna.");
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
            products = JsonSerializer.Deserialize<List<Product>>(json);
        }
    }
}
