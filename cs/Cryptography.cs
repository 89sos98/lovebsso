using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Data;
using System.IO;

namespace Beasyer.Lib
{
    ///// <summary>
    /// Cryptography ��ժҪ˵����
    /// </summary>
    public class Cryptography
    {
        //����KEY
        private static SymmetricAlgorithm key;

        static Cryptography()
        {
            key = new DESCryptoServiceProvider();
        }
        public Cryptography(SymmetricAlgorithm inKey)
        {
            key = inKey;
        }
        ///// <summary>
        /// ��ϣ�ַ���
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static string GetPWDHash(string pwd)
        {
            string ret = "";
            byte[] bpwd = Encoding.ASCII.GetBytes(pwd.Trim());
            byte[] edata;
            SHA256 sha = new SHA256Managed();
            edata = sha.ComputeHash(bpwd);
            ret=Convert.ToBase64String(edata);
            return ret.Trim();
        }
        ///// <summary>
        /// ���ַ������ԳƼ��ܷ��ؼ��ܺ������
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static string GetEncrypt(string original)
        {
            return Convert.ToBase64String(Encrypt(original, key));
        }

        ///// <summary>
        /// ���ַ������Գƽ��ܷ��ؽ��ܺ������
        /// </summary>
        /// <param name="encrypt"></param>
        /// <returns></returns>
        public static string GetDecrypt(string encrypt)
        {
            return Decrypt(Convert.FromBase64String(encrypt), key);
        }

        public string BytesToString(byte[] bs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("000"));
            }
            return sb.ToString();
        }
        public static byte[] StringToBytes(string s)
        {
            int bl = s.Length / 3;
            byte[] bs = new byte[bl];
            for (int i = 0; i < bl; i++)
            {
                bs[i] = byte.Parse(s.Substring(3 * i, 3));
            }
            return bs;
        }

        private static byte[] Encrypt(string PlainText, SymmetricAlgorithm key)
        {
            MemoryStream ms = new MemoryStream();
            CryptoStream encStream = new CryptoStream(ms, key.CreateEncryptor(), CryptoStreamMode.Write);
            StreamWriter sw = new StreamWriter(encStream);
            sw.WriteLine(PlainText);
            sw.Close();
            encStream.Close();
            byte[] buffer = ms.ToArray();
            ms.Close();
            return buffer;
        }

        private static string Decrypt(byte[] CypherText, SymmetricAlgorithm key)
        {
            MemoryStream ms = new MemoryStream(CypherText);
            CryptoStream encStream = new CryptoStream(ms, key.CreateDecryptor(), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(encStream);
            string val = sr.ReadLine();
            sr.Close();
            encStream.Close();
            ms.Close();
            return val;
        }
    }
    ///// <summary>
    /// CRC Ч��
    /// ���ټ���㷨
    /// </summary>
    public class CRC32
    {

        ///// <summary>
        /// 
        /// </summary>
        protected ulong[] crc32Table;

        ///// <summary>
        /// ���죺��ʼ��Ч���
        /// </summary>
        public CRC32()
        {
            const ulong ulPolynomial = 0xEDB88320;
            ulong dwCrc;
            crc32Table = new ulong[256];
            int i, j;
            for (i = 0; i < 256; i++)
            {
                dwCrc = (ulong)i;
                for (j = 8; j > 0; j--)
                {
                    if ((dwCrc & 1) == 1)
                        dwCrc = (dwCrc >> 1) ^ ulPolynomial;
                    else
                        dwCrc >>= 1;
                }
                crc32Table[i] = dwCrc;
            }
        }

        ///// <summary>
        /// �ֽ�����Ч��
        /// </summary>
        /// <param name="buffer">ref �ֽ�����</param>
        /// <returns></returns>
        public ulong ByteCRC(ref byte[] buffer)
        {
            ulong ulCRC = 0xffffffff;
            ulong len;
            len = (ulong)buffer.Length;
            for (ulong buffptr = 0; buffptr < len; buffptr++)
            {
                ulong tabPtr = ulCRC & 0xFF;
                tabPtr = tabPtr ^ buffer[buffptr];
                ulCRC = ulCRC >> 8;
                ulCRC = ulCRC ^ crc32Table[tabPtr];
            }
            return ulCRC ^ 0xffffffff;
        }


        ///// <summary>
        /// �ַ���Ч��
        /// </summary>
        /// <param name="sInputString">�ַ���</param>
        /// <returns></returns>
        public ulong StringCRC(string sInputString)
        {
            byte[] buffer = Encoding.Default.GetBytes(sInputString);
            return ByteCRC(ref buffer);
        }

        ///// <summary>
        /// �ļ�Ч��
        /// </summary>
        /// <param name="sInputFilename">�����ļ�</param>
        /// <returns></returns>
        public ulong FileCRC(string sInputFilename)
        {
            FileStream inFile = new System.IO.FileStream(sInputFilename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            byte[] bInput = new byte[inFile.Length];
            inFile.Read(bInput, 0, bInput.Length);
            inFile.Close();

            return ByteCRC(ref bInput);
        }

    }

    ///// <summary>
    /// MD5 ���������
    /// ��ȡΨһ���������������������
    /// (�޷���ԭ)
    /// </summary>
    public class MD5
    {

        ///// <summary>
        /// 
        /// </summary>
        public MD5()
        {
        }

        ///// <summary>
        /// ��ȡ�ַ�����������
        /// </summary>
        /// <param name="sInputString">�����ı�</param>
        /// <returns></returns>
        public static string HashString(string sInputString)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            string encoded = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(sInputString))).Replace("-", "");
            return encoded;
        }

        ///// <summary>
        /// ��ȡ�ļ���������
        /// </summary>
        /// <param name="sInputFilename">�����ļ�</param>
        /// <returns></returns>
        public string HashFile(string sInputFilename)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            FileStream inFile = new System.IO.FileStream(sInputFilename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            byte[] bInput = new byte[inFile.Length];
            inFile.Read(bInput, 0, bInput.Length);
            inFile.Close();

            string encoded = BitConverter.ToString(md5.ComputeHash(bInput)).Replace("-", "");
            return encoded;
        }

    }

    ///// <summary>
    /// Base64 UUEncoded ����
    /// �������Ʊ���ΪASCII�ı����������紫��
    /// (�ɻ�ԭ)
    /// </summary>
    public class BASE64
    {

        ///// <summary>
        /// 
        /// </summary>
        public BASE64()
        {
        }

        ///// <summary>
        /// �����ַ���
        /// </summary>
        /// <param name="sInputString">�����ı�</param>
        /// <returns></returns>
        public string DecryptString(string sInputString)
        {
            char[] sInput = sInputString.ToCharArray();
            try
            {
                byte[] bOutput = System.Convert.FromBase64String(sInputString);
                return Encoding.Default.GetString(bOutput);
            }
            catch (System.ArgumentNullException)
            {
                //base 64 �ַ�����Ϊnull
                return "";
            }
            catch (System.FormatException)
            {
                //���ȴ����޷�����4
                return "";
            }
        }

        ///// <summary>
        /// �����ַ���
        /// </summary>
        /// <param name="sInputString">�����ı�</param>
        /// <returns></returns>
        public string EncryptString(string sInputString)
        {
            byte[] bInput = Encoding.Default.GetBytes(sInputString);
            try
            {
                return System.Convert.ToBase64String(bInput, 0, bInput.Length);
            }
            catch (System.ArgumentNullException)
            {
                //����������ΪNULL.
                return "";
            }
            catch (System.ArgumentOutOfRangeException)
            {
                //���Ȳ���
                return "";
            }
        }

        ///// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="sInputFilename">�����ļ�</param>
        /// <param name="sOutputFilename">����ļ�</param>
        public void DecryptFile(string sInputFilename, string sOutputFilename)
        {
            System.IO.StreamReader inFile;
            char[] base64CharArray;

            try
            {
                inFile = new System.IO.StreamReader(sInputFilename,
                    System.Text.Encoding.ASCII);
                base64CharArray = new char[inFile.BaseStream.Length];
                inFile.Read(base64CharArray, 0, (int)inFile.BaseStream.Length);
                inFile.Close();
            }
            catch
            {//(System.Exception exp) {
                return;
            }

            // ת��Base64 UUEncodedΪ���������
            byte[] binaryData;
            try
            {
                binaryData =
                    System.Convert.FromBase64CharArray(base64CharArray,
                    0,
                    base64CharArray.Length);
            }
            catch (System.ArgumentNullException)
            {
                //base 64 �ַ�����Ϊnull
                return;
            }
            catch (System.FormatException)
            {
                //���ȴ����޷�����4
                return;
            }

            // д�������
            System.IO.FileStream outFile;
            try
            {
                outFile = new System.IO.FileStream(sOutputFilename,
                    System.IO.FileMode.Create,
                    System.IO.FileAccess.Write);
                outFile.Write(binaryData, 0, binaryData.Length);
                outFile.Close();
            }
            catch
            {// (System.Exception exp) {
                //������
            }

        }

        ///// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="sInputFilename">�����ļ�</param>
        /// <param name="sOutputFilename">����ļ�</param>
        public void EncryptFile(string sInputFilename, string sOutputFilename)
        {

            System.IO.FileStream inFile;
            byte[] binaryData;

            try
            {
                inFile = new System.IO.FileStream(sInputFilename,
                    System.IO.FileMode.Open,
                    System.IO.FileAccess.Read);
                binaryData = new Byte[inFile.Length];
                long bytesRead = inFile.Read(binaryData, 0,
                    (int)inFile.Length);
                inFile.Close();
            }
            catch
            { //(System.Exception exp) {
                return;
            }

            // ת������������ΪBase64 UUEncoded���
            // ÿ3���ֽ���Դ��������Ϊ4���ֽ� 
            long arrayLength = (long)((4.0d / 3.0d) * binaryData.Length);

            // ����޷�����4
            if (arrayLength % 4 != 0)
            {
                arrayLength += 4 - arrayLength % 4;
            }

            char[] base64CharArray = new char[arrayLength];
            try
            {
                System.Convert.ToBase64CharArray(binaryData,
                    0,
                    binaryData.Length,
                    base64CharArray,
                    0);
            }
            catch (System.ArgumentNullException)
            {
                //����������ΪNULL.
                return;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                //���Ȳ���
                return;
            }

            // дUUEncoded���ݵ��ļ���
            System.IO.StreamWriter outFile;
            try
            {
                outFile = new System.IO.StreamWriter(sOutputFilename,
                    false,
                    System.Text.Encoding.ASCII);
                outFile.Write(base64CharArray);
                outFile.Close();
            }
            catch
            {// (System.Exception exp) {
                //�ļ�������
            }


        }
    }    
}
