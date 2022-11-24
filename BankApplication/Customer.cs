using System;
using System.Collections.Generic;

namespace BankApplication {

    internal class Customer : User {

        //Accout list for the customer
        private Dictionary<string, List<string>> accounts;

        //Public get and a private set
        public string Name { get; private set; }
        public string Password { get; private set; }

        //Name, password and dictionary which includes name of account
        //and the list which hold the balance and what currency it is in
        public Customer(string name, string password, Dictionary<string, List<string>> accounts) 
            : base(name, password) {

            this.accounts = accounts;
            Name = name;  
            Password = password;  
        }

        //Basic information of the client, name + numbers of accounts and their balance
        public void CustomerInfo() {

            Console.WriteLine($"\n{name} has {accounts.Count} accounts:");

            foreach (var account in accounts) 
                Console.WriteLine($"{account.Key} has {account.Value[0]}{account.Value[1]}");
        }
    }

}
