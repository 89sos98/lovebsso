using System;
using Web.Core;
using CYQ.Data.Xml;
using CYQ.Entity;

namespace Logic
{
    public class Pager
    {
        #region ��ҳ��������
        private int _PageIndex;
        private int _RecordCount;
        private int _PageSize;
        private string _URPara;


        ///<summary>
        ///��ǰ����ҳ
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
        /// �ܼ�¼��
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
        /// ÿҳ��ʾ�Ĵ�С����
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
        /// URL��дģʽʱ�ⲿ�Ĳ����飬�磺list_isgood_{0}.html��list_type_{0}.html��{0}.html�ȣ����еġ�{0}��Ϊ�����͵�ҳ��
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
        /// <param name="urlRewriterParameter">Url��д��ַ�������磺/abc/{0}</param>
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
            if (pageCount < end)//ҳ��С��10
            {
                end = pageCount;
            }
            else
            {
                start = (PageIndex > 5) ? PageIndex - 5 : start;
                int result = (start + 9) - pageCount;//�Ƿ񳬹�������ҳ��
                if (result > 0)
                {
                    end = pageCount;
                    start -= result;//������,����
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
            int pageCount = (RecordCount % PageSize) == 0 ? RecordCount / PageSize : RecordCount / PageSize + 1;//ҳ��
            FormatFourNum(pageCount);
            FormatNum(pageCount);
        }
    }
}
