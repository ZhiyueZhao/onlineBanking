using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
//note:  this needed to be done because during development
//ef released v. 6
//ef6 needed to add this
using System.Data.Entity.Core;

namespace WindowsBanking
{
    public partial class frmHistory : Form
    {

        //data context object
        BankOfBIT.Models.BankOfBITContext db = new BankOfBIT.Models.BankOfBITContext();

        //given
        ConstructorData constructorData;

   
        public frmHistory()
        {
            InitializeComponent();
        }


        /// <summary>
        /// given
        /// </summary>
        /// <param name="data"></param>
        public frmHistory(ConstructorData data)
        {
            //given
            InitializeComponent();
            constructorData = data;
            lblBalance.Text = data.Balance;
            lblName.Text = data.Name;

            //set the Mask
            mskLblAccountNumber.Mask = Utility.BusinessRules.AccountFormat(data.Type);
            //assign AccountNumber to mskLblAccountNumber
            mskLblAccountNumber.Text = data.AccountNumber;
            //assign ClientNumber to mskLblClientNumber
            mskLblClientNumber.Text = data.ClientNumber;
            //change data.AccountNumber to integer
            int iAccountNumber = int.Parse(data.AccountNumber);
            //retrieve accountId form BankAccounts table according to the iAccountNumber
            int iAccountId = db.BankAccounts.Where(record => record.AccountNumber == iAccountNumber).Select(record => record.BankAccountId).Single();
            //retrieve data from Transactions table and assign them to DataSource of transactionBindingSource
            transactionBindingSource.DataSource = db.Transactions.Where(record => record.BankAccountId == iAccountId).
                Select(record => new { record.DateCreated, record.TransactionType.Description, record.Deposit, record.Withdrawal, record.Notes }).ToList();
        }

        /// <summary>
        /// given - further code required
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmHistory_Load(object sender, EventArgs e)
        {
            this.Location = new Point(0, 0);
        }

        /// <summary>
        /// given
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkReturn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //return to client with the data selected for this form
            frmClients frmClients = new frmClients(constructorData);
            frmClients.MdiParent = this.MdiParent;
            frmClients.Show();
            this.Close();

        }


    }
}
