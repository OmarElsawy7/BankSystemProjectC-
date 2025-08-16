using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    abstract class Account
    {

        private static int _accountSeed = 1000;


        public int AccountNumber { get; private set; }
        public decimal Balance { get; protected set; }
        public DateTime DateOpened { get; private set; }


        protected Transaction[] _transactions = new Transaction[Limits.MaxTransactionsPerAccount];
        protected int _txCount = 0;

        protected Account()
        {
            AccountNumber = ++_accountSeed;
            DateOpened = DateTime.Now;
            Balance = 0m;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0) { Console.WriteLine("Amount must be positive."); return; }
            Balance += amount;
            AddTx("Deposit", amount, "");
        }

        public virtual bool Withdraw(decimal amount)
        {
            if (amount <= 0) { Console.WriteLine("Amount must be positive."); return false; }
            if (amount > Balance) { Console.WriteLine("Insufficient funds."); return false; }

            Balance -= amount;
            AddTx("Withdraw", amount, "");
            return true;
        }

        public bool TransferTo(Account target, decimal amount)
        {
            if (ReferenceEquals(this, target)) { Console.WriteLine("Cannot transfer to the same account."); return false; }
            if (!Withdraw(amount)) return false;

            target.Deposit(amount);
            AddTx("Transfer Out", amount, $"To {target.AccountNumber}");
            target.AddTx("Transfer In", amount, $"From {AccountNumber}");
            return true;
        }

        protected void AddTx(string type, decimal amount, string note)
        {
            if (_txCount >= Limits.MaxTransactionsPerAccount) return;
            _transactions[_txCount++] = new Transaction
            {
                Type = type,
                Amount = amount,
                Date = DateTime.Now,
                Note = note
            };
        }

        public void PrintTransactions()
        {
            Console.WriteLine($"\nTransactions for Account #{AccountNumber} (Opened: {DateOpened:yyyy-MM-dd})");
            if (_txCount == 0) { Console.WriteLine("No transactions yet."); return; }
            for (int i = 0; i < _txCount; i++)
                Console.WriteLine(_transactions[i].ToString());
        }


        public abstract decimal CalculateMonthlyInterest();


        public virtual string Summary() =>
            $"Account #{AccountNumber} | Balance: {Balance:0.00} | Opened: {DateOpened:yyyy-MM-dd}";
    }
}
