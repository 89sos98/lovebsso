using System;
using Web.Core;
using CYQ.Data.Xml;
using CYQ.Entity;

namespace Logic
{
    public class Pager
    {
        #region 分页属性设置
        private int _PageIndex;
        private int _RecordCount;
        private int _PageSize;
        private string _URPara;


        ///<summary>
        ///当前索引页
        /// <summary>
        public int PageIndex
        {
            get
            {
                return _PageIndex == 0 ? 1 : _PageIndex;
            }
            set
            {
                _PageIndex = value;
            }
        }

        ///<summary>
        /// 总记录数
        /// </summary>
        public int RecordCount
        {
            get
            {
                return _RecordCount;
            }
            set
            {
                _RecordCount = value;
            }
        }

        /// <summary>
        /// 每页显示的大小条数
        /// </summary>
        public int PageSize
        {
            get
            {
                if (_PageSize == 0)
                {
                    return 10;
                }
                return _PageSize;
            }
            set
            {
                _PageSize = value;
            }
        }

        /// <summary>
        /// URL重写模式时外部的参数组，如：list_isgood_{0}.html、list_type_{0}.html或{0}.html等，其中的“{0}”为数字型的页码
        /// </summary>
        public string URLPara
        {
            get
            {
                if (string.IsNullOrEmpty(_URPara))
                {
                    return "{0}";
                }
                return _URPara;
            }
            set
            {
                _URPara = value;
            }
        }
        private XmlHelper Document;
        #endregion
        /// <param name="urlRewriterParameter">Url重写地址参数，如：/abc/{0}</param>
        public Pager(int recordCount, int pageIndex, int pageSize,string urlPara)
        {
            _RecordCount = recordCount;
            _PageIndex = pageIndex;
            _PageSize = pageSize;
           _URPara = urlPara;
        }
        private void FormatFourNum(int pageCount)
        {
            if (PageIndex > 1)
            {
                Document.Set(IDKey.labFirst, SetType.Href, URLPara.Replace("{0}", "1"));
                Document.Set(IDKey.labPrev, SetType.Href, URLPara.Replace("{0}", (PageIndex - 1).ToString()));
            }

            if (PageIndex < pageCount)
            {
                Document.Set(IDKey.labNext, SetType.Href, URLPara.Replace("{0}", (_PageIndex + 1).ToString()));
                Document.Set(IDKey.labLast, SetType.Href, URLPara.Replace("{0}", pageCount.ToString()));
            }
        }
        protected void FormatNum(int pageCount)
        {
            int start = 1, end = 10;
            if (pageCount < end)//页数小于10
            {
                end = pageCount;
            }
            else
            {
                start = (PageIndex > 5) ? PageIndex - 5 : start;
                int result = (start + 9) - pageCount;//是否超过最后面的页数
                if (result > 0)
                {
                    end = pageCount;
                    start -= result;//超过后,补差
                }
                else
                {
                    end = start + 9;
                }
            }
             SetNumLink(start, end);
        }
        private void SetNumLink(int start, int end)
        {
            string numLinks = string.Empty;
            System.Xml.XmlNode node = Document.GetByID(IDKey.labForNum);
            if (node != null)
            {
                string activeCss = "";

                if (node.Attributes!=null && node.Attributes["active"] != null)
                {
                    activeCss = node.Attributes["active"].Value;
                }
                System.Xml.XmlNode newNode = null;
                for (int i = end; i >= start; i--)
                {
                    Document.Set(IDKey.labNum, SetType.A, i.ToString(), URLPara.Replace("{0}", i.ToString()));
                    newNode = node.Clone();
                    if (i == PageIndex && activeCss.Length > 0)
                    {
                        Document.Set(newNode, SetType.Class, activeCss);
                    }
                    Document.InsertAfter(newNode, node);
                }
                Document.Remove(node);
            }
        }
        public void FormatPager(XmlHelper document)
        {
            if (_RecordCount <= 0)
            {
                return;
            }
            Document = document;
            int pageCount = (RecordCount % PageSize) == 0 ? RecordCount / PageSize : RecordCount / PageSize + 1;//页数
            FormatFourNum(pageCount);
            FormatNum(pageCount);
        }
    }
}
