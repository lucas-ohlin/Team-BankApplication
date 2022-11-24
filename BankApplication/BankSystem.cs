using System;
using System.Collections.Generic;
using System.Text;

namespace BankApplication {

    internal class BankSystem {

        //List of customer objects
        public static List<Customer> customerList = new List<Customer>();

        public static void LogIn() {

            Console.WriteLine("Welcome to the bank.\nPlease login.");

            //Store locally how many tries have been made by the user
            byte tries = 0;

            //A while do loop if tries is less than 3
            do {

                Console.WriteLine("\nname:");
                string name = Console.ReadLine();

                Console.WriteLine("password:");
                string password = Console.ReadLine();

                //Check if the name and password exist on the same object in the list
                if (customerList.Exists(x => x.Name == name && x.Password == password)) {
                    Customer Account = customerList.Find(x => x.Name == name);
                    NavigationMenu(Account);
                    break;
                }
                //If the name and password wasn't right, add one to tries   
                else {

                    Console.WriteLine("\nNot a valid customer, try again:");
                    tries++;

                }

            } while (tries < 3);

            Console.WriteLine("\nYour three tries are up.");

        }

        public static void NavigationMenu(Customer Account) {

            //Prints out the logged in account name
            Console.WriteLine($"\nWelcome: {Account.Name}");

            bool run = true;
            while (run) {

                Console.WriteLine("\n1. TODO\r\n2. Logout\r\n3. Check account balance");

                byte choice;
                if (!byte.TryParse(Console.ReadLine(), out choice))
                    Console.WriteLine("\nNumber 1-3.");

                switch (choice) {
                    default: //If not a valid choice
                        Console.WriteLine("Not a valid choice.");
                        break;
                    case 1:
                        Console.WriteLine("1");
                        break;
                    case 2: //Log out of customer
                        Console.WriteLine($"\nLogged out of: {Account.Name}");
                        run = false;
                        LogIn();
                        break;
                    case 3:
                        Console.WriteLine($"All accounts for {Account.Name}");
                        Account.CustomerInfo();
                        break;
                }

            }

        }

        //This method is a bit messy and can probably be made to look a bit better
        public static void CustomerCreation() {

            //The 1st string is the name of the account in the customer, 
            //The list includes the balance of the account and what currency it has
            //When we're gonna create new accounts for the customers these are the things we're hopefully gonna call
            var customer1Dict = new Dictionary<string, List<string>>() {
                { "Sparkonto", new List<string>() { 1000.0f.ToString(), "kr" } },
                { "Lönekonto", new List<string>() { 2000.0f.ToString(), "$" } },
            };
            //The name, the password and the dictionary from above ^
            Customer customer1 = new Customer("Tobias", "111", customer1Dict);

            //-----2nd customer-----
            var customer2Dict = new Dictionary<string, List<string>>() {
                { "Sparkonto", new List<string>() { 1000.0f.ToString(), "kr" } },
                { "Lönekonto", new List<string>() { 2000.0f.ToString(), "$" } },
            };
            Customer customer2 = new Customer("Anas", "222", customer2Dict);

            //-----3rd customer-----
            var customer3Dict = new Dictionary<string, List<string>>() {
                { "Sparkonto", new List<string>() { 1000.0f.ToString(), "kr" } },
                { "Lönekonto", new List<string>() { 2000.0f.ToString(), "$" } },
            };
            Customer customer3 = new Customer("Lucas", "333", customer3Dict);

            //Add the customers to the customerList
            customerList.Add(customer1);
            customerList.Add(customer2);
            customerList.Add(customer3);

        }

    }

}
