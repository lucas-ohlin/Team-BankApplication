using System;
using System.Collections.Generic;
using System.Text;

namespace BankApplication {

    internal class Users {

        //List of customer objects
        public static List<Customer> customerList = new List<Customer>();

        //List of admin objects
        public static List<Admin> adminList = new List<Admin>();

        public static void DefaultUserCreation() {

            //The 1st string is the name of the account in the customer, 
            //The list includes the balance of the account and what currency it has
            //When we're gonna create new accounts for the customers these are the things we're hopefully gonna call
            var customer1Dict = new Dictionary<string, List<string>>() {
                { "Sparkonto", new List<string>() { 1000.0f.ToString(), "kr", "Personkonto" } },
                { "Lönekonto", new List<string>() { 2000.0f.ToString(), "$", "Personkonto" } },
            };
            //The name, the password and the dictionary from above ^
            Customer customer1 = new Customer("Tobias", "111", customer1Dict);

            //-----2nd customer-----
            var customer2Dict = new Dictionary<string, List<string>>() {
                { "Sparkonto", new List<string>() { 1000.0f.ToString(), "kr", "Personkonto" } },
                { "Lönekonto", new List<string>() { 2000.0f.ToString(), "$", "Personkonto" } },
            };
            Customer customer2 = new Customer("Anas", "222", customer2Dict);

            //-----3rd customer-----
            var customer3Dict = new Dictionary<string, List<string>>() {
                { "Sparkonto", new List<string>() { 1000.0f.ToString(), "kr","Personkonto" } },
                { "Lönekonto", new List<string>() { 2000.0f.ToString(), "$","Personkonto" } },
            };
            Customer customer3 = new Customer("Lucas", "333", customer3Dict);

            //Add the customers to the customerList
            customerList.Add(customer1);
            customerList.Add(customer2);
            customerList.Add(customer3);

            //Admin creation
            var admin1 = new Admin("Gustav", "000");

            //Add the admin to the adminList
            adminList.Add(admin1);

        }

    }

}
