using System;

namespace Solid_3
{
    // Завдання 3.
    // Порушено принцип LSP (Liskov Substitution Principle).
    // Square наслідувався від Rectangle і змінював поведінку сеттерів:
    // зміна Width змінювала і Height, і навпаки. Код, що працює з Rectangle,
    // очікує незалежних сторін, тому для 5 x 10 отримував 100 замість 50.
    // Виправлення: Square і Rectangle не пов'язані наслідуванням,
    // обидва реалізують спільну абстракцію Shape; розміри задаються
    // в конструкторі (незмінні об'єкти).

    abstract class Shape
    {
        public abstract int GetArea();
    }

    class Rectangle : Shape
    {
        public int Width { get; }
        public int Height { get; }

        public Rectangle(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("Сторони мають бути додатними");
            Width = width;
            Height = height;
        }

        public override int GetArea() => Width * Height;
    }

    class Square : Shape
    {
        public int Side { get; }

        public Square(int side)
        {
            if (side <= 0)
                throw new ArgumentException("Сторона має бути додатною");
            Side = side;
        }

        public override int GetArea() => Side * Side;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Shape rect = new Rectangle(5, 10);
            Shape square = new Square(10);

            Console.WriteLine("Площа прямокутника 5x10: " + rect.GetArea());
            Console.WriteLine("Площа квадрата 10x10: " + square.GetArea());

            Console.ReadKey();
        }
    }
}