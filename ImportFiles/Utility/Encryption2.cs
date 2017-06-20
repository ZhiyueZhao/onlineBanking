using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//added for cryptography
using System.Security.Cryptography;
using System.IO;

namespace Utility
{
    public static class Encryption2
    {
        /// <summary>
        /// used to encrypt a file
        /// </summary>
        /// <param name="unencryptedFileName">unencrypted file name</param>
        /// <param name="encryptedFileName">encrypted file name</param>
        /// <param name="key">key for encoding</param>
        public static void Encrypt(string unencryptedFileName, string encryptedFileName, string key)
        {
            //Create a FileStream object based on the unencrypted file name
            FileStream PlainTextFileStream = new FileStream(unencryptedFileName, FileMode.Open, FileAccess.Read);

            //Creates a second FileStream object based on the (eventual) encrypted file name
            FileStream EncryptedFileStream = new FileStream(encryptedFileName, FileMode.Create, FileAccess.Write);

            //creates a DESCryptoServiceProvider object
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();

            //Set the DESCryptoServiceProvider’s Key property to AsciiEncoding.Ascii.GetBytes method
            DES.Key = ASCIIEncoding.ASCII.GetBytes(key);

            //Set the DESCryptoServiceProvider’s IV (initialization vector) property to AsciiEncoding.Ascii.GetBytes method
            DES.IV = ASCIIEncoding.ASCII.GetBytes(key);

            //Create a ICryptoTransform object using the CreateEncryptor method of the DESCryptoServiceProvider object
            ICryptoTransform desEncrypt = DES.CreateEncryptor();

            //Create a CryptoStream object using the (eventual) encrypted filestream, the ICryptoTransform object, and using Write CryptoStreamMode
            CryptoStream cryptostreamEncr = new CryptoStream(EncryptedFileStream, desEncrypt, CryptoStreamMode.Write);

            //Defines a byte array based on the Length of the unencrypted filestream
            byte[] bytearray = new byte[PlainTextFileStream.Length];

            //Read this data into the byte array with no offset using the entire length of the bytearray
            PlainTextFileStream.Read(bytearray, 0, bytearray.Length);

            //Write the encrypted data using the CryptoStream object
            cryptostreamEncr.Write(bytearray, 0, bytearray.Length);

            //Close the cryptostream object
            cryptostreamEncr.Close();

            //Close the unencrypted filestream
            PlainTextFileStream.Close();

            //Close the encrypted filestream
            EncryptedFileStream.Close();
        }

        /// <summary>
        /// method that will decrypt a file
        /// </summary>
        /// <param name="encryptedFileName">encrypted file name</param>
        /// <param name="unencryptedFileName">unencrypted file name</param>
        /// <param name="key">key for encoding</param>
        public static void Decrypt(string encryptedFileName, string unencryptedFileName, string key)
        {
            //Create a StreamWriter object based on the decrypted filename
            StreamWriter swDecrypted = new StreamWriter(unencryptedFileName);

            try
            {
                //Create a DESCryptoServiceProvider object
                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();

                //Set the DESCryptoServiceProvider’s Key property to AsciiEncoding.Ascii.GetBytes method
                DES.Key = ASCIIEncoding.ASCII.GetBytes(key);

                //Set the DESCryptoServiceProvider’s IV (initialization vector) property to AsciiEncoding.Ascii.GetBytes method
                DES.IV = ASCIIEncoding.ASCII.GetBytes(key);

                //Create a FileStream object based on the encrypted filename with an Open filemode, and a Read fileaccess
                FileStream fsDecrypt = new FileStream(encryptedFileName, FileMode.Open, FileAccess.Read);

                //Create a ICryptoTransform object using the CreateDecryptor method of the DESCryptoServiceProvider object
                ICryptoTransform desDecrypt = DES.CreateEncryptor();

                //Create a CryptoStream object using the encrypted file filestream, the ICryptoTransform object, and using Read CryptoStreamMode
                CryptoStream cryptostreamDecr = new CryptoStream(fsDecrypt, desDecrypt, CryptoStreamMode.Read);

                //Using the decrypted StreamWriter object, Write a new (decrypted) file based on the CryptoStream object
                StreamReader sr = new StreamReader(cryptostreamDecr);
                string ss = new StreamReader(cryptostreamDecr).ReadToEnd();
                swDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());

                //Flush the decrypted StreamWriter object so that any buffered data is also written to the StreamWriter’s file
                swDecrypted.Flush();

                //Close the decrypted StreamWriter object’s file
                swDecrypted.Close();
            }
            catch (Exception)
            {
                //Close the decrypted StreamWriter object’s file
                swDecrypted.Close();

                //throw an exception
                throw new Exception("An error occured decrpyting the file.");
            }
        }
    }
}
