namespace BankSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.Write("Bank Name: ");
            //string bankName = Console.ReadLine();
            //Console.Write("Branch Code: ");
            //string branch = Console.ReadLine();

            //Bank bank = new Bank(bankName, branch);
            //bank.Run();





            // Bank details
            string correctBankName = "National Bank";
            string correctBranchCode = "1234";

            Console.Write("Enter Bank Name: ");
            string bankName = Console.ReadLine();

            Console.Write("Enter Branch Code: ");
            string branchCode = Console.ReadLine();

            // Validation
            if (bankName == correctBankName && branchCode == correctBranchCode)
            {
                Console.WriteLine("\n Bank login successful!");

                // Create the Bank object (object instantiation)
                Bank bank = new Bank(bankName, branchCode);

                // Call Run() to start menu
                bank.Run();
            }
            else
            {
                Console.WriteLine("\n Invalid Bank Name or Branch Code. Program will exit.");
            }
        }
    }

    static class Limits
    {
        public const int MaxCustomers = 100;
        public const int MaxAccountsPerCustomer = 10;
        public const int MaxTransactionsPerAccount = 500;
    }

    struct Transaction
    {
        public string Type;      // Deposit / Withdraw / Transfer In / Transfer Out / Monthly Interest
        public decimal Amount;
        public DateTime Date;
        public string Note;      // تفاصيل إضافية

        public override string ToString()
        {
            return $"{Date:yyyy-MM-dd HH:mm:ss} | {Type,-14} | {Amount,10:0.00} | {Note}";
        }
    }



}
