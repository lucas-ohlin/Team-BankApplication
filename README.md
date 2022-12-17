# BankApplication - By Team Pineapple 🍍
A group project on making a bank application using C# and the .NET Core Framework.
The [requirements](https://qlok.notion.site/Initial-backlog-03eb10ead8f047b3aff215f8c9b47d0e) provided to us by the project backlog. The planning of the project was made using [Trello](https://trello.com/b/toGN7sg8/bankapplication).

### Contributors
* [Damir Mehic](https://github.com/DammirM)
* [Emil Treptow](https://github.com/Lykrat)
* [John Albrektsson](https://github.com/JohnAlbrektsson)
* [Lucas Persson Öhlin](https://github.com/lucas-ohlin) 

## Installation & Guide For The Application
To install the application either clone the repository using this link:
```sh
https://github.com/lucas-ohlin/Team-BankApplication.git
```
Or download the whole repository as a zip file by clicking the green button and press the `Download ZIP`

When starting the application you'll be met with this window:

![Skärmbild 2022-12-17 175207](https://user-images.githubusercontent.com/113690228/208254615-06f41f54-9858-4fd9-820d-ceb319420a9b.png)

Here you'll need to login using a name and a password of either a customer or admin

If you've logged into a customer account you'll be met with this screen:

![Skärmbild 2022-12-17 184425](https://user-images.githubusercontent.com/113690228/208254665-cfcd97f5-89b2-4de0-b358-f8fe046d8d09.png)

If you've logged into a admin account you'll be met with this screen:

![Skärmbild 2022-12-17 184404](https://user-images.githubusercontent.com/113690228/208254683-550bd7ba-6025-4ae3-a571-eeea36382b01.png)


## Structure Of The Project
<img src="https://user-images.githubusercontent.com/113690228/208114210-1d90ab1f-0639-4aeb-a3d9-fce6f281e871.png" align="right" height="450px">

Program is where the program starts and calls on other classes

Users creates all the users at the start of the program.

Loginhandler is the class that handles the logging function. If the number of tries exceeds 3 then it stops the program from doing anything else. If the user is a customer the navigation menu for customers starts. If the user is an admin it instead starts the admin navigation menu. After successful login it sends the user class to the navigation menu. Also adds time and who logged in in a text log.

NavigationHandler has two different methods. One is for customers and the other is for admins. In the admin menu it writes out 4 different options including admin information, customer creation, change exchange rate and logout. All the options except logout and CustomerCreation calls for methods in the admin class. The customer menu has eight different options. Customerinfo uses a method in Customer class. Logout calls Loginhandler and logs that the user has logged out. The other six are methods in the BankSystem class which includes Openaccount, TransferbetweenAccounts, TransferbetweenCustomers, Loan, SavingsAccount and SeeLog. After the method it runs PressEnter which forces the user to press enter to go back to the main menu. It also sends the Customer class to all methods that need it. PressEnter is called after most methods.

User is the parent class to admin and customer. 

Admin is a child class of User. Name and password is inherited and it is put in a constructor. The method admininfo writes the name and password of the current admin. The second method AdminUpdateRates changes the exchange rate for USD to SEK.

Customer is also a child class of User. Like admin it gets name and password inherited but also consists of a dictionary<string,List<string>> which is called accounts. We also have a constructor that takes name, password and dictionary. There is two methods in here. CustomerInfo writes out the name of the user and the number of accounts the user has. Also writes out all the accounts name, amount of currency, what kind of currency and what kind of account they are. 

BankSystem is the class with most of the bank functions. OpenAccounts opens a new account for the current customer. The customer chooses the name and the type of currency. The name has to be unique. TransferbetweenAccounts lets the customer transfer money between their own accounts. TransferbetweenCusomers lets the customer transfer money between different users. The transfer method calls ExchangeRate which checks for the currency difference and exchange rates. Loan gives customers a way to loan money with interest. the amount loaned can't be more than 5 times the amount the customers total amount of money in the bank. CustomerCreation lets the user create a completely new customer account. SavingsAccount opens a new savings account for the user. Log writes down the users activities in a text file. After the method it runs PressEnter which forces the user to press enter to go back to the main menu. It also sends the Customer class to all methods that need it.

