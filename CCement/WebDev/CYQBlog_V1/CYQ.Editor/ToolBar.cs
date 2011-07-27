using System;

namespace CYQ.Editor
{
    public class ToolBar
    {
        /// <summary>
        /// 载入不同的工具条内容
        /// </summary>
        /// <param name="CommandName">类型名称</param>
        public static string LoadToolBar(string commandName, string basePath)
        {
            string HtmlText = "";
            switch (commandName)
            {
                case "FontBold":
                    HtmlText = FontBoldToolBar(basePath);
                    break;
                case "FontItalic":
                    HtmlText = FontItalicToolBar(basePath);
                    break;
                case "UnderLine":
                    HtmlText = UnderLineToolBar(basePath);
                    break;
                case "InsertImage":
                    HtmlText = InsertImageToolBar(basePath);
                    break;
                case "InsertFlash":
                    HtmlText = InsertFlashToolBar(basePath);
                    break;
                case "InsertLink":
                    HtmlText = InsertLinkToolBar(basePath);
                    break;
                case "InsertSmiley":
                    HtmlText = InsertSmileyToolBar(basePath);
                    break;
                case "FontName":
                    HtmlText = FontNameToolBar(basePath);
                    break;
                case "FontSize":
                    HtmlText = FontSizeToolBar(basePath);
                    break;
                case "FontColor":
                    HtmlText = FontColorToolBar(basePath);
                    break;
            }

            return HtmlText;
        }

        /// <summary>
        /// 加粗文本工具
        /// </summary>
        /// <returns></returns>
        private static string FontBoldToolBar(string basePath)
        {
            return "&nbsp;请输入要加粗的文本：<input type='text' name='txtText' id='txtText' class='txtInput' style='width:150px;' />&nbsp;<input type='submit' id='Tool_CommandName' name='Tool_CommandName' class='toolbar_btn' style='background:url(" + basePath + "/editor/images/btnOk.gif) repeat-x; width:40px; height:19px;' value='FontBold' />";
        }


        /// <summary>
        /// 斜体文本工具
        /// </summary>
        /// <returns></returns>
        private static string FontItalicToolBar(string basePath)
        {
            return "&nbsp;请输入要斜体的文本：<input type='text' name='txtText' id='txtText' class='txtInput' style='width:150px;' />&nbsp;<input type='submit' id='Tool_CommandName' name='Tool_CommandName' class='toolbar_btn' style='background:url(" + basePath + "/editor/images/btnOk.gif) repeat-x; width:40px; height:19px;' value='FontItalic' />";
        }

        /// <summary>
        /// 下划线文本工具
        /// </summary>
        /// <returns></returns>
        private static string UnderLineToolBar(string basePath)
        {
            return "&nbsp;请输入要下划线的文本：<input type='text' name='txtText' id='txtText' class='txtInput' style='width:150px;' />&nbsp;<input type='submit' id='Tool_CommandName' name='Tool_CommandName' class='toolbar_btn' style='background:url(" + basePath + "/editor/images/btnOk.gif) repeat-x; width:40px; height:19px;' value='UnderLine' />";
        }

        /// <summary>
        /// 上传图片工具
        /// </summary>
        /// <returns></returns>
        private static string InsertImageToolBar(string basePath)
        {
            return "&nbsp;插入图片：<input type='file' name='UpFile' id='UpFile' class='txtInput' />&nbsp;<input type='submit' id='Tool_CommandName' name='Tool_CommandName' class='toolbar_btn' style='background:url(" + basePath + "/editor/images/btnOk.gif) repeat-x; width:40px; height:19px;' value='InsertImage' />";
        }

        /// <summary>
        /// 上传Flash工具
        /// </summary>
        /// <returns></returns>
        private static string InsertFlashToolBar(string basePath)
        {
            return "&nbsp;插入Flash：<input type='file' name='UpFile' id='UpFile' class='txtInput' />&nbsp;宽：<input type='text' name='txtTextWidth' id='txtTextWidth' class='txtInput' style='width:50px;' />&nbsp;高：<input type='text' name='txtTextHeight' id='txtTextHeight' class='txtInput' style='width:50px;' />&nbsp;<input type='submit' id='Tool_CommandName' name='Tool_CommandName' class='toolbar_btn' style='background:url(" + basePath + "/editor/images/btnOk.gif) repeat-x; width:40px; height:19px;' value='InsertFlash' />";
        }

        /// <summary>
        /// 插入链接工具
        /// </summary>
        /// <returns></returns>
        private static string InsertLinkToolBar(string basePath)
        {
            return "&nbsp;链接文字：<input type='text' name='txtText' id='txtText' class='txtInput' style='width:150px;' />&nbsp;链接地址：<input type='text' name='txtTextUrl' id='txtTextUrl' class='txtInput' style='width:150px;' />&nbsp;<input type='submit' id='Tool_CommandName' name='Tool_CommandName' class='toolbar_btn' style='background:url(" + basePath + "/editor/images/btnOk.gif) repeat-x; width:40px; height:19px;' value='InsertLink' />";
        }

        /// <summary>
        /// 插入表情
        /// </summary>
        /// <returns></returns>
        private static string InsertSmileyToolBar(string basePath)
        {
            return "&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/1.gif) no-repeat; width:21px; height:21px;' value='1' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/2.gif) no-repeat; width:21px; height:21px;' value='2' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/3.gif) no-repeat; width:21px; height:21px;' value='3' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/4.gif) no-repeat; width:21px; height:21px;' value='4' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/5.gif) no-repeat; width:21px; height:21px;' value='5' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/6.gif) no-repeat; width:21px; height:21px;' value='6' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/7.gif) no-repeat; width:21px; height:21px;' value='7' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/8.gif) no-repeat; width:21px; height:21px;' value='8' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/9.gif) no-repeat; width:21px; height:21px;' value='9' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/10.gif) no-repeat; width:21px; height:21px;' value='10' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/11.gif) no-repeat; width:21px; height:21px;' value='11' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/12.gif) no-repeat; width:21px; height:21px;' value='12' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/13.gif) no-repeat; width:21px; height:21px;' value='13' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/14.gif) no-repeat; width:21px; height:21px;' value='14' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/15.gif) no-repeat; width:21px; height:21px;' value='15' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/16.gif) no-repeat; width:21px; height:21px;' value='16' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/17.gif) no-repeat; width:21px; height:21px;' value='17' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/18.gif) no-repeat; width:21px; height:21px;' value='18' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/19.gif) no-repeat; width:21px; height:21px;' value='19' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/20.gif) no-repeat; width:21px; height:21px;' value='20' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/21.gif) no-repeat; width:21px; height:21px;' value='21' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/22.gif) no-repeat; width:21px; height:21px;' value='22' />";
        }

        /// <summary>
        /// 设置文本字体名称
        /// </summary>
        /// <returns></returns>
        private static string FontNameToolBar(string basePath)
        {
            return "&nbsp;<select id='dropFontName' name='dropFontName' title='选择字体' value='宋体'><option value='宋体'>字体</option><option value='宋体'>宋体</option><option value='楷体_GB2312'>楷体</option><option value='新宋体'>新宋体</option><option value='黑体'>黑体</option><option value='隶书'>隶书</option><option value='Andale Mono'>Andale Mono</option><option value='Arial'>Arial</option><option value='Arial Black'>Arial Black</option><option value='Book Antiqua'>Book Antiqua</option><option value='Century Gothic'>Century Gothic</option><option value='Comic Sans MS'>Comic Sans MS</option><option value='Courier New'>Courier New</option><option value='Georgia'>Georgia</option><option value='Impact'>Impact</option><option value='Tahoma'>Tahoma</option><option value='Times New Roman' >Times New Roman</option><option value='Trebuchet MS'>Trebuchet MS</option><option value='Script MT Bold'>Script MT Bold</option><option value='Stencil'>Stencil</option><option value='Verdana'>Verdana</option><option value='Lucida Console'>Lucida Console</option></select>&nbsp;文本：<input type='text' name='txtText' id='txtText' class='txtInput' style='width:150px;' />&nbsp;<input type='submit' id='Tool_CommandName' name='Tool_CommandName' class='toolbar_btn' style='background:url(" + basePath + "/editor/images/btnOk.gif) repeat-x; width:40px; height:19px;' value='FontName' />";
        }


        /// <summary>
        /// 设置文本字体大小
        /// </summary>
        /// <returns></returns>
        private static string FontSizeToolBar(string basePath)
        {
            return "&nbsp;<select id='dropFontSize' name='dropFontSize' title='请选择字体大小'><option value='2'>大小</option><option value='1'>一号</option><option value='2'>二号</option><option value='3'>三号</option><option value='4'>四号</option><option value='5'>五号</option><option value='6'>六号</option><option value='7'>七号</option></select>&nbsp;文本：<input type='text' name='txtText' id='txtText' class='txtInput' style='width:150px;' />&nbsp;<input type='submit' id='Tool_CommandName' name='Tool_CommandName' class='toolbar_btn' style='background:url(" + basePath + "/editor/images/btnOk.gif) repeat-x; width:40px; height:19px;' value='FontSize' />";
        }

        /// <summary>
        /// 设置文本字体颜色
        /// </summary>
        /// <returns></returns>
        private static string FontColorToolBar(string basePath)
        {
            return "&nbsp;<select id='dropFontColor' name='dropFontColor' title='字体颜色'><option value='Black'>颜色</option><option style='color: black; background-color: black' value='Black'>Black</option><option style='color: red; background-color: red' value='Red'>Red</option><option style='color: yellow; background-color: yellow' value='Yellow'>Yellow</option><option style='color: pink; background-color: pink' value='Pink'>Pink</option><option style='color: green; background-color: green' value='Green'>Green</option><option style='color: orange; background-color: orange' value='Orange'>Orange</option><option style='color: purple; background-color: purple' value='Purple'>Purple</option><option style='color: blue; background-color: blue' value='Blue'>Blue</option><option style='color: beige; background-color: beige' value='Beige'>Beige</option><option style='color: brown; background-color: brown' value='Brown'>Brown</option><option style='color: teal; background-color: teal' value='Teal'>Teal</option><option style='color: navy; background-color: navy' value='Navy'>Navy</option><option style='color: maroon; background-color: maroon' value='Maroon'>Maroon</option><option style='color: limegreen; background-color: limegreen' value='LimeGreen'>LimeGreen</option></select>&nbsp;文本：<input type='text' name='txtText' id='txtText' class='txtInput' style='width:150px;' />&nbsp;<input type='submit' id='Tool_CommandName' name='Tool_CommandName' class='toolbar_btn' style='background:url(" + basePath + "/editor/images/btnOk.gif) repeat-x; width:40px; height:19px;' value='FontColor' />";
        }
    }
}
