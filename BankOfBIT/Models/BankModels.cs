using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//import the necessory Class
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Data;

namespace BankOfBIT.Models
{
    /// <summary>
    /// Client class, represents Client table in database
    /// </summary>
    public class Client
    {
        //automatically generated as an identity field 
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        //pk
        [Key]
        public int ClientId { get; set; }

        //other properties
        //overide by using annotation
        //change Display name
        [Display(Name = "Client")]
        //require
        [Required]
        //remove range according to assignment3, in order to automating ClientNumber
        //set range
        //[Range(10000000, 99999999)]
        public int ClientNumber { get; set; }

        [Required]
        //set length
        [StringLength(35, MinimumLength = 1)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 1)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 1)]
        [Display(Name = "Street Address")]
        public string Address { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 1)]
        public string City { get; set; }

        //required and if not entered display error message
        [Required(ErrorMessage = "Please enter the Province field!")]
        [StringLength(2)]
        //set the require format and if not meet the format, display the error message
        [RegularExpression("[A-Z][A-Z]", ErrorMessage = "Please enter the two uppercase characters!")]
        public string Province { get; set; }

        [Required]
        [StringLength(7)]
        [RegularExpression("[A-Z][0-9][A-Z][ ][0-9][A-Z][0-9]", ErrorMessage = "Please use a format of “A9A 9A9”!")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Date\nCreated")]
        [DisplayFormat(DataFormatString = "{0:D}", ApplyFormatInEditMode=true)]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Client\nNotes")]
        public string Notes { get; set; }

        //not the real field in database, a combine of other fields
        [Display(Name = "Name")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        //not the real field in database, a combine of other fields
        [Display(Name = "Address")]
        public string FullAddress
        {
            get
            {
                return Address + " " + City + "," + Province + " " + PostalCode;
            }
        }

        public void SetNextClientNumber()
        {
            //set the ClientNumber property to the appropriate value returned from the NextKey static method
            ClientNumber = (int)StoredProcedures.NextKey("NextClientNumbers");
        }

        //navigation property represents the * in the class diagram
        //virtual allows for lazy loading
        public virtual ICollection<BankAccount> BankAccount { get; set; }
    }

    /// <summary>
    /// AccountStatus class, represents AccountStatus table in database
    /// </summary>
    public class AccountStatus
    {
        //automatically generated as an identity field 
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        //pk
        [Key]
        public int AccountStatusId { get; set; }

        //other properties
        [Required]
        [Display(Name = "Account\nStatus")]
        public string Description { get; set; }

        public virtual double RateAdjustment()
        {
            return 0;
        }
    }

    /// <summary>
    /// ActiveStatus class inherit from AccountStatus class, Service Objects
    /// </summary>
    public class ActiveStatus : AccountStatus
    {
        //override the method in base class
        public override double RateAdjustment()
        {
            return 0;
        }
    }

    /// <summary>
    /// InactiveStatus class inherit from AccountStatus class, Service Objects
    /// </summary>
    public class InactiveStatus : AccountStatus
    {
        //override the method in base class
        public override double RateAdjustment()
        {
            return 0;
        }
    }

    /// <summary>
    /// DelinquentStatus class inherit from AccountStatus class, Service Objects
    /// </summary>
    public class DelinquentStatus : AccountStatus
    {
        public override double RateAdjustment()
        {
            return 0;
        }
    }

    /// <summary>
    /// FrozenStatus class inherit from AccountStatus class, Service Objects
    /// </summary>
    public class FrozenStatus : AccountStatus
    {
        public override double RateAdjustment()
        {
            return 0;
        }
    }

    /// <summary>
    /// ClosedStatus class inherit from AccountStatus class, Service Objects
    /// </summary>
    public class ClosedStatus : AccountStatus
    {
        public override double RateAdjustment()
        {
            return 0;
        }
    }

    /// <summary>
    /// BankAccount abstract class, represents BankAccount table in database
    /// </summary>
    public abstract class BankAccount
    {
        //pk
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int BankAccountId { get; set; }

        //other properties
        [Display(Name = "Account\nNumber")]
        public int AccountNumber { get; set; }

        //FK - name must match the instance
        //FK navigation property(name of the instance)
        [ForeignKey("Client")]
        [Required]
        public int ClientId { get; set; }

        //FK - name must match the instance
        //FK navigation property(name of the instance)
        [ForeignKey("AccountStatus")]
        [Required]
        public int AccountStatusId { get; set; }

        //other properties
        [Required]
        [Display(Name = "Current\nBalance")]
        //set the data format
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double Balance { get; set; }

        [Required]
        [Display(Name = "Opening Balance")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double OpeningBalance { get; set; }

        [Required]
        [Display(Name = "Date\nCreated")]
        [DisplayFormat(DataFormatString = "{0:D}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Account\nNotes")]
        public string Notes { get; set; }

        //added for assignment 6
        public string AccountType
        {
            get
            {
                return this.GetType().Name.Substring(0, this.GetType().Name.IndexOf("Account"));
            }
        }

        public abstract void SetNextAccountNumber();

        //create an instance of foreign class
        //navigation properties represent the 1 in the class diagram
        public virtual AccountStatus AccountStatus { get; set; }
        public virtual Client Client { get; set; }

        //navigation property represents the * in the class diagram
        //virtual allows for lazy loading
        //added 2016-01-25(assignment3)
        public virtual ICollection<Transaction> Transaction { get; set; }
    }

    /// <summary>
    /// SavingsAccount class inherit from BankAccount class, represents SavingsAccount table in database
    /// </summary>
    public class SavingsAccount : BankAccount
    {
        //private field only in this sub class
        [Display(Name = "Service\nCharges")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double SavingsServiceCharges { get; set; }

        //override the method in base class
        public override void SetNextAccountNumber()
        {
            //set the AccountNumber property to the appropriate value returned from the NextKey static method 
            AccountNumber = (int)StoredProcedures.NextKey("NextSavingsAccounts");
        }
    }

    /// <summary>
    /// MortgageAccount class inherit from BankAccount class, represents MortgageAccount table in database
    /// </summary>
    public class MortgageAccount : BankAccount
    {
        [Display(Name = "Interest\nRate")]
        [DisplayFormat(DataFormatString = "{0:P}")]
        public double MortgageRate { get; set; }

        public int Ammortization { get; set; }

        public override void SetNextAccountNumber()
        {
            //set the AccountNumber property to the appropriate value returned from the NextKey static method 
            AccountNumber = (int)StoredProcedures.NextKey("NextMortgageAccounts");
        }
    }

    /// <summary>
    /// InvestmentAccount class inherit from BankAccount class, represents InvestmentAccount table in database
    /// </summary>
    public class InvestmentAccount : BankAccount
    {
        [Display(Name = "Interest\nRate")]
        [DisplayFormat(DataFormatString = "{0:P}")]
        public double InterestRate { get; set; }

        public override void SetNextAccountNumber()
        {
            //set the AccountNumber property to the appropriate value returned from the NextKey static method 
            AccountNumber = (int)StoredProcedures.NextKey("NextInvestmentAccounts");
        }
    }

    /// <summary>
    /// ChequingAccount class inherit from BankAccount class, represents ChequingAccount table in database
    /// </summary>
    public class ChequingAccount : BankAccount
    {
        [Display(Name = "Service\nCharges")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double ChequingServiceCharges { get; set; }

        public override void SetNextAccountNumber()
        {
            //set the AccountNumber property to the appropriate value returned from the NextKey static method 
            AccountNumber = (int)StoredProcedures.NextKey("NextChequingAccounts");
        }
    }

    /// <summary>
    /// NextClientNumber class, represents NextClientNumber table in database
    /// </summary>
    public class NextClientNumber
    {
        //pk
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int NextClientNumberId { get; set; }

        //other properties
        [Display(Name = "Next Client")]
        //set range
        [Range(10000000, 99999999)]
        public int NextAvailableNumber { get; set; }
    }

    /// <summary>
    /// NextSavingsAccount class, represents NextSavingsAccount table in database
    /// </summary>
    public class NextSavingsAccount
    {
        //pk
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int NextSavingsAccountId { get; set; }

        //other properties
        [Display(Name = "Next Savings Account")]
        //set range
        [Range(10000, 99999)]
        public int NextAvailableNumber { get; set; }
    }

    /// <summary>
    /// NextMortgageAccount class, represents NextMortgageAccount table in database
    /// </summary>
    public class NextMortgageAccount 
    {
        //pk
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int NextMortgageAccountId { get; set; }

        //other properties
        [Display(Name = "Next Mortgage Account")]
        //set range
        [Range(100000, 999999)]
        public int NextAvailableNumber { get; set; }
    }

    /// <summary>
    /// NextInvestmentAccount class, represents NextInvestmentAccount table in database
    /// </summary>
    public class NextInvestmentAccount
    {
        //pk
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int NextInvestmentAccountId { get; set; }

        //other properties
        [Display(Name = "Next Investment Account")]
        //set range
        [Range(1000000, 9999999)]
        public int NextAvailableNumber { get; set; }
    }

    /// <summary>
    /// NextChequingAccount class, represents NextChequingAccount table in database
    /// </summary>
    public class NextChequingAccount
    {
        //pk
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int NextChequingAccountId { get; set; }

        //other properties
        [Display(Name = "Next Chequing Account")]
        //set range
        [Range(10000000, 99999999)]
        public int NextAvailableNumber { get; set; }
    }

    /// <summary>
    /// NextTransactionNumber class, represents NextTransactionNumber table in database
    /// </summary>
    public class NextTransactionNumber
    {
        //pk
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int NextTransactionNumberId { get; set; }

        //other properties
        [Display(Name = "Next Transaction")]
        //set range
        [Range(100, 10000000)]
        public int NextAvailableNumber { get; set; }
    }

    /// <summary>
    /// Payee class, represents Payee table in database
    /// </summary>
    public class Payee
    {
        //pk
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int PayeeId { get; set; }

        //other properties
        [Display(Name = "Payee")]
        public string Description { get; set; }
    }

    /// <summary>
    /// Institution class, represents Institution table in database
    /// </summary>
    public class Institution
    {
        //pk
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int InstitutionId { get; set; }

        //other properties
        [Display(Name = "Institution\nNumber")]
        public int InstitutionNumber { get; set; }

        [Display(Name = "Institution")]
        public string Description { get; set; }
    }

    /// <summary>
    /// TransactionType class, represents TransactionType table in database
    /// </summary>
    public class TransactionType
    {
        //pk
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int TransactionTypeId { get; set; }

        //other properties
        [Display(Name = "Transaction Type")]
        public string Description { get; set; }

        [Display(Name = "ServiceCharges")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double ServiceCharges { get; set; }
    }

    /// <summary>
    /// Transaction class, represents Transaction table in database
    /// </summary>
    public class Transaction
    {
        //pk
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int TransactionId { get; set; }

        //other properties
        [Required]
        [Display(Name = "Transaction\nNumber")]
        public int TransactionNumber { get; set; }

        //FK - name must match the instance
        //FK navigation property(name of the instance)
        [ForeignKey("BankAccount")]
        [Required]
        [Display(Name = "Account\nNotes")]
        public int BankAccountId { get; set; }

        //FK - name must match the instance
        //FK navigation property(name of the instance)
        [ForeignKey("TransactionType")]
        [Required]
        [Display(Name = "Transaction\nType")]
        public int TransactionTypeId { get; set; }

        //other properties
        //set the data format
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double Deposit { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public double Withdrawal { get; set; }

        [Required]
        [Display(Name = "Date\nCreated")]
        [DisplayFormat(DataFormatString = "{0:D}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        public void SetNextTransactionNumber()
        {
            //set the TransactionNumber property to the appropriate value returned from the NextKey static method 
            TransactionNumber = (int)StoredProcedures.NextKey("NextTransactionNumbers");
        }

        //create an instance of foreign class
        //navigation properties represent the 1 in the class diagram
        public virtual TransactionType TransactionType { get; set; }
        public virtual BankAccount BankAccount { get; set; }
    }

    /// <summary>
    /// RFIDTag class, represents RFIDTag table in database
    /// </summary>
    public class RFIDTag
    {
        //pk
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int RFIDTagId { get; set; }

        //other properties
        public long CardNumber { get; set; }

        //FK
        [ForeignKey("Client")]
        public int ClientId { get; set; }

        //create an instance of foreign class
        //navigation properties represent the 1 in the class diagram
        public virtual Client Client { get; set; }
    }

    /// <summary>
    /// a static class that contain all the storeProcedures
    /// </summary>
    public static class StoredProcedures
    {
        /// <summary>
        /// a static method that call the nextKey procedure in the database
        /// </summary>
        /// <param name="tableName">the table name which wanna access</param>
        /// <returns>the next number available</returns>
        public static long? NextKey(string tableName)
        {
            try
            {
                //create a connection string to the database
                SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=BankofBITContext;Integrated Security=True");
                //declare and initialize the returnValue
                long? returnValue = 0;
                //declare a sqlcommand that access to the "next_key" StoredProcedure
                SqlCommand storedProcedure = new SqlCommand("next_key", connection);
                //set SqlCommand's commandType to StoredProcedure
                storedProcedure.CommandType = CommandType.StoredProcedure;
                //pass the Parameter to the "next_key" StoredProcedure
                storedProcedure.Parameters.AddWithValue("@TableName", tableName);
                //declare the outputParameter
                SqlParameter outputParameter = new SqlParameter("@NewVal", SqlDbType.BigInt)
                {
                    Direction = ParameterDirection.Output
                };
                //pass the outputParameter to the "next_key" StoredProcedure
                storedProcedure.Parameters.Add(outputParameter);
                //open the  connection
                connection.Open();
                //Execute the storedProcedure according to the Parameters
                storedProcedure.ExecuteNonQuery();
                //close the connection
                connection.Close();
                //set the returnValue according to the return from the StoredProcedure
                returnValue = (long?)outputParameter.Value;
                //return the value
                return returnValue;
            }
            catch (NoNullAllowedException)
            {
                return null;
            }
        }
    }
}