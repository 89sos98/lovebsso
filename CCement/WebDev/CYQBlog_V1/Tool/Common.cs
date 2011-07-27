using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Tool
{
    public class Common
    {
        /// <summary>
        /// 截取Url参数[如Url是/select/para/3],调用GetPara(Url,2)返回para
        /// </summary>
        /// <param name="SourceUrl">原始Url</param>
        /// <param name="Num">截取第几个参数</param>
        /// <returns></returns>
        public static string GetPara(string sourceUrl, int num)
        {
            return GetPara(sourceUrl, num, "1");
        }
        public static string GetPara(string sourceUrl, int num, string defaultValue)
        {
            if (string.IsNullOrEmpty(sourceUrl) || sourceUrl == "/")
            {
                return defaultValue;
            }
            if (sourceUrl.Substring(0, 1) != "/")
            {
                sourceUrl = "/" + sourceUrl;
            }
            string[] para = sourceUrl.TrimEnd('/').Split('/');
            if (para.Length > num)
            {
                if (num > 0)
                {
                    return para[num];
                }
                else
                {
                    return para[para.Length - 1];
                }
            }
            else
            {
                return defaultValue;
            }

        }

        /// <summary>
        /// 过滤Html标签
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static string CleanHtml(string strIn)
        {
            string tempStrIn = strIn.ToString();
            return Regex.Replace(tempStrIn, @"</?[^>]*>", "");
        }

    }
}
