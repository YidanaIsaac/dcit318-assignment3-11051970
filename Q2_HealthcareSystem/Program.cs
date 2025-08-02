using System;
using System.Collections.Generic;

// Record for Transaction
public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

// Interface for Transaction Processing
public interface ITransactionProcessor
{
    void Process(Transaction transaction);
}

// Transaction Processors
public class BankTransferProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[Bank Transfer] Amount: {transaction.Amount:C} for {transaction.Category}");
    }
}

public class MobileMoneyProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[Mobile Money] Amount: {transaction.Amount:C} for {transaction.Category}");
    }
}

public class CryptoWalletProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[Crypto Wallet] Amount: {transaction.Amount:C} for {transaction.Category}");
    }
}

// Base Account Class
public class Account
{
    public string AccountNumber { get; }
    public decimal Balance { get; protected set; }

    public Account(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }

    public virtual void ApplyTransaction(Transaction transaction)
    {
        Balance -= transaction.Amount;
    }
}

// Sealed SavingsAccount
public sealed class SavingsAccount : Account
{
    public SavingsAccount(string accountNumber, decimal initialBalance) 
        : base(accountNumber, initialBalance) { }

    public override void ApplyTransaction(Transaction transaction)
    {
        if (transaction.Amount > Balance)
        {
            Console.WriteLine("❌ Insufficient funds");
        }
        else
        {
            Balance -= transaction.Amount;
            Console.WriteLine($"✅ Transaction applied. New balance: {Balance:C}");
        }
    }
}

// FinanceApp
public class FinanceApp
{
    private List<Transaction> _transactions = new List<Transaction>();

    public void Run()
    {
        var account = new SavingsAccount("ACC123456", 1000);

        var t1 = new Transaction(1, DateTime.Now, 200, "Groceries");
        var t2 = new Transaction(2, DateTime.Now, 150, "Utilities");
        var t3 = new Transaction(3, DateTime.Now, 300, "Entertainment");

        ITransactionProcessor mobileProcessor = new MobileMoneyProcessor();
        ITransactionProcessor bankProcessor = new BankTransferProcessor();
        ITransactionProcessor cryptoProcessor = new CryptoWalletProcessor();

        mobileProcessor.Process(t1);
        bankProcessor.Process(t2);
        cryptoProcessor.Process(t3);

        account.ApplyTransaction(t1);
        account.ApplyTransaction(t2);
        account.ApplyTransaction(t3);

        _transactions.AddRange(new[] { t1, t2, t3 });
    }
}

// Main Program
class Program
{
    static void Main()
    {
        var app = new FinanceApp();
        app.Run();
    }
}
