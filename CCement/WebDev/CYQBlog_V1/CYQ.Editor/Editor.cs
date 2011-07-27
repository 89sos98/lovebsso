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
        #region 常见属性
        private string _EditorID;   //编辑器ID
        private string _BasePath=Config.HttpHost;   //编辑器路径
        private string _UpFilePath=Config.EditorUploadPath; //上传文件存储路径
        private string _Text;   //编辑器内容
        private string _ToolBarList;   //编辑器工具集列表
        private int _Width; //编辑器宽度
        private int _Height;    //编辑器高度
        protected System.Web.UI.HtmlControls.HtmlInputHidden txtContentText = new System.Web.UI.HtmlControls.HtmlInputHidden();   //用与保存内容的TextBox


        /// <summary>
        /// 编辑器ID
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
        /// 编辑器宽度
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
        /// 编辑器高度
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
        /// 编辑器路径
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
        /// 编辑器上传文件存储路径，字符串格式，如：/UploadFile/files/，不包括文件名及扩展名
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
        /// 编辑器内容
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
        /// 编辑器工具集列表，用与载入指定的工具，为空则加载全部，格式为：xxx|xxxx,(可用工具集：FontBold|Italic|UnderLine|Image|Flash|Link|Unlink|RemoveFormat|Smiley|FontName|FontSize|FontColor|Design|Preview|HtmlCode|EditorSize)
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
        /// 生成客户端编辑器
        /// </summary>
        /// <returns></returns>
        public string DEditorHtml(string sourceUrl)
        {
            if (HeadNode != null)//追加样式和脚本
            {
                HeadNode.AppendChild(Document.CreateNode("link", "", "href", BasePath + "/editor/editor.css", "type", "text/css", "rel", "stylesheet"));
                HeadNode.AppendChild(Document.CreateNode("script", " ", "type", "text/javascript", "src", BasePath + "/editor/editor.js"));
            }

            StringBuilder innerHTML = new StringBuilder();


            int iHeight = Height - 32; //编辑器文本区高度(-32px工具栏高度)
            int iWidth = Width - 2;    //编辑器真实宽度(-2px边框宽度)

            //检查当前浏览器是否支持Script脚本
            if (ScriptCheck(sourceUrl))
            {//支持Script脚本
                #region

                innerHTML.Append("<div id='Editor_Div' class='deditor' style='width:" + Width + "px; height:" + Height + "px;'>");
                innerHTML.Append("  <div class='toolbar' id='toolbar' style='background:url(" + BasePath + "/editor/images/toolbar_bg.gif) repeat-x'>");
                innerHTML.Append("      <div class='toolbar_start'><img src='" + BasePath + "/editor/images/start.gif' /></div>");
                if (ToolBarCheck("FontBold"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><img class='toolbar_ico' id='Bold' onmouseover='deditorClass.OverIco(this)' title='" + Language.Get("EditorFontBold") + "' onclick=\"deditorClass.FontBold(\'" + EditorID + "\')\" onmouseout=\"deditorClass.OutIco(this,'" + BasePath + "')\" src='" + BasePath + "/editor/images/bold.gif' /></div>");  //粗体
                }
                if (ToolBarCheck("Italic"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><img class='toolbar_ico' id='Italic' onmouseover='deditorClass.OverIco(this)' title='" + Language.Get("EditorFontItalic") + "' onclick=\"deditorClass.FontItalic('" + EditorID + "')\" onmouseout=\"deditorClass.OutIco(this,'" + BasePath + "')\" src='" + BasePath + "/editor/images/italic.gif' /></div>");    //斜体
                }
                if (ToolBarCheck("UnderLine"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><img class='toolbar_ico' id='UnderLine' onmouseover='deditorClass.OverIco(this)' title='" + Language.Get("EditorFontUnderLine") + "' onclick=\"deditorClass.FontUnderLine(\'" + EditorID + "\')\" onmouseout=\"deditorClass.OutIco(this,'" + BasePath + "')\" src='" + BasePath + "/editor/images/underline.gif' /></div>"); //下划线
                }
                if (ToolBarCheck("Image"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><img class='toolbar_ico' id='InsertImage' onmouseover='deditorClass.OverIco(this)' title='" + Language.Get("EditorInsertImage") + "' onclick=\"deditorClass.InsertTypeData(this,'InsertImage',340,'" + BasePath + "','" + EditorID + "','" + UpFilePath + "')\" onmouseout=\"deditorClass.OutIco(this,'" + BasePath + "')\" src='" + BasePath + "/editor/images/image.gif' /></div>"); //插入图片
                }
                if (ToolBarCheck("Flash"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><img class='toolbar_ico' id='InsertFlash' onmouseover='deditorClass.OverIco(this)' title='" + Language.Get("EditorInsertFlash") + "' onclick=\"deditorClass.InsertTypeData(this,'InsertFlash',340,'" + BasePath + "','" + EditorID + "','" + UpFilePath + "')\" onmouseout=\"deditorClass.OutIco(this,'" + BasePath + "')\" src='" + BasePath + "/editor/images/flash.gif' /></div>"); //插入Flash
                }
                if (ToolBarCheck("Link"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><img class='toolbar_ico' id='InsertLink' onmouseover='deditorClass.OverIco(this)' title='" + Language.Get("EditorInsertLink") + "' onclick=\"deditorClass.InsertTypeData(this,'InsertLink',280,'" + BasePath + "','" + EditorID + "')\" onmouseout=\"deditorClass.OutIco(this,'" + BasePath + "')\" src='" + BasePath + "/editor/images/link.gif' /></div>"); //插入超级链接
                }
                if (ToolBarCheck("Unlink"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><img class='toolbar_ico' id='InsertUnlink' onmouseover='deditorClass.OverIco(this)' title='" + Language.Get("EditorInsertUnlink") + "' onclick=\"deditorClass.InsertUnlink('" + EditorID + "')\" onmouseout=\"deditorClass.OutIco(this,'" + BasePath + "')\" src='" + BasePath + "/editor/images/unlink.gif' /></div>"); //取消超级链接
                }
                if (ToolBarCheck("RemoveFormat"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><img class='toolbar_ico' id='RemoveFormat' onmouseover='deditorClass.OverIco(this)' title='" + Language.Get("EditorRemoveFormats") + "' onclick=\"deditorClass.RemoveFormats('" + EditorID + "')\" onmouseout=\"deditorClass.OutIco(this,'" + BasePath + "')\" src='" + BasePath + "/editor/images/removeformat.gif' /></div>"); //清除格式
                }
                if (ToolBarCheck("Smiley"))
                {
                    innerHTML.Append("      <div class='toolbar_div' id='DivInsertSmiley'><img class='toolbar_ico' id='InsertSmiley' onmouseover='deditorClass.OverIco(this)' title='" + Language.Get("EditorInsertSmiley") + "' onclick=\"deditorClass.InsertTypeData(this,'InsertSmiley',134,'" + BasePath + "','" + EditorID + "')\" onmouseout=\"deditorClass.OutIco(this,'" + BasePath + "')\" src='" + BasePath + "/editor/images/smiley.gif' /></div>"); //插入表情
                }
                if (ToolBarCheck("FontName"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><div class='toolbar_text'><select id='dropFontName' title='" + Language.Get("EditorSetFontName") + "' onchange=\"deditorClass.SetFontName('" + EditorID + "')\"><option value='宋体'>" + Language.Get("EditorFontName") + "</option><option value='宋体'>宋体</option><option value='楷体_GB2312'>楷体</option><option value='新宋体'>新宋体</option><option value='黑体'>黑体</option><option value='隶书'>隶书</option><option value='Andale Mono'>Andale Mono</option><option value='Arial'>Arial</option><option value='Arial Black'>Arial Black</option><option value='Book Antiqua'>Book Antiqua</option><option value='Century Gothic'>Century Gothic</option><option value='Comic Sans MS'>Comic Sans MS</option><option value='Courier New'>Courier New</option><option value='Georgia'>Georgia</option><option value='Impact'>Impact</option><option value='Tahoma'>Tahoma</option><option value='Times New Roman' >Times New Roman</option><option value='Trebuchet MS'>Trebuchet MS</option><option value='Script MT Bold'>Script MT Bold</option><option value='Stencil'>Stencil</option><option value='Verdana'>Verdana</option><option value='Lucida Console'>Lucida Console</option></select></div></div>"); //字体名称
                }
                if (ToolBarCheck("FontSize"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><div class='toolbar_text'><select id='dropFontSize' title='" + Language.Get("EditorSetFontSize") + "' onchange=\"deditorClass.SetFontSize('" + EditorID + "')\"><option value='2'>" + Language.Get("EditorFontSize") + "</option><option value='1'>" + Language.Get("EditorSetFontSize1") + "</option><option value='2'>" + Language.Get("EditorSetFontSize2") + "</option><option value='3'>" + Language.Get("EditorSetFontSize3") + "</option><option value='4'>" + Language.Get("EditorSetFontSize4") + "</option><option value='5'>" + Language.Get("EditorSetFontSize5") + "</option><option value='6'>" + Language.Get("EditorSetFontSize6") + "</option><option value='7'>" + Language.Get("EditorSetFontSize7") + "</option></select></div></div>"); //文字大小
                }
                if (ToolBarCheck("FontColor"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><div class='toolbar_text'><select id='dropFontColor' title='" + Language.Get("EditorSetFontColor") + "' onchange=\"deditorClass.SetFontColor('" + EditorID + "')\" ><option value='Black'>" + Language.Get("EditorFontColor") + "</option><option style='color: black; background-color: black' value='Black'>Black</option><option style='color: red; background-color: red' value='Red'>Red</option><option style='color: yellow; background-color: yellow' value='Yellow'>Yellow</option><option style='color: pink; background-color: pink' value='Pink'>Pink</option><option style='color: green; background-color: green' value='Green'>Green</option><option style='color: orange; background-color: orange' value='Orange'>Orange</option><option style='color: purple; background-color: purple' value='Purple'>Purple</option><option style='color: blue; background-color: blue' value='Blue'>Blue</option><option style='color: beige; background-color: beige' value='Beige'>Beige</option><option style='color: brown; background-color: brown' value='Brown'>Brown</option><option style='color: teal; background-color: teal' value='Teal'>Teal</option><option style='color: navy; background-color: navy' value='Navy'>Navy</option><option style='color: maroon; background-color: maroon' value='Maroon'>Maroon</option><option style='color: limegreen; background-color: limegreen' value='LimeGreen'>LimeGreen</option></select></div></div>"); //文字颜色
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
            {//不支持Script脚本
                #region

                string strContentText = _Text;   //初始原编辑器文本区值
                string txtText = "";    //初始工具栏返回文本值
                string CommandName = "";    //初始工具命令
                string Tool_CommandName = "";   //初始工具内部产生的命令
                string strFileUrl = "";
                int intTextWidth;
                int intTextHeight;
                string Tool_InsertSmiley = "";
                string Mode_CommandName = "";

                //取得编辑器原文本区值
                if (QueryString(EditorID) != "")
                {
                    strContentText = QueryString(EditorID).ToString();
                }

                //取得工具命令
                if (QueryString("CommandName") != "")
                {
                    iHeight = iHeight - 24; //编辑器文本区高度(-32px工具栏高度)

                    CommandName = QueryString("CommandName").ToString();   //取得工具命令名称
                }

                //根据不同的工具命令，指定txtText工具命令值
                if (QueryString("Tool_CommandName") != "")
                {
                    Tool_CommandName = QueryString("Tool_CommandName");   //取得工具内部产生的命令名称

                    txtText = QueryString("txtText");

                    switch (Tool_CommandName)
                    {
                        case "FontBold":    //加粗文本
                            txtText = "<B>" + txtText + "</B>";
                            break;
                        case "FontItalic":  //斜体文本
                            txtText = "<EM>" + txtText + "</EM>";
                            break;
                        case "UnderLine":   //下划线文本
                            txtText = "<U>" + txtText + "</U>";
                            break;
                        case "InsertImage":   //上传图片文件
                            strFileUrl = Uploader.UploadFile(HttpContext.Current.Request.Files["UpFile"], 1);
                            txtText = "<IMG src='" + strFileUrl + "' border='0' />";
                            break;
                        case "InsertFlash":   //上传Flash文件
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
                        case "InsertLink":   //插入链接
                            if (txtText == "")
                            {
                                txtText = "<A href='" + QueryString("txtTextUrl") + "'>" + QueryString("txtTextUrl") + "</A>";
                            }
                            else
                            {
                                txtText = "<A href='" + QueryString("txtTextUrl") + "' target='_blank'>" + txtText + "</A>";
                            }
                            break;
                        case "RemoveFormat":   //清除格式
                            strContentText = System.Text.RegularExpressions.Regex.Replace(strContentText, "<B>|</B>|<EM>|</EM>|<U>|</U>|<FONT(.*?)>|</FONT>", "");
                            break;
                        case "FontName":  //字体名称
                            txtText = "<FONT face='" + QueryString("dropFontName") + "'>" + txtText + "</FONT>";
                            break;
                        case "FontSize":  //字体大小
                            txtText = "<FONT size='" + QueryString("dropFontSize") + "'>" + txtText + "</FONT>";
                            break;
                        case "FontColor":  //字体颜色
                            txtText = "<FONT color='" + QueryString("dropFontColor") + "'>" + txtText + "</FONT>";
                            break;
                    }
                }
                else if (QueryString("Tool_InsertSmiley") != "")   //插入表情
                {
                    Tool_InsertSmiley = QueryString("Tool_InsertSmiley");   //取得表情代号
                    txtText = "<IMG src='" + BasePath + "Smiley/" + Tool_InsertSmiley + ".gif' border='0' />";
                }
                else if (QueryString("Mode_CommandName") != "")
                {
                    Mode_CommandName = QueryString("Mode_CommandName");
                }
                //Code Fixed By Mao
                strContentText = strContentText + txtText;  //编辑器文本区内容等于原文本值加上工具命令产生的值

                //stringBuilder.Append("<form id='" + EditorID + "_form' action='" + PageUrl + "' method='post' enctype='multipart/form-data'>");
                innerHTML.Append("<div id='Editor_Div' class='deditor' style='width:" + Width + "px; height:" + Height + "px;'>");
                innerHTML.Append("  <div class='toolbar' id='toolbar' style='background:url(" + BasePath + "/editor/images/toolbar_bg.gif) repeat-x'>");
                innerHTML.Append("      <div class='toolbar_start'><img src='" + BasePath + "/editor/images/start.gif' /></div>");
                if (ToolBarCheck("FontBold"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><input type='submit' id='CommandName' name='CommandName' value='FontBold' class='toolbar_ico2' style='background:url(" + BasePath + "/editor/images/bold.gif) repeat-x; width:23px; height:23px; cursor:pointer;' /></div>");  //粗体
                }
                if (ToolBarCheck("Italic"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><input type='submit' id='CommandName' name='CommandName' value='FontItalic' class='toolbar_ico2' style='background:url(" + BasePath + "/editor/images/italic.gif) repeat-x; width:23px; height:23px; cursor:pointer;' /></div>");    //斜体
                }
                if (ToolBarCheck("UnderLine"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><input type='submit' id='CommandName' name='CommandName' value='UnderLine' class='toolbar_ico2' style='background:url(" + BasePath + "/editor/images/underline.gif) repeat-x; width:23px; height:23px; cursor:pointer;' /></div>"); //下划线
                }
                if (ToolBarCheck("Image"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><input type='submit' id='CommandName' name='CommandName' value='InsertImage' class='toolbar_ico2' style='background:url(" + BasePath + "/editor/images/image.gif) repeat-x; width:23px; height:23px; cursor:pointer;' /></div>"); //插入图片
                }
                if (ToolBarCheck("Flash"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><input type='submit' id='CommandName' name='CommandName' value='InsertFlash' class='toolbar_ico2' style='background:url(" + BasePath + "/editor/images/flash.gif) repeat-x; width:23px; height:23px; cursor:pointer;' /></div>"); //插入Flash
                }
                if (ToolBarCheck("Link"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><input type='submit' id='CommandName' name='CommandName' value='InsertLink' class='toolbar_ico2' style='background:url(" + BasePath + "/editor/images/link.gif) repeat-x; width:23px; height:23px; cursor:pointer;' /></div>"); //插入超级链接
                }
                if (ToolBarCheck("Unlink"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><img class='toolbar_ico' id='InsertUnlink' title='" + Language.Get("EditorInsertUnlink") + "' src='" + BasePath + "/editor/images/unlink2.gif' /></div>"); //取消超级链接
                }
                if (ToolBarCheck("RemoveFormat"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><input type='submit' id='Tool_CommandName' name='Tool_CommandName' value='RemoveFormat' class='toolbar_ico2' style='background:url(" + BasePath + "/editor/images/removeformat.gif) repeat-x; width:23px; height:23px; cursor:pointer;' /></div>"); //清除格式
                }
                if (ToolBarCheck("Smiley"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><input type='submit' id='CommandName' name='CommandName' value='InsertSmiley' class='toolbar_ico2' style='background:url(" + BasePath + "/editor/images/smiley.gif) repeat-x; width:23px; height:23px; cursor:pointer;' /></div>"); //插入表情
                }
                if (ToolBarCheck("FontName"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><input type='submit' id='CommandName' name='CommandName' value='FontName' class='toolbar_ico2' style='background:url(" + BasePath + "/editor/images/fontname.gif) repeat-x; width:108px; height:23px; cursor:pointer;' /></div>"); //字体名称
                }
                if (ToolBarCheck("FontSize"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><input type='submit' id='CommandName' name='CommandName' value='FontSize' class='toolbar_ico2' style='background:url(" + BasePath + "/editor/images/fontsize.gif) repeat-x; width:65px; height:23px; cursor:pointer;' /></div>"); //文字大小
                }
                if (ToolBarCheck("FontColor"))
                {
                    innerHTML.Append("      <div class='toolbar_div'><input type='submit' id='CommandName' name='CommandName' value='FontColor' class='toolbar_ico2' style='background:url(" + BasePath + "/editor/images/fontcolor.gif) repeat-x; width:83px; height:23px; cursor:pointer;' /></div>"); //文字颜色
                }
                innerHTML.Append("  </div>");
                innerHTML.Append("  <div class='contenttext'>");

                //是否显示工具栏命令项按扭
                if (CommandName != "")  //如不为空则显示工具栏命令项按扭
                {
                    innerHTML.Append("  <div class='toolbarbg'>");
                    innerHTML.Append(ToolBar.LoadToolBar(CommandName, BasePath));
                    innerHTML.Append("  </div>");
                }

                //显示各种模式
                if (Mode_CommandName == "ModeDesign" || Mode_CommandName == "ModeHtml")
                {//设计模式及HTML源码格式
                    if (ToolBarCheck("HtmlCode") || ToolBarCheck("Design"))
                    {
                        innerHTML.Append("      <textarea id='" + EditorID + "' name='" + EditorID + "' style='border:0px; width:" + iWidth + "px; height:" + iHeight + "px; left:0px; right:0px;'>" + strContentText + "</textarea>");
                    }
                }
                else if (Mode_CommandName == "ModePreview")
                {//预览模式 
                    if (ToolBarCheck("Preview"))
                    {
                        innerHTML.Append("      <div style='border:0px; width:" + iWidth + "px; height:" + iHeight + "px; float:left; overflow: scroll;'>" + strContentText + "</div>");
                        innerHTML.Append("      <textarea id='" + EditorID + "' name='" + EditorID + "' style='border:0px; width:0px; height:0px; left:0px; right:0px; display:none'>" + strContentText + "</textarea>");
                    }
                }
                else
                {//设计模式
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


                //stringBuilder.Append("  <div class='toolbar_mode_c'><img src='" + BasePath + "/editor/images/minus.gif' border='0' align='absmiddle' title='缩小输入框' /></div>");
                //stringBuilder.Append("  <div class='toolbar_mode_c'><img src='" + BasePath + "/editor/images/plus.gif' border='0' align='absmiddle' title='增大输入框' /></div>");

                innerHTML.Append("</div>");
                //stringBuilder.Append("</form>");

                #endregion
            }

            return innerHTML.ToString();
        }

        /// <summary>
        /// 检查当前浏览器是否支持Script脚本
        /// </summary>
        /// <param name="GoUrl">跳转的URL地址</param>
        /// <returns></returns>
        public bool ScriptCheck(string sourceUrl)
        {
            if (!sourceUrl.Contains("nojs"))//如果不能获取nojs值，说明未检查过
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
        /// 动态生成并加载用与存取内容的TextBox控件txtContentText
        /// </summary>
        /// <param name="control"></param>
        public void ControlsAdd(Control control)
        {
            txtContentText.ID = "txtContentText";
            control.Controls.Add(txtContentText);
        }

        /// <summary>
        /// 返回指定参数的值，如为空时，返回""
        /// </summary>
        /// <param name="para">参数名</param>
        /// <returns></returns>
        private string QueryString(string para)
        {
            return HttpContext.Current.Request[para] != null ? HttpContext.Current.Request[para].Trim() : "";
        }


        /// <summary>
        /// 根据工具名称，返回列表中是否存在
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
