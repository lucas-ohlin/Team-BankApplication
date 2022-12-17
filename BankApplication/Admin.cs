using System;
using System.Collections.Generic;
using System.IO;

namespace BankApplication {

    /// <summary>
    /// Admin class which is made up of; name and password
    /// </summary>
   
    internal class Admin : User {

        public string Name { get; private set; }
        public string Password { get; private set; }

        public Admin(string name, string password) 
            : base(name, password) {

            Name = name;
            Password = password;

        }

        public void AdminInfo() {

            Console.WriteLine($"{Name} & {Password}");

        }

        public void AdminUpdateRates() {

            File.WriteAllText("ExchangeRate.txt", String.Empty);

            Console.WriteLine("What is the new USD to SEK rate:");
            string usdToSek = Console.ReadLine();

            //Writes to the text file
            using StreamWriter sw = File.CreateText("ExchangeRate.txt");
            sw.WriteLine(usdToSek);
            sw.WriteLine(DateTime.Now.ToString());
            sw.Close();

        }

    }

}
