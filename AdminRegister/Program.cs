using Shopping.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminRegister
{
    class Program
    {
        static void Main(string[] args)
        {
            var repo = new AccountRepository(Properties.Settings.Default.ConStr);
            Console.WriteLine("Enter first name:");
            string firstName = Console.ReadLine();
            Console.WriteLine("Enter last name:");
            string lastName = Console.ReadLine();
            Console.WriteLine("Enter email address:");
            string email = Console.ReadLine();
            Console.WriteLine("Enter password");
            string password = Console.ReadLine();
            repo.AddAdmin(firstName, lastName, email, password);
            Console.WriteLine("Admin created.  Press any key to exit.");
            Console.ReadKey(true);
        }
    }
}
