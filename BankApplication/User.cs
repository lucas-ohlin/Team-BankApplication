using System;
using System.Collections.Generic;

namespace BankApplication {

    /// <summary>
    /// User is an abstract class which is the base of both Customer and Admin
    /// </summary>

    //Abstract since this class is not called and only inherited
    abstract class User {

        //protected, cannot be accessed from a non-derived class from either the same assembly or a different one
        protected string name;
        protected string password;

        public User(string name, string password) {

            this.name = name;
            this.password = password;

        }

    }

}

