using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace session7C_
{
    class SavingsAccount : Account
    {
        // نسبة فائدة (كنسبة مئوية: 5 = 5%)
        public decimal InterestRate { get; private set; }

        public SavingsAccount(decimal interestPercent)
        {
            if (interestPercent < 0) interestPercent = 0;
            InterestRate = interestPercent;
        }

        public override decimal CalculateMonthlyInterest()
        {
            // فائدة شهرية = الرصيد × (النسبة/100) / 12
            return Balance * (InterestRate / 100m) / 12m;
        }

        public void ApplyMonthlyInterest()
        {
            decimal interest = CalculateMonthlyInterest();
            if (interest > 0)
            {
                Balance += interest;
                AddTx("Monthly Interest", interest, $"{InterestRate}% / 12");
            }
        }

        public override string Summary() =>
            base.Summary() + $" | Savings (Rate: {InterestRate:0.##}%)";
    }


}
