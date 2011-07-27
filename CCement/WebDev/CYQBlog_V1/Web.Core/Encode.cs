using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Core
{
    /// <summary>
    /// 整站的加密核心函数
    /// 请注意，为了发布版本的安全，此版本的内部加密方法已被调整修改，和发布版本已不一致,但是接口和调用方法是一致的。
    /// 同样，为了您使用时站点的安全，最好请修改这里的加密方法。
    /// </summary>
    public class Encode
    {
        #region 对外调用方法
        /// <summary>
        /// Base64四位编码
        /// </summary>
        /// <param name="strText"></param>
        /// <param name="flag">true为ToBase64String,flase为FromBase64String</param>
        /// <returns>string</returns>
        public static string Password(string text, bool flag)
        {
            return Action(true, text, flag, "E", "FC", "2");
        }
        public static string Url(string text, bool flag)
        {
            return Action(false, text, flag, "D", "bD", "3");
        }
        public static string Cookie(string text, bool flag)
        {
            return Action(true, text, flag, "d", "dc", "3");
        }
        #endregion

        #region 内部实现
        private static string Encode2(string text)
        {
            return text;//二次加密。
        }
        public static string To64(string text, bool flag)
        {
            System.Text.Encoding myEncoding = System.Text.Encoding.Unicode;
            if (flag)
            {
                byte[] myText = myEncoding.GetBytes(text.ToCharArray());
                return Convert.ToBase64String(myText);
            }
            else
            {
                byte[] myText = null;
                try
                {
                    myText = Convert.FromBase64String(text);
                }
                catch
                {
                    return string.Empty;
                }
                return myEncoding.GetString(myText);
            }
        }
        /// <summary>
        /// 用于页面级的Base64四位编码
        /// </summary>
        /// <param name="strText"></param>
        /// <param name="flag">true为ToBase64String,flase为FromBase64String</param>
        /// <returns>string</returns>
        private static string Action(bool encode2, string text, bool flag, params string[] para)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            System.Text.Encoding myEncoding = System.Text.Encoding.Unicode;
            if (flag)
            {
                int length = text.Length;
                text = RandomKey(encode2, text, flag);
                bool isRandom = length != text.Length;
                byte[] myText = myEncoding.GetBytes(text.ToCharArray());
                string base64String = Convert.ToBase64String(myText);
                //if (isRandom)
                //{
                //    base64String = base64String;
                //}
                if (base64String.Length > 4)
                {
                    base64String = base64String.Insert(int.Parse(para[2]), para[0]);
                }
                if (base64String.Length > 55)
                {
                    base64String = base64String.Insert(51, para[1]);
                }

                return base64String;
            }
            else
            {
                if (text.Length > 55)
                {
                    text = text.Remove(51, 2);
                }
                if (text.Length > 4)
                {
                    text = text.Remove(int.Parse(para[2]), 1);
                }
                byte[] myText = null;
                try
                {
                    myText = Convert.FromBase64String(text);
                }
                catch
                {
                    return string.Empty;
                }
                char[] myChar = myEncoding.GetChars(myText);
                string ReturnValue = string.Empty;
                foreach (char aa in myChar)
                {
                    ReturnValue += aa.ToString();
                }
                return RandomKey(encode2, ReturnValue, false);
            }
        }

        private static string RandomKey(bool encode2, string text, bool flag)
        {
            string key = Config.PasswordKey;
            if (string.IsNullOrEmpty(key) || key.Length < 5 || !encode2)
            {
                return text;
            }
            string newKey = Encode2(key);
            if (flag)
            {
                text = key.Substring(0, 3) + text + key.Substring(3, 3);
                text = text.Insert(4, newKey);
            }
            else
            {
                if (text.Length > newKey.Length + 6)
                {
                    text = text.Remove(4, newKey.Length);
                    text = text.Remove(0, 3);
                    text = text.Remove(text.Length - 3, 3);
                }
                else
                {
                    text = string.Empty;
                }
            }
            return text;
        }
        #endregion
    }
}
