using System;
using System.Collections.Generic;

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

    }

}
