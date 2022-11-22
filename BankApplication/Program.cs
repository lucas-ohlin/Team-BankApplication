using System;
using System.Collections.Generic;

namespace BankApplication {

    internal class Program {

        //Maybe a seperate class for everything here? "BankHub" is an idea, and just have the "Main method" here
        public static List<Customer> customerArray = new List<Customer>();

        private static void Main(string[] args) {

            //Creation of the 3 customers
            CustomerCreation();

            //Login
            LogIn();

        }

        private static void LogIn() {

            Console.WriteLine("Welcome to the bank.\nPlease login.");

            //Store how many tries have been made by the user
            byte tries = 0;

            //A while do loop if tries is less than 3
            do {

                Console.WriteLine("\nname:");
                string name = Console.ReadLine();

                Console.WriteLine("password:");
                string password = Console.ReadLine();

                //Check if the name and password exist on the same object in the list
                if (customerArray.Exists(x => x.ReturnName() == name && x.ReturnPassword() == password)) { 

                    //Navigation menu here
                    Console.WriteLine("Customer existed");
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

        //This method is a bit messy and can probably be made to look a bit better
        private static void CustomerCreation() {

            //The 1st string is the name of the account in the customer, 
            //The list includes the balance of the account and what currency it has
            var customer1Dict = new Dictionary<string, List<string> >() {
                { "Sparkonto", new List<string>() { 1000.0f.ToString(), "kr" } },
                { "Lönekonto", new List<string>() { 2000.0f.ToString(), "$" } },
            };

            //The name, the password and the dictionary from above ^
            Customer customer1 = new Customer("Tobias", "111", customer1Dict);
            //customer1.CustomerInfo();

            //-----2nd customer-----
            var customer2Dict = new Dictionary<string, List<string>>() {
                { "Sparkonto", new List<string>() { 1000.0f.ToString(), "kr" } },
                { "Lönekonto", new List<string>() { 2000.0f.ToString(), "$" } },
            };
            Customer customer2 = new Customer("Anas", "222", customer2Dict);
            //customer2.CustomerInfo();

            //-----3rd customer-----
            var customer3Dict = new Dictionary<string, List<string>>() {
                { "Sparkonto", new List<string>() { 1000.0f.ToString(), "kr" } },
                { "Lönekonto", new List<string>() { 2000.0f.ToString(), "$" } },
            };
            Customer customer3 = new Customer("Lucas", "333", customer3Dict);
            //customer3.CustomerInfo();

            //Add the customers to the customer array
            customerArray.Add(customer1);
            customerArray.Add(customer2);
            customerArray.Add(customer3);

        }

    }

}
