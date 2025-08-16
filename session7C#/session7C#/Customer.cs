using session7C_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace session7C_
{
    class Customer
    {
        private static int _idSeed = 0;

        public int Id { get; private set; }
        public string FullName { get; set; }
        public string NationalId { get; private set; }
        public DateTime DateOfBirth { get; set; }

        private Account[] _accounts = new Account[Limits.MaxAccountsPerCustomer];
        private int _accCount = 0;

        public Customer(string fullName, string nationalId, DateTime dob)
        {
            Id = ++_idSeed;
            FullName = fullName;
            NationalId = nationalId;
            DateOfBirth = dob;
        }

        public bool AddAccount(Account acc)
        {
            if (_accCount >= Limits.MaxAccountsPerCustomer) return false;
            _accounts[_accCount++] = acc;
            return true;
        }

        public int AccountCount => _accCount;

        public Account GetAccountByIndex(int index)
        {
            if (index < 0 || index >= _accCount) return null;
            return _accounts[index];
        }

        public Account FindAccountByNumber(int accountNumber)
        {
            for (int i = 0; i < _accCount; i++)
                if (_accounts[i].AccountNumber == accountNumber) return _accounts[i];
            return null;
        }

        public decimal TotalBalance()
        {
            decimal total = 0;
            for (int i = 0; i < _accCount; i++)
                total += _accounts[i].Balance;
            return total;
        }

        public bool AllAccountsZero()
        {
            for (int i = 0; i < _accCount; i++)
                if (_accounts[i].Balance != 0) 
            return false;
            return true;
        }

        public string Summary()
        {
            return $"[{Id}] {FullName} | NID: {NationalId} | DOB: {DateOfBirth:yyyy-MM-dd} | Total: {TotalBalance():0.00}";
        }

        public void PrintAccounts()
        {
            if (_accCount == 0) { Console.WriteLine("  No accounts."); return; }
            for (int i = 0; i < _accCount; i++)
                Console.WriteLine($"  {i + 1}. {_accounts[i].Summary()}");
        }
    }
}
