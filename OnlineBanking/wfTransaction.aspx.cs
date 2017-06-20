using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BankOfBIT.Migrations;
using System.Data;
using ServiceReference1;
using BankOfBIT.Models;
using App_Code;

public partial class wfTransaction : System.Web.UI.Page
{
    //contains functionality crud operations against the database
    BankOfBIT.Models.BankOfBITContext db = new BankOfBIT.Models.BankOfBITContext();
    //create an array to bind DataFields
    private string[,] arrDataFields = new string[2,2] {{"Description", "PayeeId" },
                                                {"AccountNumber", "BankAccountId"}};
    //set constant messages
    private const string INSUFFICIENT_FUNDS_ERROR = "Insufficient funds.";
    private const string ERROR_OCCUR_TEMPLATE = "Error occur when {0}";
    private const string SUCCESS_MESSAGE_TEMPLATE = "Transaction Successful!\nTransaction Type: {0}\nAmount: ${1}\nTransaction Number: {2}\n";

    protected void Page_Load(object sender, EventArgs e)
    {
        //Ensure that the code executes only when the page loads
        if (!IsPostBack)
        {
            //add the account number to the account number label using the session variable
            lblAccountNumber.Text = Session["AccountNumber"].ToString();

            //add the balance to the account balance label using the session variable
            lblBalance.Text = Session["AccountBalance"].ToString();
            //invoke the setDdlAccountPayee method to bind the data to drop down list
            setDdlAccountPayee();
        }
    }

    /// <summary>
    /// when change select item of TransactionType drop down list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlTransactionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //set data bindings to null
        ddlAccountPayee.DataSource = null;
        ddlAccountPayee.DataTextField = null;
        ddlAccountPayee.DataValueField = null;
        //invoke setDdlAccountPayee method
        setDdlAccountPayee();
    }
    /// <summary>
    /// when click the submit button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //check validation
        if (IsValid)
        {
            //get the douBalance and douAmount from the labels
            double douBalance = double.Parse(lblBalance.Text.Replace("$", ""));
            double douAmount = double.Parse(txtAmount.Text.Replace("$", ""));
            //when balance is insufficient
            if (douAmount > douBalance)
            {
                //set txtSummary 
                txtSummary.Text = INSUFFICIENT_FUNDS_ERROR;
            }
            else
            {
                //create new instance
                TransactionManagerClient transactionManagerClient = new TransactionManagerClient();

                switch (ddlTransactionType.SelectedIndex)
                {
                    //when select Bill Payment
                    case 0:
                        //when have error throw execption
                        if (transactionManagerClient.BillPayment((int)Session["BankAccountID"], douAmount) == null)
                        {
                            throw new Exception(string.Format(ERROR_OCCUR_TEMPLATE, "pay the bill!"));
                        }
                        else
                        {
                            //invoke CreateTransaction to create a transaction
                            long? transactionNumber = transactionManagerClient.CreateTransaction(false, (int)Session["BankAccountID"], douAmount,
                                                                        (int)TransactionTypes.BillPayment, "Payee: " + ddlAccountPayee.SelectedItem);
                            //when have error
                            if (transactionNumber == null)
                            {
                                // reverse the transaction and if there is an error
                                if (transactionManagerClient.Deposit((int)Session["BankAccountID"], douAmount) == null)
                                {
                                    throw new Exception(string.Format(ERROR_OCCUR_TEMPLATE, "reverse the transaction!"));
                                }
                                else
                                {
                                    //output error message to txtSummary
                                    txtSummary.Text = "Payment is not successful!";
                                }
                            }
                            else
                            {
                                //output success message to txtSummary
                                txtSummary.Text = string.Format(SUCCESS_MESSAGE_TEMPLATE, TransactionTypes.BillPayment, douAmount, transactionNumber);
                            }
                        }
                        break;
                    //when select Transfer Funds
                    case 1:
                        //set toAccount id according to the Selected item
                        int toAccount = int.Parse(ddlAccountPayee.SelectedValue);
                        //transfer the money and when there is error
                        if (transactionManagerClient.Transfer((int)Session["BankAccountID"], toAccount, douAmount) == null)
                        {
                            throw new Exception(string.Format(ERROR_OCCUR_TEMPLATE, "transfer money!"));
                        }
                        else
                        {
                            //create Transaction for fromAccount
                            long? fromTransactionNumber = transactionManagerClient.CreateTransaction(false, (int)Session["BankAccountID"], douAmount,
                                                                        (int)TransactionTypes.Transfer, "Transferred to: " + ddlAccountPayee.SelectedItem);
                            //when error
                            if(fromTransactionNumber == null)
                            {
                                // reverse the transaction
                                if (transactionManagerClient.Deposit((int)Session["BankAccountID"], douAmount) == null)
                                {
                                    throw new Exception(string.Format(ERROR_OCCUR_TEMPLATE, "reverse the transaction!"));
                                }
                                else
                                {
                                    //output error message to txtSummary
                                    txtSummary.Text = "Transaction is not successful!";
                                }
                            }
                            else
                            {
                                //create Transaction for toAccount
                                long? toTransactionNumber = transactionManagerClient.CreateTransaction(true, toAccount, douAmount,
                                                                            (int)TransactionTypes.Transfer, "Transferred from: " + lblAccountNumber.Text);
                                //when error
                                if (toTransactionNumber == null)
                                {
                                    // reverse the transaction and when reverse error
                                    if (transactionManagerClient.Deposit((int)Session["BankAccountID"], douAmount) == null)
                                    {
                                        throw new Exception(string.Format(ERROR_OCCUR_TEMPLATE, "reverse the transaction!"));
                                    }
                                    else
                                    {
                                        try
                                        {
                                            //find out the transaction inserted
                                            Transaction transactionResult = (from t in db.Transactions
                                                                             where t.TransactionNumber == fromTransactionNumber
                                                                             select t).Single();
                                            //remove the transaction
                                            db.Transactions.Remove(transactionResult);
                                            //output the error message to txtSummary
                                            txtSummary.Text = "Transaction is not successful!";
                                        }
                                        catch (Exception)
                                        {
                                            throw new Exception(string.Format(ERROR_OCCUR_TEMPLATE, "delete the transaction!"));
                                        }
                                    }
                                }
                                else
                                {
                                    //when succed set success message
                                    txtSummary.Text = string.Format(SUCCESS_MESSAGE_TEMPLATE, TransactionTypes.Transfer, douAmount, fromTransactionNumber);
                                    txtSummary.Text += string.Format(SUCCESS_MESSAGE_TEMPLATE, TransactionTypes.Transfer, douAmount, toTransactionNumber);
                                }
                            }
                        }
                        break;
                }
                //get clientId and bankAccountId from Session
                int clientId = (int)Session["ClientID"];
                int bankAccountId = (int)Session["BankAccountID"];

                //add the account balance to the account balance label from the balance in database
                lblBalance.Text = (from record in db.BankAccounts
                                   where record.ClientId == clientId
                                   && record.BankAccountId == bankAccountId
                                   select record.Balance).Single().ToString("C");
            }
        }
    }

    /// <summary>
    /// set the AccountPayee Dropdown list
    /// </summary>
    private void setDdlAccountPayee()
    {
        switch (ddlTransactionType.SelectedIndex)
        {
            //when select Bill Payment
            case 0:
                //get payees records from payees table
                IEnumerable<Payee> payees = from record in db.Payees
                                                             select record;
                //Bind the result set to the Payee/Account DropDownList 
                ddlAccountPayee.DataSource = payees.ToList();
                break;
            //when select Transfer Funds
            case 1:
                //get clientId and bankAccountId from session variable
                int clientId = (int)Session["ClientID"];
                int bankAccountId = (int)Session["BankAccountID"];
                //get bankAccounts records from bankAccounts table according to clientId and bankAccountId
                IQueryable<BankAccount> bankAccounts = from record in db.BankAccounts
                                                                        where record.ClientId == clientId
                                                                        && record.BankAccountId != bankAccountId
                                                                        select record;

                //Bind the result set to the Payee/Account DropDownList 
                ddlAccountPayee.DataSource = bankAccounts.ToList();
                break;
        }
        //set DataTextField
        ddlAccountPayee.DataTextField = arrDataFields[ddlTransactionType.SelectedIndex, 0];
        //set DataValueField
        ddlAccountPayee.DataValueField = arrDataFields[ddlTransactionType.SelectedIndex, 1];
        //band data
        ddlAccountPayee.DataBind();
        //set default select item
        ddlAccountPayee.SelectedIndex = 0;
    }
}