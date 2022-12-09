﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Globalization;
using System.Linq;
using System.Timers;


namespace BankApplication
{

    /// <summary>
    /// BankApplication contains most of the methods for the actual project
    /// </summary>

    internal class BankSystem
    {

        //List of customer objects
        public static List<Customer> customerList = new List<Customer>();

        //List of admin objects
        public static List<Admin> adminList = new List<Admin>();

        //Exchange rate for USD
        private static float sekToUsd = 10.3f;



        public static void LogIn()
        {

            Console.WriteLine("Welcome to the bank.\nPlease login.");

            //Store locally how many tries have been made by the user
            byte tries = 0;

            //A while do loop if tries is less than 3
            do
            {

                Console.WriteLine("\nname:");
                string name = Console.ReadLine();

                Console.WriteLine("password:");
                string password = Console.ReadLine();

                //Check if the name and password exist on the same object in the customerList
                if (customerList.Exists(x => x.Name == name && x.Password == password))
                {

                    //Sends the correct account to the navmenu for further use
                    Customer account = customerList.Find(x => x.Name == name && x.Password == password);
                    string sendlog = $"{DateTime.Now}: {account.Name} logged in";
                    Log(account, sendlog);
                    NavigationMenu(account);
                    break;

                }

                //If there's no such customer check if a admin with the name and password exist in the adminList
                else if (adminList.Exists(x => x.Name == name && x.Password == password))
                {

                    //Sends the correct admin to the admin navmenu for further use
                    Admin admin = adminList.Find(x => x.Name == name && x.Password == password);
                    AdminNavigationMenu(admin);
                    break;

                }

                //If the name and password doesn't exist in either list, add one to tries   
                else if (!customerList.Exists(x => x.Name == name && x.Password == password) || !adminList.Exists(x => x.Name == name && x.Password == password))
                {

                    Console.WriteLine("\nNot a valid user, try again:");
                    tries++;

                }

            } while (tries < 3);

            Console.WriteLine("\nYour three tries are up.");

        }

        public static void AdminNavigationMenu(Admin admin)
        {

            //Prints out the logged in admin name
            Console.WriteLine($"\nWelcome ADMIN: {admin.Name}");

            bool run = true;
            while (run)
            {

                Console.WriteLine("\n1. Admin information\r\n2. Create a new customer\r\n3. Logout");

                byte choice;
                if (!byte.TryParse(Console.ReadLine(), out choice))
                    Console.WriteLine("\nNumber 1-2.");

                switch (choice)
                {
                    default: //If not a valid choice
                        Console.WriteLine("Not a valid choice.");
                        break;
                    case 1: //Admin information
                        admin.AdminInfo();
                        PressEnter();
                        break;
                    case 2: //Create new customers
                        CustomerCreation();
                        PressEnter();
                        break;
                    case 3: //Log out of Admin
                        Console.WriteLine($"\nLogged out of: {admin.Name}");
                        run = false;
                        LogIn();
                        break;
                }

            }

        }

        public static void NavigationMenu(Customer account)
        {

            //Prints out the logged in account name
            Console.WriteLine($"\nWelcome: {account.Name}");

            bool run = true;
            while (run)
            {

                Console.WriteLine("\n1. Check account balance\r\n2. Open new account\r\n3. Transfer between accounts\r\n4. Transfer funds to another costumer\r\n5. Take a loan\r\n6. Open savingsaccount\r\n7. See log of my activites\r\n8. Logout");

                byte choice;
                if (!byte.TryParse(Console.ReadLine(), out choice))
                    Console.WriteLine("\nNumber 1-5.");

                switch (choice)
                {
                    default: //If not a valid choice
                        Console.WriteLine("Not a valid choice.");
                        break;
                    case 1: //Check account balance
                        Console.WriteLine($"All accounts for {account.Name}");
                        account.CustomerInfo();
                        PressEnter();
                        break;
                    case 2: //Open new account
                        OpenAccount(account);
                        PressEnter();
                        break;
                    case 3: //Transfer between accounts
                        TransferbetweenAccounts(account);
                        PressEnter();
                        break;
                    case 4: //Transfer between customers
                        TransferBetweenCustomers(account);
                        PressEnter();
                        break;
                    case 5: //Take a loan
                        Loan(account);
                        break;
                    case 6: //Open a savingsaccount
                        Savingsaccount(account);
                        break;
                    case 7: //See the logged activites of the user
                        SeeLog(account);
                        break;
                    case 8: //Log out of customer
                        Console.WriteLine($"\nLogged out of: {account.Name}");
                        string sendlog = $"{DateTime.Now}: {account.Name} logged out";
                        Log(account, sendlog);
                        run = false;
                        LogIn();
                        break;

                }

            }

        }

        public static void OpenAccount(Customer customer)
        {

            Console.WriteLine("What do you want to name your new account(between 4 and 20 characters)");

            while (true)
            {

                string newAccChoice = Console.ReadLine();

                //A character limit between 4-20
                if (newAccChoice.Length > 20 || newAccChoice.Length < 4)
                    Console.WriteLine("The account name needs to be between 4 and 20 characters");

                //Check if the account name already exists
                else if (customer.accounts.ContainsKey(newAccChoice))
                    Console.WriteLine("This account already exists for this user");

                else
                {

                    while (true)
                    {

                        Console.WriteLine("Which currency do you want to use? \n Available types of currency:\n kr \n $");
                        string curchoice = Console.ReadLine();

                        //Check if the currency is either kr or $ and make it lower case
                        if (curchoice.ToLower() == "kr" || curchoice == "$")
                        {

                            //Add the account to the accounts dictionary with a default amount at 0 and the choosen currency
                            customer.accounts.Add(newAccChoice, new List<string>() { 0.0f.ToString(), curchoice.ToLower(), "Personkonto" });
                            Console.WriteLine($"Account {newAccChoice} was added and it has {customer.accounts[newAccChoice][0]}{customer.accounts[newAccChoice][1]} in it");
                            string sendlog = $"{DateTime.Now}: Account {newAccChoice} was added and it has {customer.accounts[newAccChoice][0]}{customer.accounts[newAccChoice][1]} in it";
                            Log(customer, sendlog);
                            break;

                        }
                        else
                        {
                            //Console.Clear();
                            Console.WriteLine("Invalid choice, try again");
                        }

                    }
                    break;
                }

            }

        }

        public static void TransferbetweenAccounts(Customer customer)
        {

            float transfer;
            bool run = true;

            do
            {

                //Write out only the account names of the logged in user
                customer.AccountName();
                Console.WriteLine("Which account do you want to transfer from: Name of the account");
                string transferFrom = Console.ReadLine();

                //Check if the accounts dictionary contains the correct account name
                if (customer.accounts.ContainsKey(transferFrom) == true)
                {

                    while (true)
                    {

                        Console.WriteLine("Amount to transfer from {0} : {1}", transferFrom, customer.accounts[transferFrom][0]);

                        if (!float.TryParse(Console.ReadLine(), out transfer))
                            Console.WriteLine("Numbers only... Try again:");

                        //Parse the account balance as a double and check if its less than the amount specified for a transfer
                        if (transfer > 0 && transfer <= double.Parse(customer.accounts[transferFrom][0]))
                        {

                            customer.AccountName();

                            Console.WriteLine("Which of the accounts above do you want to transfer To:");
                            string transferTo = Console.ReadLine();

                            //Check if the accounts contains the name
                            if (customer.accounts.ContainsKey(transferTo) == true)
                            {

                                //Check currency and send over correct exchange
                                ExchangeRate(customer, customer, transferFrom, transferTo, transfer);

                                Console.WriteLine($"You have succesfully transfered {transfer}{customer.accounts[transferFrom][1]} from " +
                                    $"{transferFrom} to {transferTo}");
                                string sendlog = $"{DateTime.Now}: You have succesfully transfered {transfer}{customer.accounts[transferFrom][1]} from {transferFrom} to {transferTo}";
                                Log(customer, sendlog);

                                customer.AccountName();
                                run = false;
                                break;

                            }
                            else
                            {
                                //Console.Clear(); 
                                Console.WriteLine("Account not found of the name: " + transferTo);
                            }

                        }
                        else
                            Console.WriteLine("Amount is not valid");
                    }

                }
                else
                    Console.WriteLine("Account not found of the name: " + transferFrom);

            } while (run == true);

            //Suspends the current thread for the specified amount of time and clears the console
            //Thread.Sleep(7000);
            //Console.Clear();

        }

        private static void TransferBetweenCustomers(Customer customer)
        {

            //Displays the accounts in the logged in customer
            customer.CustomerInfo();

            //Logged in customer account + account to send it to
            string choice, choice2, customerName;
            Customer customer2;

            //Find account in logged into customer
            while (true)
            {

                Console.WriteLine($"\nAccount name in {customer.Name} to transfer from:");
                choice = Console.ReadLine();

                //If the customer exists, exit out of the while loop
                if (customer.accounts.ContainsKey(choice))
                    break;
                else
                    Console.WriteLine("Not a valid account name, try again.");

            }

            //Find the 2nd customer account
            while (true)
            {

                Console.WriteLine("\nCustomer name to transfer to:");
                customerName = Console.ReadLine();

                //Check if the customer exists in the list using the previous input
                if (customerList.Exists(x => x.Name == customerName))
                {

                    //Create an object using the name
                    customer2 = customerList.Find(x => x.Name == customerName);

                    //Display accounts in that customer
                    customer2.CustomerInfo();

                    while (true)
                    {

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

            while (true)
            {

                //Error handling, check if it can't parse it as a float
                float amount;
                if (float.TryParse(Console.ReadLine(), out amount))
                {

                    //Check currency and send over correct amount
                    ExchangeRate(customer, customer2, choice, choice2, amount);

                    //Displays what was transfered and the new balance of the logged in customer
                    Console.WriteLine($"\nTransfered {amount}{customer.accounts[choice][1]} from {customer.Name} to {customer2.Name}");
                    string sendlog = $"{DateTime.Now}: Transfered {amount}{customer.accounts[choice][1]} from {customer.Name} to {customer2.Name}";
                    Log(customer, sendlog);
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

        public static void ExchangeRate(Customer customer1, Customer customer2, string account1, string account2, float transfer1)
        {

            //Creates new variables that is later used
            float transfer2;
            string currency1 = customer1.accounts[account1][1];
            string currency2 = customer2.accounts[account2][1];
            string filePath = "ExchangeRate.txt";

            //Creates seperate txt file for exchange rate if ut does not currently exist
            if (!File.Exists(filePath))
            {
                using StreamWriter sw = File.CreateText(filePath);
                sw.WriteLine("10,3");
                sw.WriteLine(DateTime.Now.ToString());
                sw.Close();
            }

            //Checks the current exchange rate in the file
            using StreamReader sr = File.OpenText(filePath);
            float usdToSek;
            string Time;
            usdToSek = float.Parse(sr.ReadLine());
            Time = sr.ReadLine();
            Console.WriteLine("\nThe exchange rate was last updated:{0}", Time);
            sr.Close();

            //If its the same currency its sends over the same amount
            if (currency1 == currency2)
            {
                customer1.accounts[account1][0] = (float.Parse(customer1.accounts[account1][0]) - transfer1).ToString();
                customer2.accounts[account2][0] = (float.Parse(customer2.accounts[account2][0]) + transfer1).ToString();
            }
            //If its not the same and the second one is dollar the amount is divided by the exchange rate
            else if (currency2 == "$")
            {
                transfer2 = transfer1 / usdToSek;
                customer1.accounts[account1][0] = (float.Parse(customer1.accounts[account1][0]) - transfer1).ToString();
                customer2.accounts[account2][0] = (float.Parse(customer2.accounts[account2][0]) + transfer2).ToString();
            }
            //If its not the same and the second one is swedish crowns the amount is multiplied by the exchange rate
            else if (currency2 == "kr")
            {
                transfer2 = transfer1 * usdToSek;
                customer1.accounts[account1][0] = (float.Parse(customer1.accounts[account1][0]) - transfer1).ToString();
                customer2.accounts[account2][0] = (float.Parse(customer2.accounts[account2][0]) + transfer2).ToString();
            }

        }

        public static void Loan(Customer customer)
        {
            double interest = 0.03;
            while (true)
            {
                Console.WriteLine("How much would you like to loan?");
                //Makes sure only numbers are entered and gets the amount the user wishes to loan
                if (!double.TryParse(Console.ReadLine(), out double loanamount))
                    Console.WriteLine("Numbers only, try again:");
                //Makes sure the user can't enter a number below 0
                if (loanamount <= 0)
                    Console.WriteLine("Please choose an amount above 0");
                if (loanamount > 0)
                {
                    while (true)
                    {
                        customer.AccountName();
                        Console.WriteLine("Which account would you like to loan to?");
                        string loanto = Console.ReadLine();
                        if (loanto == "") { break; } //return to nav menu by pressing enter
                        if ((double.Parse(customer.accounts[loanto][0]) * 5 > loanamount))
                        {
                            //Checks if the specified account exists
                            if (customer.accounts.ContainsKey(loanto) == true)
                            {
                                //Displays the interest rate and how much the user will have to pay monthly
                                Console.WriteLine($"The interest rate is currently: {interest * 100}%\nMonthly interest for a loan of {loanamount}{customer.accounts[loanto][1]} is {loanamount * interest}{customer.accounts[loanto][1]}");
                                Console.WriteLine("\nDo you wish to take this loan? Yes or No");
                                while (true)
                                {
                                    string loanchoice = Console.ReadLine();
                                    //If the user decides to take the loan
                                    if (loanchoice.ToUpper() == "YES")
                                    {
                                        //Adds the specified amount to the account of choice by user
                                        customer.accounts[loanto][0] = (double.Parse(customer.accounts[loanto][0]) + loanamount).ToString();
                                        Console.WriteLine($"{loanamount}{customer.accounts[loanto][1]} has been added to {loanto}");
                                        string sendlog = $"{DateTime.Now}: Took a loan of {loanamount}{customer.accounts[loanto][1]} and added it to {loanto}";
                                        Log(customer, sendlog);
                                        break;
                                    }
                                    //Returns the user to the navigation menu if they choose not to take the loan
                                    if (loanchoice.ToUpper() == "NO") { break; }

                                    else Console.WriteLine("Invalid answer, please answer Yes or No");
                                }
                                break;
                            }
                            else Console.WriteLine("Account not found, try again");
                        }
                        else Console.WriteLine("You can't take a loan that is 5 times larger than what you currently have on your account");
                    }
                    break;


                }
            }

        }
        public static void CustomerCreation()
        {

            Console.WriteLine("\nName:");
            string name = Console.ReadLine();

            //A character limit between 1-20
            if (name.Length > 20 || name.Length < 1)
                Console.WriteLine("The account name needs to be between 1 and 20 characters");

            //Check if the account name already exists
            else if (customerList.Exists(x => x.Name == name))
                Console.WriteLine("This account already exists for this user");

            Console.WriteLine("\nPassword:");
            string password = Console.ReadLine();

            //Creates a new customer object and adds it to the customerList
            customerList.Add(new Customer(name, password, new Dictionary<string, List<string>>()));

        }

        public static void PressEnter()
        {
            Console.WriteLine("\ntryck på ENTER för meny");
            ConsoleKeyInfo x;
            do
            {
                x = Console.ReadKey();
            }
            while (x.Key != ConsoleKey.Enter);
            Console.Write("\n");
        }

        public static void DefaultUserCreation()
        {

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

        public static void Savingsaccount(Customer customer)
        {
            float Rate = 0;
            string Account = "";
            float Balance = 0;

            // A Tuple over available type of savings accounts
            Tuple<string, string, float> Vacation = new Tuple<string, string, float>("1", "Vacation", 20f);
            Tuple<string, string, float> Pension = new Tuple<string, string, float>("2", "Pension", 2.42f);
            Tuple<string, string, float> Childsavings = new Tuple<string, string, float>("3", "Childsavings", 3.49f);


            Console.WriteLine("What type of account do you want to open: Type nr");
            Console.WriteLine($"{Vacation.Item1}.{Vacation.Item2} Rate: {Vacation.Item3}");
            Console.WriteLine($"{Pension.Item1}.{Pension.Item2} Rate: {Pension.Item3}");
            Console.WriteLine($"{Childsavings.Item1}.{Childsavings.Item2} Rate: {Childsavings.Item3}");



            bool run = true;
            // Depending on choice i take out the choosen Account
            do
            {
                string choice = Console.ReadLine();

                switch (choice)
                {

                    case "1":
                        Account = Vacation.Item2;
                        Rate = Vacation.Item3;
                        run = false;
                        break;
                    case "2":
                        Account = Pension.Item2;
                        Rate = Pension.Item3;
                        run = false;
                        break;
                    case "3":
                        Account = Childsavings.Item2;
                        Rate = Childsavings.Item3;
                        run = false;
                        break;
                    default:
                        Console.WriteLine("Option 1-3");
                        break;
                }

            } while (run);

            Console.WriteLine("What do you want to name your new account(between 2 and 10 characters)");

            while (true)
            {
                string newAccChoice = Console.ReadLine();

                //A character limit between 2 - 10
                if (newAccChoice.Length > 10 || newAccChoice.Length < 2)
                    Console.WriteLine("The account name needs to be between 2 and 10 characters");

                //Check if the account name already exists
                else if (customer.accounts.ContainsKey(newAccChoice))
                    Console.WriteLine("This account already exists for this user");

                else
                {

                    while (true)
                    {
                        float Depo;

                        Console.WriteLine("Which currency do you want to use? \n Available types of currency:\nkr \n$");
                        string curchoice = Console.ReadLine();

                        Console.WriteLine("deposit");

                        while (true)
                        {
                            // Parsing if entered a letter it will be a 0
                            float.TryParse(Console.ReadLine(), out Depo);

                            if (Depo > 0)
                            {
                                break;
                            }
                            else Console.WriteLine("Enter a valid sum");
                        }


                        //Check if the currency is either kr or $ and make it lower case
                        if (curchoice.ToLower() == "kr" || curchoice == "$")
                        {

                            //Add the account to the accounts dictionary with a default amount at 0 and the choosen currency
                            customer.accounts.Add(newAccChoice, new List<string>() { Depo.ToString(), curchoice.ToLower(), Account });

                            Console.WriteLine($"Account {newAccChoice} was added and it has {Depo}{customer.accounts[newAccChoice][1]} in it");
                            string sendlog = $"{DateTime.Now}: Opened a new account {newAccChoice} with a rate of {Rate}. Deposited {Depo}{customer.accounts[newAccChoice][1]} to the account";
                            Log(customer, sendlog);
                            Console.WriteLine("Example how your Account will grow with your chosen Rate");

                            int index = 0;
                            float interestAmount = Depo * (Rate / 100);
                            float TotalAmount = Depo + interestAmount;
                            // while loop interest rate for 3 yeas
                            while (index < 3)
                            {
                                index++;
                                Console.WriteLine($"year:{index} Amount:{TotalAmount}");
                                interestAmount = TotalAmount * (Rate / 100);
                                TotalAmount = TotalAmount + interestAmount;

                            }

                            Console.ReadKey();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid choice, try again");
                        }

                    }
                    break;
                }




            }

        }

        public static void Log(Customer account, string sendlog)
        {
            //Creates filename by adding a and b together
            string a = account.Name;
            string b = ".txt";
            string filePath = a + b;

            //Creates seperate txt file for exchange rate if ut does not currently exist
            if (!File.Exists(filePath))
            {
                using StreamWriter sw = File.CreateText(filePath);
                sw.Close();
            }
            //Writes text to the file when the method is called 
            File.AppendAllText(filePath, sendlog + Environment.NewLine);

        }
        public static void SeeLog(Customer account)
        {
            //Creates filename by adding a and b together
            string a = account.Name;
            string b = ".txt";
            string filePath = a + b;
            //Writes out all activites of the logged in user
            string text = File.ReadAllText(filePath);
            Console.WriteLine(text);   
        }
    }

}
