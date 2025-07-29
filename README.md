🔐 **Password Manager – C# OOP Project**

A secure, role-based password management system built with C#, MySQL, and AES encryption, developed as part of an Object-Oriented Programming assignment (COS20019).

This application allows users to create accounts, log in, and manage their password vaults. Features include encrypted password storage, password filtering, strong password generation, and role-based permissions (Admin vs Standard users).

🚀 Features

🔐 AES Encryption of all user credentials using BouncyCastle
👥 Role-based Access Control: Admin and Standard users
🧾 User Account Management: Sign up and log in securely
📁 Add / View / Delete Credentials
🔎 Filter Passwords by Category
🔄 Strong Password Generator
🗄️ Persistent Storage with MySQL backend
🧠 Object-Oriented Design

The application follows core OOP principles:

Abstraction: Base User class defines shared behavior for AdminUser and StandardUser
Encapsulation: Sensitive data and encryption logic contained within dedicated methods
Inheritance: Role-specific behavior implemented via class hierarchy
Polymorphism: Shared interfaces and overridden methods for flexible behavior
🧩 Key Classes
Class	Responsibility
User.cs	Abstract base class with shared user methods
AdminUser.cs	Inherits from User, can add/view/delete credentials
StandardUser.cs	Inherits from User, can add/view credentials
Credentials.cs	Stores individual credentials with encrypted password storage
Encryption.cs	Implements IEncryptable interface using AES encryption
PasswordVault.cs	Stores a user’s credentials and handles database interaction
UserDBManager.cs	Handles MySQL user operations: login, signup, validation
CredentialsDBManager.cs	Handles MySQL credential operations: CRUD
PasswordGenerator.cs	Generates secure random passwords
Program.cs	Manages overall program flow and user interaction
🧱 Design Patterns

🏭 Factory Pattern: Used in ValidateUser() to instantiate user types based on access level
🧠 Strategy Pattern: Encryption logic abstracted via IEncryptable interface
🧰 Façade Pattern: PasswordVault class simplifies access to credential management and DB operations
📈 Enhancements Beyond Option 3

Integrated MySQL database for persistent user and credential storage
Applied AES encryption for secure password handling
Implemented strong password generator for enhanced user security
Added filtering functionality for better user experience
Structured program for modularity and scalability using OOP and design patterns
🖥️ Usage

✅ Prerequisites
.NET 6 SDK
MySQL Server
Swinburne VPN access to connect to the Mercury MySQL server (if using Swinburne's server)
▶️ Running the Program
Clone the repository
Open the project in Visual Studio
Configure your database connection in the code (UserDBManager.cs / CredentialsDBManager.cs)
Build and run the project

