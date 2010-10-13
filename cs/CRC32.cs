using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text;
namespace YNEC.Services.Encrypt 
{
  ///////// <summary>
  /// CRC Ч��
  /// ���ټ���㷨
  /// </summary>
  public class CRC32
{

    protected ulong[] crc32Table;

    ///////// <summary>
    /// ���죺��ʼ��Ч���
    /// </summary>
    public CRC32() 
{
      const ulong ulPolynomial = 0xEDB88320;
      ulong dwCrc;
      crc32Table = new ulong[256];
      int i,j;
      for(i = 0; i < 256; i++) 
{
        dwCrc = (ulong)i;
        for(j = 8; j > 0; j--) 
{
          if((dwCrc & 1)==1)
            dwCrc = (dwCrc >> 1) ^ ulPolynomial;
          else
            dwCrc >>= 1;
        }
        crc32Table[i] = dwCrc;
      }
    }

    ///////// <summary>
    /// �ֽ�����Ч��
    /// </summary>
    /// <param name="buffer">ref �ֽ�����</param>
    /// <returns></returns>
    public ulong ByteCRC(ref byte[] buffer) 
{
      ulong ulCRC = 0xffffffff; 
      ulong len; 
      len = (ulong)buffer.Length;
      for (ulong buffptr=0; buffptr < len; buffptr++) 
{
             ulong tabPtr = ulCRC & 0xFF;
        tabPtr = tabPtr ^ buffer[buffptr];
        ulCRC = ulCRC >> 8;
        ulCRC = ulCRC ^ crc32Table[tabPtr];
      }
      return ulCRC ^ 0xffffffff; 
    }


    ///////// <summary>
    /// �ַ���Ч��
    /// </summary>
    /// <param name="sInputString">�ַ���</param>
    /// <returns></returns>
    public ulong StringCRC(string sInputString)
{
      byte[] buffer = Encoding.Default.GetBytes(sInputString);
      return ByteCRC(ref buffer);
    }

    ///////// <summary>
    /// �ļ�Ч��
    /// </summary>
    /// <param name="sInputFilename">�����ļ�</param>
    /// <returns></returns>
    public ulong FileCRC(string sInputFilename)
{
      FileStream inFile = new System.IO.FileStream(sInputFilename, System.IO.FileMode.Open,  System.IO.FileAccess.Read);
      byte[] bInput = new byte[inFile.Length];
      inFile.Read(bInput,0,bInput.Length);
      inFile.Close();

      return ByteCRC(ref bInput);
    }

  }
  ///////// <summary>
  /// MD5 ���������
  /// ��ȡΨһ���������������������
  /// (�޷���ԭ)
  /// </summary>
  public class MD5 
{

    public MD5()
{
    }

    ///////// <summary>
    /// ��ȡ�ַ�����������
    /// </summary>
    /// <param name="sInputString">�����ı�</param>
    /// <returns></returns>
    public string HashString(string sInputString)
{
      System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
      string encoded = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(sInputString))).Replace("-","");
      return encoded;
    }

    ///////// <summary>
    /// ��ȡ�ļ���������
    /// </summary>
    /// <param name="sInputFilename">�����ļ�</param>
    /// <returns></returns>
    public string HashFile(string sInputFilename)
{
      System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
      FileStream inFile = new System.IO.FileStream(sInputFilename, System.IO.FileMode.Open,  System.IO.FileAccess.Read);
      byte[] bInput = new byte[inFile.Length];
      inFile.Read(bInput,0,bInput.Length);
      inFile.Close();

      string encoded = BitConverter.ToString(md5.ComputeHash(bInput)).Replace("-","");
      return encoded;
    }

  }

  ///////// <summary>
  /// Base64 UUEncoded ����
  /// �������Ʊ���ΪASCII�ı����������紫��
  /// (�ɻ�ԭ)
  /// </summary>
  public class BASE64
{

    public BASE64()
{
    }

    ///////// <summary>
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
      catch ( System.ArgumentNullException ) 
{
        //base 64 �ַ�����Ϊnull
        return "";
      }
      catch ( System.FormatException ) 
{
        //���ȴ����޷�����4
        return "";
      }      
    }

    ///////// <summary>
    /// �����ַ���
    /// </summary>
    /// <param name="sInputString">�����ı�</param>
    /// <returns></returns>
    public string EncryptString(string sInputString)
{
      byte[] bInput = Encoding.Default.GetBytes(sInputString);
      try 
{
        return System.Convert.ToBase64String(bInput,0,bInput.Length);
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

    ///////// <summary>
    /// �����ļ�
    /// </summary>
    /// <param name="sInputFilename">�����ļ�</param>
    /// <param name="sOutputFilename">����ļ�</param>
    public void DecryptFile(string sInputFilename,string sOutputFilename) 
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
      catch ( System.ArgumentNullException ) 
{
        //base 64 �ַ�����Ϊnull
        return;
      }
      catch ( System.FormatException ) 
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

    ///////// <summary>
    /// �����ļ�
    /// </summary>
    /// <param name="sInputFilename">�����ļ�</param>
    /// <param name="sOutputFilename">����ļ�</param>
    public void EncryptFile(string sInputFilename,string sOutputFilename)
{

      System.IO.FileStream inFile;     
      byte[]                 binaryData;

      try 
{
        inFile = new System.IO.FileStream(sInputFilename,
          System.IO.FileMode.Open,
          System.IO.FileAccess.Read);
        binaryData = new Byte[inFile.Length];
        long bytesRead = inFile.Read(binaryData, 0,
          (int) inFile.Length);
        inFile.Close();
      }
      catch 
{ //(System.Exception exp) {
        return;
      }

      // ת������������ΪBase64 UUEncoded���
      // ÿ3���ֽ���Դ��������Ϊ4���ֽ� 
      long arrayLength = (long) ((4.0d/3.0d) * binaryData.Length);
    
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
  ///////// <summary>
  /// DES ����
  /// ֧��Key(Կ��)���ܱ仯
  /// ֧�ֻ�ԭ
  /// 
  /// ��ʾ������
  ///  // 64λ��8���ֽ�
  ///  string sSecretKey;
  ///
  ///  // ��ȡKey
  ///  sSecretKey = GenerateKey();
  ///
  ///  // �й�
  ///  GCHandle gch = GCHandle.Alloc( sSecretKey,GCHandleType.Pinned );
  ///
  ///  // �����ļ�
  ///  EncryptFile(@"C:MyData.txt",
  ///  @"C:Encrypted.txt",
  ///  sSecretKey);
  ///
  ///  // �����ļ�
  ///  DecryptFile(@"C:Encrypted.txt",
  ///  @"C:Decrypted.txt",
  ///  sSecretKey);
  ///
  ///  // �ͷ��й�����
  ///  ZeroMemory(gch.AddrOfPinnedObject(), sSecretKey.Length * 2);
  ///  gch.Free();
  /// </summary>
  public class DES 
{
    [DllImport("KERNEL32.DLL", EntryPoint="RtlZeroMemory")]
    public static extern bool ZeroMemory(IntPtr Destination, int Length);

    public DES() 
{
      //
      // TODO: �ڴ˴���ӹ��캯���߼�
      //
    }

    ///////// <summary>
    /// ����Key
    /// </summary>
    /// <returns></returns>
    public string GenerateKey() 
{
      // ����һ��DES �㷨��ʵ�����Զ�����Key
      DESCryptoServiceProvider desCrypto =(DESCryptoServiceProvider)DESCryptoServiceProvider.Create();

      // �����Զ�������Key ���ڼ���
      return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
    }

    ///////// <summary>
    /// �����ַ���
    /// </summary>
    /// <param name="sInputString">�����ַ�</param>
    /// <param name="sKey">Key</param>
    /// <returns>���ܽ��</returns>
    public string EncryptString(string sInputString,string sKey)
{
      byte[] data = Encoding.Default.GetBytes(sInputString);
      byte[] result;
      DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
      DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
      DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
      ICryptoTransform desencrypt = DES.CreateEncryptor();
      result = desencrypt.TransformFinalBlock(data,0,data.Length);

      string desString = "";
      for(int i=0;i<result.Length;i++)
{
        desString += result[i].ToString() + "-";
      }
      
      //return desString.TrimEnd('-');
      return BitConverter.ToString(result);
    }

    ///////// <summary>
    /// �����ַ���
    /// </summary>
    /// <param name="sInputString">�����ַ�</param>
    /// <param name="sKey">Key</param>
    /// <returns>���ܽ��</returns>
    public string DecryptString(string sInputString,string sKey)
{
      string[] sInput = sInputString.Split("-".ToCharArray());
      byte[] data = new byte[sInput.Length];
      byte[] result;
      for(int i=0;i<sInput.Length;i++)
        data[i] = byte.Parse(sInput[i],System.Globalization.NumberStyles.HexNumber);

      DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
      DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
      DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
      ICryptoTransform desencrypt = DES.CreateDecryptor();
      result = desencrypt.TransformFinalBlock(data,0,data.Length);
      return Encoding.Default.GetString(result);
    }

    ///////// <summary>
    /// �����ļ�
    /// </summary>
    /// <param name="sInputFilename">�����ļ�</param>
    /// <param name="sOutputFilename">����ļ�</param>
    /// <param name="sKey">Key</param>
    public void EncryptFile(string sInputFilename,
      string sOutputFilename,
      string sKey) 
{
      FileStream fsInput = new FileStream(sInputFilename,
        FileMode.Open,
        FileAccess.Read);

      FileStream fsEncrypted = new FileStream(sOutputFilename,
        FileMode.Create,
        FileAccess.Write);
      DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
      DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
      DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
      ICryptoTransform desencrypt = DES.CreateEncryptor();
      CryptoStream cryptostream = new CryptoStream(fsEncrypted,
        desencrypt,
        CryptoStreamMode.Write);

      byte[] bytearrayinput = new byte[fsInput.Length];
      fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
      cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
      cryptostream.Close();
      fsInput.Close();
      fsEncrypted.Close();
    }

    ///////// <summary>
    /// �����ļ�
    /// </summary>
    /// <param name="sInputFilename">�����ļ�</param>
    /// <param name="sOutputFilename">����ļ�</param>
    /// <param name="sKey">Key</param>
    public void DecryptFile(string sInputFilename,
      string sOutputFilename,
      string sKey) 
{
      DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
      DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
      DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

      FileStream fsread = new FileStream(sInputFilename,
        FileMode.Open,
        FileAccess.Read);
      ICryptoTransform desdecrypt = DES.CreateDecryptor();
      CryptoStream cryptostreamDecr = new CryptoStream(fsread,
        desdecrypt,
        CryptoStreamMode.Read);
      StreamWriter fsDecrypted = new StreamWriter(sOutputFilename);
      fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
      fsDecrypted.Flush();
      fsDecrypted.Close();
    }
  }
}
