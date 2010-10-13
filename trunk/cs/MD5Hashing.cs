using System;   
using System.Text;   
using System.Security.Cryptography;   
  
namespace Common   
{   
    /// <summary>   
    /// һ��ʵ��MD5ɢ���ַ�������   
    /// ���ߣ��ܹ�   
    /// ���ڣ�2007   
    /// </summary>   
    public sealed class MD5Hashing   
    {   
        private static MD5 md5 = MD5.Create();   
        //˽�л����캯��   
        private MD5Hashing()   
        {   
        }   
        /// <summary>   
        /// ʹ��utf8���뽫�ַ���ɢ��   
        /// </summary>   
        /// <param name="sourceString">Ҫɢ�е��ַ���</param>   
        /// <returns>ɢ�к���ַ���</returns>   
       public static string HashString(string sourceString)   
       {   
            return HashString(Encoding.UTF8, sourceString);   
       }   
       /// <summary>   
       /// ʹ��ָ���ı��뽫�ַ���ɢ��   
       /// </summary>   
       /// <param name="encode">����</param>   
       /// <param name="sourceString">Ҫɢ�е��ַ���</param>   
       /// <returns>ɢ�к���ַ���</returns>   
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
        *����Ĵ�����ַ�������MD5��ϣ����Ľ���������һ�仰��Ч��

				*System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower())

				*����str��Ҫ���й�ϣ������ַ����������ASP.NETӦ���е�Ҳ���ˣ�����ڷ�ASP.NETӦ���л���Ҫ���Ӷ�System.Web.dll�����á�
				*/
       
    }   
} 