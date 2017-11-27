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
using System.Runtime.Caching;

namespace LedgerConsoleApp
{
    /* 
     * Driver class runs console based ledger system.
     * Loops to allow for any number of users and transactions to occur
     * Stores user account info in MemoryCache in place of database 
     */
    class Driver
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            Run();
            Console.WriteLine("Terminating...");
        }

        // Uses a while loop that collects user inputs indefinitely until users exit the application
        static void Run()
        {
            // Boolean to keep system running
            bool KeepGoing = true;

            // Current user
            UserAccount curUser = null;

            // Simple intro
            Console.WriteLine("Hello. Welcome to the World's Greatest Banking Ledger!" +
                "\nNote: Commands are case sensitive.");

            // Use local cache to store user information. Helps keep threadsafe for future
            MemoryCache cache = MemoryCache.Default;

            // While loop to allow the system to keep running until users Exit
            while (KeepGoing)
            {
                // Ensure user is logged in before continuing further
                if (curUser == null)
                {
                    Console.WriteLine("Currently logged out. Please log in before using commands!");
                    curUser = Login(cache);
                }

                // Inform user of available options 
                Console.WriteLine("Commands are \"Deposit\", \"Withdrawal\", \"CheckBalance\", \"History\", " +
                    "\"Logout\", and \"Exit\"");

                // Get command from user
                Console.Write("({0}) > ", curUser.username);
                string cmd = Console.ReadLine();

                // Completely terminate program with Exit
                if (cmd.Equals("Exit"))
                {
                    KeepGoing = false;
                }

                // Check if command is valid and perform desired UserAccount method
                else if (cmd.Equals("Logout") || cmd.Equals("Deposit") || cmd.Equals("Withdrawal") ||
                    cmd.Equals("CheckBalance") || cmd.Equals("History"))
                {
                    if (cmd.Equals("Logout"))
                    {
                        curUser = null;
                        Console.WriteLine("Logged Out!");
                    }

                    if (cmd.Equals("Deposit"))
                    {
                        curUser.Deposit();
                    }

                    if (cmd.Equals("Withdrawal"))
                    {
                        curUser.Withdrawal();
                    }

                    if (cmd.Equals("CheckBalance"))
                    {
                        curUser.CheckBalance();
                    }

                    if (cmd.Equals("History"))
                    {
                        curUser.History();
                    }

                }

                // Invalid command if it gets here
                else
                {
                    Console.WriteLine("Invalid Command. Please try again.");
                }
            }
        }

        // Login method to handle user logins and ensure it's done properly
        static UserAccount Login(MemoryCache cache)
        {
            Console.Write("Username (type \"Create\" to create account): ");
            string userName = Console.ReadLine();

            // Create account if user types "Create"
            if (userName.Equals("Create"))
            {
                // Get a new username
                Console.Write("Creating user account...\nType new username: ");
                string newUser = Console.ReadLine();

                // Make sure someone doesn't use the username "Create"
                while (newUser.Equals("Create") || cache.Contains(newUser))
                {
                    Console.WriteLine("Invalid username (Could already be in use)");
                    Console.Write("Creating user account...\nType new username: ");
                    newUser = Console.ReadLine();
                }

                // Confirm new password
                string newPswd = NewPassword();

                // Create user and return UserAccount
                return CreateUserAccount(newUser, newPswd, cache);
            }

            // Check if username is in system
            if (!cache.Contains(userName))
            {
                Console.WriteLine("Username is not in system. Please try again");
            }

            // Get UserAccount if username is in system
            else
            {
                // Get password from user
                Console.Write("Password for {0}: ", userName);
                string userPswd = Console.ReadLine();

                // Get UserAccount from storage
                UserAccount user = (UserAccount) cache.Get(userName);

                // Check if the account password matches inputted password
                if (user.CheckPassword(userPswd))
                    return user;
                else
                    Console.WriteLine("Incorrect Password");
            }

            // Rerun login if fails
            return Login(cache);
        }

        // Add a new user to UserAccount storage
        static UserAccount CreateUserAccount(string name, string pswd, MemoryCache cache)
        {
            UserAccount user = new UserAccount(name, pswd);

            // Prevent users from being removed
            CacheItemPolicy policy = new CacheItemPolicy { Priority = CacheItemPriority.NotRemovable };

            cache.Add(name, user, policy);
            return user;
        }

        // Gets and confirms a new user password
        static string NewPassword()
        {
            // Initialize passwords to be different
            string pswd1 = "0", pswd2 = "1";

            // Loop while passwords are different to ensure proper password input
            while (!pswd1.Equals(pswd2))
            {
                // User inputs new password
                Console.Write("Enter Password: ");
                pswd1 = Console.ReadLine();

                // User confirms new password
                Console.Write("Confirm Password: ");
                pswd2 = Console.ReadLine();
                
                // If passwords are different then notify user and go back through loop
                if (!pswd1.Equals(pswd2))
                {
                    Console.WriteLine("Passwords didn't match. Please re-enter.");
                }
            }

            // Return confirmed password
            return pswd1;
        }
    }
}