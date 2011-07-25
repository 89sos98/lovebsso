#region License and Copyright

/*
 * Copyright (c) 2007-2008 by Hornow Science & Technology Co.,Ltd
 * All rights reserved.
 */

#endregion

using System;
using System.Web;
using System.IO;

namespace CCement
{
    public class HttpErrorModule : IHttpModule
    {
        public const string HttpErrorWrapper = "HttpErrorWrapper";

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.Error += Error;
        }

        private void Error(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;

            Exception ex = context.Server.GetLastError();
            if (null == ex)
                return;
            int httpCode = 0;
            if (ex.GetType() == typeof(HttpException))
            {
                httpCode = ((HttpException)ex).GetHttpCode();
            }

            if ((httpCode == 404 || (ex.InnerException != null && ex.InnerException.GetType() == typeof(FileNotFoundException)) || (ex.GetType() == typeof(FileNotFoundException))) && context.Request.Url.LocalPath.ToLower().EndsWith(".aspx"))
            {
                string error404Url = "Error.aspx";//SecurityConfigAccessor.SecurityConfig.GetError404URL();
                if (!string.IsNullOrEmpty(error404Url))
                {
                    try
                    {
                        //clear the error and redirect it to the error page
                        HttpContext.Current.Server.ClearError();
                        HttpContext.Current.Server.Transfer(error404Url, true);
                    }
                    catch (Exception exc)
                    {
                        //log.Error("Error redirect to error page " + error404Url, exc);
                    }
                    return;
                }
            }

            string localPath = context.Request.Url.LocalPath.ToLower();
            ErrorWrapper error = new ErrorWrapper();
            error.ErrorException = ex;
            error.ErrorPage = localPath;
            if (null != context.Session)
            {
                context.Session[HttpErrorWrapper] = error;
            }
            if (localPath.EndsWith(".aspx") || localPath.EndsWith(".asmx") || localPath.EndsWith(".ashx"))
            {
                string errorUrl = "Error.aspx";//SecurityConfigAccessor.SecurityConfig.GetErrorURL();
                if (!string.IsNullOrEmpty(errorUrl))
                {
                    try
                    {
                        //clear the error and redirect it to the error page
                        HttpContext.Current.Server.ClearError();
                        HttpContext.Current.Server.Transfer(errorUrl, true);
                        //HttpContext.Current.Response.Write(string.Format("<script>window.location.href='{0}';</script>", errorUrl));
                    }
                    catch (Exception exc)
                    {
                        //log.Error("Error redirect to error page " + errorUrl, exc);
                    }
                }
            }
        }
    }

    [Serializable]
    public class ErrorWrapper
    {
        public string ErrorPage { get; set; }

        public Exception ErrorException { get; set; }
    }
}
