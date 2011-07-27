using System;
using CYQ.Data.Xml;
using Module;
using System.Xml;

namespace Logic
{
    public class Translate
    {
        public static void Do(XmlHelper xDoc, LanguageSetting language)
        {
           XmlNodeList list=xDoc.GetList("lable", "key");
           if (list != null && list.Count > 0)
           {
               string key=null;
               for (int i = 0; i < list.Count; i++)
               {
                   key=list[i].Attributes["key"].Value;
                   list[i].InnerXml = language.Get(key);
               }
           }
        }
    }
}
