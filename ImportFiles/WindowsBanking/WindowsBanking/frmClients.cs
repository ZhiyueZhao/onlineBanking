using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Linq;
using System.Data.Linq.Mapping;

//note:  this needed to be done because during development
//ef released v. 6
//ef6 needed to add this
using System.Data.Entity.Core;

using System.IO.Ports;      //for rfid assignment

namespace WindowsBanking
{

   public partial class frmClients : Form
    {
       //assignment 10
       //Add a SerialPort object to the form. Give this object a meaningful name.
       SerialPort COMReader = new SerialPort();

       //define a delegate with the same signature as the callback method
       delegate void delCallback(string strData);

        //data context object
       BankOfBIT.Models.BankOfBITContext db = new BankOfBIT.Models.BankOfBITContext();

        public frmClients()
        {  
            InitializeComponent();
        }


        public frmClients(ConstructorData data)
        {
            //given
            InitializeComponent();
            //set the clientNumberMaskedTextBox according to the data
            clientNumberMaskedTextBox.Text = data.ClientNumber;
            //invoke the loadData method to load the data
            LoadData();
            //set the accountNumberComboBox according to the data
            accountNumberComboBox.Text = data.AccountNumber;
        }


       /// <summary>
       /// given:  Captures important data to be passed to forms launched
       /// based on selections in this form
       /// USAGE:  replace "value" below with values from the various populated controls
       /// </summary>
       /// <returns></returns>
        private ConstructorData LoadConstructorData()
        {
            ConstructorData constructorData = new ConstructorData();

            //account number
            constructorData.AccountNumber = accountNumberComboBox.Text;
            //balance
            constructorData.Balance = balanceLabel1.Text;
            //client number
            constructorData.ClientNumber = clientNumberMaskedTextBox.Text.Replace("-", "");
            //name
            constructorData.Name = fullNameLabel1.Text;
            //type
            constructorData.Type = lblAccountType.Text;

            return constructorData;
        }

       /// <summary>
       /// given - open history form passing data
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void lnkDetails_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            //instance of frmHistory passing constructor data
            frmHistory frmHistory = new frmHistory(LoadConstructorData());
            //open in frame
            frmHistory.MdiParent = this.MdiParent;
            //show form
            frmHistory.Show();
            this.Close();
        }

       /// <summary>
       /// given - open transaction form passing constructor data
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void lnkTransaction_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //instance of frmTransaction passing constructor data
            frmTransaction frmTransaction = new frmTransaction(LoadConstructorData());
            //open in frame
            frmTransaction.MdiParent = this.MdiParent;
            //show form
            frmTransaction.Show();
            this.Close();
        }


       /// <summary>
       /// given
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void frmClients_Load(object sender, EventArgs e)
        {
            //keeps location of form static when opened and closed
            this.Location = new Point(0, 0);

        }

       /// <summary>
       /// after insert the clientNumber when you try to change the focus
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void clientNumberMaskedTextBox_Leave(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            //handle the exceptions
            try
            {
                //when the insert data match the mask
                if (clientNumberMaskedTextBox.MaskFull)
                {
                    //get the clientNumber from the insert
                    int clientNumber = int.Parse(clientNumberMaskedTextBox.Text.Replace("-", ""));
                    //assign the Client information retrieved from database according to the clientNumber to clientBindingSource
                    clientBindingSource.DataSource = db.Clients.Where(record => record.ClientNumber == clientNumber).ToList();
                    //Evaluate the List.Count property of the Client BindingSource object
                    // the count indicates that no records were retrieved
                    if (clientBindingSource.List.Count <= 0)
                    {
                        //disable the links
                        lnkTransaction.Enabled = false;
                        lnkDetails.Enabled = false;
                        //clear the datasource
                        bankAccountBindingSource.Clear();
                        //output a message box
                        MessageBox.Show("client number entered does not exist!", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        //set focus back 
                        clientNumberMaskedTextBox.Focus();
                    }
                    else
                    {
                        string s = clientNumberMaskedTextBox.Tag.ToString();
                        //get the cliendId from clientNumberMaskedTextBox
                        int clientId = clientNumberMaskedTextBox.Tag.ToString() != "" ? int.Parse(clientNumberMaskedTextBox.Tag.ToString()) : 
                            db.Clients.Where(record => record.ClientNumber == clientNumber).Select(record=> record.ClientId).Single();
                        //retrive data according to the clientId
                        IQueryable<BankOfBIT.Models.BankAccount> query = db.BankAccounts.Where(record => record.ClientId == clientId);
                        //assign the query to bankAccountBindingSource
                        bankAccountBindingSource.DataSource = query.ToList();

                        //Evaluate the List.Count property of the Bank Account BindingSource object
                        if (bankAccountBindingSource.List.Count > 0)
                        {
                            //bind AccountType manually
                            //create an binding instance
                            /*System.Windows.Forms.Binding AccountTypeBinding = new Binding("Text", bankAccountBindingSource, "AccountType", true);
                            //bind to the lblAccountType
                            lblAccountType.DataBindings.Add(AccountTypeBinding);*/

                            lblAccountType.DataBindings.Clear();
                            lblAccountType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bankAccountBindingSource, "AccountType", true));

                            //enable the link
                            lnkTransaction.Enabled = true;
                            lnkDetails.Enabled = true;
                        }
                        else
                        {
                            accountNumberComboBox.Focus();
                            //disable the link
                            lnkTransaction.Enabled = false;
                            lnkDetails.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void setLeave(string strData)
        {
            // Use the InvokeRequired to compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.clientNumberMaskedTextBox.InvokeRequired)
            {
                //Threads differ – currently within the secondary thread:
                //Create an object array containing the string argument that was received by this method
                object[] objArray = new object[] { strData };
                //create an instance of the delegate object
                //Define an instance of the SetLeaveCallback delegate passing the name of this method as an argument
                delCallback objCallBack = new delCallback(setLeave);
                //cause the called back method to execute on the main thread.
                //this will trigger this same method, but the InvokeRequired method call will
                //evaluate to False indicating that control has transferred to the main thread
                //the invoke method requires an object array – the array contains the
                //data passed to this method
                this.Invoke(objCallBack, objArray);
            }
            //If the threads are the same
            else
            {
                //Convert the string argument from a hexadecimal value to a long value using the long.parse method
                long lData = long.Parse(strData.Trim(), System.Globalization.NumberStyles.HexNumber);

                //Define a LINQ-to-SQL Server query against the RfidTag entity class retrieving the record that corresponds with the tag value

            }
        }

        private void openPort()
        {

        }
   }
}
