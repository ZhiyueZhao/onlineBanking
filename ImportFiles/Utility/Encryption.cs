using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Utility
{ 
    public static class Encryption
    {
        /// <summary>
        /// This method will be used encrypt a file
        /// </summary>
        /// <param name="unencryptedFileName"></param>
        /// <param name="encryptedFileName"></param>
        /// <param name="key"></param>
        public static void Encrypt(string unencryptedFileName, string encryptedFileName, string key)
        {
            //Create a FileStream object based on the unencrypted file name with an Open filemode, and a Read fileaccess
            FileStream UnEncryptedFileStream = new FileStream(unencryptedFileName, FileMode.Open, FileAccess.Read);
            //Create a second FileStream object based on the (eventual) encrypted file name with a Create filemode and a Write fileaccess
            FileStream EncryptedFileStream = new FileStream(encryptedFileName, FileMode.Create, FileAccess.Write);
            //Create a DESCryptoServiceProvider object
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            //Set the DESCryptoServiceProvider’s Key property to this sequence of bytes
            DES.Key = ASCIIEncoding.ASCII.GetBytes(key);
            //Set the DESCryptoServiceProvider’s IV (initialization vector) property to this sequence of bytes
            DES.IV = ASCIIEncoding.ASCII.GetBytes(key);

            //Create a ICryptoTransform object using the CreateEncryptor method of the DESCryptoServiceProvider object
            ICryptoTransform desEncrypt = DES.CreateEncryptor();

            //Create a CryptoStream object using the (eventual) encrypted filestream, the ICryptoTransform object, and using Write CryptoStreamMode
            CryptoStream cryptostreamEncr = new CryptoStream(EncryptedFileStream, desEncrypt, CryptoStreamMode.Write);

            //Define a byte array based on the Length of the unencrypted filestream
            byte[] bytearray = new byte[UnEncryptedFileStream.Length];

            //Read data using the unencrypted filestream
            UnEncryptedFileStream.Read(bytearray, 0, bytearray.Length);

            //Write the encrypted data using the CryptoStream object
            cryptostreamEncr.Write(bytearray, 0, bytearray.Length);
            //Close the cryptostream object
            cryptostreamEncr.Close();
            //Close the unencrypted filestream
            UnEncryptedFileStream.Close();
            //Close the encrypted filestream
            EncryptedFileStream.Close();
        }

        /// <summary>
        /// This method will decrypt a file
        /// </summary>
        /// <param name="encryptedFileName"></param>
        /// <param name="unencryptedFileName"></param>
        /// <param name="key"></param>
        public static void Decrypt(string encryptedFileName, string unencryptedFileName, string key)
        {
            //Create a StreamWriter object based on the decrypted filename
            StreamWriter swWrite;
            //check if the file exist
            if (!File.Exists(unencryptedFileName))
            {
                FileStream stream = new FileStream(unencryptedFileName, FileMode.Create);
                swWrite = new StreamWriter(stream);
            }
            else
            {
                swWrite = new StreamWriter(unencryptedFileName, true);
            }

            try
            {
                //Create a DESCryptoServiceProvider object
                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                //Set the DESCryptoServiceProvider’s Key property to this sequence of bytes
                DES.Key = ASCIIEncoding.ASCII.GetBytes(key);
                //Set the DESCryptoServiceProvider’s IV (initialization vector) property to this sequence of bytes
                DES.IV = ASCIIEncoding.ASCII.GetBytes(key);

                //Create a FileStream object based on the encrypted filename with an Open filemode, and a Read fileaccess
                FileStream EncryptedFileStream = new FileStream(encryptedFileName, FileMode.Open, FileAccess.Read);

                //Create a ICryptoTransform object using the CreateDecryptor method of the DESCryptoServiceProvider object
                ICryptoTransform desDecrypt = DES.CreateDecryptor();

                //Create a CryptoStream object using the encrypted file filestream, the ICryptoTransform object, and using Read CryptoStreamMode
                CryptoStream cryptostreamDecr = new CryptoStream(EncryptedFileStream, desDecrypt, CryptoStreamMode.Read);
                //Using the decrypted StreamWriter object, Write a new (decrypted) file based on the CryptoStream object
                swWrite.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
                //Flush the decrypted StreamWriter object so that any buffered data is also written to the StreamWriter’s file
                swWrite.Flush();
                //Close the decrypted StreamWriter object’s file
                swWrite.Close();
            }
            catch (Exception)
            {
                //Close the decrypted StreamWriter object’s file
                swWrite.Close();
                //Throw an exception (to be caught in the Windows Banking project) with an appropriate message
                var ex = new Exception("decrypt fail!");
                throw ex;
            }
        }
    }
}
