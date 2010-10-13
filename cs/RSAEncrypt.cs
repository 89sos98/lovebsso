using System;   
using System.Collections.Generic;   
using System.Text;   
using System.Security.Cryptography;   
using System.IO;   
namespace Lib.Encryption   
{   
    /// <summary>   
    /// RSA���ܡ�����String��byte[]��������Կ   
    /// </summary>   
    public class RSAEncrypt   
    {  
        #region RSA ����Կ����   
        /// <summary>   
        /// RSA ����Կ���� ����˽Կ �͹�Կ   
        /// </summary>   
        /// <param name="xmlPrivateKey">˽�_</param>   
        /// <param name="xmlPublicKey">��Կ</param>   
        public static void GenerateRSAKey(out string xmlPrivateKey, out string xmlPublicKey)   
        {   
            System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();   
            xmlPrivateKey = rsa.ToXmlString(true);   
            xmlPublicKey = rsa.ToXmlString(false);   
        }  
        #endregion  
        #region RSA��Կ�ĵ��� ����   
        /// <summary>   
        /// ��ָ��λ�õ�����Կ����ȡ��Կ   
        /// </summary>   
        /// <param name="xmlRSAKeyPath">��Կ·��</param>   
        /// <returns>��Կ��ֵ</returns>   
        public static string ImportRSAKey(string xmlRSAKeyPath)   
        {   
            string xmlRSAKeyValue = "";   
            using (StreamReader reader = new StreamReader(xmlRSAKeyPath))   
            {   
                xmlRSAKeyValue = reader.ReadToEnd();   
                reader.Close();   
            }   
            return xmlRSAKeyValue;   
        }   
        /// <summary>   
        /// ��ָ��λ�õ�����Կ   
        /// </summary>   
        /// <param name="xmlRSAKeyValue">��Կ��ֵ</param>   
        /// <param name="xmlRSAKeyPath">��Կ�����·��</param>   
        public static void ExportRSAKey(string xmlRSAKeyValue, string xmlRSAKeyPath)   
        {   
            using (StreamWriter writer = new StreamWriter(xmlRSAKeyPath))   
            {   
                writer.Write(xmlRSAKeyValue);   
                writer.Close();   
            }   
        }  
        #endregion  
        #region RSA�ļ��ܺ���   
        //##############################################################################   
        //RSA ��ʽ����   
        //˵��KEY������XML����ʽ,���ص����ַ���   
        //����һ����Ҫ˵�������ü��ܷ�ʽ�� ���� ���Ƶģ���   
        //##############################################################################   
        /// <summary>   
        /// RSA�ļ��ܷ���   
        /// </summary>   
        /// <param name="xmlPublicKey">��Կ</param>   
        /// <param name="plaintextString">���� string</param>   
        /// <returns>����</returns>   
        public static string Encrypt(string xmlPublicKey, string plaintextString)   
        {   
            byte[] plaintextByteArray;   
            byte[] ciphertextByteArray;   
            string ciphertextString;              
            plaintextByteArray = (new UnicodeEncoding()).GetBytes(plaintextString);   
            ciphertextByteArray = Encrypt(xmlPublicKey, plaintextByteArray);   
            ciphertextString = Convert.ToBase64String(ciphertextByteArray);   
            return ciphertextString;   
        }   
        /// <summary>   
        /// RSA�ļ��ܷ���   
        /// </summary>   
        /// <param name="xmlPublicKey">��Կ</param>   
        /// <param name="plaintextByteArray">���� byte[]</param>   
        /// <returns>����</returns>   
        public static byte[] Encrypt(string xmlPublicKey, byte[] plaintextByteArray)   
        {   
            byte[] ciphertextByteArray;              
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();   
            rsa.FromXmlString(xmlPublicKey);   
            ciphertextByteArray = rsa.Encrypt(plaintextByteArray, false);   
            return ciphertextByteArray;   
        }  
        #endregion  
        #region RSA�Ľ��ܺ���   
        /// <summary>   
        /// RSA�Ľ��ܷ���   
        /// </summary>   
        /// <param name="xmlPrivateKey">˽�_</param>   
        /// <param name="ciphertextString">���� string</param>   
        /// <returns>����</returns>   
        public static string Decrypt(string xmlPrivateKey, string ciphertextString)   
        {   
            byte[] plaintextByteArray;   
            byte[] ciphertextByteArray;   
            string plaintextString;             
            ciphertextByteArray = Convert.FromBase64String(ciphertextString);   
            plaintextByteArray = Decrypt(xmlPrivateKey, ciphertextByteArray);   
            plaintextString = (new UnicodeEncoding()).GetString(plaintextByteArray);   
            return plaintextString;   
        }   
        /// <summary>   
        /// RSA�Ľ��ܷ���   
        /// </summary>   
        /// <param name="xmlPrivateKey">˽�_</param>   
        /// <param name="ciphertextByteArray">���� byte[]</param>   
        /// <returns>����</returns>   
        public static byte[] Decrypt(string xmlPrivateKey, byte[] ciphertextByteArray)   
        {   
            byte[] plaintextByteArray;              
            System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();   
            rsa.FromXmlString(xmlPrivateKey);   
            plaintextByteArray = rsa.Decrypt(ciphertextByteArray, false);   
            return plaintextByteArray;   
        }  
        #endregion   
    }   
}