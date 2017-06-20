namespace WindowsBanking
{
    partial class frmTransaction
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblBalance = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mskLblAccountNumber = new EWSoftware.MaskedLabelControl.MaskedLabel();
            this.mskLblClientNumber = new EWSoftware.MaskedLabelControl.MaskedLabel();
            this.lblName = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblNoAccounts = new System.Windows.Forms.Label();
            this.lblAcctPayee = new System.Windows.Forms.Label();
            this.lnkProcess = new System.Windows.Forms.LinkLabel();
            this.lnkReturn = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.descriptionComboBox = new System.Windows.Forms.ComboBox();
            this.transactionTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cboAccountPayee = new System.Windows.Forms.ComboBox();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.transactionTypeBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // lblBalance
            // 
            this.lblBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBalance.Location = new System.Drawing.Point(477, 71);
            this.lblBalance.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(179, 28);
            this.lblBalance.TabIndex = 29;
            this.lblBalance.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(352, 71);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Current Balance:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mskLblAccountNumber);
            this.groupBox1.Controls.Add(this.mskLblClientNumber);
            this.groupBox1.Controls.Add(this.lblBalance);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lblName);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(34, 32);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(685, 123);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Client Data";
            // 
            // mskLblAccountNumber
            // 
            this.mskLblAccountNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mskLblAccountNumber.Location = new System.Drawing.Point(131, 71);
            this.mskLblAccountNumber.Name = "mskLblAccountNumber";
            this.mskLblAccountNumber.Size = new System.Drawing.Size(100, 23);
            this.mskLblAccountNumber.TabIndex = 31;
            this.mskLblAccountNumber.Text = "maskedLabel2";
            // 
            // mskLblClientNumber
            // 
            this.mskLblClientNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mskLblClientNumber.Location = new System.Drawing.Point(131, 23);
            this.mskLblClientNumber.Mask = "0000-9999";
            this.mskLblClientNumber.Name = "mskLblClientNumber";
            this.mskLblClientNumber.Size = new System.Drawing.Size(100, 23);
            this.mskLblClientNumber.TabIndex = 30;
            this.mskLblClientNumber.Text = "maskedLabel1";
            // 
            // lblName
            // 
            this.lblName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblName.Location = new System.Drawing.Point(477, 24);
            this.lblName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(179, 28);
            this.lblName.TabIndex = 27;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(416, 24);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Name:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 71);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(90, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Account Number:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 24);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Client Number:";
            // 
            // lblNoAccounts
            // 
            this.lblNoAccounts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNoAccounts.Location = new System.Drawing.Point(8, 161);
            this.lblNoAccounts.Name = "lblNoAccounts";
            this.lblNoAccounts.Size = new System.Drawing.Size(355, 23);
            this.lblNoAccounts.TabIndex = 8;
            this.lblNoAccounts.Text = "No accounts exist to receive transferred funds";
            this.lblNoAccounts.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblNoAccounts.Visible = false;
            // 
            // lblAcctPayee
            // 
            this.lblAcctPayee.AutoSize = true;
            this.lblAcctPayee.Location = new System.Drawing.Point(30, 131);
            this.lblAcctPayee.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAcctPayee.Name = "lblAcctPayee";
            this.lblAcctPayee.Size = new System.Drawing.Size(66, 13);
            this.lblAcctPayee.TabIndex = 4;
            this.lblAcctPayee.Text = "To Account:";
            this.lblAcctPayee.Visible = false;
            // 
            // lnkProcess
            // 
            this.lnkProcess.AutoSize = true;
            this.lnkProcess.Location = new System.Drawing.Point(83, 201);
            this.lnkProcess.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lnkProcess.Name = "lnkProcess";
            this.lnkProcess.Size = new System.Drawing.Size(104, 13);
            this.lnkProcess.TabIndex = 6;
            this.lnkProcess.TabStop = true;
            this.lnkProcess.Text = "Process Transaction";
            this.lnkProcess.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkProcess_LinkClicked);
            // 
            // lnkReturn
            // 
            this.lnkReturn.AutoSize = true;
            this.lnkReturn.Location = new System.Drawing.Point(218, 201);
            this.lnkReturn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lnkReturn.Name = "lnkReturn";
            this.lnkReturn.Size = new System.Drawing.Size(106, 13);
            this.lnkReturn.TabIndex = 7;
            this.lnkReturn.TabStop = true;
            this.lnkReturn.Text = "Return to Client Data";
            this.lnkReturn.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkReturn_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 90);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Amount:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.descriptionComboBox);
            this.groupBox2.Controls.Add(this.cboAccountPayee);
            this.groupBox2.Controls.Add(this.lblNoAccounts);
            this.groupBox2.Controls.Add(this.lblAcctPayee);
            this.groupBox2.Controls.Add(this.lnkProcess);
            this.groupBox2.Controls.Add(this.txtAmount);
            this.groupBox2.Controls.Add(this.lnkReturn);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(191, 203);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(403, 234);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Transaction Data";
            // 
            // descriptionComboBox
            // 
            this.descriptionComboBox.DataSource = this.transactionTypeBindingSource;
            this.descriptionComboBox.DisplayMember = "Description";
            this.descriptionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.descriptionComboBox.FormattingEnabled = true;
            this.descriptionComboBox.Location = new System.Drawing.Point(164, 44);
            this.descriptionComboBox.Name = "descriptionComboBox";
            this.descriptionComboBox.Size = new System.Drawing.Size(121, 21);
            this.descriptionComboBox.TabIndex = 33;
            this.descriptionComboBox.ValueMember = "TransactionTypeId";
            this.descriptionComboBox.SelectedIndexChanged += new System.EventHandler(this.descriptionComboBox_SelectedIndexChanged);
            // 
            // transactionTypeBindingSource
            // 
            this.transactionTypeBindingSource.DataSource = typeof(BankOfBIT.Models.TransactionType);
            // 
            // cboAccountPayee
            // 
            this.cboAccountPayee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAccountPayee.FormattingEnabled = true;
            this.cboAccountPayee.Location = new System.Drawing.Point(164, 128);
            this.cboAccountPayee.Name = "cboAccountPayee";
            this.cboAccountPayee.Size = new System.Drawing.Size(160, 21);
            this.cboAccountPayee.TabIndex = 32;
            this.cboAccountPayee.Visible = false;
            // 
            // txtAmount
            // 
            this.txtAmount.Location = new System.Drawing.Point(164, 90);
            this.txtAmount.Margin = new System.Windows.Forms.Padding(4);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(160, 20);
            this.txtAmount.TabIndex = 3;
            this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 47);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Transaction Type:";
            // 
            // frmTransaction
            // 
            this.ClientSize = new System.Drawing.Size(751, 464);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmTransaction";
            this.Text = "Account Transaction";
            this.Load += new System.EventHandler(this.frmTransaction_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.transactionTypeBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblNoAccounts;
        private System.Windows.Forms.Label lblAcctPayee;
        private System.Windows.Forms.LinkLabel lnkProcess;
        private System.Windows.Forms.LinkLabel lnkReturn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboAccountPayee;
        private System.Windows.Forms.BindingSource transactionTypeBindingSource;
        private EWSoftware.MaskedLabelControl.MaskedLabel mskLblAccountNumber;
        private EWSoftware.MaskedLabelControl.MaskedLabel mskLblClientNumber;
        private System.Windows.Forms.ComboBox descriptionComboBox;
    }
}
