using System;
using CYQ.Data.Xml;
using System.Xml;
using System.Web.UI;
using System.Web;
using System.Text;
using Web.Core;


namespace CYQ.Editor
{
    public class Editor
    {
        #region ��������
        private string _EditorID;   //�༭��ID
        private string _BasePath=Config.HttpHost;   //�༭��·��
        private string _UpFilePath=Config.EditorUploadPath; //�ϴ��ļ��洢·��
        private string _Text;   //�༭������
        private string _ToolBarList;   //�༭�����߼��б�
        private int _Width; //�༭�����
        private int _Height;    //�༭���߶�
        protected System.Web.UI.HtmlControls.HtmlInputHidden txtContentText = new System.Web.UI.HtmlControls.HtmlInputHidden();   //���뱣�����ݵ�TextBox


        /// <summary>
        /// �༭��ID
        /// </summary>
        public string EditorID
        {
            set
            {
                _EditorID = value;
            }

            get
            {
                return _EditorID;
            }
        }

        /// <summary>
        /// �༭�����
        /// </summary>
        public int Width
        {
            set
            {
                _Width = value;
            }

            get
            {
                return _Width;
            }
        }

        /// <summary>
        /// �༭���߶�
        /// </summary>
        public int Height
        {
            set
            {
                _Height = value;
            }

            get
            {
                return _Height;
            }
        }

        /// <summary>
        /// �༭��·��
        /// </summary>
        public string BasePath
        {
            set
            {
                _BasePath = value;
            }

            get
            {
                return _BasePath;
            }
        }


        /// <summary>
        /// �༭���ϴ��ļ��洢·�����ַ�����ʽ���磺/UploadFile/files/���������ļ�������չ��
        /// </summary>
        public string UpFilePath
        {
            set
            {
                _UpFilePath = value;
            }

            get
            {
                return _UpFilePath;
            }
        }

        /// <summary>
        /// �༭������
        /// </summary>
        public string Text
        {
            set
            {
                _Text = value;
                //OnEditorPost(value);
            }
        }

        /// <summary>
        /// �༭�����߼��б���������ָ���Ĺ��ߣ�Ϊ�������ȫ������ʽΪ��xxx|xxxx,(���ù��߼���FontBold|Italic|UnderLine|Image|Flash|Link|Unlink|RemoveFormat|Smiley|FontName|FontSize|FontColor|Design|Preview|HtmlCode|EditorSize)
        /// </summary>
        public string ToolBarList
        {
            set
            {
                _ToolBarList = value;
            }

            get
            {
                return _ToolBarList;
            }
        }
        #endregion
        XmlHelper Document;
        MutilLanguage Language;
        XmlNode HeadNode;
        public Editor(XmlHelper document, MutilLanguage language)
        {
            Document = document;
            Language = language;
            XmlNodeList nodeList = Document.GetList("head");
            if (nodeList != null)
            {
                HeadNode = nodeList[0];
            }
        }
        /// <summary>
        /// ���ɿͻ��˱༭��
        /// </summary>
        /// <returns></returns>
        public string DEditorHtml(string sourceUrl)
        {
            if (HeadNode != null)//׷����ʽ�ͽű�
            {
                HeadNode.AppendChild(Document.CreateNode("link", "", "href", BasePath + "/editor/editor.css", "type", "text/css", "rel", "stylesheet"));
                HeadNode.AppendChild(Document.CreateNode("script", " ", "type", "text/javascript", "src", BasePath + "/editor/editor.js"));
            }

            StringBuilder innerHTML = new StringBuilder();


            int iHeight = Height - 32; //�༭���ı����߶�(-32px�������߶�)
            int iWidth = Width - 2;    //�༭����ʵ���(-2px�߿���)

            //��鵱ǰ������Ƿ�֧��Script�ű�
            if (ScriptCheck(sourceUrl))
            {//֧��Script�ű�
                #region

                innerHTML.Append("<div id='Editor_Div' class='deditor' style='width:" + Width + "px; height:" + Height + "px;'>");
                innerHTML.Append("  <div class='toolbar' id='toolbar' style='background:url(" + BasePath + "/editor/images/toolbar_bg.gif) repeat-x'>");
                innerHTML.Append("      <div class='toolbar_start'><img src='" + BasePath + "/editor/images/start.gif' /></div>");
                if (ToolBarCheck("FontBold"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><img class='toolbar_ico' id='Bold' onmouseover='deditorClass.OverIco(this)' title='" + Language.Get("EditorFontBold") + "' onclick=\"deditorClass.FontBold(\'" + EditorID + "\')\" onmouseout=\"deditorClass.OutIco(this,'" + BasePath + "')\" src='" + BasePath + "/editor/images/bold.gif' /></div>");  //����
                }
                if (ToolBarCheck("Italic"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><img class='toolbar_ico' id='Italic' onmouseover='deditorClass.OverIco(this)' title='" + Language.Get("EditorFontItalic") + "' onclick=\"deditorClass.FontItalic('" + EditorID + "')\" onmouseout=\"deditorClass.OutIco(this,'" + BasePath + "')\" src='" + BasePath + "/editor/images/italic.gif' /></div>");    //б��
                }
                if (ToolBarCheck("UnderLine"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><img class='toolbar_ico' id='UnderLine' onmouseover='deditorClass.OverIco(this)' title='" + Language.Get("EditorFontUnderLine") + "' onclick=\"deditorClass.FontUnderLine(\'" + EditorID + "\')\" onmouseout=\"deditorClass.OutIco(this,'" + BasePath + "')\" src='" + BasePath + "/editor/images/underline.gif' /></div>"); //�»���
                }
                if (ToolBarCheck("Image"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><img class='toolbar_ico' id='InsertImage' onmouseover='deditorClass.OverIco(this)' title='" + Language.Get("EditorInsertImage") + "' onclick=\"deditorClass.InsertTypeData(this,'InsertImage',340,'" + BasePath + "','" + EditorID + "','" + UpFilePath + "')\" onmouseout=\"deditorClass.OutIco(this,'" + BasePath + "')\" src='" + BasePath + "/editor/images/image.gif' /></div>"); //����ͼƬ
                }
                if (ToolBarCheck("Flash"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><img class='toolbar_ico' id='InsertFlash' onmouseover='deditorClass.OverIco(this)' title='" + Language.Get("EditorInsertFlash") + "' onclick=\"deditorClass.InsertTypeData(this,'InsertFlash',340,'" + BasePath + "','" + EditorID + "','" + UpFilePath + "')\" onmouseout=\"deditorClass.OutIco(this,'" + BasePath + "')\" src='" + BasePath + "/editor/images/flash.gif' /></div>"); //����Flash
                }
                if (ToolBarCheck("Link"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><img class='toolbar_ico' id='InsertLink' onmouseover='deditorClass.OverIco(this)' title='" + Language.Get("EditorInsertLink") + "' onclick=\"deditorClass.InsertTypeData(this,'InsertLink',280,'" + BasePath + "','" + EditorID + "')\" onmouseout=\"deditorClass.OutIco(this,'" + BasePath + "')\" src='" + BasePath + "/editor/images/link.gif' /></div>"); //���볬������
                }
                if (ToolBarCheck("Unlink"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><img class='toolbar_ico' id='InsertUnlink' onmouseover='deditorClass.OverIco(this)' title='" + Language.Get("EditorInsertUnlink") + "' onclick=\"deditorClass.InsertUnlink('" + EditorID + "')\" onmouseout=\"deditorClass.OutIco(this,'" + BasePath + "')\" src='" + BasePath + "/editor/images/unlink.gif' /></div>"); //ȡ����������
                }
                if (ToolBarCheck("RemoveFormat"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><img class='toolbar_ico' id='RemoveFormat' onmouseover='deditorClass.OverIco(this)' title='" + Language.Get("EditorRemoveFormats") + "' onclick=\"deditorClass.RemoveFormats('" + EditorID + "')\" onmouseout=\"deditorClass.OutIco(this,'" + BasePath + "')\" src='" + BasePath + "/editor/images/removeformat.gif' /></div>"); //�����ʽ
                }
                if (ToolBarCheck("Smiley"))
                {
                    innerHTML.Append("      <div class='toolbar_div' id='DivInsertSmiley'><img class='toolbar_ico' id='InsertSmiley' onmouseover='deditorClass.OverIco(this)' title='" + Language.Get("EditorInsertSmiley") + "' onclick=\"deditorClass.InsertTypeData(this,'InsertSmiley',134,'" + BasePath + "','" + EditorID + "')\" onmouseout=\"deditorClass.OutIco(this,'" + BasePath + "')\" src='" + BasePath + "/editor/images/smiley.gif' /></div>"); //�������
                }
                if (ToolBarCheck("FontName"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><div class='toolbar_text'><select id='dropFontName' title='" + Language.Get("EditorSetFontName") + "' onchange=\"deditorClass.SetFontName('" + EditorID + "')\"><option value='����'>" + Language.Get("EditorFontName") + "</option><option value='����'>����</option><option value='����_GB2312'>����</option><option value='������'>������</option><option value='����'>����</option><option value='����'>����</option><option value='Andale Mono'>Andale Mono</option><option value='Arial'>Arial</option><option value='Arial Black'>Arial Black</option><option value='Book Antiqua'>Book Antiqua</option><option value='Century Gothic'>Century Gothic</option><option value='Comic Sans MS'>Comic Sans MS</option><option value='Courier New'>Courier New</option><option value='Georgia'>Georgia</option><option value='Impact'>Impact</option><option value='Tahoma'>Tahoma</option><option value='Times New Roman' >Times New Roman</option><option value='Trebuchet MS'>Trebuchet MS</option><option value='Script MT Bold'>Script MT Bold</option><option value='Stencil'>Stencil</option><option value='Verdana'>Verdana</option><option value='Lucida Console'>Lucida Console</option></select></div></div>"); //��������
                }
                if (ToolBarCheck("FontSize"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><div class='toolbar_text'><select id='dropFontSize' title='" + Language.Get("EditorSetFontSize") + "' onchange=\"deditorClass.SetFontSize('" + EditorID + "')\"><option value='2'>" + Language.Get("EditorFontSize") + "</option><option value='1'>" + Language.Get("EditorSetFontSize1") + "</option><option value='2'>" + Language.Get("EditorSetFontSize2") + "</option><option value='3'>" + Language.Get("EditorSetFontSize3") + "</option><option value='4'>" + Language.Get("EditorSetFontSize4") + "</option><option value='5'>" + Language.Get("EditorSetFontSize5") + "</option><option value='6'>" + Language.Get("EditorSetFontSize6") + "</option><option value='7'>" + Language.Get("EditorSetFontSize7") + "</option></select></div></div>"); //���ִ�С
                }
                if (ToolBarCheck("FontColor"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><div class='toolbar_text'><select id='dropFontColor' title='" + Language.Get("EditorSetFontColor") + "' onchange=\"deditorClass.SetFontColor('" + EditorID + "')\" ><option value='Black'>" + Language.Get("EditorFontColor") + "</option><option style='color: black; background-color: black' value='Black'>Black</option><option style='color: red; background-color: red' value='Red'>Red</option><option style='color: yellow; background-color: yellow' value='Yellow'>Yellow</option><option style='color: pink; background-color: pink' value='Pink'>Pink</option><option style='color: green; background-color: green' value='Green'>Green</option><option style='color: orange; background-color: orange' value='Orange'>Orange</option><option style='color: purple; background-color: purple' value='Purple'>Purple</option><option style='color: blue; background-color: blue' value='Blue'>Blue</option><option style='color: beige; background-color: beige' value='Beige'>Beige</option><option style='color: brown; background-color: brown' value='Brown'>Brown</option><option style='color: teal; background-color: teal' value='Teal'>Teal</option><option style='color: navy; background-color: navy' value='Navy'>Navy</option><option style='color: maroon; background-color: maroon' value='Maroon'>Maroon</option><option style='color: limegreen; background-color: limegreen' value='LimeGreen'>LimeGreen</option></select></div></div>"); //������ɫ
                }
                innerHTML.Append("  </div>");
                innerHTML.Append("  <div class='contenttext'>");
                innerHTML.Append("      <iframe id='" + EditorID + "_iframe' name='" + EditorID + "_iframe' frameborder='no' onblur=\"deditorClass.IframeToContentText('" + EditorID + "')\" ONSELECT=\"pos=document.selection.createRange();\"  onCLICK=\"pos=document.selection.createRange();\"  onKEYUP=\"pos=document.selection.createRange();\" style='border:0px; width:" + iWidth + "px; height:" + iHeight + "px; left:0px; right:0px;display:block;'></iframe>");
                innerHTML.Append("      <textarea id='" + EditorID + "' name='" + EditorID + "' onblur=\"deditorClass.ContentTextToIframe('" + EditorID + "')\" style='border:0px; width:" + iWidth + "px; height:" + (iHeight + 32) + "px; left:0px; right:0px;display:none'>" + _Text + "</textarea>");
                innerHTML.Append("  </div>");
                innerHTML.Append("</div>");
                innerHTML.Append("<div class='toolbar_mode' style='width:" + Width + "px;'>");
                if (ToolBarCheck("Design"))
                {
                    innerHTML.Append("  <div id='EditorDesign_div' class='toolbar_mode_b' onclick=\"deditorClass.EditorDesign('" + EditorID + "')\"><img src='" + BasePath + "/editor/images/mode_design.gif' border='0' align='absmiddle' /></div>");
                }
                if (ToolBarCheck("Preview"))
                {
                    innerHTML.Append("  <div id='EditorPreview_div' class='toolbar_mode_a' onclick=\"deditorClass.EditorPreview('" + EditorID + "','" + BasePath + "'," + Width + ",560)\"><img src='" + BasePath + "/editor/images/mode_view.gif' border='0' align='absmiddle' /></div>");
                }
                if (ToolBarCheck("HtmlCode"))
                {
                    innerHTML.Append("  <div id='EditorHtmlCode_div' class='toolbar_mode_a' onclick=\"deditorClass.EditorHtmlCode('" + EditorID + "')\"><img src='" + BasePath + "/editor/images/mode_html.gif' border='0' align='absmiddle' /></div>");
                }
                if (ToolBarCheck("EditorSize"))
                {
                    innerHTML.Append("  <div class='toolbar_mode_c' onclick=\"deditorClass.EditorSize(-100,'" + EditorID + "','Editor_Div')\"><img src='" + BasePath + "/editor/images/minus.gif' border='0' align='absmiddle' title='" + Language.Get("EditorEditorZoomOut") + "' /></div>");
                    innerHTML.Append("  <div class='toolbar_mode_c' onclick=\"deditorClass.EditorSize(+100,'" + EditorID + "','Editor_Div')\"><img src='" + BasePath + "/editor/images/plus.gif' border='0' align='absmiddle' title='" + Language.Get("EditorEditorZoomIn") + "' /></div>");
                }
                innerHTML.Append("</div>");
                innerHTML.Append("  <script type='text/javascript'>deditorClass.EditorDesignMode('" + EditorID + "');deditorClass.SaveContentTextGo('" + EditorID + "');deditorClass.LoadContentTextGo('" + EditorID + "', '');</script>");
                #endregion
            }
            else
            {//��֧��Script�ű�
                #region

                string strContentText = _Text;   //��ʼԭ�༭���ı���ֵ
                string txtText = "";    //��ʼ�����������ı�ֵ
                string CommandName = "";    //��ʼ��������
                string Tool_CommandName = "";   //��ʼ�����ڲ�����������
                string strFileUrl = "";
                int intTextWidth;
                int intTextHeight;
                string Tool_InsertSmiley = "";
                string Mode_CommandName = "";

                //ȡ�ñ༭��ԭ�ı���ֵ
                if (QueryString(EditorID) != "")
                {
                    strContentText = QueryString(EditorID).ToString();
                }

                //ȡ�ù�������
                if (QueryString("CommandName") != "")
                {
                    iHeight = iHeight - 24; //�༭���ı����߶�(-32px�������߶�)

                    CommandName = QueryString("CommandName").ToString();   //ȡ�ù�����������
                }

                //���ݲ�ͬ�Ĺ������ָ��txtText��������ֵ
                if (QueryString("Tool_CommandName") != "")
                {
                    Tool_CommandName = QueryString("Tool_CommandName");   //ȡ�ù����ڲ���������������

                    txtText = QueryString("txtText");

                    switch (Tool_CommandName)
                    {
                        case "FontBold":    //�Ӵ��ı�
                            txtText = "<B>" + txtText + "</B>";
                            break;
                        case "FontItalic":  //б���ı�
                            txtText = "<EM>" + txtText + "</EM>";
                            break;
                        case "UnderLine":   //�»����ı�
                            txtText = "<U>" + txtText + "</U>";
                            break;
                        case "InsertImage":   //�ϴ�ͼƬ�ļ�
                            strFileUrl = Uploader.UploadFile(HttpContext.Current.Request.Files["UpFile"], 1);
                            txtText = "<IMG src='" + strFileUrl + "' border='0' />";
                            break;
                        case "InsertFlash":   //�ϴ�Flash�ļ�
                            strFileUrl =Uploader.UploadFile(HttpContext.Current.Request.Files["UpFile"], 2);
                            intTextWidth = 300;
                            intTextHeight = 200;

                            if (QueryString("txtTextWidth") != "" && QueryString("txtTextHeight") != "")
                            {
                                intTextWidth = Convert.ToInt32(QueryString("txtTextWidth"));
                                intTextHeight = Convert.ToInt32(QueryString("txtTextHeight"));
                            }
                            txtText = "<object classid='clsid:D27CDB6E-AE6D-11cf-96B8-444553540000' codebase='http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0' width='" + intTextWidth + "' height='" + intTextHeight + "'><param name='movie' value='" + strFileUrl + "' /><param name='quality' value='high' /><embed src='" + strFileUrl + "' quality='high' pluginspage='http://www.macromedia.com/go/getflashplayer' type='application/x-shockwave-flash' width='" + intTextWidth + "' height='" + intTextHeight + "'></embed></object>";
                            break;
                        case "InsertLink":   //��������
                            if (txtText == "")
                            {
                                txtText = "<A href='" + QueryString("txtTextUrl") + "'>" + QueryString("txtTextUrl") + "</A>";
                            }
                            else
                            {
                                txtText = "<A href='" + QueryString("txtTextUrl") + "' target='_blank'>" + txtText + "</A>";
                            }
                            break;
                        case "RemoveFormat":   //�����ʽ
                            strContentText = System.Text.RegularExpressions.Regex.Replace(strContentText, "<B>|</B>|<EM>|</EM>|<U>|</U>|<FONT(.*?)>|</FONT>", "");
                            break;
                        case "FontName":  //��������
                            txtText = "<FONT face='" + QueryString("dropFontName") + "'>" + txtText + "</FONT>";
                            break;
                        case "FontSize":  //�����С
                            txtText = "<FONT size='" + QueryString("dropFontSize") + "'>" + txtText + "</FONT>";
                            break;
                        case "FontColor":  //������ɫ
                            txtText = "<FONT color='" + QueryString("dropFontColor") + "'>" + txtText + "</FONT>";
                            break;
                    }
                }
                else if (QueryString("Tool_InsertSmiley") != "")   //�������
                {
                    Tool_InsertSmiley = QueryString("Tool_InsertSmiley");   //ȡ�ñ������
                    txtText = "<IMG src='" + BasePath + "Smiley/" + Tool_InsertSmiley + ".gif' border='0' />";
                }
                else if (QueryString("Mode_CommandName") != "")
                {
                    Mode_CommandName = QueryString("Mode_CommandName");
                }
                //Code Fixed By Mao
                strContentText = strContentText + txtText;  //�༭���ı������ݵ���ԭ�ı�ֵ���Ϲ������������ֵ

                //stringBuilder.Append("<form id='" + EditorID + "_form' action='" + PageUrl + "' method='post' enctype='multipart/form-data'>");
                innerHTML.Append("<div id='Editor_Div' class='deditor' style='width:" + Width + "px; height:" + Height + "px;'>");
                innerHTML.Append("  <div class='toolbar' id='toolbar' style='background:url(" + BasePath + "/editor/images/toolbar_bg.gif) repeat-x'>");
                innerHTML.Append("      <div class='toolbar_start'><img src='" + BasePath + "/editor/images/start.gif' /></div>");
                if (ToolBarCheck("FontBold"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><input type='submit' id='CommandName' name='CommandName' value='FontBold' class='toolbar_ico2' style='background:url(" + BasePath + "/editor/images/bold.gif) repeat-x; width:23px; height:23px; cursor:pointer;' /></div>");  //����
                }
                if (ToolBarCheck("Italic"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><input type='submit' id='CommandName' name='CommandName' value='FontItalic' class='toolbar_ico2' style='background:url(" + BasePath + "/editor/images/italic.gif) repeat-x; width:23px; height:23px; cursor:pointer;' /></div>");    //б��
                }
                if (ToolBarCheck("UnderLine"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><input type='submit' id='CommandName' name='CommandName' value='UnderLine' class='toolbar_ico2' style='background:url(" + BasePath + "/editor/images/underline.gif) repeat-x; width:23px; height:23px; cursor:pointer;' /></div>"); //�»���
                }
                if (ToolBarCheck("Image"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><input type='submit' id='CommandName' name='CommandName' value='InsertImage' class='toolbar_ico2' style='background:url(" + BasePath + "/editor/images/image.gif) repeat-x; width:23px; height:23px; cursor:pointer;' /></div>"); //����ͼƬ
                }
                if (ToolBarCheck("Flash"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><input type='submit' id='CommandName' name='CommandName' value='InsertFlash' class='toolbar_ico2' style='background:url(" + BasePath + "/editor/images/flash.gif) repeat-x; width:23px; height:23px; cursor:pointer;' /></div>"); //����Flash
                }
                if (ToolBarCheck("Link"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><input type='submit' id='CommandName' name='CommandName' value='InsertLink' class='toolbar_ico2' style='background:url(" + BasePath + "/editor/images/link.gif) repeat-x; width:23px; height:23px; cursor:pointer;' /></div>"); //���볬������
                }
                if (ToolBarCheck("Unlink"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><img class='toolbar_ico' id='InsertUnlink' title='" + Language.Get("EditorInsertUnlink") + "' src='" + BasePath + "/editor/images/unlink2.gif' /></div>"); //ȡ����������
                }
                if (ToolBarCheck("RemoveFormat"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><input type='submit' id='Tool_CommandName' name='Tool_CommandName' value='RemoveFormat' class='toolbar_ico2' style='background:url(" + BasePath + "/editor/images/removeformat.gif) repeat-x; width:23px; height:23px; cursor:pointer;' /></div>"); //�����ʽ
                }
                if (ToolBarCheck("Smiley"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><input type='submit' id='CommandName' name='CommandName' value='InsertSmiley' class='toolbar_ico2' style='background:url(" + BasePath + "/editor/images/smiley.gif) repeat-x; width:23px; height:23px; cursor:pointer;' /></div>"); //�������
                }
                if (ToolBarCheck("FontName"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><input type='submit' id='CommandName' name='CommandName' value='FontName' class='toolbar_ico2' style='background:url(" + BasePath + "/editor/images/fontname.gif) repeat-x; width:108px; height:23px; cursor:pointer;' /></div>"); //��������
                }
                if (ToolBarCheck("FontSize"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><input type='submit' id='CommandName' name='CommandName' value='FontSize' class='toolbar_ico2' style='background:url(" + BasePath + "/editor/images/fontsize.gif) repeat-x; width:65px; height:23px; cursor:pointer;' /></div>"); //���ִ�С
                }
                if (ToolBarCheck("FontColor"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><input type='submit' id='CommandName' name='CommandName' value='FontColor' class='toolbar_ico2' style='background:url(" + BasePath + "/editor/images/fontcolor.gif) repeat-x; width:83px; height:23px; cursor:pointer;' /></div>"); //������ɫ
                }
                innerHTML.Append("  </div>");
                innerHTML.Append("  <div class='contenttext'>");

                //�Ƿ���ʾ�����������Ť
                if (CommandName != "")  //�粻Ϊ������ʾ�����������Ť
                {
                    innerHTML.Append("  <div class='toolbarbg'>");
                    innerHTML.Append(ToolBar.LoadToolBar(CommandName, BasePath));
                    innerHTML.Append("  </div>");
                }

                //��ʾ����ģʽ
                if (Mode_CommandName == "ModeDesign" || Mode_CommandName == "ModeHtml")
                {//���ģʽ��HTMLԴ���ʽ
                    if (ToolBarCheck("HtmlCode") || ToolBarCheck("Design"))
                    {
                        innerHTML.Append("      <textarea id='" + EditorID + "' name='" + EditorID + "' style='border:0px; width:" + iWidth + "px; height:" + iHeight + "px; left:0px; right:0px;'>" + strContentText + "</textarea>");
                    }
                }
                else if (Mode_CommandName == "ModePreview")
                {//Ԥ��ģʽ 
                    if (ToolBarCheck("Preview"))
                    {
                        innerHTML.Append("      <div style='border:0px; width:" + iWidth + "px; height:" + iHeight + "px; float:left; overflow: scroll;'>" + strContentText + "</div>");
                        innerHTML.Append("      <textarea id='" + EditorID + "' name='" + EditorID + "' style='border:0px; width:0px; height:0px; left:0px; right:0px; display:none'>" + strContentText + "</textarea>");
                    }
                }
                else
                {//���ģʽ
                    if (ToolBarCheck("Design"))
                    {
                        innerHTML.Append("      <textarea id='" + EditorID + "' name='" + EditorID + "' style='border:0px; width:" + iWidth + "px; height:" + iHeight + "px; left:0px; right:0px;'>" + strContentText + "</textarea>");
                    }
                }

                innerHTML.Append("  </div>");
                innerHTML.Append("</div>");
                innerHTML.Append("<div class='toolbar_mode' style='width:" + Width + "px;'>");

                if (Mode_CommandName == "ModeDesign")
                {
                    if (ToolBarCheck("Design"))
                    {
                        innerHTML.Append("  <div id='EditorDesign_div' class='toolbar_mode_b'><input type='submit' id='Mode_CommandName' name='Mode_CommandName' value='ModeDesign' class='toolbar_ico3' style='background:url(" + BasePath + "/editor/images/mode_design.gif) repeat-x; width:58px; height:20px; cursor:pointer;' /></div>");
                    }
                    if (ToolBarCheck("Preview"))
                    {
                        innerHTML.Append("  <div id='EditorPreview_div' class='toolbar_mode_a'><input type='submit' id='Mode_CommandName' name='Mode_CommandName' value='ModePreview' class='toolbar_ico3' style='background:url(" + BasePath + "/editor/images/mode_view.gif) repeat-x; width:58px; height:20px; cursor:pointer;' /></div>");
                    }
                    if (ToolBarCheck("HtmlCode"))
                    {
                        innerHTML.Append("  <div id='EditorHtmlCode_div' class='toolbar_mode_a'><input type='submit' id='Mode_CommandName' name='Mode_CommandName' value='ModeHtml' class='toolbar_ico3' style='background:url(" + BasePath + "/editor/images/mode_html.gif) repeat-x; width:58px; height:20px; cursor:pointer;' /></div>");
                    }
                }
                else if (Mode_CommandName == "ModePreview")
                {
                    if (ToolBarCheck("Design"))
                    {
                        innerHTML.Append("  <div id='EditorDesign_div' class='toolbar_mode_a'><input type='submit' id='Mode_CommandName' name='Mode_CommandName' value='ModeDesign' class='toolbar_ico3' style='background:url(" + BasePath + "/editor/images/mode_design.gif) repeat-x; width:58px; height:20px; cursor:pointer;' /></div>");
                    }
                    if (ToolBarCheck("Preview"))
                    {
                        innerHTML.Append("  <div id='EditorPreview_div' class='toolbar_mode_b'><input type='submit' id='Mode_CommandName' name='Mode_CommandName' value='ModePreview' class='toolbar_ico3' style='background:url(" + BasePath + "/editor/images/mode_view.gif) repeat-x; width:58px; height:20px; cursor:pointer;' /></div>");
                    }
                    if (ToolBarCheck("HtmlCode"))
                    {
                        innerHTML.Append("  <div id='EditorHtmlCode_div' class='toolbar_mode_a'><input type='submit' id='Mode_CommandName' name='Mode_CommandName' value='ModeHtml' class='toolbar_ico3' style='background:url(" + BasePath + "/editor/images/mode_html.gif) repeat-x; width:58px; height:20px; cursor:pointer;' /></div>");
                    }
                }
                else if (Mode_CommandName == "ModeHtml")
                {
                    if (ToolBarCheck("Design"))
                    {
                        innerHTML.Append("  <div id='EditorDesign_div' class='toolbar_mode_a'><input type='submit' id='Mode_CommandName' name='Mode_CommandName' value='ModeDesign' class='toolbar_ico3' style='background:url(" + BasePath + "/editor/images/mode_design.gif) repeat-x; width:58px; height:20px; cursor:pointer;' /></div>");
                    }
                    if (ToolBarCheck("Preview"))
                    {
                        innerHTML.Append("  <div id='EditorPreview_div' class='toolbar_mode_a'><input type='submit' id='Mode_CommandName' name='Mode_CommandName' value='ModePreview' class='toolbar_ico3' style='background:url(" + BasePath + "/editor/images/mode_view.gif) repeat-x; width:58px; height:20px; cursor:pointer;' /></div>");
                    }
                    if (ToolBarCheck("HtmlCode"))
                    {
                        innerHTML.Append("  <div id='EditorHtmlCode_div' class='toolbar_mode_b'><input type='submit' id='Mode_CommandName' name='Mode_CommandName' value='ModeHtml' class='toolbar_ico3' style='background:url(" + BasePath + "/editor/images/mode_html.gif) repeat-x; width:58px; height:20px; cursor:pointer;' /></div>");
                    }
                }
                else
                {
                    if (ToolBarCheck("Design"))
                    {
                        innerHTML.Append("  <div id='EditorDesign_div' class='toolbar_mode_b'><input type='submit' id='Mode_CommandName' name='Mode_CommandName' value='ModeDesign' class='toolbar_ico3' style='background:url(" + BasePath + "/editor/images/mode_design.gif) repeat-x; width:58px; height:20px; cursor:pointer;' /></div>");
                    }
                    if (ToolBarCheck("Preview"))
                    {
                        innerHTML.Append("  <div id='EditorPreview_div' class='toolbar_mode_a'><input type='submit' id='Mode_CommandName' name='Mode_CommandName' value='ModePreview' class='toolbar_ico3' style='background:url(" + BasePath + "/editor/images/mode_view.gif) repeat-x; width:58px; height:20px; cursor:pointer;' /></div>");
                    }
                    if (ToolBarCheck("HtmlCode"))
                    {
                        innerHTML.Append("  <div id='EditorHtmlCode_div' class='toolbar_mode_a'><input type='submit' id='Mode_CommandName' name='Mode_CommandName' value='ModeHtml' class='toolbar_ico3' style='background:url(" + BasePath + "/editor/images/mode_html.gif) repeat-x; width:58px; height:20px; cursor:pointer;' /></div>");
                    }
                }


                //stringBuilder.Append("  <div class='toolbar_mode_c'><img src='" + BasePath + "/editor/images/minus.gif' border='0' align='absmiddle' title='��С�����' /></div>");
                //stringBuilder.Append("  <div class='toolbar_mode_c'><img src='" + BasePath + "/editor/images/plus.gif' border='0' align='absmiddle' title='���������' /></div>");

                innerHTML.Append("</div>");
                //stringBuilder.Append("</form>");

                #endregion
            }

            return innerHTML.ToString();
        }

        /// <summary>
        /// ��鵱ǰ������Ƿ�֧��Script�ű�
        /// </summary>
        /// <param name="GoUrl">��ת��URL��ַ</param>
        /// <returns></returns>
        public bool ScriptCheck(string sourceUrl)
        {
            if (!sourceUrl.Contains("nojs"))//������ܻ�ȡnojsֵ��˵��δ����
            {
                if (HeadNode != null)
                {
                    HeadNode.AppendChild(Document.CreateNode("noscript", "<meta http-equiv=\"refresh\" content=\"0:url=" + sourceUrl + "/nojs\" />"));
                }
            }
            if (sourceUrl.Contains("nojs"))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// ��̬���ɲ����������ȡ���ݵ�TextBox�ؼ�txtContentText
        /// </summary>
        /// <param name="control"></param>
        public void ControlsAdd(Control control)
        {
            txtContentText.ID = "txtContentText";
            control.Controls.Add(txtContentText);
        }

        /// <summary>
        /// ����ָ��������ֵ����Ϊ��ʱ������""
        /// </summary>
        /// <param name="para">������</param>
        /// <returns></returns>
        private string QueryString(string para)
        {
            return HttpContext.Current.Request[para] != null ? HttpContext.Current.Request[para].Trim() : "";
        }


        /// <summary>
        /// ���ݹ������ƣ������б����Ƿ����
        /// </summary>
        /// <param name="ToolBarName"></param>
        /// <returns></returns>
        private bool ToolBarCheck(string name)
        {
            if (!string.IsNullOrEmpty(_ToolBarList))
            {
                return _ToolBarList.ToLower().IndexOf(name.ToLower()) > -1;
            }
            else
            {
                return true;
            }
        }
      
    }
}
