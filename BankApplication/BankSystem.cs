using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace BankApplication {

    /// <summary>
    /// BankApplication contains most of the methods for the actual project
    /// </summary>

    internal class BankSystem {

        public static void OpenAccount(Customer customer) {

            Console.WriteLine("What do you want to name your new account(between 4 and 20 characters)");

            while (true) {

                string newAccChoice = Console.ReadLine();

                //A character limit between 4-20
                if (newAccChoice.Length > 20 || newAccChoice.Length < 4)
                    Console.WriteLine("The account name needs to be between 4 and 20 characters");

                //Check if the account name already exists
                else if (customer.accounts.ContainsKey(newAccChoice))
                    Console.WriteLine("This account already exists for this user");

                else {

                    while (true) {

                        Console.WriteLine("Which currency do you want to use? \n Available types of currency:\n kr \n $");
                        string curchoice = Console.ReadLine();

                        //Check if the currency is either kr or $ and make it lower case
                        if (curchoice.ToLower() == "kr" || curchoice == "$") {

                            //Add the account to the accounts dictionary with a default amount at 0 and the choosen currency
                            customer.accounts.Add(newAccChoice, new List<string>() { 0.0f.ToString(), curchoice.ToLower(), "Personkonto" });
                            Console.WriteLine($"Account {newAccChoice} was added and it has {customer.accounts[newAccChoice][0]}{customer.accounts[newAccChoice][1]} in it");

                            //Logs the information
                            string sendlog = $"{DateTime.Now}: Account {newAccChoice} was added and it has {customer.accounts[newAccChoice][0]}{customer.accounts[newAccChoice][1]} in it";
                            Log(customer, sendlog);

                            break;

                        } else
                            Console.WriteLine("Invalid choice, try again");
                    }

                    break;

                }

            }

        }

        public static void TransferbetweenAccounts(Customer customer) {

            float transfer;
            string transferTo = "";
            bool run = true;

            Console.Clear();
            do {

                //Method for account names to show the user the alternatives
                customer.AccountName();
                Console.WriteLine("Which account do you want to transfer from: Name of the account");
                string transferFrom = Console.ReadLine();

                //Check if the accounts dictionary contains the correct account name
                if (customer.accounts.ContainsKey(transferFrom) == true)
                {
                    // while loop to get the  first account to transfer money from
                    while (true)
                    {
                        Console.WriteLine($"Amount to transfer from {transferFrom} : {customer.accounts[transferFrom][0]}");

                        // Handling letters and incorrect funds
                        if (!float.TryParse(Console.ReadLine(), out transfer))
                            Console.Clear();

                        //Parsing the account balance and checking if the user has the funds
                        if (transfer > 0 && transfer <= float.Parse(customer.accounts[transferFrom][0]))
                        {
                            Console.Clear();
                            // do while for the answer of account to transfer To
                            do
                            {
                                customer.AccountName();

                                Console.WriteLine("Which of the accounts above do you want to transfer To:");
                                transferTo = Console.ReadLine();

                                if (transferTo == transferFrom)
                                {
                                    Console.WriteLine("You can not Transfer from and to the SAME account");
                                }
                                else if (transferTo != transferFrom && customer.accounts.ContainsKey(transferTo) == true)
                                {
                                    break;
                                }
                                else Console.WriteLine("That account doesnt exist");

                            } while (true);


                            //Check if the accounts contains the name
                            if (customer.accounts.ContainsKey(transferTo) == true)
                            {
                                Console.Clear();
                                //Check currency and send over correct exchange
                                ExchangeRate(customer, customer, transferFrom, transferTo, transfer);

                                Console.WriteLine($"You have succesfully transfered {transfer}{customer.accounts[transferFrom][1]} from " +
                                    $"{transferFrom} to {transferTo}");

                                //Logs the information
                                string sendlog = $"{DateTime.Now}: You have succesfully transfered {transfer}{customer.accounts[transferFrom][1]} from {transferFrom} to {transferTo}";
                                Log(customer, sendlog);

                                customer.AccountName();
                                run = false;
                                break;
                            }
                            else
                                Console.WriteLine("Account not found of the name: " + transferTo);
                        }
                        else
                            Console.WriteLine("Not a valid choice");
                    }

                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Account not found of the name: " + transferFrom);
                }
            } while (run == true);

        }

        public static void TransferBetweenCustomers(Customer customer) {

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
                if (Users.customerList.Exists(x => x.Name == customerName)) {

                    //Create an object using the name
                    customer2 = Users.customerList.Find(x => x.Name == customerName);

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
                else if (!Users.customerList.Exists(x => x.Name == customerName))
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

                    //Check currency and send over correct amount
                    ExchangeRate(customer, customer2, choice, choice2, amount);

                    //Displays what was transfered and the new balance of the logged in customer
                    Console.WriteLine($"\nTransfered {amount}{customer.accounts[choice][1]} from {customer.Name} to {customer2.Name}");

                    //Logs the information
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

        public static void ExchangeRate(Customer customer1, Customer customer2, string account1, string account2, float transfer1) {

            //Creates new variables that is later used
            float transfer2;
            string currency1 = customer1.accounts[account1][1];
            string currency2 = customer2.accounts[account2][1];
            string filePath = "ExchangeRate.txt";

            //Creates seperate txt file for exchange rate if ut does not currently exist
            if (!File.Exists(filePath)) {

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
            Console.WriteLine("The exchange rate was last updated:{0}", Time);
            sr.Close();

            //If its the same currency its sends over the same amount
            if (currency1 == currency2) {
                customer1.accounts[account1][0] = (float.Parse(customer1.accounts[account1][0]) - transfer1).ToString();
                customer2.accounts[account2][0] = (float.Parse(customer2.accounts[account2][0]) + transfer1).ToString();
            }
            //If its not the same and the second one is dollar the amount is divided by the exchange rate
            else if (currency2 == "$") {
                transfer2 = transfer1 / usdToSek;
                customer1.accounts[account1][0] = (float.Parse(customer1.accounts[account1][0]) - transfer1).ToString();
                customer2.accounts[account2][0] = (float.Parse(customer2.accounts[account2][0]) + transfer2).ToString();
            }
            //If its not the same and the second one is swedish crowns the amount is multiplied by the exchange rate
            else if (currency2 == "kr") {
                transfer2 = transfer1 * usdToSek;
                customer1.accounts[account1][0] = (float.Parse(customer1.accounts[account1][0]) - transfer1).ToString();
                customer2.accounts[account2][0] = (float.Parse(customer2.accounts[account2][0]) + transfer2).ToString();
            }

        }

        public static void Loan(Customer customer) {

            double interest = 0.03;

            while (true) {

                Console.WriteLine("How much would you like to loan?");

                //Makes sure only numbers are entered and gets the amount the user wishes to loan
                if (!double.TryParse(Console.ReadLine(), out double loanamount))
                    Console.WriteLine("Numbers only, try again:");

                //Makes sure the user can't enter a number below 0
                if (loanamount <= 0)
                    Console.WriteLine("Please choose an amount above 0");

                if (loanamount > 0) {

                    while (true) {

                        customer.AccountName();
                        Console.WriteLine("Which account would you like to loan to?");
                        string loanto = Console.ReadLine();
                        if (loanto == "") { break; } //return to nav menu by pressing enter

                        if ((double.Parse(customer.accounts[loanto][0]) * 5 > loanamount)) {

                            //Checks if the specified account exists
                            if (customer.accounts.ContainsKey(loanto) == true) {

                                //Displays the interest rate and how much the user will have to pay monthly
                                Console.WriteLine($"The interest rate is currently: {interest * 100}%\nMonthly interest for a loan of {loanamount}{customer.accounts[loanto][1]} is {loanamount * interest}{customer.accounts[loanto][1]}");
                                Console.WriteLine("\nDo you wish to take this loan? Yes or No");

                                while (true) {

                                    string loanchoice = Console.ReadLine();

                                    //If the user decides to take the loan
                                    if (loanchoice.ToUpper() == "YES") {
                                        //Adds the specified amount to the account of choice by user
                                        customer.accounts[loanto][0] = (double.Parse(customer.accounts[loanto][0]) + loanamount).ToString();
                                        Console.WriteLine($"{loanamount}{customer.accounts[loanto][1]} has been added to {loanto}");

                                        //Logs the information
                                        string sendlog = $"{DateTime.Now}: Took a loan of {loanamount}{customer.accounts[loanto][1]} and added it to {loanto}";
                                        Log(customer, sendlog);

                                        break;
                                    }

                                    //Returns the user to the navigation menu if they choose not to take the loan
                                    if (loanchoice.ToUpper() == "NO") { break; } else Console.WriteLine("Invalid answer, please answer Yes or No");

                                }

                                break;
                            } else Console.WriteLine("Account not found, try again");
                        } else Console.WriteLine("You can't take a loan that is 5 times larger than what you currently have on your account");
                    }

                    break;
                }

            }

        }

        public static void CustomerCreation() {

            Console.WriteLine("\nName:");
            string name = Console.ReadLine();

            //A character limit between 1-20
            if (name.Length > 20 || name.Length < 1)
                Console.WriteLine("The account name needs to be between 1 and 20 characters");

            //Check if the account name already exists
            else if (Users.customerList.Exists(x => x.Name == name))
                Console.WriteLine("This account already exists for this user");

            Console.WriteLine("\nPassword:");
            string password = Console.ReadLine();

            //Creates a new customer object and adds it to the customerList
            Users.customerList.Add(new Customer(name, password, new Dictionary<string, List<string>>()));

        }

        public static void PressEnter() {

            Console.WriteLine("\ntryck på ENTER för meny");

            ConsoleKeyInfo x;
            do {
                x = Console.ReadKey();
            }
            while (x.Key != ConsoleKey.Enter);
            Console.Clear();

        }

        public static void SavingsAccount(Customer customer) {

            Console.Clear();
            float Rate = 0;
            string AccountName = "";

            // A Tuple over available type of savings accounts that the bank has to offer
            Tuple<string, string, float> Vacation = new Tuple<string, string, float>("1", "Vacation", 20f);
            Tuple<string, string, float> Pension = new Tuple<string, string, float>("2", "Pension", 2.42f);
            Tuple<string, string, float> Childsavings = new Tuple<string, string, float>("3", "Childsavings", 3.49f);

            Console.WriteLine("What type of account do you want to open: Type nr");
            Console.WriteLine($"{Vacation.Item1}.{Vacation.Item2} Rate: {Vacation.Item3} %");
            Console.WriteLine($"{Pension.Item1}.{Pension.Item2} Rate: {Pension.Item3} %");
            Console.WriteLine($"{Childsavings.Item1}.{Childsavings.Item2} Rate: {Childsavings.Item3} %");

            bool run = true;
            // Depending on choice i take out the chosen Account
            do {

                string choice = Console.ReadLine();

                switch (choice) {
                    case "1":
                        AccountName = Vacation.Item2;
                        Rate = Vacation.Item3;
                        run = false;
                        break;
                    case "2":
                        AccountName = Pension.Item2;
                        Rate = Pension.Item3;
                        run = false;
                        break;
                    case "3":
                        AccountName = Childsavings.Item2;
                        Rate = Childsavings.Item3;
                        run = false;
                        break;
                    default:
                        Console.WriteLine("Option 1-3");
                        break;
                }

            } while (run);
            Console.Clear();
            Console.WriteLine("What do you want to name your new account(between 2 and 10 characters)");

            while (true) {

                string newAccChoice = Console.ReadLine();

                //A character limit between 2 - 10
                if (newAccChoice.Length > 10 || newAccChoice.Length < 2)
                    Console.WriteLine("The account name needs to be between 2 and 10 characters");

                //Check if the account name already exists
                else if (customer.accounts.ContainsKey(newAccChoice))
                    Console.WriteLine("You already have an account with this name");

                else {

                    while (true) {

                        float Depo;
                        Console.Clear();
                        Console.WriteLine("Which currency do you want to use?\nAvailable types of currency:\nkr \n$");
                        string curchoice = Console.ReadLine();

                        //If curency is correct, and ToLower to avoid misunderstandings
                        if (curchoice.ToLower() == "kr" || curchoice == "$") {
                            Console.Clear();
                            Console.WriteLine("What amount do you want to deposit");

                            while (true)
                            {
                                // Parsing
                                float.TryParse(Console.ReadLine(), out Depo);

                                if (Depo > 0)
                                    break;
                                else
                                    Console.WriteLine("Enter a valid sum");

                            }

                            //Add the account to the accounts dictionary with a default amount at 0 and the choosen currency
                            customer.accounts.Add(newAccChoice, new List<string>() { Depo.ToString(), curchoice.ToLower(), AccountName });
                            Console.Clear();
                            Console.WriteLine($"Account {newAccChoice} was added and it has {Depo}{customer.accounts[newAccChoice][1]} in it.");
                            Console.WriteLine("Press any button to continue and see your rate"); Console.ReadKey(); Console.Clear();
                            Console.WriteLine("Example how your Account will grow with your chosen rate.");

                            //Logs the information
                            string sendlog = $"{DateTime.Now}: Account {newAccChoice} was added and it has {Depo}{customer.accounts[newAccChoice][1]} in it, and a rate of {Rate} %";
                            Log(customer, sendlog);

                            int index = 0;
                            float interestAmount = Depo * (Rate / 100);
                            float TotalAmount = Depo + interestAmount;

                            // while loop interest rate for 3 yeas
                            while (index < 3) {

                                index++;
                                Console.WriteLine($"year:{index} Amount:{TotalAmount}");
                                interestAmount = TotalAmount * (Rate / 100);
                                TotalAmount = TotalAmount + interestAmount;
                            }
                            break;
                        } else
                            Console.WriteLine("Invalid currency choice..... Loading choices"); 
                             
                             // finess for fun
                             for (int i = 3; i >= 1; i--)
                             {
                               Console.WriteLine(i);
                               Thread.Sleep(700);
                             }
                    }
                    break;
                }

            }

        }

        public static void Log(Customer account, string sendlog) {

            //Creates filename by adding a and b together
            string a = account.Name;
            string b = ".txt";
            string filePath = a + b;

            //Creates seperate txt file for exchange rate if ut does not currently exist
            if (!File.Exists(filePath)) {
                using StreamWriter sw = File.CreateText(filePath);
                sw.Close();
            }

            //Writes text to the file when the method is called 
            File.AppendAllText(filePath, sendlog + Environment.NewLine);

        }

        public static void SeeLog(Customer account) {

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
