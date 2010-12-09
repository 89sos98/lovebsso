using System;
using System.IO;
using System.Net;
using System.Text;

namespace Hornow.Horn.Web.Core.Extension.Utils
{
    public class HttpWebUtils
    {
        const string sUserAgent =
           "Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN; rv:1.9.2.3) Gecko/20100401 Firefox/3.6.3";
        const string sContentType =
            "application/x-www-form-urlencoded";

        /// <summary>
        /// Post data到url
        /// </summary>
        /// <param name="data">要post的数据</param>
        /// <param name="url">目标url</param>
        /// <returns>服务器响应</returns>
        public static string PostDataToUrl(string url, string data)
        {
            return PostDataToUrl(url, data, Encoding.UTF8, Encoding.UTF8);
        }

        /// <summary>
        /// Post data到url
        /// </summary>
        /// <param name="data">要post的数据</param>
        /// <param name="url">目标url</param>
        /// <param name="requestEncoding"></param>
        /// <param name="responseEncoding"></param>
        /// <returns>服务器响应</returns>
        public static string PostDataToUrl(string url, string data, Encoding requestEncoding, Encoding responseEncoding)
        {
            byte[] bytesToPost = requestEncoding.GetBytes(data);
            return PostDataToUrl(url, bytesToPost, responseEncoding);
        }

        /// <summary>
        /// Post data到url
        /// </summary>
        /// <param name="data">要post的数据</param>
        /// <param name="url">目标url</param>
        /// <returns>服务器响应</returns>
        public static string PostDataToUrl(string url, byte[] data)
        {
            return PostDataToUrl(url, data, Encoding.UTF8);
        }

        /// <summary>
        /// Post data到url
        /// </summary>
        /// <param name="data">要post的数据</param>
        /// <param name="url">目标url</param>
        /// <param name="responseEncoding"></param>
        /// <returns>服务器响应</returns>
        public static string PostDataToUrl(string url, byte[] data, Encoding responseEncoding)
        {
            #region 创建httpWebRequest对象
            WebRequest webRequest = WebRequest.Create(url);
            HttpWebRequest httpRequest = webRequest as HttpWebRequest;
            if (httpRequest == null)
            {
                throw new ApplicationException(string.Format("Invalid url string: {0}", url));
            }
            #endregion

            #region 填充httpWebRequest的基本信息
            httpRequest.KeepAlive = false;
            httpRequest.ProtocolVersion = HttpVersion.Version10;
            httpRequest.UserAgent = sUserAgent;
            httpRequest.ContentType = sContentType;
            httpRequest.Method = "POST";
            //httpRequest.KeepAlive = false;
            #endregion

            #region 填充要post的内容
            httpRequest.ContentLength = data.Length;
            Stream requestStream = httpRequest.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();
            #endregion

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            #region 发送post请求到服务器并读取服务器返回信息
            Stream responseStream;
            try
            {
                responseStream = response.GetResponseStream();
            }
            catch (Exception e)
            {
                // log error
                //Console.WriteLine(
                //    string.Format("POST操作发生异常：{0}", e.Message)
                //    );
                throw e;
            }
            #endregion

            #region 读取服务器返回信息
            string stringResponse = string.Empty;
            using (StreamReader responseReader = new StreamReader(responseStream, responseEncoding))
            {
                stringResponse = responseReader.ReadToEnd();
            }
            responseStream.Close();
            #endregion

            try
            {
                if (null != httpRequest)
                {
                    httpRequest.Abort();
                }
                if (null != response)
                {
                    response.Close();
                }
            }
            catch
            {
            }

            return stringResponse;
        }

        /// <summary>
        /// Get方式请求页面
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetUrl(string url)
        {
            return GetUrl(url, Encoding.UTF8);
        }

        /// <summary>
        /// Get方式请求页面
        /// </summary>
        /// <param name="url"></param>
        /// <param name="responseEncoding"></param>
        /// <returns></returns>
        public static string GetUrl(string url, Encoding responseEncoding)
        {
            #region 创建httpWebRequest对象
            WebRequest webRequest = WebRequest.Create(url);
            HttpWebRequest httpRequest = webRequest as HttpWebRequest;
            if (httpRequest == null)
            {
                throw new ApplicationException(string.Format("Invalid url string: {0}", url));
            }
            #endregion

            //#region 填充httpWebRequest的基本信息
            //httpRequest.UserAgent = sUserAgent;
            //httpRequest.ContentType = sContentType;
            //httpRequest.Method = "GET";
            //httpRequest.KeepAlive = false;


            //#endregion

            //#region 填充要post的内容
            //Stream requestStream = httpRequest.GetRequestStream();
            //requestStream.Write(data, 0, data.Length);
            //requestStream.Close();
            //#endregion

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();

            #region 发送post请求到服务器并读取服务器返回信息
            Stream responseStream;
            try
            {
                responseStream = response.GetResponseStream();
            }
            catch (Exception e)
            {
                // log error
                //Console.WriteLine(string.Format("POST操作发生异常：{0}", e.Message));
                throw e;
            }
            #endregion

            #region 读取服务器返回信息
            string stringResponse = string.Empty;
            using (StreamReader responseReader = new StreamReader(responseStream, responseEncoding))
            {
                stringResponse = responseReader.ReadToEnd();
            }
            responseStream.Close();

            try
            {
                if (null != httpRequest)
                {
                    httpRequest.Abort();
                }
                if (null != response)
                {
                    response.Close();
                }
            }
            catch
            {
            }
            #endregion
            return stringResponse;
        }
    }
}