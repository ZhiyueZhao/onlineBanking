using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BankOfBIT.Models;
using Utility;

namespace WindowsBanking
{
    public partial class frmBatch : Form
    {
        BankOfBITContext db = new BankOfBITContext();
        List<int> institutionNumbers = new List<int>();

        public frmBatch()
        {
            InitializeComponent();
        }

        /// <summary>
        /// given - further code required
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmBatch_Load(object sender, EventArgs e)
        {
            this.Location = new Point(0, 0);

            institutionBindingSource.DataSource = db.Institutions.ToList();
            institutionNumbers = db.Institutions.Select(d => d.InstitutionNumber).ToList();
            cboInstitution.DisplayMember = "Description";
            cboInstitution.ValueMember = "InstitutionNumber";
        }

        /// <summary>
        /// given - further code required
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkProcess_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //given - for use in encryption assignment
            if (txtKey.Text.Trim().Length != 8)
            {
                MessageBox.Show("64 Bit Decryption Key must be entered", "Enter Key");
                txtKey.Focus();
            }

            Batch batchProcess = new Batch();
            //Evaluate the RadioButton selection
            if (radSelect.Checked)
            {
                //a single transmission selection has been made
                //Call the ProcessTransmission method of the Batch class passing appropriate arguments
                batchProcess.ProcessTransmission(cboInstitution.SelectedValue.ToString(), txtKey.Text);
                //Call the WriteLogData method of the Batch class to write all logging information associated with this transmission file.
                //Capture the return value and append this value in the richText control.
                rtxtLog.Text = batchProcess.WriteLogData();
                
            }
            else
            {
                string stxtLogs = "";
                //Iterate through each item in the ComboBox collection.
                foreach (int institutionNumber in institutionNumbers)
                {
                    //Call the ProcessTransmission method of the Batch class passing appropriate arguments
                    batchProcess.ProcessTransmission(institutionNumber.ToString(), txtKey.Text);
                    //Call the WriteLogData method of the Batch class to write all logging information associated with this transmission file. 
                    //Capture the return value and append this value in the richText control.
                    stxtLogs += batchProcess.WriteLogData();
                }
                rtxtLog.Text = stxtLogs;
            }
        }

        /// <summary>
        /// Ensure that the Transaction combobox is enabled only when the Select a Transmission radiobutton is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radSelect_CheckedChanged(object sender, EventArgs e)
        {
            //Ensure that the Transaction combobox is enabled only when the Select a Transmission radiobutton is checked
            if (radSelect.Checked)
            {
                cboInstitution.Enabled = true;
            }
            else
            {
                cboInstitution.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //given - for use in encryption assignment
            if (txtKey.Text.Trim().Length != 8)
            {
                MessageBox.Show("64 Bit Decryption Key must be entered", "Enter Key");
                txtKey.Focus();
            }
            else
            {
                //Evaluate the RadioButton selection
                if (radSelect.Checked)
                {
                    //a single transmission selection has been made
                    //Call the ProcessTransmission method of the Batch class passing appropriate arguments
                    string fileName = System.DateTime.Today.Year + "-" + System.DateTime.Today.DayOfYear.ToString("000") + "-" + cboInstitution.SelectedValue.ToString() + ".xml";

                    //add for assignment 9
                    //Define a string variable to represent the encrypted file name
                    string EncryptedFilename = fileName + ".encrypted";

                    Encryption.Encrypt(fileName, EncryptedFilename, txtKey.Text);

                }
            }
        }

    }
}
