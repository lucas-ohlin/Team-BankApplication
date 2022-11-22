using System;
using System.Collections.Generic;

namespace BankApplication {

    internal class Customer : User {

        private Dictionary<string, List<string>> accounts;

        //Name, password and dictionary which includes name of account
        //and the list which hold the balance and what currency it is in
        public Customer(string name, string password, Dictionary<string, List<string>> accounts) 
            : base(name, password) {

            this.accounts = accounts;

        }

        //Basic information of the client, name + numbers of accounts and their balance
        public void CustomerInfo() {

            Console.WriteLine($"\n{name} has {accounts.Count} accounts:");

            foreach (var account in accounts) 
                Console.WriteLine($"{account.Key} has {account.Value[0]}{account.Value[1]}");

        }

        //Probably not the best way to return the name and password but it works for now
        //Returms the string name, and password
        public string ReturnName() { return name; }
        public string ReturnPassword() { return password; }

    
    }

}
