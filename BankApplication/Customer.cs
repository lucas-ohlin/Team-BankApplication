using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace BankApplication {

    /// <summary>
    /// Customer class which is made up of; name, password and account(s)
    /// </summary>
    internal class Customer : User {

        //Accout list for the customer
        public Dictionary<string, List<string>> accounts;

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


        // Method to only show accounts when the user chooses Account to transfer to/from
        public void AccountName()
        {
            int index = 1;
            foreach (var account in accounts)
                Console.WriteLine("{0}. {1}: {2} {3}", index++, account.Key,account.Value[0],account.Value[1]);
            
        }

        
       
        }
        public void OpenAccount()
        {
            Console.WriteLine("What do you want to name your new account(between 4 and 20 characters)");
            while (true)
            {
                string NewAccChoice=Console.ReadLine();
                if(NewAccChoice.Length>20 || NewAccChoice.Length < 4)
                {
                    Console.WriteLine("The account name needs to be between 4 and 20 characters");
                }
                else if (accounts.ContainsKey(NewAccChoice))
                {
                    Console.WriteLine("This account already exists for this user");
                }
                else
                {
                    accounts.Add(NewAccChoice, new List<string>() { 0.0f.ToString(), "kr" });
                    Console.WriteLine($"Account {NewAccChoice} was added and it has {accounts[NewAccChoice][0]}{accounts[NewAccChoice][1]} in it");
                    break;
                }
            }
        }
    }

}
