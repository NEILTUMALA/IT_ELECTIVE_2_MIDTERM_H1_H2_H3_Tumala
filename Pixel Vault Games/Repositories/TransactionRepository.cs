using PosWebApplication.Models.Entities;

namespace PosWebApplication.Repositories
{
    public static class TransactionRepository
    {
        public static List<Transaction> Transactions { get; private set; } = new List<Transaction>();

        public static void AddTransaction(Transaction transaction)
        {
            Transactions.Add(transaction);
        }
    }
}