using System;   
using System.Collections.Generic;   
using System.Text;   
using System.Security.Cryptography;   
using System.IO;   
namespace Lib.Encryption   
{   
    /// <summary>   
    /// RSA加密、解密String、byte[]，生成密钥   
    /// </summary>   
    public class RSAEncrypt   
    {  
        #region RSA 的密钥产生   
        /// <summary>   
        /// RSA 的密钥产生 产生私钥 和公钥   
        /// </summary>   
        /// <param name="xmlPrivateKey">私鈅</param>   
        /// <param name="xmlPublicKey">公钥</param>   
        public static void GenerateRSAKey(out string xmlPrivateKey, out string xmlPublicKey)   
        {   
            System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();   
            xmlPrivateKey = rsa.ToXmlString(true);   
            xmlPublicKey = rsa.ToXmlString(false);   
        }  
        #endregion  
        #region RSA密钥的导入 导出   
        /// <summary>   
        /// 从指定位置导入密钥，读取密钥   
        /// </summary>   
        /// <param name="xmlRSAKeyPath">密钥路径</param>   
        /// <returns>密钥的值</returns>   
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
        /// 向指定位置导出密钥   
        /// </summary>   
        /// <param name="xmlRSAKeyValue">密钥的值</param>   
        /// <param name="xmlRSAKeyPath">密钥的输出路径</param>   
        public static void ExportRSAKey(string xmlRSAKeyValue, string xmlRSAKeyPath)   
        {   
            using (StreamWriter writer = new StreamWriter(xmlRSAKeyPath))   
            {   
                writer.Write(xmlRSAKeyValue);   
                writer.Close();   
            }   
        }  
        #endregion  
        #region RSA的加密函数   
        //##############################################################################   
        //RSA 方式加密   
        //说明KEY必须是XML的行式,返回的是字符串   
        //在有一点需要说明！！该加密方式有 长度 限制的！！   
        //##############################################################################   
        /// <summary>   
        /// RSA的加密方法   
        /// </summary>   
        /// <param name="xmlPublicKey">公钥</param>   
        /// <param name="plaintextString">明文 string</param>   
        /// <returns>密文</returns>   
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
        /// RSA的加密方法   
        /// </summary>   
        /// <param name="xmlPublicKey">公钥</param>   
        /// <param name="plaintextByteArray">明文 byte[]</param>   
        /// <returns>密文</returns>   
        public static byte[] Encrypt(string xmlPublicKey, byte[] plaintextByteArray)   
        {   
            byte[] ciphertextByteArray;              
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();   
            rsa.FromXmlString(xmlPublicKey);   
            ciphertextByteArray = rsa.Encrypt(plaintextByteArray, false);   
            return ciphertextByteArray;   
        }  
        #endregion  
        #region RSA的解密函数   
        /// <summary>   
        /// RSA的解密方法   
        /// </summary>   
        /// <param name="xmlPrivateKey">私鈅</param>   
        /// <param name="ciphertextString">密文 string</param>   
        /// <returns>明文</returns>   
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
        /// RSA的解密方法   
        /// </summary>   
        /// <param name="xmlPrivateKey">私鈅</param>   
        /// <param name="ciphertextByteArray">密文 byte[]</param>   
        /// <returns>明文</returns>   
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