using System;
using System.IO;

namespace Solid_2
{
    // Завдання 2.
    // Порушено принцип SRP (Single Responsibility Principle).
    // Клас EmailSender відповідав і за відправку листів, і за спосіб логування
    // (жорстко прописаний Console.WriteLine). Зміна способу логування
    // вимагала б правок у класі, який займається відправкою.
    // Виправлення: логування винесено в окремі класи за інтерфейсом ILogger,
    // EmailSender отримує логер ззовні через конструктор.

    class Email
    {
        public string Theme { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }

    // Відповідальність: логування (абстракція)
    interface ILogger
    {
        void Write(string message);
    }

    // Логування в консоль
    class ConsoleLogger : ILogger
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }

    // Логування у файл (приклад: змінюємо спосіб логування без правок EmailSender)
    class FileLogger : ILogger
    {
        private readonly string path;

        public FileLogger(string path)
        {
            this.path = path;
        }

        public void Write(string message)
        {
            File.AppendAllText(path, message + Environment.NewLine);
        }
    }

    // Відповідальність: лише відправка листів
    class EmailSender
    {
        private readonly ILogger logger;

        public EmailSender(ILogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Send(Email email)
        {
            // ... sending...
            logger.Write("Email from '" + email.From + "' to '" + email.To + "' was sent");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Email e1 = new Email() { From = "Me", To = "Vasya", Theme = "Who are you?" };
            Email e2 = new Email() { From = "Vasya", To = "Me", Theme = "vacuum cleaners!" };
            Email e3 = new Email() { From = "Kolya", To = "Vasya", Theme = "No! Thanks!" };
            Email e4 = new Email() { From = "Vasya", To = "Me", Theme = "washing machines!" };
            Email e5 = new Email() { From = "Me", To = "Vasya", Theme = "Yes" };
            Email e6 = new Email() { From = "Vasya", To = "Petya", Theme = "+2" };

            // Щоб писати лог у файл, достатньо замінити на: new FileLogger("log.txt")
            EmailSender es = new EmailSender(new ConsoleLogger());
            es.Send(e1);
            es.Send(e2);
            es.Send(e3);
            es.Send(e4);
            es.Send(e5);
            es.Send(e6);

            Console.ReadKey();
        }
    }
}