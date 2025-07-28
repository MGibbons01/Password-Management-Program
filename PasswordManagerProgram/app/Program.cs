using System;

namespace PasswordManagerProgram
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //connect to Database through DatabaseManager
            string connStr = "server=ictstu-db1.cc.swin.edu.au;port=3306;user=s105335503;password=Mgibbo123!;database=s105335503_db;";
            IEncryptable encryptor = new Encryption();
            var userDb = new UserDBManager(connStr, encryptor);
            var credDb = new CredentialsDBManager(connStr, encryptor);

            User currentUser = null;

        // Global loop
            while (true)
            {
                // Login / Signup loop
                while (currentUser == null)
                {
                    // Login or make account
                    Console.WriteLine("=========== Welcome to Password Manager (PSM) ==============");
                    Console.WriteLine("1. Log in");
                    Console.WriteLine("2. Create Account");
                    Console.WriteLine("3. Exit");
                    Console.Write("Choice: ");
                    string choice1 = Console.ReadLine();

                    // Log in option
                    if (choice1 == "1")
                    {
                        Console.Write("Enter username: ");
                        string username = Console.ReadLine();

                        Console.Write("Enter password: ");
                        string password = Console.ReadLine();

                        currentUser = userDb.ValidateUser(username, password);

                        if (currentUser == null)
                        {
                            Console.WriteLine("\nInvalid username or password.\n");
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("\nLogin successful!");
                            break;
                        }
                    }
                    // Sign Up option
                    else if (choice1 == "2")
                    {

                        // Choice access level    
                        Console.WriteLine("Select your new accounts access level:");
                        Console.WriteLine("1. User");
                        Console.WriteLine("2. Admin");
                        Console.Write("Choice: ");
                        string choice = Console.ReadLine();

                        // Uses the AccessLevel Enumeration
                        AccessLevel level;
                        if (choice == "1")
                        {
                            level = AccessLevel.Standard;
                        }
                        else if (choice == "2")
                        {
                            level = AccessLevel.Admin;
                        }
                        else
                        {
                            Console.WriteLine("Invalid access level. Defaulting to Standard User.");
                            level = AccessLevel.Standard;
                        }

                        Console.Write("Enter your new account's username: ");
                        string username = Console.ReadLine();

                        Console.Write("Enter your new account's password: ");
                        string password = Console.ReadLine();

                        // temporary userid
                        int newUserId = 0;

                        switch (level)
                        {
                            case AccessLevel.Standard:
                                currentUser = new StandardUser(newUserId, username, password, userDb, credDb);
                                break;
                            case AccessLevel.Admin:
                                currentUser = new AdminUser(newUserId, username, password, userDb, credDb);
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Defaulting to User.");
                                currentUser = new StandardUser(newUserId, username, password, userDb, credDb);
                                break;
                        }
                        //calls the Save User method to create new user
                        userDb.SaveUser(currentUser);
                        currentUser.Vault = new PasswordVault(credDb, currentUser.UserId);
                        Console.WriteLine("\nSuccessfully created new account!\n");
                        break;
                    }
                    // Exit option
                    else if (choice1 == "3")
                    {
                        Console.WriteLine("\nGoodbye!\n");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice. Please choose 1 or 2.");
                        continue;
                        //create while loop
                    }
                }

            // Main menu loop
                while (currentUser != null)
                {
                    Console.WriteLine("\n=========== Main Menu ===========\n");
                    Console.WriteLine("1. View All Passwords");
                    Console.WriteLine("2. Add New Password");
                    Console.WriteLine("3. Filter Passwords by Category");

                    // Admin user vs Standard user main menu
                    if (currentUser.AccessLevel == AccessLevel.Admin)
                    {
                        Console.WriteLine("4. Delete Password");
                        Console.WriteLine("5. Log Out");
                        Console.WriteLine("6. Exit");
                    }
                    else
                    {
                        Console.WriteLine("4. Log Out");
                        Console.WriteLine("5. Exit");
                    }
                    
                    Console.Write("Choice: ");
                    string mainChoice = Console.ReadLine();

                    switch (mainChoice)
                    {
                        case "1":
                            // Lists all credentials in table
                            Console.WriteLine("\n=========================== STORED CREDENTIALS ===========================");
                            Console.WriteLine("| {0,-15} | {1,-15} | {2,-20} | {3,-12} |", "Site", "Username", "Password", "Category");
                            Console.WriteLine(new string('-', 75));

                            foreach (var cred in currentUser.Vault.GetAllCredentials())
                            {
                                Console.WriteLine("| {0,-15} | {1,-15} | {2,-20} | {3,-12} |",
                                    cred.Site,
                                    cred.Username,
                                    cred.GetPassword(),
                                    cred.PasswordCategory);
                            }
                            Console.WriteLine(new string('-', 75));

                            Console.WriteLine("\nPress Enter to return to the main menu...");
                            Console.ReadLine();
                            break;

                        case "2":
                            // Adds a new credential
                            Console.Write("\nEnter site name: ");
                            string site = Console.ReadLine();

                            Console.Write("Enter username: ");
                            string siteUsername = Console.ReadLine();

                            Console.WriteLine("Select password type: ");
                            Console.WriteLine("1. Enter custom password: ");
                            Console.WriteLine("2. Generate a strong password: ");
                            string passwordChoice = Console.ReadLine();

                            string sitePassword;

                            switch (passwordChoice)
                            {
                                case "1":
                                    Console.Write("Enter password: ");
                                    sitePassword = Console.ReadLine();
                                    break;

                                case "2":
                                    Console.WriteLine("Generating Strong Password...");
                                    sitePassword = PasswordGenerator.GenerateStrongPassword();
                                    break;

                                default:
                                    Console.WriteLine("Invalid choice.");

                                    continue;
                            }
                            Console.Write("Enter category (e.g., Email, Social, Banking): ");
                            string category = Console.ReadLine();

                            Credentials newCredential = new Credentials(site, siteUsername, sitePassword, category, encryptor, currentUser.UserId);
                            currentUser.AddCredential(newCredential);

                            Console.WriteLine("\nCredential added successfully.\n");
                            break;


                        case "3":
                            Console.Write("Enter category to filter by (e.g., Email, Social, Banking): ");
                            string filterCategory = Console.ReadLine();
                            Console.WriteLine($"\nAll credentials in category: {filterCategory}");
                            currentUser.FilterCredentials(filterCategory);

                            Console.WriteLine("\nPress Enter to return to the main menu...");
                            Console.ReadLine();
                            break;

                        case "4":

                            if (currentUser.AccessLevel == AccessLevel.Admin)
                            {
                                Console.WriteLine("Choose a site to delete: ");
                                string siteToDelete = Console.ReadLine();

                                AdminUser admin = (AdminUser)currentUser;
                                bool success = admin.DeleteCredential(siteToDelete);

                                if (success)
                                {
                                    Console.WriteLine("Credential deleted successfully.");
                                }
                                else
                                {
                                    Console.WriteLine("Credential not found.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("\nLogging Out...");
                                currentUser = null;

                            }
                            break;

                        case "5":
                            if (currentUser.AccessLevel == AccessLevel.Admin)
                            {
                                Console.WriteLine("\nLogging Out...");
                                currentUser = null;
                            }
                            else
                            {
                                Console.WriteLine("\nLogging Out...");
                                Console.WriteLine("Goodbye!\n");
                                return;
                            }
                            break;

                         case "6":
                            if (currentUser.AccessLevel == AccessLevel.Admin)
                            {
                                Console.WriteLine("\nLogging Out...");
                                Console.WriteLine("Goodbye!\n");
                                return;
                            }
                            else
                            {
                                Console.WriteLine("Invalid choice.");

                            }
                            return;

                        default:
                            Console.WriteLine("Invalid choice.");

                            break;
                    }
                }
            }

        }
    }
}