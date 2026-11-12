using System;
using System.Globalization;

namespace Solid_4
{
    // Завдання 4.
    // Інтерфейс IItem порушував принцип ISP (Interface Segregation Principle):
    // він містив забагато методів, і класи, яким частина з них не потрібна
    // (наприклад, книзі - колір і розмір), були б змушені їх реалізовувати.
    // Виправлення: IItem розбито на дрібні інтерфейси, кожен клас
    // реалізує лише ті, що йому притаманні.

    interface IPriceable
    {
        void SetPrice(double price);
    }

    interface IDiscountable
    {
        void ApplyDiscount(string discount);
    }

    interface IPromocodable
    {
        void ApplyPromocode(string promocode);
    }

    interface IColorable
    {
        void SetColor(byte color);
    }

    interface ISizeable
    {
        void SetSize(byte size);
    }

    // Допоміжний клас: розбір знижки виду "10" або "10%"
    static class DiscountParser
    {
        public static double ParsePercent(string discount)
        {
            if (string.IsNullOrWhiteSpace(discount))
                throw new ArgumentException("Знижка не задана");

            string s = discount.Trim().TrimEnd('%').Trim();
            double percent;
            if (!double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out percent)
                || percent < 0 || percent > 100)
                throw new ArgumentException("Некоректна знижка: " + discount);

            return percent;
        }
    }

    // Книжка: потрібні лише ціна та знижки
    class Book : IPriceable, IDiscountable
    {
        public string Title { get; }
        public double Price { get; private set; }

        public Book(string title)
        {
            Title = title;
        }

        public void SetPrice(double price)
        {
            if (price < 0)
                throw new ArgumentException("Ціна не може бути від'ємною");
            Price = price;
        }

        public void ApplyDiscount(string discount)
        {
            double percent = DiscountParser.ParsePercent(discount);
            Price = Price * (1 - percent / 100);
        }

        public override string ToString()
        {
            return "Книжка \"" + Title + "\", ціна: " + Price.ToString("0.##");
        }
    }

    // Верхній одяг: колір, розмір, ціна, знижки
    class Outerwear : IPriceable, IDiscountable, IColorable, ISizeable
    {
        public string Name { get; }
        public double Price { get; private set; }
        public byte Color { get; private set; }
        public byte Size { get; private set; }

        public Outerwear(string name)
        {
            Name = name;
        }

        public void SetPrice(double price)
        {
            if (price < 0)
                throw new ArgumentException("Ціна не може бути від'ємною");
            Price = price;
        }

        public void ApplyDiscount(string discount)
        {
            double percent = DiscountParser.ParsePercent(discount);
            Price = Price * (1 - percent / 100);
        }

        public void SetColor(byte color)
        {
            Color = color;
        }

        public void SetSize(byte size)
        {
            Size = size;
        }

        public override string ToString()
        {
            return "Одяг \"" + Name + "\", колір: " + Color + ", розмір: " + Size
                   + ", ціна: " + Price.ToString("0.##");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Book book = new Book("Чистий код");
            book.SetPrice(500);
            book.ApplyDiscount("10%");
            Console.WriteLine(book);

            Outerwear jacket = new Outerwear("Куртка");
            jacket.SetPrice(2000);
            jacket.SetColor(3);
            jacket.SetSize(48);
            jacket.ApplyDiscount("25");
            Console.WriteLine(jacket);

            Console.ReadKey();
        }
    }
}