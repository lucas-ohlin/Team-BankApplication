using System;
using System.Collections.Generic;

namespace BankApplication {

    internal class Program {

        private static void Main(string[] args) {

            //TEMP INFORMATION

            //Default Customer logins
            //Tobias - 111
            //Anas - 222
            //Lucas - 333

            //Default Admin Login
            //Gustav - 000

            //Creation of the 3 customers using the method from BankSystem as well as an Admin
            BankSystem.DefaultUserCreation();

            //Calls the login method from the BankSystem class
            BankSystem.LogIn();
        
        }

    }

}
