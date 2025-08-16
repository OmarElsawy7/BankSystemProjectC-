using session7C_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace session7C_
{
    class Bank
    {
        public string Name { get; private set; }
        public string BranchCode { get; private set; }

        private Customer[] _customers = new Customer[Limits.MaxCustomers];
        private int _custCount = 0;

        public Bank(string name, string branchCode)
        {
            Name = name;
            BranchCode = branchCode;
        }

        // --------- Menu ---------
        public void Run()
        {
            while (true)
            {
                Console.WriteLine($"\n=== {Name} ({BranchCode}) ===");
                Console.WriteLine("1) Add Customer");
                Console.WriteLine("2) Update Customer (name/DOB)");
                Console.WriteLine("3) Remove Customer (all accounts must be zero)");
                Console.WriteLine("4) Search Customer (name / national ID)");
                Console.WriteLine("5) Open Account (Savings / Current)");
                Console.WriteLine("6) Deposit");
                Console.WriteLine("7) Withdraw");
                Console.WriteLine("8) Transfer");
                Console.WriteLine("9) Show Customer Total Balance");
                Console.WriteLine("10) Apply Monthly Interest (Savings)");
                Console.WriteLine("11) Show Bank Report");
                Console.WriteLine("12) Show Account Transactions");
                Console.WriteLine("0) Exit");
                Console.Write("Choose: ");
                string ch = Console.ReadLine();

                try
                {
                    switch (ch)
                    {
                        case "1": MenuAddCustomer(); break;
                        case "2": MenuUpdateCustomer(); break;
                        case "3": MenuRemoveCustomer(); break;
                        case "4": MenuSearchCustomer(); break;
                        case "5": MenuOpenAccount(); break;
                        case "6": MenuDeposit(); break;
                        case "7": MenuWithdraw(); break;
                        case "8": MenuTransfer(); break;
                        case "9": MenuShowTotalBalance(); break;
                        case "10": MenuApplyMonthlyInterest(); break;
                        case "11": BankReport(); break;
                        case "12": MenuShowTransactions(); break;
                        case "0": return;
                        default: Console.WriteLine("Invalid choice."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

       
        public bool AddCustomer(string fullName, string nationalId, DateTime dob)
        {
            if (_custCount >= Limits.MaxCustomers) { Console.WriteLine("Customer capacity reached."); 
                return false; 
            }
            if (FindCustomerByNationalId(nationalId) != null) { Console.WriteLine("National ID already exists."); return false; }

            _customers[_custCount++] = new Customer(fullName, nationalId, dob);
            return true;
        }

        public Customer FindCustomerById(int id)
        {
            for (int i = 0; i < _custCount; i++)
                if (_customers[i].Id == id) return _customers[i];
            return null;
        }

        public Customer FindCustomerByNationalId(string nid)
        {
            for (int i = 0; i < _custCount; i++)
                if (_customers[i].NationalId == nid) return _customers[i];
            return null;
        }

        public void SearchCustomers(string keyword)
        {
            bool found = false;
            for (int i = 0; i < _custCount; i++)
            {
                if (_customers[i].FullName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    _customers[i].NationalId.Equals(keyword, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(_customers[i].Summary());
                    found = true;
                }
            }
            if (!found) Console.WriteLine("No matching customers.");
        }

        public bool UpdateCustomer(int id, string newName, DateTime newDob)
        {
            Customer c = FindCustomerById(id);
            if (c == null) return false;
            c.FullName = newName;
            c.DateOfBirth = newDob;
            return true;
        }

        public bool RemoveCustomer(int id)
        {
            for (int i = 0; i < _custCount; i++)
            {
                if (_customers[i].Id == id)
                {
                    if (!_customers[i].AllAccountsZero())
                    {
                        Console.WriteLine("Cannot remove: customer has non-zero account(s).");
                        return false;
                    }
                   
                    _customers[i] = _customers[_custCount - 1];
                    _customers[_custCount - 1] = null;
                    _custCount--;
                    return true;
                }
            }
            return false;
        }

       
        public bool OpenSavingsAccount(int customerId, decimal interestPercent, out int newAccNumber)
        {
            newAccNumber = -1;
            Customer c = FindCustomerById(customerId);
            if (c == null) return false;

            Account acc = new SavingsAccount(interestPercent);
            if (!c.AddAccount(acc)) return false;
            newAccNumber = acc.AccountNumber;
            return true;
        }

        public bool OpenCurrentAccount(int customerId, decimal overdraftLimit, out int newAccNumber)
        {
            newAccNumber = -1;
            Customer c = FindCustomerById(customerId);
            if (c == null) return false;

            Account acc = new CurrentAccount(overdraftLimit);
            if (!c.AddAccount(acc)) return false;
            newAccNumber = acc.AccountNumber;
            return true;
        }

        public Account FindAccount(int accountNumber)
        {
            for (int i = 0; i < _custCount; i++)
            {
                Account acc = _customers[i].FindAccountByNumber(accountNumber);
                if (acc != null) return acc;
            }
            return null;
        }

        
        public void BankReport()
        {
            Console.WriteLine($"\n--- Bank Report: {Name} / {BranchCode} ---");
            if (_custCount == 0) { Console.WriteLine("No customers."); return; }

            for (int i = 0; i < _custCount; i++)
            {
                Customer c = _customers[i];
                Console.WriteLine(c.Summary());
                c.PrintAccounts();
            }
        }

        
        private void MenuAddCustomer()
        {
            Console.Write("Full Name: "); var name = Console.ReadLine();
            Console.Write("National ID: "); var nid = Console.ReadLine();
            Console.Write("Date of Birth (yyyy-mm-dd): "); 
            DateTime dob = DateTime.Parse(Console.ReadLine());

            if (AddCustomer(name, nid, dob)) Console.WriteLine("Customer added.");
        }

        private void MenuUpdateCustomer()
        {
            int id = ReadInt("Customer ID: ");
            Console.Write("New Full Name: "); 
            string name = Console.ReadLine();
            Console.Write("New DOB (yyyy-mm-dd): "); 
            DateTime dob = DateTime.Parse(Console.ReadLine());

            Console.WriteLine(UpdateCustomer(id, name, dob) ? "Updated." : "Customer not found.");
        }

        private void MenuRemoveCustomer()
        {
            int id = ReadInt("Customer ID: ");
            Console.WriteLine(RemoveCustomer(id) ? "Removed." : "Remove failed.");
        }

        private void MenuSearchCustomer()
        {
            Console.Write("Search by Name or National ID: ");
            string key = Console.ReadLine();
            SearchCustomers(key);
        }

        private void MenuOpenAccount()
        {
            int id = ReadInt("Customer ID: ");
            Console.Write("Type (S = Savings, C = Current): ");
            string t = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

            if (t == "S")
            {
                decimal rate = ReadDecimal("Interest Rate (%) e.g. 5: ");
                if (OpenSavingsAccount(id, rate, out int accNo))
                    Console.WriteLine($"Savings Account opened. #{accNo}");
                else Console.WriteLine("Open failed.");
            }
            else if (t == "C")
            {
                decimal od = ReadDecimal("Overdraft Limit: ");
                if (OpenCurrentAccount(id, od, out int accNo))
                    Console.WriteLine($"Current Account opened. #{accNo}");
                else Console.WriteLine("Open failed.");
            }
            else Console.WriteLine("Invalid type.");
        }

        private void MenuDeposit()
        {
            int acc = ReadInt("Account Number: ");
            decimal amt = ReadDecimal("Amount: ");
            var a = FindAccount(acc);
            if (a == null) { Console.WriteLine("Account not found."); return; }
            a.Deposit(amt);
            Console.WriteLine("Deposited.");
        }

        private void MenuWithdraw()
        {
            int acc = ReadInt("Account Number: ");
            decimal amt = ReadDecimal("Amount: ");
            var a = FindAccount(acc);
            if (a == null) { Console.WriteLine("Account not found."); return; }
            Console.WriteLine(a.Withdraw(amt) ? "Withdrawn." : "Withdraw failed.");
        }

        private void MenuTransfer()
        {
            int from = ReadInt("From Account #: ");
            int to = ReadInt("To Account #: ");
            decimal amt = ReadDecimal("Amount: ");

            var a1 = FindAccount(from);
            var a2 = FindAccount(to);
            if (a1 == null || a2 == null) { Console.WriteLine("Account not found."); return; }

            Console.WriteLine(a1.TransferTo(a2, amt) ? "Transferred." : "Transfer failed.");
        }

        private void MenuShowTotalBalance()
        {
            int id = ReadInt("Customer ID: ");
            var c = FindCustomerById(id);
            if (c == null) { Console.WriteLine("Customer not found."); return; }
            Console.WriteLine($"Total Balance = {c.TotalBalance():0.00}");
        }

        private void MenuApplyMonthlyInterest()
        {
            int accNo = ReadInt("Savings Account #: ");
            var acc = FindAccount(accNo) as SavingsAccount;
            if (acc == null)
            {
                Console.WriteLine("Savings account not found."); return;
            }
            decimal interest = acc.CalculateMonthlyInterest();
            acc.ApplyMonthlyInterest();
            Console.WriteLine($"Applied monthly interest: {interest:0.00}");
        }

        private void MenuShowTransactions()
        {
            int accNo = ReadInt("Account #: ");
            Account acc = FindAccount(accNo);
            if (acc == null) { Console.WriteLine("Account not found."); 
                return; 
            }
            acc.PrintTransactions();
        }

        // Helpers
        private static int ReadInt(string prompt)
        {
            Console.Write(prompt);
            return int.Parse(Console.ReadLine() ?? "0");
        }

        private static decimal ReadDecimal(string prompt)
        {
            Console.Write(prompt);
            return decimal.Parse(Console.ReadLine() ?? "0");
        }
    }

}
