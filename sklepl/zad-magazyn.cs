using System; // biblioteka do obsługi konsoli i podstawowych funkcji
using System.Collections.Generic; // potrzebne do listy produktów
using System.IO; // do zapisu i odczytu pliku
using System.Linq; // umożliwia korzystanie z LINQ, np. FirstOrDefault, Sum
using System.Text.Json; // do serializacji (zapis/odczyt JSON)

class Product // klasa reprezentująca produkt w magazynie
{
    public string Name { get; set; } // nazwa produktu
    public int Quantity { get; set; } // ilość sztuk
    public double UnitPrice { get; set; } // cena za sztukę

    public Product(string name, int quantity, double unitPrice) // konstruktor ustawiający dane produktu
    {
        Name = name;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public double GetValue() => Quantity * UnitPrice; // metoda licząca wartość danego produktu (ilość * cena)
}

class Program // główna klasa programu
{
    private static List<Product> products = new List<Product>(); // lista przechowująca wszystkie produkty
    private static string filePath = "products.json"; // ścieżka do pliku, gdzie zapisujemy produkty

    static void Main() // główna metoda uruchamiająca program
    {
        LoadProducts(); // wczytanie danych z pliku na start
        bool isRunning = true; // zmienna sterująca pętlą

        while (isRunning) // pętla główna programu
        {
            // menu programu
            Console.WriteLine("\nSystem zarządzania magazynem");
            Console.WriteLine("1. Dodaj produkt");
            Console.WriteLine("2. Usuń produkt");
            Console.WriteLine("3. Wyświetl listę produktów");
            Console.WriteLine("4. Aktualizuj produkt");
            Console.WriteLine("5. Oblicz wartość magazynu");
            Console.WriteLine("6. Wyjście");
            Console.Write("\nWybierz opcję: ");

            // wczytywanie opcji od użytkownika
            if (int.TryParse(Console.ReadLine(), out int choice)) // próba konwersji wpisu na liczbę
            {
                switch (choice) // wykonanie odpowiedniej akcji w zależności od wyboru
                {
                    case 1: AddProduct(); break; // dodanie produktu
                    case 2: RemoveProduct(); break; // usunięcie produktu
                    case 3: DisplayProducts(); break; // wyświetlenie listy
                    case 4: UpdateProduct(); break; // aktualizacja
                    case 5: CalculateWarehouseValue(); break; // obliczenie wartości magazynu
                    case 6: isRunning = false; SaveProducts(); break; // zakończenie programu i zapis danych
                    default: Console.WriteLine("Nieprawidłowa opcja!"); break; // nieznana opcja
                }
            }
            else
            {
                Console.WriteLine("Nieprawidłowa opcja! Wprowadź liczbę."); // błąd przy wpisaniu opcji
            }
        }
    }

    static void AddProduct() // metoda dodająca produkt
    {
        Console.Write("Podaj nazwę produktu: ");
        string name = Console.ReadLine(); // wczytanie nazwy

        Console.Write("Podaj ilość: ");
        if (!int.TryParse(Console.ReadLine(), out int quantity)) // sprawdzenie poprawności liczby
        {
            Console.WriteLine("Błąd: Nieprawidłowa ilość.");
            return;
        }

        Console.Write("Podaj cenę jednostkową: ");
        if (!double.TryParse(Console.ReadLine(), out double unitPrice)) // sprawdzenie poprawności ceny
        {
            Console.WriteLine("Błąd: Nieprawidłowa cena.");
            return;
        }

        products.Add(new Product(name, quantity, unitPrice)); // dodanie nowego produktu do listy
        Console.WriteLine("Produkt został dodany.");
    }

    static void RemoveProduct() // metoda usuwająca produkt
    {
        Console.Write("Podaj nazwę produktu do usunięcia: ");
        string name = Console.ReadLine(); // nazwa produktu do usunięcia
        Product productToRemove = products.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)); // szukanie produktu w liście

        if (productToRemove != null)
        {
            products.Remove(productToRemove); // usunięcie znalezionego produktu
            Console.WriteLine("Produkt został usunięty.");
        }
        else Console.WriteLine("Nie znaleziono produktu."); // brak produktu o podanej nazwie
    }

    static void DisplayProducts() // metoda wyświetlająca listę produktów
    {
        if (products.Count == 0)
        {
            Console.WriteLine("Magazyn jest pusty."); // jeśli lista pusta
            return;
        }

        Console.WriteLine("\nLista produktów:");
        foreach (var product in products) // wyświetlanie każdego produktu z listy
        {
            Console.WriteLine($"{product.Name}, Ilość: {product.Quantity}, Cena: {product.UnitPrice:C}"); // {C} = format waluty
        }
    }

    static void UpdateProduct() // metoda aktualizująca dane produktu
    {
        Console.Write("Podaj nazwę produktu do aktualizacji: ");
        string name = Console.ReadLine(); // nazwa produktu
        Product productToUpdate = products.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)); // szukanie produktu

        if (productToUpdate != null)
        {
            Console.Write("Podaj nową ilość: ");
            if (!int.TryParse(Console.ReadLine(), out int newQuantity)) // sprawdzenie ilości
            {
                Console.WriteLine("Błąd: Nieprawidłowa ilość.");
                return;
            }
            productToUpdate.Quantity = newQuantity; // aktualizacja ilości

            Console.Write("Podaj nową cenę: ");
            if (!double.TryParse(Console.ReadLine(), out double newPrice)) // sprawdzenie ceny
            {
                Console.WriteLine("Błąd: Nieprawidłowa cena.");
                return;
            }
            productToUpdate.UnitPrice = newPrice; // aktualizacja ceny

            Console.WriteLine("Produkt zaktualizowany.");
        }
        else Console.WriteLine("Nie znaleziono produktu."); // brak produktu o tej nazwie
    }

    static void CalculateWarehouseValue() // metoda obliczająca całkowitą wartość magazynu
    {
        double totalValue = products.Sum(p => p.GetValue()); // suma wartości wszystkich produktów
        Console.WriteLine($"Całkowita wartość magazynu: {totalValue:C}"); // wyświetlenie wartości
    }

    static void SaveProducts() // metoda zapisująca produkty do pliku JSON
    {
        string json = JsonSerializer.Serialize(products); // konwersja listy na tekst JSON
        File.WriteAllText(filePath, json); // zapis do pliku
    }

    static void LoadProducts() // metoda wczytująca dane z pliku
    {
        if (File.Exists(filePath)) // sprawdzenie czy plik istnieje
        {
            string json = File.ReadAllText(filePath); // odczyt pliku jako tekst
            products = JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>(); // konwersja JSON -> lista produktów
        }
    }
}