using System;
using System.Collections.Generic;
using System.Text;

namespace BankApplication {

    internal class LoginHandler {

        public static void LogIn() {

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"  _______ _            ____            _     ____              _    
 |__   __| |          |  _ \          | |   |  _ \            | |   
    | |  | |__   ___  | |_) | ___  ___| |_  | |_) | __ _ _ __ | | __
    | |  | '_ \ / _ \ |  _ < / _ \/ __| __| |  _ < / _` | '_ \| |/ /
    | |  | | | |  __/ | |_) |  __/\__ \ |_  | |_) | (_| | | | |   < 
    |_|  |_| |_|\___| |____/ \___||___/\__| |____/ \__,_|_| |_|_|\_\
                                                                    
            ");
            Console.ResetColor();
            Console.WriteLine("Welcome to the bank.\nPlease login.");

            //Store locally how many tries have been made by the user
            byte tries = 0;

            //A while do loop if tries is less than 3
            do {

                Console.WriteLine("\nname:");
                string name = Console.ReadLine();

                Console.WriteLine("password:");
                string password = Console.ReadLine();

                //Check if the name and password exist on the same object in the customerList
                if (Users.customerList.Exists(x => x.Name == name && x.Password == password)) {

                    //Sends the correct account to the navmenu for further use
                    Customer account = Users.customerList.Find(x => x.Name == name && x.Password == password);

                    //Logs the information
                    string sendlog = $"{DateTime.Now}: {account.Name} logged in";
                    BankSystem.Log(account, sendlog);

                    NavigationHandler.NavigationMenu(account);
                    break;

                }

                //If there's no such customer check if a admin with the name and password exist in the adminList
                else if (Users.adminList.Exists(x => x.Name == name && x.Password == password)) {

                    //Sends the correct admin to the admin navmenu for further use
                    Admin admin = Users.adminList.Find(x => x.Name == name && x.Password == password);
                    NavigationHandler.AdminNavigationMenu(admin);
                    break;

                }

                //If the name and password doesn't exist in either list, add one to tries   
                else if (!Users.customerList.Exists(x => x.Name == name && x.Password == password) || !Users.adminList.Exists(x => x.Name == name && x.Password == password)) {

                    Console.WriteLine("\nNot a valid user, try again:");
                    tries++;

                }

            } while (tries < 3);

            Console.WriteLine("\nYour three tries are up.");

        }

    }

}
