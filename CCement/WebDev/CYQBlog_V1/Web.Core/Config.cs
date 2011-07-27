using System;
using System.Configuration;

namespace Web.Core
{
    public class Config
    {
        private static string _Www="www";
        /// <summary>
        /// www主域名[更改成任意二级域名]
        /// </summary>
        public static string Www
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["Www"]))
                {
                    _Www = ConfigurationManager.AppSettings["Www"];
                }
                return _Www;
            }
        }

        /// <summary>
        /// 主域名[http://www.xxx.com]
        /// </summary>
        public static string HttpHost
        {
            get
            {
                return "http://"+Www+"." + Domain;
            }
        }
        /// <summary>
        /// 主域名[不带www]
        /// </summary>
        public static string Domain
        {
            get
            {
                return Convert.ToString(ConfigurationManager.AppSettings["Domain"]);
            }
        }
        private static string _LoginUrl;
        /// <summary>
        /// 登陆Url
        /// </summary>
        public static string LoginUrl
        {
            get
            {
                _LoginUrl = Convert.ToString(ConfigurationManager.AppSettings["LoginUrl"]);
                if (string.IsNullOrEmpty(_LoginUrl))
                {
                    _LoginUrl = "/sys/login";
                }
                return _LoginUrl;
            }
        }
        private static string _SystemSkinPath;
        /// <summary>
        /// 系统皮肤/语言skin路径
        /// </summary>
        public static string SystemSkinPath
        {
            get
            {
                _SystemSkinPath=Convert.ToString(ConfigurationManager.AppSettings["SystemSkinPath"]);
                if (string.IsNullOrEmpty(_SystemSkinPath))
                {
                    _SystemSkinPath = "/skin/system/";
                }
                return _SystemSkinPath;
            }
        }
        private static string _AdminSkinPath;
        /// <summary>
        /// 系统皮肤/语言skin路径
        /// </summary>
        public static string AdminSkinPath
        {
            get
            {
                _AdminSkinPath = Convert.ToString(ConfigurationManager.AppSettings["AdminSkinPath"]);
                if (string.IsNullOrEmpty(_AdminSkinPath))
                {
                    _AdminSkinPath = "/skin/admin/";
                }
                return _AdminSkinPath;
            }
        }
        public static string CDATAFormat
        {
            get
            {
                return CYQ.Data.AppConfig.CDataLeft + "{0}" + CYQ.Data.AppConfig.CDataRight;
            }
        }
        private static string _FileAllowExName;
        /// <summary>
        /// 文件允许上传的扩展名
        /// </summary>
        public static string FileAllowExName
        {
            get
            {
                _FileAllowExName = Convert.ToString(ConfigurationManager.AppSettings["FileAllowExName"]);
                if (string.IsNullOrEmpty(_FileAllowExName))
                {
                    _FileAllowExName = ".jpg;.gif;.bmp;.png;.swf;.rar;";
                }
                return _FileAllowExName;
            }
        }
        private static string _EditorUploadPath;
        /// <summary>
        /// 编辑器文件上传路径
        /// </summary>
        public static string EditorUploadPath
        {
            get
            {
                _EditorUploadPath = Convert.ToString(ConfigurationManager.AppSettings["EditorUploadPath"]);
                if (string.IsNullOrEmpty(_EditorUploadPath))
                {
                    _EditorUploadPath = "/Upload/Editor/";
                }
                return _EditorUploadPath;
            }
        }
       
        /// <summary>
        ///用户头像上传路径
        /// </summary>
        public static string UserHeadUploadPath
        {
            get
            {
                _UserHeadUploadPath = Convert.ToString(ConfigurationManager.AppSettings["UserHeadUploadPath"]);
                if (string.IsNullOrEmpty(_UserHeadUploadPath))
                {
                    _UserHeadUploadPath = "/Upload/UserHead/";
                }
                return _UserHeadUploadPath;
            }
        }
        private static string _UserHeadUploadPath;
        /// <summary>
        ///用户头像上传路径
        /// </summary>
        public static string UserPhotoUploadPath
        {
            get
            {
                _UserPhotoUploadPath = Convert.ToString(ConfigurationManager.AppSettings["UserPhotoUploadPath"]);
                if (string.IsNullOrEmpty(_UserPhotoUploadPath))
                {
                    _UserPhotoUploadPath = "/Upload/UserPhoto/";
                }
                return _UserPhotoUploadPath;
            }
        }
        private static string _UserPhotoUploadPath;
        /// <summary>
        /// 页面缓存,设置了值即开启缓存。[单位分钟]
        /// </summary>
        public static int PageCacheTime
        {
            get
            {
                int.TryParse(Convert.ToString(ConfigurationManager.AppSettings["PageCacheTime"]), out _PageCacheTime);
                return _PageCacheTime;
            }
        }
        private static int _PageCacheTime=0;

        public static string UrlAspx
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["UrlAspx"];
            }
        }
        public static string DefaultHead
        {
            get
            {
                return UserHeadUploadPath + "default1.jpg";
            }
        }
        public static string DefaultHead2
        {
            get
            {
                return UserHeadUploadPath + "default2.jpg";
            }
        }
        public static string PasswordKey
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["PasswordKey"];
            }
        }
    }
}
