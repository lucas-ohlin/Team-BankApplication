using System;
using System.Collections.Generic;
using System.Threading;

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


        // Method to only show accounts when the user chooses
        public void AccountName()
        {
            foreach (var account in accounts)
                Console.WriteLine(account.Key);
        }

        // working progreess soon done
        public void TransferbetweenAccounts(Customer customer)
        {
            double Transfer;
            Console.Clear();
            do
            {
                customer.AccountName();
                Console.WriteLine("Which account do you want to transfer from: Name of the account");
                string TransferFrom = Console.ReadLine();

                if (customer.accounts.ContainsKey(TransferFrom) == true)
                {
                    Console.Clear();
                    Console.WriteLine("Amount to transfer from {0} : {1}", TransferFrom, customer.accounts[TransferFrom][0]);
                    double.TryParse(Console.ReadLine(), out Transfer);
                    if (Transfer > 0 && Transfer <= double.Parse(customer.accounts[TransferFrom][0]))
                    {
                        Console.Clear();
                        customer.AccountName();
                        Console.WriteLine("Which of the accounts above do you want to transfer To: Name of the account");
                        string TransferTo = Console.ReadLine();

                        if (customer.accounts.ContainsKey(TransferTo) == true)
                        {
                            customer.accounts[TransferFrom][0] = (double.Parse(customer.accounts[TransferFrom][0]) - Transfer).ToString();
                            customer.accounts[TransferTo][0] = (double.Parse(customer.accounts[TransferTo][0]) + Transfer).ToString();
                            Console.WriteLine($"You have succesfully transfered {Transfer}{customer.accounts[TransferFrom][1]} from " +
                                $"{TransferFrom} to {TransferTo}");
                            break;
                        }
                        else Console.Clear(); Console.WriteLine("Account not found, you dont have a account of the name: " + TransferTo);
                        
                    }
                    else Console.WriteLine("Amount is not valid");

                }
                else Console.Clear(); Console.WriteLine("Account not found, you dont have a account of the name: "+TransferFrom);
                

            } while (true);
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
