using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Utility;
using System.IO;
using BankOfBIT.Models;
using WindowsBanking.ServiceReference1;
using System.Windows.Forms;

namespace WindowsBanking
{
    class Batch
    {
        // This string property will represent the file being processed 
        private string fileName { get; set; }
        // This string will represent all data to be written to the log file that corresponds with the file being processed 
        private string logData { get; set; }
        //This string will represent the name of the log file that corresponds with the file being processed
        private string logName { get; set; }

        XDocument xDocDetails;
        BankOfBITContext db = new BankOfBITContext();

        /// <summary>
        /// This method will process all errors found within the current file being processed
        /// </summary>
        /// <param name="beforeQuery"> represents the records that existed before the round of validation </param>
        /// <param name="afterQuery"> represents the records that remained after the round of validation</param>
        /// <param name="message"> represents the error message that is to be written to the log file based on the record failing the round of validation </param>
        private void processErrors(IEnumerable<XElement> beforeQuery, IEnumerable<XElement> afterQuery, string message)
        {
            //Compare the records from the beforeQuery with those from the afterQuery.
            if (beforeQuery.Count() != afterQuery.Count())
            {
                //Any records that do not exist in both failed that round of validation
                IEnumerable<XElement> differentQuery = beforeQuery.Except(afterQuery);
                foreach( XElement xele in differentQuery)
                {
                    logData += "\n-------ERROR-------";
                    logData += "\nFile: " + fileName;
                    int iNoteCount = xele.Elements().Count();
                    IEnumerable<XElement> notes = xele.Elements();
                    foreach (XElement note in notes)
                    {
                        string sEleName = note.Name.ToString();

                        switch (sEleName)
                        {
                            case "account_no":
                                logData += "\nAccount No: " + note.Value;
                                break;
                            case "type":
                                logData += "\nType: " + note.Value;
                                break;
                            case "in":
                                logData += "\nIn: " + note.Value;
                                break;
                            case "out":
                                logData += "\nOut: " + note.Value;
                                break;
                            case "notes":
                                logData += "\nNotes: " + note.Value;
                                break;
                        }
                    }
                    logData += "\nNotes: " + iNoteCount;
                    logData += "\n" + message;
                    logData += "\n-------------------";
                }
            }
        }

        /// <summary>
        /// This method is used to verify the attributes of the xml file’s root element. 
        /// </summary>
        /// <returns>If any of the validation described fails, return a value of false </returns>
        private bool processHeader()
        {
            //Define an XDocument object and populate this object with the contents of the current file (fileName)
            xDocDetails = XDocument.Load(fileName);

            //Define an XElement object and populate this object with the data found within the transfer element of the xml file
            XElement xeleElement = xDocDetails.Element("transfer");

            try
            {
                //The XElement object must have 4 attributes
                if (xeleElement.Attributes().Count() != 4)
                {
                    var ex = new Exception("The XElement object must have 4 attributes");
                    throw ex;
                }
                    //Attribute
                XAttribute dateAttribute = xeleElement.Attribute("date");
                
                //The date attribute of the XElement object must be equal to today’s date
                if (!dateAttribute.Value.Equals(System.DateTime.Today.ToString("yyyy-MM-dd")))
                {
                    var ex = new Exception("The date attribute of the XElement object must be equal to today’s date");
                    throw ex;
                }
                XAttribute toAttribute = xeleElement.Attribute("to");
                
                //The to attribute must match the Bank of BIT institution number 
                if (!toAttribute.Value.Equals(BusinessRules.BankNumber().ToString()))
                {
                    var ex = new Exception("The to attribute must match the Bank of BIT institution number ");
                    throw ex;
                }
                XAttribute fromAttribute = xeleElement.Attribute("from");
                int ifromAttribute = 0;
                if (fromAttribute != null)
                {
                    ifromAttribute = int.Parse(fromAttribute.Value);
                }
                //retrive data form institution table
                List<int> institutionNumbers = db.Institutions.Where(d => d.InstitutionNumber == ifromAttribute).Select(d => d.InstitutionNumber).ToList();

                //The from attribute must match one of the institution number values within the Institution Entity class 
                if (institutionNumbers.Count == 0)
                {
                    var ex = new Exception("The from attribute must match one of the institution number values within the Institution Entity class");
                    throw ex;
                }

                XAttribute checksumAttribute = xeleElement.Attribute("checksum");
                int iChecksum = 0;
                IEnumerable<XElement> accountNoElements =
                    xDocDetails.Descendants().Where(d => d.Name == "account_no");

                foreach (XElement xele in accountNoElements)
                {
                    iChecksum += int.Parse(xele.Value);
                }
                //The checksum attribute must match the sum of all account_no elements in the file 
                if (int.Parse(checksumAttribute.Value) != iChecksum)
                {
                    var ex = new Exception("The checksum attribute must match the sum of all account_no elements in the file ");
                    throw ex;
                }
                return true;
            }
            catch (Exception ex)
            {

                logData += "\nError occor cause: " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// This method is used to verify the contents of the detail records in the input file. 
        /// </summary>
        private void processDetails()
        {
            //Define an XDocument object and populate this object with the contents of the current file (fileName) 
            //xDocDetails = XDocument.Load(this.fileName);
            xDocDetails = XDocument.Load(fileName);

            IEnumerable<XElement> transactionElements = xDocDetails.Descendants().Where(d => d.Name == "transaction");

            //Each transaction node in the xml file must have 6 child nodes.
            //IEnumerable<XElement> sixChildtransactionElements = transactionElements.Where(d => d.Element("from_institution") != null
            //    && d.Element("account_no") != null && d.Element("type") != null && d.Element("in") != null
            //    && d.Element("out") != null && d.Element("notes") != null);

            IEnumerable<XElement> sixChildtransactionElements = transactionElements.Where(d => d.Nodes().Count() == 6);

            processErrors(transactionElements, sixChildtransactionElements, "Missing Notes");

            //The in, out, and type nodes within each transaction node must be numeric.
            decimal dnumericValue;
            int inumericValue;

            IEnumerable<XElement> numerictransactionElements = sixChildtransactionElements.Where(d => decimal.TryParse(d.Element("in").Value, out dnumericValue)
                && decimal.TryParse(d.Element("out").Value, out dnumericValue)
                && int.TryParse(d.Element("type").Value, out inumericValue));

            processErrors(sixChildtransactionElements, numerictransactionElements, "Deposit, withdrawal and type should be numeric value");

            //The type node within each transaction node must have a value of 1 or 2.
            IEnumerable<XElement> typetransactionElements = numerictransactionElements.Where(d => d.Element("type").Value.Equals("1")
                || d.Element("type").Value.Equals("2"));

            processErrors(numerictransactionElements, typetransactionElements, "Invalid Transaction Type");

            //The in and out nodes within each transaction node must be a positive number.
            IEnumerable<XElement> positivetransactionElements = typetransactionElements.Where(d => decimal.Parse(d.Element("in").Value) >= 0
                && decimal.Parse(d.Element("out").Value) >= 0);

            processErrors(typetransactionElements, positivetransactionElements, "Deposit and withdrawal should be positive");

            //The account_no node within each transaction node must exist in the database.
            //IEnumerable<XElement> existtransactionElements = from positiveElement in positivetransactionElements
            //                                                 join bankAccount in db.BankAccounts on positiveElement.Element("account_no").Value equals bankAccount.AccountNumber.ToString()
            //                                                 select positiveElement; 

            //Define an IEnumerable<int> LINQ-to-SQL Server query against the BankAccount Entity Class retrieving a list of all Account Numbers.
            IEnumerable<int> accountNumbers = db.BankAccounts.Select(account => account.AccountNumber);
            //Define an IEnumerable<XElement> LINQ-to-Object query against the previous query results
            IEnumerable<XElement> finaltransactionElements = from positiveElement in positivetransactionElements
                                                             join bankAccount in accountNumbers on positiveElement.Element("account_no").Value equals bankAccount.ToString()
                                                             select positiveElement;


            processErrors(positivetransactionElements, finaltransactionElements, "Account Number is not exist");
            processTransactions(finaltransactionElements);
        }

        /// <summary>
        /// This method is used to process all valid transaction records 
        /// </summary>
        /// <param name="transactionRecords"> the result set passed to this method </param>
        private void processTransactions(IEnumerable<XElement> transactionRecords)
        {
            //Evaluate the type node
            IEnumerable<XElement> depositTransactionElements = transactionRecords.Where(d => d.Element("type").Value.Equals("1"));

            int iDepositCount = depositTransactionElements.Count();

            IEnumerable<XElement> withdrawalTransactionElements = transactionRecords.Where(d => d.Element("type").Value.Equals("2"));

            int iWithdrawalCount = withdrawalTransactionElements.Count();

            //create new instance
            TransactionManagerClient transactionManagerClient = new TransactionManagerClient();

            long? transactionNumber;

            //If the type is a 1 (indicating deposit)
            if (iDepositCount > 0)
            {
                //retrive the account number
                List<string> sBankAccountNumbers = depositTransactionElements.Select(d => d.Element("account_no").Value).ToList();
                List<string> sAmounts = depositTransactionElements.Select(d => d.Element("in").Value).ToList();
                List<string> sNotes = depositTransactionElements.Select(d => d.Element("notes").Value).ToList();
                for (int i = 0; i < iDepositCount; i++)
                {
                    int iBankAccountNumber = int.Parse(sBankAccountNumbers[i]);

                    //retrive the accountID
                    int iBankAccountID = db.BankAccounts.Where(d => d.AccountNumber == iBankAccountNumber).Select(d => d.BankAccountId).Single();
                    //make the deposit
                    transactionManagerClient.Deposit(iBankAccountID, double.Parse(sAmounts[i]));

                    transactionNumber = transactionManagerClient.CreateTransaction(true, iBankAccountID, double.Parse(sAmounts[i]),
                                                                        1, sNotes[i]);

                    logData += "\nTransaction " + transactionNumber + " completed successfully";
                }
                
            }

            //If the type is a 2 (indicating withdrawal)
            if (iWithdrawalCount > 0)
            {
                //retrive the account number
                //IEnumerable<XElement> xBankAccountNumbers = withdrawalTransactionElements.Where(d => d.Element("account_no")).ToList();
                List<string> sBankAccountNumbers = withdrawalTransactionElements.Select(d => d.Element("account_no").Value).ToList();
                List<string> sAmounts = withdrawalTransactionElements.Select(d => d.Element("out").Value).ToList();
                List<string> sNotes = withdrawalTransactionElements.Select(d => d.Element("notes").Value).ToList();
                for (int i = 0; i < iWithdrawalCount; i++)
                {
                    int iBankAccountNumber = int.Parse(sBankAccountNumbers[i]);
                    //retrive the accountID
                    int iBankAccountID = db.BankAccounts.Where(d => d.AccountNumber == iBankAccountNumber).Select(d => d.BankAccountId).Single();
                    //make the deposit
                    transactionManagerClient.Withdrawal(iBankAccountID, double.Parse(sAmounts[i]));

                    transactionNumber = transactionManagerClient.CreateTransaction(false, iBankAccountID, double.Parse(sAmounts[i]),
                                                                        2, sNotes[i]);
                    logData += "\nTransaction " + transactionNumber + " completed successfully";
                }
            }
        }

        /// <summary>
        /// This method will be called when a file has been processed 
        /// </summary>
        /// <returns></returns>
        public string WriteLogData()
        {
            string sReturn = "The file is not exit!";

            //Determine whether the "COMPLETE" file already exists
            if (File.Exists("COMPLETE-" + fileName))
            {
                //use the File.Delete method to delete the "COMPLETE" file
                File.Delete("COMPLETE-" + fileName);
            }
            //Determine whether the original input file exists and if so
            if (File.Exists(fileName))
            {
                //Use the File.Move method to cause the input xml file to be renamed to include the word "COMPLETE" at the beginning of the file name.
                File.Move(fileName, "COMPLETE-" + fileName);

                string filePath = string.Format(@"{0}\Logs\{1}",
                                                Application.StartupPath,
                                                logName);

                StreamWriter swWrite;

                if (!File.Exists(filePath))
                {
                    FileStream stream = new FileStream(filePath, FileMode.Create);
                    swWrite = new StreamWriter(stream);
                }
                else
                {
                    swWrite = new StreamWriter(filePath, true);
                }

                //Writes a stream of characters as well as a new line marker to output file associated with StreamWrite
                swWrite.WriteLine(logData);

                //When all data has been written, close the StreamWriter
                swWrite.Close();
            }
            //Capture the contents of the module level logging variable to return from this method
            sReturn = logData;

            //Clear the contents of logData so that it may be used to capture data for the next transmission file
            logData = "";
            //Clear the contents of logName, so that it may be used to store the next log file name
            logName = "";
            

            return sReturn;
        }

        /// <summary>
        /// This method will initiate the bank transmission process by determining the appropriate filename and then proceeding with the header and detail processing
        /// </summary>
        /// <param name="institution"></param>
        /// <param name="key">can be ignored for now</param>
        public void ProcessTransmission(string institution, string key)
        {
            fileName = System.DateTime.Today.Year + "-" + System.DateTime.Today.DayOfYear.ToString("000") + "-" + institution + ".xml";

            //add for assignment 9
            //Define a string variable to represent the encrypted file name
            string EncryptedFilename = fileName + ".encrypted";

            logName = "LOG " + fileName.Replace(".xml", ".txt");

            //Determine whether the encrypted file exists
            try
            {
                if (File.Exists(EncryptedFilename))
                {
                    Utility.Encryption.Decrypt(EncryptedFilename, fileName, key);
                    //Utility.Encryption2.Decrypt(EncryptedFilename, fileName, key);
                    //Determine whether a file exists matching the input filename created above (not the LOG filename)
                    if (File.Exists(fileName))
                    {
                        //Call the processHeader method defined earlier
                        if (processHeader())
                        {
                            processDetails();
                        }

                    }
                    else
                    {
                        //Append a relevant message to logData indicating that the file does not exist.
                        logData += "\n" + fileName + " does not exist!";
                    }
                }
                else
                {
                    logData += "\n" + EncryptedFilename + " does not exist!";
                }
            }
            catch (Exception ex)
            {
                logData += "\nError occor cause: " + ex.Message;
            }
        }
    }
}
