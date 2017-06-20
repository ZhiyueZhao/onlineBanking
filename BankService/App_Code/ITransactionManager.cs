using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITransactionManager" in both code and config file together.
[ServiceContract]
public interface ITransactionManager
{
	[OperationContract]
	void DoWork();
    //create interface for Deposit method
    [OperationContract]
    double? Deposit(int accountId, double amount);
    //create interface for Withdrawal method
    [OperationContract]
    double? Withdrawal(int accountId, double amount);
    //create interface for BillPayment method
    [OperationContract]
    double? BillPayment(int accountId, double amount);
    //create interface for Transfer method
    [OperationContract]
    double? Transfer(int fromAccountId, int toAccountId, double amount);
    //create interface for CreateTransaction method
    [OperationContract]
    long? CreateTransaction(bool depositWithdrawal, int accountId, double amount, int transactionTypeId, string notes);
}
