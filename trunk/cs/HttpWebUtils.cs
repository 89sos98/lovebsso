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
        /// Post data��url
        /// </summary>
        /// <param name="data">Ҫpost������</param>
        /// <param name="url">Ŀ��url</param>
        /// <returns>��������Ӧ</returns>
        public static string PostDataToUrl(string url, string data)
        {
            return PostDataToUrl(url, data, Encoding.UTF8, Encoding.UTF8);
        }

        /// <summary>
        /// Post data��url
        /// </summary>
        /// <param name="data">Ҫpost������</param>
        /// <param name="url">Ŀ��url</param>
        /// <param name="requestEncoding"></param>
        /// <param name="responseEncoding"></param>
        /// <returns>��������Ӧ</returns>
        public static string PostDataToUrl(string url, string data, Encoding requestEncoding, Encoding responseEncoding)
        {
            byte[] bytesToPost = requestEncoding.GetBytes(data);
            return PostDataToUrl(url, bytesToPost, responseEncoding);
        }

        /// <summary>
        /// Post data��url
        /// </summary>
        /// <param name="data">Ҫpost������</param>
        /// <param name="url">Ŀ��url</param>
        /// <returns>��������Ӧ</returns>
        public static string PostDataToUrl(string url, byte[] data)
        {
            return PostDataToUrl(url, data, Encoding.UTF8);
        }

        /// <summary>
        /// Post data��url
        /// </summary>
        /// <param name="data">Ҫpost������</param>
        /// <param name="url">Ŀ��url</param>
        /// <param name="responseEncoding"></param>
        /// <returns>��������Ӧ</returns>
        public static string PostDataToUrl(string url, byte[] data, Encoding responseEncoding)
        {
            #region ����httpWebRequest����
            WebRequest webRequest = WebRequest.Create(url);
            HttpWebRequest httpRequest = webRequest as HttpWebRequest;
            if (httpRequest == null)
            {
                throw new ApplicationException(string.Format("Invalid url string: {0}", url));
            }
            #endregion

            #region ���httpWebRequest�Ļ�����Ϣ
            httpRequest.KeepAlive = false;
            httpRequest.ProtocolVersion = HttpVersion.Version10;
            httpRequest.UserAgent = sUserAgent;
            httpRequest.ContentType = sContentType;
            httpRequest.Method = "POST";
            //httpRequest.KeepAlive = false;
            #endregion

            #region ���Ҫpost������
            httpRequest.ContentLength = data.Length;
            Stream requestStream = httpRequest.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();
            #endregion

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            #region ����post���󵽷���������ȡ������������Ϣ
            Stream responseStream;
            try
            {
                responseStream = response.GetResponseStream();
            }
            catch (Exception e)
            {
                // log error
                //Console.WriteLine(
                //    string.Format("POST���������쳣��{0}", e.Message)
                //    );
                throw e;
            }
            #endregion

            #region ��ȡ������������Ϣ
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
        /// Get��ʽ����ҳ��
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetUrl(string url)
        {
            return GetUrl(url, Encoding.UTF8);
        }

        /// <summary>
        /// Get��ʽ����ҳ��
        /// </summary>
        /// <param name="url"></param>
        /// <param name="responseEncoding"></param>
        /// <returns></returns>
        public static string GetUrl(string url, Encoding responseEncoding)
        {
            #region ����httpWebRequest����
            WebRequest webRequest = WebRequest.Create(url);
            HttpWebRequest httpRequest = webRequest as HttpWebRequest;
            if (httpRequest == null)
            {
                throw new ApplicationException(string.Format("Invalid url string: {0}", url));
            }
            #endregion

            //#region ���httpWebRequest�Ļ�����Ϣ
            //httpRequest.UserAgent = sUserAgent;
            //httpRequest.ContentType = sContentType;
            //httpRequest.Method = "GET";
            //httpRequest.KeepAlive = false;


            //#endregion

            //#region ���Ҫpost������
            //Stream requestStream = httpRequest.GetRequestStream();
            //requestStream.Write(data, 0, data.Length);
            //requestStream.Close();
            //#endregion

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();

            #region ����post���󵽷���������ȡ������������Ϣ
            Stream responseStream;
            try
            {
                responseStream = response.GetResponseStream();
            }
            catch (Exception e)
            {
                // log error
                //Console.WriteLine(string.Format("POST���������쳣��{0}", e.Message));
                throw e;
            }
            #endregion

            #region ��ȡ������������Ϣ
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