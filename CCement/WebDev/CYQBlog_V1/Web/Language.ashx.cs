using System;
using Module;
using Web.Core;


namespace Web
{
    public class Language : Module.HttpCustom
    {
        protected override void Page_Load()
        {
            string lanKey=GetPara(2,"error").ToLower();
            switch (lanKey)
            {
                case "china":
                case "english":
                case "french":
                case "german":
                case "korean":
                case "japanese":
                case "hindi":
                case "russian":
                case "italian":
                case "custom":
                    Language.SetToCookie(lanKey.Substring(0, 1).ToUpper() + lanKey.Substring(1));
                    break;
                default:
                    GoTo(Config.HttpHost + "/error/" + UrlType);
                    break;
            }
            GoTo(Convert.ToString(Request.UrlReferrer));
        }

    }
}
