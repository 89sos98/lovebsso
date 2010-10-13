using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text;
namespace YNEC.Services.Encrypt 
{
  ///////// <summary>
  /// CRC 效验
  /// 快速检测算法
  /// </summary>
  public class CRC32
{

    protected ulong[] crc32Table;

    ///////// <summary>
    /// 构造：初始化效验表
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
    /// 字节数组效验
    /// </summary>
    /// <param name="buffer">ref 字节数组</param>
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
    /// 字符串效验
    /// </summary>
    /// <param name="sInputString">字符串</param>
    /// <returns></returns>
    public ulong StringCRC(string sInputString)
{
      byte[] buffer = Encoding.Default.GetBytes(sInputString);
      return ByteCRC(ref buffer);
    }

    ///////// <summary>
    /// 文件效验
    /// </summary>
    /// <param name="sInputFilename">输入文件</param>
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
  /// MD5 无逆向编码
  /// 获取唯一特征串，可用于密码加密
  /// (无法还原)
  /// </summary>
  public class MD5 
{

    public MD5()
{
    }

    ///////// <summary>
    /// 获取字符串的特征串
    /// </summary>
    /// <param name="sInputString">输入文本</param>
    /// <returns></returns>
    public string HashString(string sInputString)
{
      System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
      string encoded = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(sInputString))).Replace("-","");
      return encoded;
    }

    ///////// <summary>
    /// 获取文件的特征串
    /// </summary>
    /// <param name="sInputFilename">输入文件</param>
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
  /// Base64 UUEncoded 编码
  /// 将二进制编码为ASCII文本，用于网络传输
  /// (可还原)
  /// </summary>
  public class BASE64
{

    public BASE64()
{
    }

    ///////// <summary>
    /// 解码字符串
    /// </summary>
    /// <param name="sInputString">输入文本</param>
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
        //base 64 字符数组为null
        return "";
      }
      catch ( System.FormatException ) 
{
        //长度错误，无法整除4
        return "";
      }      
    }

    ///////// <summary>
    /// 编码字符串
    /// </summary>
    /// <param name="sInputString">输入文本</param>
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
        //二进制数组为NULL.
        return "";
      }
      catch (System.ArgumentOutOfRangeException) 
{
        //长度不够
        return "";
      }
    }

    ///////// <summary>
    /// 解码文件
    /// </summary>
    /// <param name="sInputFilename">输入文件</param>
    /// <param name="sOutputFilename">输出文件</param>
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

      // 转换Base64 UUEncoded为二进制输出
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
        //base 64 字符数组为null
        return;
      }
      catch ( System.FormatException ) 
{
        //长度错误，无法整除4
        return;
      }

      // 写输出数据
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
        //流错误
      }

    }

    ///////// <summary>
    /// 编码文件
    /// </summary>
    /// <param name="sInputFilename">输入文件</param>
    /// <param name="sOutputFilename">输出文件</param>
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

      // 转换二进制输入为Base64 UUEncoded输出
      // 每3个字节在源数据里作为4个字节 
      long arrayLength = (long) ((4.0d/3.0d) * binaryData.Length);
    
      // 如果无法整除4
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
        //二进制数组为NULL.
        return;
      }
      catch (System.ArgumentOutOfRangeException) 
{
        //长度不够
        return;
      }

      // 写UUEncoded数据到文件内
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
        //文件流出错
      }
 

    }
  }
  ///////// <summary>
  /// DES 加密
  /// 支持Key(钥匙)加密变化
  /// 支持还原
  /// 
  /// 演示操作：
  ///  // 64位，8个字节
  ///  string sSecretKey;
  ///
  ///  // 获取Key
  ///  sSecretKey = GenerateKey();
  ///
  ///  // 托管
  ///  GCHandle gch = GCHandle.Alloc( sSecretKey,GCHandleType.Pinned );
  ///
  ///  // 加密文件
  ///  EncryptFile(@"C:MyData.txt",
  ///  @"C:Encrypted.txt",
  ///  sSecretKey);
  ///
  ///  // 解密文件
  ///  DecryptFile(@"C:Encrypted.txt",
  ///  @"C:Decrypted.txt",
  ///  sSecretKey);
  ///
  ///  // 释放托管内容
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
      // TODO: 在此处添加构造函数逻辑
      //
    }

    ///////// <summary>
    /// 创建Key
    /// </summary>
    /// <returns></returns>
    public string GenerateKey() 
{
      // 创建一个DES 算法的实例。自动产生Key
      DESCryptoServiceProvider desCrypto =(DESCryptoServiceProvider)DESCryptoServiceProvider.Create();

      // 返回自动创建的Key 用于加密
      return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
    }

    ///////// <summary>
    /// 加密字符串
    /// </summary>
    /// <param name="sInputString">输入字符</param>
    /// <param name="sKey">Key</param>
    /// <returns>加密结果</returns>
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
    /// 解密字符串
    /// </summary>
    /// <param name="sInputString">输入字符</param>
    /// <param name="sKey">Key</param>
    /// <returns>解密结果</returns>
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
    /// 加密文件
    /// </summary>
    /// <param name="sInputFilename">输入文件</param>
    /// <param name="sOutputFilename">输出文件</param>
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
    /// 解密文件
    /// </summary>
    /// <param name="sInputFilename">输入文件</param>
    /// <param name="sOutputFilename">输出文件</param>
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
