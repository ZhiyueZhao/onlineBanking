using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BankOfBIT.Models;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TransactionManager" in code, svc and config file together.
public class TransactionManager : ITransactionManager
{
    private BankOfBITContext db = new BankOfBITContext();

	public void DoWork()
	{
	}
    /// <summary>
    /// invoke the updateBalance method to update bankAccount table when the user choose to deposit money
    /// </summary>
    /// <param name="accountId">primary key in bankAccount table</param>
    /// <param name="amount">the value user deposit</param>
    /// <returns>null when error, balance when succeed</returns>
    public double? Deposit(int accountId, double amount)
    {
        //inwoke the updateBalance method
        return updateBalance(accountId, amount);
    }
    /// <summary>
    /// invoke the updateBalance method to update bankAccount table when the user choose to withdraw money
    /// </summary>
    /// <param name="accountId">primary key in bankAccount table</param>
    /// <param name="amount">the value user withdraw</param>
    /// <returns>null when error, balance when succeed</returns>
    public double? Withdrawal(int accountId, double amount)
    {
        //inwoke the updateBalance method
        return updateBalance(accountId, -amount);
    }
    /// <summary>
    /// invoke the updateBalance method to update bankAccount table when the user choose to pay bills
    /// </summary>
    /// <param name="accountId">primary key in bankAccount table</param>
    /// <param name="amount">the value user pay</param>
    /// <returns>null when error, balance when succeed</returns>
    public double? BillPayment(int accountId, double amount)
    {
        //inwoke the updateBalance method
        return updateBalance(accountId, -amount);
    }
    /// <summary>
    /// invoke the updateBalance method to update bankAccount table when the user choose to transfer money
    /// </summary>
    /// <param name="fromAccountId">primary key in bankAccount table, where the money comes from</param>
    /// <param name="toAccountId">primary key in bankAccount table, where the money goes</param>
    /// <param name="amount">the value user transfer</param>
    /// <returns>null when error, balance when succeed</returns>
    public double? Transfer(int fromAccountId, int toAccountId, double amount)
    {
        //inwoke the updateBalance method
        double? fromAccountBalance = updateBalance(fromAccountId, -amount);
        //when error
        if (fromAccountBalance != null)
        {
            //inwoke the updateBalance method
            double? toAccountBalance = updateBalance(toAccountId, amount);
            //when error
            if (toAccountBalance == null)
            {
                //inwoke the updateBalance method to restore
                updateBalance(fromAccountId, amount);
                //set flag to null
                fromAccountBalance = null;
            }
        }
        return fromAccountBalance;
    }
    /// <summary>
    /// create the transaction and insert it to the transaction table
    /// </summary>
    /// <param name="depositWithdrawal">true when deposit, false when withdraw</param>
    /// <param name="accountId">the account where the transaction happen</param>
    /// <param name="amount">the value in this transaction</param>
    /// <param name="transactionTypeId">transactiontypeid refer to the transactiontype table</param>
    /// <param name="notes">description of this transaction</param>
    /// <returns>null when error, balance when succeed</returns>
    public long? CreateTransaction(bool depositWithdrawal, int accountId, double amount, int transactionTypeId, string notes)
    {
        try
        {
            //create new instance
            Transaction transaction = new Transaction();
            //set accountId
            transaction.BankAccountId = accountId;

            //set deposit and withdrawal
            transaction.Deposit = depositWithdrawal ? amount : 0;
            transaction.Withdrawal = depositWithdrawal ? 0 : amount;
            //invoke SetNextTransactionNumber to set TransactionNumber
            transaction.SetNextTransactionNumber();
            //set date to now
            transaction.DateCreated = DateTime.Now;
            //set TransactionTypeId
            transaction.TransactionTypeId = transactionTypeId;
            //set Notes
            transaction.Notes = notes;
            //insert
            db.Transactions.Add(transaction);
            //invoke updateDate method to commit change
            updateDate();

            return transaction.TransactionNumber;
        }
        //when error
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// submit the changes
    /// </summary>
    private void updateDate()
    {
        //invoke SaveChanges
        db.SaveChanges();
    }

    /// <summary>
    /// update the balance in the bankAccount table
    /// </summary>
    /// <param name="accountId">primary key in the bankAccount table</param>
    /// <param name="amount">the amount change</param>
    /// <returns>null when error, balance when succeed</returns>
    private double? updateBalance(int accountId, double amount)
    {
        try
        {
            //get the bankAccount record according to the accountId
            BankAccount bankAccount = (from account in db.BankAccounts
                                       where account.BankAccountId == accountId
                                       select account).Single();
            //add amount to balance
            bankAccount.Balance += amount;
            //commit update
            updateDate();

            return bankAccount.Balance;
        }
        //when error
        catch (Exception)
        {
            return null;
        }
    }
}
