using System;
using System.Collections.Generic;
using System.Linq;

namespace Solid1
{
    // Завдання 1.
    // Порушено принцип SRP (Single Responsibility Principle).
    // Клас Order відповідав одразу за: керування товарами, розрахунок суми,
    // відображення (Print/Show) і збереження (Load/Save/Update/Delete).
    // Виправлення: кожна відповідальність винесена в окремий клас.

    class Item
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    // Відповідальність: дані замовлення та керування товарами
    class Order
    {
        private readonly List<Item> itemList = new List<Item>();

        public int Id { get; set; }
        public IReadOnlyList<Item> Items => itemList;
        public int ItemCount => itemList.Count;

        public void AddItem(Item item) => itemList.Add(item);
        public void DeleteItem(Item item) => itemList.Remove(item);
    }

    // Відповідальність: розрахунок суми
    class OrderCalculator
    {
        public decimal CalculateTotalSum(Order order) =>
            order.Items.Sum(i => i.Price * i.Quantity);
    }

    // Відповідальність: відображення
    class OrderPrinter
    {
        private readonly OrderCalculator calculator = new OrderCalculator();

        public void PrintOrder(Order order)
        {
            Console.WriteLine($"Замовлення #{order.Id}");
            foreach (var item in order.Items)
                Console.WriteLine($"  {item.Name} x{item.Quantity} = {item.Price * item.Quantity}");
            Console.WriteLine($"Разом: {calculator.CalculateTotalSum(order)}");
        }

        public void ShowOrder(Order order) => PrintOrder(order);
    }

    // Відповідальність: збереження
    class OrderRepository
    {
        private readonly Dictionary<int, Order> storage = new Dictionary<int, Order>();

        public Order Load(int id) => storage.TryGetValue(id, out var o) ? o : null;
        public void Save(Order order) => storage[order.Id] = order;
        public void Update(Order order) => storage[order.Id] = order;
        public void Delete(Order order) => storage.Remove(order.Id);
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var order = new Order { Id = 1 };
            order.AddItem(new Item { Name = "Ручка", Price = 15m, Quantity = 3 });
            order.AddItem(new Item { Name = "Зошит", Price = 40m, Quantity = 2 });

            var repository = new OrderRepository();
            repository.Save(order);

            var printer = new OrderPrinter();
            printer.PrintOrder(repository.Load(1));

            Console.ReadKey();
        }
    }
}