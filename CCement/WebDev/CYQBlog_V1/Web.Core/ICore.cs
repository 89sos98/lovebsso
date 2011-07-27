using System;
using CYQ.Data.Xml;
using CYQ.Data.Table;
using System.Web;
namespace Web.Core
{
    public interface ICore
    {
        HttpRequest Request { get;}
        XmlHelper Document { get;}
        MDataRow DomainUser { get;}
        UserLogin UserAction { get;}
        MutilLanguage Language { get;}
        string Domain { get;}
        int DomainID {  get;}
        int LoginUserID
        {
            get;
        }
        string UrlPara { get;}
        string UrlType { get;}
        string UrlReferrer { get;}
        int GetParaInt(int num);
        string GetPara(int num);
        string GetPara(int num,string defaultValue);
        void GoTo(string url);
        string Get(string name, params string[] defaultValue);
        string MapPath(string path);
        string SetCDATA(string text);
    }
}
