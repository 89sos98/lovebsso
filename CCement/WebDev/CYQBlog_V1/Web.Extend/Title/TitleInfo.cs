using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Extend
{
    public class TitleInfo
    {
        public TitleInfo()
        {
        }
        public TitleInfo(string title, string keywords, string description)
        {
            _Title = title;
            _Keywords = keywords;
            _Description = description;
        }
        private string _Title;
        /// <summary>
        /// 页面标题
        /// </summary>
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
            }
        }

        private string _Keywords;
        /// <summary>
        /// 页面关键字
        /// </summary>
        public string Keywords
        {
            get
            {
                return _Keywords;
            }
            set
            {
                _Keywords = value;
            }
        }
        private string _Description;
        /// <summary>
        /// 页面描述
        /// </summary>
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
            }
        }
        internal string Split = " - ";
        internal void ClearHtml()
        {
            if (!string.IsNullOrEmpty(_Title))
            {
                _Title=Tool.Common.CleanHtml(_Title);
            }
            if (!string.IsNullOrEmpty(_Keywords))
            {
                _Keywords=Tool.Common.CleanHtml(_Keywords);
            }
            if (!string.IsNullOrEmpty(_Description))
            {
               _Description= Tool.Common.CleanHtml(_Description);
            }
        }
    }
}
