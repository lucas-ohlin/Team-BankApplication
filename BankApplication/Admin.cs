using System;
using System.Collections.Generic;

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

    }

}
