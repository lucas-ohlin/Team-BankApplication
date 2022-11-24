using System;
using System.Collections.Generic;

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
                    //Sends the correct account to the navmenu for further use
                    Customer Account = customerList.Find(x => x.Name == name);
                    NavigationMenu(Account);
                    break;
                }
                //If the name and password wasn't right, add one to tries   
                else if (!customerList.Exists(x => x.Name == name && x.Password == password)) {

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

                Console.WriteLine("\n1. TODO\r\n2. Logout\r\n3. Check account balance\r\n4. Transfer funds to another costumer");

                byte choice;
                if (!byte.TryParse(Console.ReadLine(), out choice))
                    Console.WriteLine("\nNumber 1-4.");

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
                    case 4:
                        TransferBetweenCustomers(Account);
                        break;
                }

            }

        }

        private static void TransferBetweenCustomers(Customer customer) {

            //Displays the accounts in the logged in customer
            customer.CustomerInfo();

            //Logged in customer account + account to send it to
            string choice, choice2, customerName;
            Customer customer2;

            //Find account in logged into customer
            while (true) {

                Console.WriteLine($"\nAccount name in {customer.Name} to transfer from:");
                choice = Console.ReadLine();

                //If the customer exists, exit out of the while loop
                if (customer.accounts.ContainsKey(choice))
                    break;
                else
                    Console.WriteLine("Not a valid account name, try again.");
            }

            //Find the 2nd customer account
            while (true) {

                Console.WriteLine("\nCustomer name to transfer to:");
                customerName = Console.ReadLine();
                
                //Check if the customer exists in the list using the previous input
                if (customerList.Exists(x => x.Name == customerName)) {

                    //Create an object using the name
                    customer2 = customerList.Find(x => x.Name == customerName);

                    //Display accounts in that customer
                    customer2.CustomerInfo();

                    while (true) {

                        Console.WriteLine($"\nAccount name in {customer2.Name} to transfer to:");
                        choice2 = Console.ReadLine();

                        //Boolean, check if customer two contains key (choice2)
                        if (customer2.accounts.ContainsKey(choice2))
                            break;
                        else
                            Console.WriteLine("\nNot a valid account name, try again.");
                    }

                    break;

                }

                //If no customer with the name inputed exists
                else if (!customerList.Exists(x => x.Name == customerName)) 
                    Console.WriteLine("Not a valid name, try again.");
            }

            //customer.accounts[choice][0] = balance & customer.accounts[choice][1] = currency [kr, $, etc.]
            Console.WriteLine($"\nYour account: {customer.Name} : {customer.accounts[choice][0] + customer.accounts[choice][1]}\n" +
                              $"Transfer to: {customer2.Name} : {customer2.accounts[choice2][0] + customer2.accounts[choice2][1]}\n" +
                              $"How much do you want to transfer?:");

            while (true) {

                //Error handling, check if it can't parse it as a float
                float amount;
                if (float.TryParse(Console.ReadLine(), out amount)) {
                    
                    //Subtracts amount from the account which we want to move the funds from
                    customer.accounts[choice][0] = (float.Parse(customer.accounts[choice][0]) - amount).ToString();
    
                    //Adds the amount to the account we wanted to move the fund to
                    customer2.accounts[choice2][0] = (float.Parse(customer2.accounts[choice2][0]) + amount).ToString();

                    //Displays what was transfered and the new balance of the logged in customer
                    Console.WriteLine($"\nTransfered {amount}{customer.accounts[choice][1]} from {customer.Name} to {customer2.Name}");
                    customer.CustomerInfo();
                    customer2.CustomerInfo();

                    return;
                }

                //Check if amount is bigger than the funds in the account
                if (float.Parse(customer.accounts[choice][0]) < amount)
                    Console.WriteLine("\nAccount does not have the necessary funds available\nTry again:");
                else
                    Console.WriteLine("Number only.");
            }

        }

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
