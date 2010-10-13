using System;   
using System.Text;   
using System.Security;   
using System.Security.Cryptography;   
using System.IO;   
namespace EncryptClasses   
{   
 /// <summary>   
 /// �˴��������DES����,Ϊ�˱��ڽ��Ĺ����ά��   
 /// �벻Ҫ���Ķ�����,���߸ı����������һ��Ҫ   
 /// �μ���ǰ������,���򽫻��ճɲ���Ԥ�ϵ���ʧ   
 /// </summary>   
 public class DESEncrypt   
 {  
  #region "member fields"   
  private string iv="12345678";   
  private string key="12345678";   
  private Encoding encoding=new UnicodeEncoding();   
  private DES des;  
  #endregion   
  /// <summary>   
  /// ���캯��   
  /// </summary>   
  public DESEncrypt()   
  {   
   des=new DESCryptoServiceProvider();   
  }  
  #region "propertys"   
  /// <summary>   
  /// ���ü�����Կ   
  /// </summary>   
  public string EncryptKey   
  {   
   get{return this.key;}   
   set  
   {   
      this.key=value;   
   }   
  }   
  /// <summary>   
  /// Ҫ�����ַ��ı���ģʽ   
  /// </summary>   
  public Encoding EncodingMode   
  {   
   get{return this.encoding;}   
   set{this.encoding=value;}   
  }  
  #endregion  
  #region "methods"   
  /// <summary>   
  /// �����ַ��������ؼ��ܺ�Ľ��   
  /// </summary>   
  /// <param name="str"></param>   
  /// <returns></returns>   
  public string EncryptString(string str)   
  {   
   byte[] ivb=Encoding.ASCII.GetBytes(this.iv);   
   byte[] keyb=Encoding.ASCII.GetBytes(this.EncryptKey);//�õ�������Կ   
   byte[] toEncrypt=this.EncodingMode.GetBytes(str);//�õ�Ҫ���ܵ�����   
   byte[] encrypted;   
   ICryptoTransform encryptor=des.CreateEncryptor(keyb,ivb);   
   MemoryStream msEncrypt=new MemoryStream();   
   CryptoStream csEncrypt=new CryptoStream(msEncrypt,encryptor,CryptoStreamMode.Write);   
   csEncrypt.Write(toEncrypt,0,toEncrypt.Length);   
   csEncrypt.FlushFinalBlock();   
   encrypted=msEncrypt.ToArray();   
   csEncrypt.Close();   
   msEncrypt.Close();   
   return this.EncodingMode.GetString(encrypted);   
  }   
  /// <summary>   
  /// ����ָ�����ļ�,����ɹ�����True,����false   
  /// </summary>   
  /// <param name="filePath">Ҫ���ܵ��ļ�·��</param>   
  /// <param name="outPath">���ܺ���ļ����·��</param>   
  public void EncryptFile(string filePath,string outPath)   
  {   
   bool isExist=File.Exists(filePath);   
   if(isExist)//�������   
   {   
    byte[] ivb=Encoding.ASCII.GetBytes(this.iv);   
    byte[] keyb=Encoding.ASCII.GetBytes(this.EncryptKey);   
    //�õ�Ҫ�����ļ����ֽ���   
    FileStream fin=new FileStream(filePath,FileMode.Open,FileAccess.Read);   
    StreamReader reader=new StreamReader(fin,this.EncodingMode);   
    string dataStr=reader.ReadToEnd();   
    byte[] toEncrypt=this.EncodingMode.GetBytes(dataStr);   
    fin.Close();   
  
    FileStream fout=new FileStream(outPath,FileMode.Create,FileAccess.Write);   
    ICryptoTransform encryptor=des.CreateEncryptor(keyb,ivb);   
    CryptoStream csEncrypt=new CryptoStream(fout,encryptor,CryptoStreamMode.Write);   
    try  
    {   
     //���ܵõ����ļ��ֽ���   
     csEncrypt.Write(toEncrypt,0,toEncrypt.Length);   
     csEncrypt.FlushFinalBlock();   
    }   
    catch(Exception err)   
    {   
     throw new ApplicationException(err.Message);   
    }   
    finally  
    {   
     try  
     {   
      fout.Close();   
      csEncrypt.Close();   
     }   
     catch  
     {   
      ;   
  }   
    }   
   }   
   else  
   {   
    throw new FileNotFoundException("û���ҵ�ָ�����ļ�");   
   }   
  }   
  /// <summary>   
  /// �ļ����ܺ��������ذ汾,�����ָ�����·��,   
  /// ��ôԭ�����ļ��������ܺ���ļ�����   
  /// </summary>   
  /// <param name="filePath"></param>   
  public void EncryptFile(string filePath)   
  {   
   this.EncryptFile(filePath,filePath);   
  }   
  /// <summary>   
  /// ���ܸ������ַ���   
  /// </summary>   
  /// <param name="str">Ҫ���ܵ��ַ�</param>   
  /// <returns></returns>   
  public string DecryptString(string str)   
  {   
   byte[] ivb=Encoding.ASCII.GetBytes(this.iv);   
   byte[] keyb=Encoding.ASCII.GetBytes(this.EncryptKey);   
   byte[] toDecrypt=this.EncodingMode.GetBytes(str);   
   byte[] deCrypted=new byte[toDecrypt.Length];   
   ICryptoTransform deCryptor=des.CreateDecryptor(keyb,ivb);   
   MemoryStream msDecrypt=new MemoryStream(toDecrypt);   
   CryptoStream csDecrypt=new CryptoStream(msDecrypt,deCryptor,CryptoStreamMode.Read);   
   try  
   {   
    csDecrypt.Read(deCrypted,0,deCrypted.Length);   
   }   
   catch(Exception err)   
   {   
    throw new ApplicationException(err.Message);   
   }   
   finally  
   {   
    try  
    {   
     msDecrypt.Close();   
     csDecrypt.Close();   
    }   
    catch{;}   
   }   
   return this.EncodingMode.GetString(deCrypted);   
  }   
  /// <summary>   
  /// ����ָ�����ļ�   
  /// </summary>   
  /// <param name="filePath">Ҫ���ܵ��ļ�·��</param>   
  /// <param name="outPath">���ܺ���ļ����·��</param>   
  public void DecryptFile(string filePath,string outPath)   
  {   
   bool isExist=File.Exists(filePath);   
   if(isExist)//�������   
   {   
    byte[] ivb=Encoding.ASCII.GetBytes(this.iv);   
    byte[] keyb=Encoding.ASCII.GetBytes(this.EncryptKey);   
    FileInfo file=new FileInfo(filePath);   
    byte[] deCrypted=new byte[file.Length];   
    //�õ�Ҫ�����ļ����ֽ���   
    FileStream fin=new FileStream(filePath,FileMode.Open,FileAccess.Read);   
    //�����ļ�   
    try  
    {   
     ICryptoTransform decryptor=des.CreateDecryptor(keyb,ivb);   
     CryptoStream csDecrypt=new CryptoStream(fin,decryptor,CryptoStreamMode.Read);   
     csDecrypt.Read(deCrypted,0,deCrypted.Length);   
    }   
    catch(Exception err)   
    {   
     throw new ApplicationException(err.Message);   
    }   
    finally  
    {   
     try  
     {   
      fin.Close();   
     }   
     catch{;}   
    }   
    FileStream fout=new FileStream(outPath,FileMode.Create,FileAccess.Write);   
    fout.Write(deCrypted,0,deCrypted.Length);   
    fout.Close();   
   }   
   else  
   {   
    throw new FileNotFoundException("ָ���Ľ����ļ�û���ҵ�");   
   }   
  }   
  /// <summary>   
  /// �����ļ������ذ汾,���û�и������ܺ��ļ������·��,   
  /// ����ܺ���ļ���������ǰ���ļ�   
  /// </summary>   
  /// <param name="filePath"></param>   
  public void DecryptFile(string filePath)   
  {   
   this.DecryptFile(filePath,filePath);   
  }  
  #endregion   
 }   
 /// <summary>   
 /// MD5������,ע�⾭MD5���ܹ�����Ϣ�ǲ���ת����ԭʼ���ݵ�   
 /// ,�벻Ҫ���û����е���Ϣ��ʹ�ô˼��ܼ���,�����û�������,   
 /// �뾡��ʹ�öԳƼ���   
/// </summary>   
 public class MD5Encrypt   
 {   
  private MD5 md5;   
  public MD5Encrypt()   
  {   
   md5=new MD5CryptoServiceProvider();   
  }   
  /// <summary>   
  /// ���ַ����л�ȡɢ��ֵ   
  /// </summary>   
  /// <param name="str">Ҫ����ɢ��ֵ���ַ���</param>   
  /// <returns></returns>   
  public string GetMD5FromString(string str)   
  {   
   byte[] toCompute=Encoding.Unicode.GetBytes(str);   
   byte[] hashed=md5.ComputeHash(toCompute,0,toCompute.Length);   
   return Encoding.ASCII.GetString(hashed);   
  }   
  /// <summary>   
  /// �����ļ�������ɢ��ֵ   
  /// </summary>   
  /// <param name="filePath">Ҫ����ɢ��ֵ���ļ�·��</param>   
  /// <returns></returns>   
  public string GetMD5FromFile(string filePath)   
  {   
   bool isExist=File.Exists(filePath);   
   if(isExist)//����ļ�����   
   {   
    FileStream stream=new FileStream(filePath,FileMode.Open,FileAccess.Read);   
    StreamReader reader=new StreamReader(stream,Encoding.Unicode);   
    string str=reader.ReadToEnd();   
    byte[] toHash=Encoding.Unicode.GetBytes(str);   
    byte[] hashed=md5.ComputeHash(toHash,0,toHash.Length);   
    stream.Close();   
    return Encoding.ASCII.GetString(hashed);   
   }   
   else//�ļ�������   
   {   
    throw new FileNotFoundException("ָ�����ļ�û���ҵ�");   
   }   
  }   
 }   
 /// <summary>   
 /// ��������ǩ����hash��   
 /// </summary>   
 public class MACTripleDESEncrypt   
 {   
  private MACTripleDES mact;   
  private string __key="ksn168ch";   
  private byte[] __data=null;   
  public MACTripleDESEncrypt()   
  {   
   mact=new MACTripleDES();   
  }   
  /// <summary>   
  /// ��ȡ��������������ǩ������Կ   
  /// </summary>   
  public string Key   
  {   
   get{return this.__key;}   
   set  
   {   
    int keyLength=value.Length;   
    int[] keyAllowLengths=new int[]{8,16,24};   
    bool isRight=false;   
    foreach(int i in keyAllowLengths)   
    {   
     if(keyLength==keyAllowLengths[i])   
     {   
      isRight=true;   
      break;   
     }   
    }   
    if(!isRight)   
     throw new ApplicationException("��������ǩ������Կ���ȱ�����8,16,24ֵ֮һ");   
    else  
     this.__key=value;   
   }   
  }   
  /// <summary>   
  /// ��ȡ��������������ǩ�����û�����   
  /// </summary>   
  public byte[] Data   
  {   
   get{return this.__data;}   
   set{this.__data=value;}   
  }   
  /// <summary>   
  /// �õ�ǩ�����hashֵ   
  /// </summary>   
  /// <returns></returns>   
  public string GetHashValue()   
  {   
   if(this.Data==null)   
    throw new NotSetSpecialPropertyException("û������Ҫ��������ǩ�����û�"+   
                                         "����(property:Data)");   
   byte[] key=Encoding.ASCII.GetBytes(this.Key);   
   this.mact.Key=key;   
   byte[] hash_b=this.mact.ComputeHash(this.mact.ComputeHash(this.Data));   
   return Encoding.ASCII.GetString(hash_b);   
  }   
 }   
}  
