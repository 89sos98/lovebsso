using System;   
using System.Text;   
using System.Security.Cryptography;   
  
namespace Common   
{   
    /// <summary>   
    /// 一个实现MD5散列字符串的类   
    /// 作者：周公   
    /// 日期：2007   
    /// </summary>   
    public sealed class MD5Hashing   
    {   
        private static MD5 md5 = MD5.Create();   
        //私有化构造函数   
        private MD5Hashing()   
        {   
        }   
        /// <summary>   
        /// 使用utf8编码将字符串散列   
        /// </summary>   
        /// <param name="sourceString">要散列的字符串</param>   
        /// <returns>散列后的字符串</returns>   
       public static string HashString(string sourceString)   
       {   
            return HashString(Encoding.UTF8, sourceString);   
       }   
       /// <summary>   
       /// 使用指定的编码将字符串散列   
       /// </summary>   
       /// <param name="encode">编码</param>   
       /// <param name="sourceString">要散列的字符串</param>   
       /// <returns>散列后的字符串</returns>   
        public static string HashString(Encoding encode, string sourceString)   
        {   
            byte[] source = md5.ComputeHash(encode.GetBytes(sourceString));   
            StringBuilder sBuilder = new StringBuilder();   
            for (int i = 0; i < source.Length; i++)   
            {   
                sBuilder.Append(source[i].ToString("x2"));   
            }   
            return sBuilder.ToString();   
        }   
        /*
        *上面的代码对字符串进行MD5哈希计算的结果与下面的一句话等效：

				*System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower())

				*其中str是要进行哈希计算的字符串，如果在ASP.NET应用中倒也罢了，如果在非ASP.NET应用中还需要增加对System.Web.dll的引用。
				*/
       
    }   
} 