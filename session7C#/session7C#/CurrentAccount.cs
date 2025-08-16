using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace session7C_
{
    class CurrentAccount : Account
    {
        // حد السحب على المكشوف
        public decimal OverdraftLimit { get; private set; }

        public CurrentAccount(decimal overdraftLimit)
        {
            if (overdraftLimit < 0) overdraftLimit = 0;
            OverdraftLimit = overdraftLimit;
        }

        public override bool Withdraw(decimal amount)
        {
            if (amount <= 0) { Console.WriteLine("Amount must be positive."); return false; }
            if (Balance - amount < -OverdraftLimit) { Console.WriteLine("Overdraft limit exceeded."); return false; }

            Balance -= amount;

            typeof(Account)
                .GetMethod("AddTx", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                ?.Invoke(this, new object[] { "Withdraw", amount, "" });

            return true;
        }

        public override decimal CalculateMonthlyInterest() => 0m;

        public override string Summary() =>
            base.Summary() + $" | Current (Overdraft: {OverdraftLimit:0.00})";
    }
}
