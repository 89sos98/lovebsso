using System;
using System.Text.RegularExpressions;
using CYQ.Entity;

namespace Web.Core
{
    public class DomainFiter
    {
        public static bool IsOk(string userName)
        {
            bool result = false;
            if (IsOkKey(userName))
            {
                CYQ.Data.Xml.XmlHelper helper = new CYQ.Data.Xml.XmlHelper(true);
                if (helper.Load(AppDomain.CurrentDomain.BaseDirectory+Config.SystemSkinPath + IDPage.DomailFilter))
                {
                    result = helper.GetByID(userName) == null;
                }
                helper.Dispose();
            }
            return result;
        }
        public static bool IsOkKey(string userName)
        {
            if (userName.Length > 25)
            {
                return false;
            }
            string strExp = @"^[a-zA-Z]+[a-zA-Z_0-9]*$";  //只允许域由字母开头,数字、字母以及下划线组成
            Regex r = new Regex(strExp);
            Match m = r.Match(userName);
            return m.Success;
        }
    }
}
