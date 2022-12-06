using System;
using System.Collections.Generic;
using System.IO;

namespace BankApplication {

    /// <summary>
    /// Admin class which is made up of; name and password
    /// </summary>
    internal class Admin : User {

        private string Name { get; set; }
        private string Password { get; set; }
        private string Tag { get; set; }

        public Admin(string name, string password) 
            : base(name, password) {

            Name = name;
            Password = password;
            Tag = "Admin";

        }

        public void AdminInfo() {

            Console.WriteLine($"{Name} & {Password}");

        }

        public static void AdminUpdateRates()
        {
            File.WriteAllText("ExchangeRate.txt", String.Empty);
            Console.WriteLine("What is the new USD to SEK rate:");
            string usdToSek = Console.ReadLine();
            using StreamWriter sw = File.CreateText("ExchangeRate.txt");
            sw.WriteLine(usdToSek);
            sw.WriteLine(DateTime.Now.ToString());
            sw.Close();
        }

    }

}
