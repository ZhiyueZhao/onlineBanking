using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//note:  this needed to be done because during development
//ef released v. 6
//ef6 needed to add this
using System.Data.Entity.Core;
using WindowsBanking.ServiceReference1;
using BankOfBIT.Models;

namespace WindowsBanking
{
    public partial class frmTransaction : Form
    {

        //data context object
        BankOfBIT.Models.BankOfBITContext db = new BankOfBIT.Models.BankOfBITContext();
        
        //declare global private variables
        private int iClientNumber,
                    iAccountNumber,
                    iBankAccountID,
                    iClientId;
        private double dAmount,
                       dBalance;

        private bool bTransactionFlag = true;
        private string sTransactionString;

        //declare a gloable enum
        private enum TransactionTypes
        {
            Deposit = 1,
            Withdrawal = 2,
            BillPayment = 3,
            Transfer = 4
        }

        ConstructorData constructorData;
  
        public frmTransaction()
        {
            InitializeComponent();
        }


        /// <summary>
        /// given
        /// </summary>
        /// <param name="data"></param>
        public frmTransaction(ConstructorData data)
        {
            //given
            InitializeComponent();
            lblBalance.Text = data.Balance;
            lblName.Text = data.Name;

            constructorData = data;

            //set the Mask
            mskLblAccountNumber.Mask = Utility.BusinessRules.AccountFormat(data.Type);
            //assign AccountNumber to mskLblAccountNumber
            mskLblAccountNumber.Text = data.AccountNumber;
            //assign ClientNumber to mskLblClientNumber
            mskLblClientNumber.Text = data.ClientNumber;
            //retrieve TransactionTypes from DB and set the DataSource of transactionTypeBindingSource
            transactionTypeBindingSource.DataSource = db.TransactionTypes.ToList();

            //set the global private variables according to the data introduced
            // set iClientNumber
            iClientNumber = int.Parse(constructorData.ClientNumber);
            // set iAccountNumber
            iAccountNumber = int.Parse(constructorData.AccountNumber);
            //retrieve ClientId from Clients table according to iClientNumber
            iClientId = db.Clients.Where(record => record.ClientNumber == iClientNumber).
                Select(record => record.ClientId).Single();
            
        }

        /// <summary>
        /// given - further code required
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTransaction_Load(object sender, EventArgs e)
        {
            this.Location = new Point(0, 0);
        }

        /// <summary>
        /// given
        /// </summary>
        private void returnToClient()
        {
            //re-open client form and close this form
            frmClients frmClients = new frmClients(constructorData);
            frmClients.MdiParent = this.MdiParent;
            frmClients.Show();
            this.Close();
        }

        /// <summary>
        /// given
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkReturn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            returnToClient();

        }

        /// <summary>
        /// event handler that user change the select item of the descriptionComboBox(TransactionType), when user select "Bill Payment" or "Transfer" will display 
        /// dropdown box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void descriptionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //assign the selected text of descriptionComboBox to sTransactionString
            sTransactionString = descriptionComboBox.Text;
            //check sTransactionString
            switch (sTransactionString)
            {
                //when select "Bill Payment"
                case "Bill Payment":
                    //assign "Payee" to lblAcctPayee.Text
                    lblAcctPayee.Text = "Payee";
                    //make lblAcctPayee visible
                    lblAcctPayee.Visible = true;
                    //make cboAccountPayee Visible
                    cboAccountPayee.Visible = true;
                    //retrieve data from Payees table and assign it to DataSource of cboAccountPayee
                    cboAccountPayee.DataSource = db.Payees.ToList();
                    //set "Description" field as DisplayMember
                    cboAccountPayee.DisplayMember = "Description";
                    //set "PayeeId" field as ValueMember
                    cboAccountPayee.ValueMember = "PayeeId";
                    //go out of switch
                    break;
                //when select "Transfer"
                case "Transfer":
                    //declare a new BindingSource to store the AccountPayee data
                    BindingSource cboAccountPayeeBingding = new BindingSource();
                    //retrieve data from BankAccounts table according to iClientId and iAccountNumber, assign the data to the DataSource of cboAccountPayeeBingding
                    cboAccountPayeeBingding.DataSource = db.BankAccounts.Where(record => record.ClientId == iClientId && 
                        record.AccountNumber != iAccountNumber).ToList();
                    //when there is some account rather than the login account
                    if (cboAccountPayeeBingding.List.Count > 0)
                    {
                        //set the lblAcctPayee Text
                        lblAcctPayee.Text = "To Account:";
                        //make lblAcctPayee Visible
                        lblAcctPayee.Visible = true;
                        //make cboAccountPayee Visible
                        cboAccountPayee.Visible = true;
                        //assign cboAccountPayeeBingding to datasource of cboAccountPayee
                        cboAccountPayee.DataSource = cboAccountPayeeBingding;
                        //set "AccountNumber" field as DisplayMember
                        cboAccountPayee.DisplayMember = "AccountNumber";
                        //set "BankAccountId"field as DisplayMember
                        cboAccountPayee.ValueMember = "BankAccountId";
                    }
                    //otherwise
                    else
                    {
                        //make lblAcctPayee inVisible
                        lblAcctPayee.Visible = false;
                        //make cboAccountPayee inVisible
                        cboAccountPayee.Visible = false;
                        //make lblNoAccounts Visible
                        lblNoAccounts.Visible = true;
                    }
                    //go out of switch
                    break;
                //otherwise
                default:
                    //make lblAcctPayee inVisible
                    lblAcctPayee.Visible = false;
                    //make cboAccountPayee inVisible
                    cboAccountPayee.Visible = false;
                    //go out of switch
                    break;
            }
        }

        /// <summary>
        /// eventhandler when user click Process_Link then process the transaction and return to the client form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkProcess_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //innumeric data has been entered into the Amount textbox
            if (!Utility.Numeric.isNumeric(txtAmount.Text, System.Globalization.NumberStyles.Currency))
            {
                //output a message box
                MessageBox.Show("Please insert a numeric value!", "Warning", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
            //numeric data has been entered into the Amount textbox
            else
            {
                //assign the selected text of descriptionComboBox to sTransactionString
                sTransactionString = descriptionComboBox.Text;
                //assign the value of txtAmount to dAmount
                dAmount = double.Parse(txtAmount.Text);
                //retrieve data from BankAccounts according to iAccountNumber
                var bankAccount = db.BankAccounts.Where(record => record.AccountNumber == iAccountNumber).Single();
                //assign Balance to dBalance
                dBalance = bankAccount.Balance;
                //assign BankAccountId to iBankAccountID
                iBankAccountID = bankAccount.BankAccountId;
                //check the type of the Transaction
                switch (sTransactionString)
                {
                    //when Transaction type is "Bill Payment" or "Transfer" or "Withdrawal"
                    case "Bill Payment":
                    case "Transfer":
                    case "Withdrawal":
                        //check the Balance of the account when it is less then the amount wanna pay/Transfer/Withdrawal
                        if (dBalance < double.Parse(txtAmount.Text))
                        {
                            //output a message box
                            MessageBox.Show("insufficient funds!", "Warning", MessageBoxButtons.OK,
                                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        }
                        //otherwise
                        else
                        {
                            //invoke the makeTransaction method
                            bTransactionFlag = makeTransaction(sTransactionString);
                        }
                        //jump out of the switch
                        break;
                    //other transaction types
                    default:
                        //invoke the makeTransaction method
                        bTransactionFlag = makeTransaction(sTransactionString);
                        //jump out of the switch
                        break;
                }
                //when error occor
                if (!bTransactionFlag)
                {
                    //display
                    MessageBox.Show("Error occor!", "Exception", MessageBoxButtons.OK,
                                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
                //otherwise
                else
                {
                    //return to client form
                    returnToClient();
                }
            }
        }

        /// <summary>
        /// according to the TransactionType, create the Transaction, insert and update the database
        /// </summary>
        /// <param name="sTransactionType"></param>
        /// <returns>true when there is no error, false when there have error</returns>
        private bool makeTransaction(string sTransactionType)
        {
            //create new instance
            TransactionManagerClient transactionManagerClient = new TransactionManagerClient();

            try
            {
                //declare a bool variable and set default value
                bool bErrorFlag = true;
                //check the TransactionType
                switch (sTransactionType)
                {
                    //when TransactionType is "Bill Payment"
                    case "Bill Payment":
                        //execute BillPayment method and when have error
                        if (transactionManagerClient.BillPayment(iBankAccountID, dAmount) == null)
                        {
                            //set bErrorFlag to false
                            bErrorFlag = false;
                        }
                        //when no error
                        else
                        {
                            //invoke CreateTransaction to create a transaction
                            long? transactionNumber = transactionManagerClient.CreateTransaction(false, iBankAccountID, dAmount,
                                                                        (int)TransactionTypes.BillPayment, "Payee: " + cboAccountPayee.Text);
                            //when have error
                            if (transactionNumber == null)
                            {
                                //set bErrorFlag to false
                                bErrorFlag = false;
                                // reverse the transaction and if there is an error
                                transactionManagerClient.Deposit(iBankAccountID, dAmount);
                            }
                        }
                        //jump out of the switch
                        break;
                    //when TransactionType is "Transfer"
                    case "Transfer":
                        //set toAccount id according to the Selected item
                        int toAccountNumber = int.Parse(cboAccountPayee.Text);
                        int toAccountId = db.BankAccounts.Where(record => record.AccountNumber == toAccountNumber).Select(record => record.BankAccountId).Single();
                        //transfer the money and when there is error
                        if (transactionManagerClient.Transfer(iBankAccountID, toAccountId, dAmount) == null)
                        {
                            //set bErrorFlag to false
                            bErrorFlag = false;
                        }
                        else
                        {
                            //create Transaction for fromAccount
                            long? fromTransactionNumber = transactionManagerClient.CreateTransaction(false, iBankAccountID, dAmount,
                                                                        (int)TransactionTypes.Transfer, "Transferred to: " + cboAccountPayee.Text);
                            //when error
                            if (fromTransactionNumber == null)
                            {
                                //set bErrorFlag to false
                                bErrorFlag = false;
                                // reverse the transaction
                                transactionManagerClient.Deposit(iBankAccountID, dAmount);
                            }
                            else
                            {
                                //create Transaction for toAccount
                                long? toTransactionNumber = transactionManagerClient.CreateTransaction(true, toAccountId, dAmount,
                                                                            (int)TransactionTypes.Transfer, "Transferred from: " + cboAccountPayee.Text);
                                //when error
                                if (toTransactionNumber == null)
                                {
                                    //set bErrorFlag to false
                                    bErrorFlag = false;
                                    // reverse the transaction and when reverse error
                                    transactionManagerClient.Deposit(iBankAccountID, dAmount);

                                    //find out the transaction inserted
                                    Transaction transactionResult = (from t in db.Transactions
                                                                     where t.TransactionNumber == fromTransactionNumber
                                                                     select t).Single();
                                    //remove the transaction
                                    db.Transactions.Remove(transactionResult);
                                }
                            }
                        }
                        //jump out of the switch
                        break;
                    //when TransactionType is "Deposit"
                    case "Deposit":
                        //execute Deposit method and when occor error
                        if (transactionManagerClient.Deposit(iBankAccountID, dAmount) == null)
                        {
                            //set bErrorFlag to false
                            bErrorFlag = false;
                        }
                        else
                        {
                            //invoke CreateTransaction to create a transaction
                            long? transactionNumber = transactionManagerClient.CreateTransaction(true, iBankAccountID, dAmount,
                                                                        (int)TransactionTypes.Deposit, "Deposit amount: " + dAmount.ToString());
                            //when have error
                            if (transactionNumber == null)
                            {
                                //set bErrorFlag to false
                                bErrorFlag = false;
                                // reverse the transaction
                                transactionManagerClient.Withdrawal(iBankAccountID, dAmount);
                            }
                        }
                        //jump out of the switch
                        break;
                    //when TransactionType is "Withdrawal"
                    case "Withdrawal":
                        //execute Withdrawal method and when occor error
                        if (transactionManagerClient.Withdrawal(iBankAccountID, dAmount) == null)
                        {
                            //set bErrorFlag to false
                            bErrorFlag = false;
                        }
                        else
                        {
                            //invoke CreateTransaction to create a transaction
                            long? transactionNumber = transactionManagerClient.CreateTransaction(false, iBankAccountID, dAmount,
                                                                        (int)TransactionTypes.Deposit, "Withdrawal amount: " + dAmount.ToString());
                            //when have error
                            if (transactionNumber == null)
                            {
                                //set bErrorFlag to false
                                bErrorFlag = false;
                                // reverse the transaction
                                transactionManagerClient.Deposit(iBankAccountID, dAmount);
                            }
                        }
                        //jump out of the switch
                        break;
                }
                //return the flag
                return bErrorFlag;
            }
            //when occor Exception throw it
            catch (Exception)
            {
                
                throw;
            }
        }

    }
}
