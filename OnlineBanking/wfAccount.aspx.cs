using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class wfAccount : System.Web.UI.Page
{
    //access the database, contains functionality crud operations against the database
    BankOfBIT.Models.BankOfBITContext db = new BankOfBIT.Models.BankOfBITContext();

    /// <summary>
    /// execute when load the page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        //Ensure that the code executes only when the page loads
        if (!IsPostBack)
        {
            try
            {
                //add the account balance to the account balance label using the session variable
                lblBalance.Text = Session["AccountBalance"].ToString();

                //add the account number to the account number label using the session variable
                lblAccountNumber.Text = Session["AccountNumber"].ToString();

                //add the client number to the client number label using the session variable
                lblClientNumber.Text = Session["ClientNumber"].ToString();

                //parse the login AccountNumber to integer
                int accountNumber = int.Parse(Session["AccountNumber"].ToString());

                //Define a LINQ-to-SQL Server query populating a local int variable with the BankAccountId value from the database that corresponds with the AccountNumber 
                int bankAccountId = db.BankAccounts.Where(record => record.AccountNumber == accountNumber).Select(record => record.BankAccountId).Single();

                //Store this BankAccountId value in a Session Variable
                Session["BankAccountId"] = bankAccountId;


                //Define a LINQ-to-SQL Server query which joins the Transactions and TransactionTypes tables to obtain the values based on the BankAccountId
                var transactionQuery = db.Transactions.Join(db.TransactionTypes,
                                 transactions => transactions.TransactionTypeId,
                                 transactionTypes => transactionTypes.TransactionTypeId,
                                 (transactions, transactionTypes) => new { Transactions = transactions, TransactionTypes = transactionTypes })
                                 .Where(results => results.Transactions.BankAccountId == bankAccountId)
                                 .Select(record => new { record.Transactions.DateCreated, record.TransactionTypes.Description, record.Transactions.Deposit, record.Transactions.Withdrawal, record.Transactions.Notes});

                /*var transactionQuery = from transactions in db.Transactions
                                       join transactionTypes in db.TransactionTypes
                                       on transactions.TransactionTypeId equals transactionTypes.TransactionTypeId
                                       where transactions.BankAccountId == bankAccountId
                                       select new { transactions.DateCreated, transactionTypes.Description, transactions.Deposit, transactions.Withdrawal, transactions.Notes };
                */
                //Bind the result set to the Gridview control 
                gvAccountDetail.DataSource = transactionQuery.ToList();
                //mutiple controls option
                this.DataBind();
            }
            //If an exception occurs during this routine, display an appropriate message in the Message label at the bottom of the form 
            catch (Exception)
            {
                //if something goes wrong during the page load then display an error to the user
                lblWarnings.Text = "Something went wrong while loading the transaction data";
            }
        }
    }
    protected void lnkPay_Click(object sender, EventArgs e)
    {
        Server.Transfer("wfTransaction.aspx");
    }
}