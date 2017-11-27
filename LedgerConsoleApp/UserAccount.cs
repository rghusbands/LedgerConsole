/* 
 * World's Greatest Banking Ledger Code Sample
 * 
 * Sample by: Robert Husbands
 * 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LedgerConsoleApp
{
    /* The UserAccount class is an object that holds all user variables and necessary methods
     * Has methods to perform Deposit, Withdrawal, Check Balance, and Transaction History
     */
    public class UserAccount
    {
        // Necessary class variables
        public string username { get; }
        private string password { get; }
        private decimal balance { get; set; }
        private List<string> transactionsList { get; set; }

        // UserAccount Contructor
        public UserAccount(string name, string pswd)
        {
            this.username = name;
            this.password = pswd;
            this.balance = 0;
            this.transactionsList = new List<string>();
        }

        // Deposit user defined quantity into account
        public void Deposit()
        {
            Console.WriteLine("Starting a deposit...");
            Console.Write("Enter the deposit amount: ");

            // Grab user inputted deposit as a string
            string str_deposit = Console.ReadLine();

            // Attempt to convert input into a decimal. Catch failure if user inputs something that doesn't make sense
            try
            {
                // Conversion from string to decimal
                decimal deposit = decimal.Parse(str_deposit, System.Globalization.NumberStyles.Currency);

                // Ensure input is a positive value
                if (deposit < 0)
                {
                    throw new Exception();
                }

                // Add input to user balance
                this.balance += deposit;

                // Gather transaction information. Display and add to history
                string transaction = DecimalToString(deposit) + " deposited";
                this.AddTransaction(transaction);
                Console.WriteLine(transaction + " into your account.");
            }
            catch (Exception)
            {
                Console.WriteLine("Deposit Failed. Please use positive numbers only.");
            }
        }

        // Withdraw user definted quantity into account
        public void Withdrawal()
        {
            Console.WriteLine("Starting a withdrawal...");
            Console.Write("Enter the withdrawal amount: ");

            // Grab user inputted withdrawal as a string
            string str_withdrawal = Console.ReadLine();

            // Attempt to convert input into a decimal. Catch failure if user inputs something that doesn't make sense
            try
            {
                decimal withdrawal = decimal.Parse(str_withdrawal, System.Globalization.NumberStyles.Currency);
                if (withdrawal < 0)
                {
                    throw new Exception();
                }
                this.balance -= withdrawal;
                string transaction = DecimalToString(withdrawal) + " withdrawn";
                this.AddTransaction(transaction);
                Console.WriteLine(transaction + " from your account.");
            }
            catch (Exception)
            {
                Console.WriteLine("Withdrawal Failed. Please use positive numbers only.");
            }
        }

        // Display current balance of user account
        public void CheckBalance()
        {
            Console.WriteLine("Balance: {0}", DecimalToString(this.balance));
        }

        // Add a transaction to the transaction list
        public void AddTransaction(string msg)
        {
            // Simply format the strings to show necessary information and combine them together
            string transaction = "Transaction: " + msg;
            string date = "Date: " + DateTime.Now.ToString("HH:mm:ss MM-dd-yyyy");
            string curBalance = "Balance: " + DecimalToString(this.balance);
            string report = transaction + "          " +
                date + "         " + curBalance;

            // Add to the current list
            this.transactionsList.Add(report);
        }

        // Displays user transaction history
        public void History()
        {
            Console.WriteLine("Checking history...");

            // If empty notify the user that no transaction have occured yet
            if (this.transactionsList.Count == 0)
                Console.WriteLine("No transations yet");

            // Iterate through transaction list and display transactions to user
            foreach (string item in this.transactionsList)
            {
                Console.WriteLine(" - " + item);
            }
            Console.Write("\n");
        }

        // Simply convert decimal into a currency format
        private static string DecimalToString(decimal num)
        {
            return ("$" + num.ToString("#,##0.00"));
        }

        // Simple password check when user tries to log in
        public bool CheckPassword(string pswd)
        {
            if (pswd.Equals(this.password))
                return true;
            return false;
        }
    }
}