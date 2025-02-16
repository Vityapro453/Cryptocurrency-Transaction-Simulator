using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

class Program
{
    static void Main()
    {
        // Create wallets
        Wallet alice = new Wallet();
        Wallet bob = new Wallet();

        // Display initial balances
        Console.WriteLine($"Alice's Balance: {alice.Balance}");
        Console.WriteLine($"Bob's Balance: {bob.Balance}");

        // Alice sends 50 tokens to Bob
        Transaction tx1 = alice.CreateTransaction(bob.PublicKey, 50);
        Blockchain.AddTransaction(tx1);

        // Bob sends 20 tokens to Alice
        Transaction tx2 = bob.CreateTransaction(alice.PublicKey, 20);
        Blockchain.AddTransaction(tx2);

        // Display final balances
        Console.WriteLine($"Alice's Final Balance: {alice.Balance}");
        Console.WriteLine($"Bob's Final Balance: {bob.Balance}");
    }
}

// Blockchain class to store and validate transactions
public static class Blockchain
{
    public static List<Transaction> Ledger = new List<Transaction>();

    public static void AddTransaction(Transaction tx)
    {
        if (tx.IsValid())
        {
            Ledger.Add(tx);
            tx.SenderWallet.UpdateBalance();
            tx.RecipientWallet.UpdateBalance();
            Console.WriteLine($"Transaction Success: {tx.Amount} tokens sent.");
        }
        else
        {
            Console.WriteLine("Transaction Failed: Invalid signature or insufficient funds.");
        }
    }
}

// Wallet class representing a user's wallet
public class Wallet
{
    private RSAParameters privateKey;
    public RSAParameters PublicKey { get; private set; }
    public int Balance { get; private set; }

    public Wallet()
    {
        using (var rsa = new RSACryptoServiceProvider(2048))
        {
            privateKey = rsa.ExportParameters(true);
            PublicKey = rsa.ExportParameters(false);
        }
        Balance = 100; // Default balance
    }

    public Transaction CreateTransaction(RSAParameters recipient, int amount)
    {
        return new Transaction(this, recipient, amount, privateKey);
    }

    public void UpdateBalance()
    {
        int received = Blockchain.Ledger.Where(tx => tx.RecipientWallet == this).Sum(tx => tx.Amount);
        int spent = Blockchain.Ledger.Where(tx => tx.SenderWallet == this).Sum(tx => tx.Amount);
        Balance = 100 + received - spent;
    }
}

// Transaction class for storing transaction data
public class Transaction
{
    public Wallet SenderWallet { get; }
    public RSAParameters Sender { get; }
    public RSAParameters Recipient { get; }
    public int Amount { get; }
    public byte[] Signature { get; private set; }

    public Transaction(Wallet senderWallet, RSAParameters recipient, int amount, RSAParameters privateKey)
    {
        SenderWallet = senderWallet;
        Sender = senderWallet.PublicKey;
        Recipient = recipient;
        Amount = amount;
        Signature = SignTransaction(privateKey);
    }

    private byte[] SignTransaction(RSAParameters privateKey)
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.ImportParameters(privateKey);
            byte[] data = Encoding.UTF8.GetBytes(Sender.ToString() + Recipient.ToString() + Amount);
            return rsa.SignData(data, new SHA256CryptoServiceProvider());
        }
    }

    public bool IsValid()
    {
        if (SenderWallet.Balance < Amount) return false; // Insufficient funds

        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.ImportParameters(Sender);
            byte[] data = Encoding.UTF8.GetBytes(Sender.ToString() + Recipient.ToString() + Amount);
            return rsa.VerifyData(data, new SHA256CryptoServiceProvider(), Signature);
        }
    }
}
