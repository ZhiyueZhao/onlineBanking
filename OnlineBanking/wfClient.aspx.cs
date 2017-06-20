using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using net.webservicex.www;

public partial class wfClient : System.Web.UI.Page
{
    //contains functionality crud operations against the database
    BankOfBIT.Models.BankOfBITContext db = new BankOfBIT.Models.BankOfBITContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        //Ensure that the code executes only when the page loads
        if (!IsPostBack)
        {
            try
            {
                //captures the user long id
                lblClientNumber.Text = Page.User.Identity.Name;
                //parse the login clientnumber to integer
                int clientNumber = int.Parse(Page.User.Identity.Name.ToString());

                //Store this clientNumber value in a Session variable
                Session["ClientNumber"] = clientNumber;

                //lambda used to return the clientID
                int clientId = db.Clients.Where(record => record.ClientNumber == clientNumber).Select(record => record.ClientId).Single();
                //Store this ClientId value in a Session variable
                Session["ClientId"] = clientId;
                //Define a LINQ-to-SQL Server query  object with all records from the BankAccount table whose ClientID matches the ClientID retrieved in the previous step
                IQueryable<BankOfBIT.Models.BankAccount> query = db.BankAccounts.Where(record => record.ClientId == clientId);
                //binding the data
                gvAccounts.DataSource = query.ToList();
                //mutiple controls option
                this.DataBind();

                //add for assignment5 exchange lbl
                GlobalWeather weather = new GlobalWeather();

                lblExchangeRate.Text = weather.GetWeather("Winnipeg", "Canada");
            }
            //If an exception occurs during this routine, display an appropriate message in the Message label at the bottom of the form 
            catch (Exception)
            {
                lblWarnings.Text = "Something went wrong trying to load the data";
            }
        }
    }

    protected void gvAccounts_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Define a Session variable and set it to the value of the selected Account Number 
        Session["AccountNumber"] = gvAccounts.Rows[gvAccounts.SelectedIndex].Cells[1].Text;
        //Define a Session variable and set it to the value of the selected Account Balance 
        Session["AccountBalance"] = gvAccounts.Rows[gvAccounts.SelectedIndex].Cells[3].Text;
        //Call the Server.Transfer() method to open the wfAccount Web Form 
        Server.Transfer("wfAccount.aspx");
    }
}