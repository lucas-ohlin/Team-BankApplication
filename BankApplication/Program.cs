using System;
using System.Collections.Generic;

namespace BankApplication {

    internal class Program {

        private static void Main(string[] args) {

            //Creation of the 3 customers using the method from BankSystem as well as an Admin
            Users.DefaultUserCreation();

            //Calls the login method from the BankSystem class
            LoginHandler.LogIn();
        
        }

    }

}
