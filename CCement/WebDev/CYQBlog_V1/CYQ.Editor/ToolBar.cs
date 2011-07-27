using System;

namespace CYQ.Editor
{
    public class ToolBar
    {
        /// <summary>
        /// ���벻ͬ�Ĺ���������
        /// </summary>
        /// <param name="CommandName">��������</param>
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
        /// �Ӵ��ı�����
        /// </summary>
        /// <returns></returns>
        private static string FontBoldToolBar(string basePath)
        {
            return "&nbsp;������Ҫ�Ӵֵ��ı���<input type='text' name='txtText' id='txtText' class='txtInput' style='width:150px;' />&nbsp;<input type='submit' id='Tool_CommandName' name='Tool_CommandName' class='toolbar_btn' style='background:url(" + basePath + "/editor/images/btnOk.gif) repeat-x; width:40px; height:19px;' value='FontBold' />";
        }


        /// <summary>
        /// б���ı�����
        /// </summary>
        /// <returns></returns>
        private static string FontItalicToolBar(string basePath)
        {
            return "&nbsp;������Ҫб����ı���<input type='text' name='txtText' id='txtText' class='txtInput' style='width:150px;' />&nbsp;<input type='submit' id='Tool_CommandName' name='Tool_CommandName' class='toolbar_btn' style='background:url(" + basePath + "/editor/images/btnOk.gif) repeat-x; width:40px; height:19px;' value='FontItalic' />";
        }

        /// <summary>
        /// �»����ı�����
        /// </summary>
        /// <returns></returns>
        private static string UnderLineToolBar(string basePath)
        {
            return "&nbsp;������Ҫ�»��ߵ��ı���<input type='text' name='txtText' id='txtText' class='txtInput' style='width:150px;' />&nbsp;<input type='submit' id='Tool_CommandName' name='Tool_CommandName' class='toolbar_btn' style='background:url(" + basePath + "/editor/images/btnOk.gif) repeat-x; width:40px; height:19px;' value='UnderLine' />";
        }

        /// <summary>
        /// �ϴ�ͼƬ����
        /// </summary>
        /// <returns></returns>
        private static string InsertImageToolBar(string basePath)
        {
            return "&nbsp;����ͼƬ��<input type='file' name='UpFile' id='UpFile' class='txtInput' />&nbsp;<input type='submit' id='Tool_CommandName' name='Tool_CommandName' class='toolbar_btn' style='background:url(" + basePath + "/editor/images/btnOk.gif) repeat-x; width:40px; height:19px;' value='InsertImage' />";
        }

        /// <summary>
        /// �ϴ�Flash����
        /// </summary>
        /// <returns></returns>
        private static string InsertFlashToolBar(string basePath)
        {
            return "&nbsp;����Flash��<input type='file' name='UpFile' id='UpFile' class='txtInput' />&nbsp;��<input type='text' name='txtTextWidth' id='txtTextWidth' class='txtInput' style='width:50px;' />&nbsp;�ߣ�<input type='text' name='txtTextHeight' id='txtTextHeight' class='txtInput' style='width:50px;' />&nbsp;<input type='submit' id='Tool_CommandName' name='Tool_CommandName' class='toolbar_btn' style='background:url(" + basePath + "/editor/images/btnOk.gif) repeat-x; width:40px; height:19px;' value='InsertFlash' />";
        }

        /// <summary>
        /// �������ӹ���
        /// </summary>
        /// <returns></returns>
        private static string InsertLinkToolBar(string basePath)
        {
            return "&nbsp;�������֣�<input type='text' name='txtText' id='txtText' class='txtInput' style='width:150px;' />&nbsp;���ӵ�ַ��<input type='text' name='txtTextUrl' id='txtTextUrl' class='txtInput' style='width:150px;' />&nbsp;<input type='submit' id='Tool_CommandName' name='Tool_CommandName' class='toolbar_btn' style='background:url(" + basePath + "/editor/images/btnOk.gif) repeat-x; width:40px; height:19px;' value='InsertLink' />";
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        private static string InsertSmileyToolBar(string basePath)
        {
            return "&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/1.gif) no-repeat; width:21px; height:21px;' value='1' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/2.gif) no-repeat; width:21px; height:21px;' value='2' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/3.gif) no-repeat; width:21px; height:21px;' value='3' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/4.gif) no-repeat; width:21px; height:21px;' value='4' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/5.gif) no-repeat; width:21px; height:21px;' value='5' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/6.gif) no-repeat; width:21px; height:21px;' value='6' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/7.gif) no-repeat; width:21px; height:21px;' value='7' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/8.gif) no-repeat; width:21px; height:21px;' value='8' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/9.gif) no-repeat; width:21px; height:21px;' value='9' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/10.gif) no-repeat; width:21px; height:21px;' value='10' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/11.gif) no-repeat; width:21px; height:21px;' value='11' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/12.gif) no-repeat; width:21px; height:21px;' value='12' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/13.gif) no-repeat; width:21px; height:21px;' value='13' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/14.gif) no-repeat; width:21px; height:21px;' value='14' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/15.gif) no-repeat; width:21px; height:21px;' value='15' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/16.gif) no-repeat; width:21px; height:21px;' value='16' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/17.gif) no-repeat; width:21px; height:21px;' value='17' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/18.gif) no-repeat; width:21px; height:21px;' value='18' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/19.gif) no-repeat; width:21px; height:21px;' value='19' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/20.gif) no-repeat; width:21px; height:21px;' value='20' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/21.gif) no-repeat; width:21px; height:21px;' value='21' />&nbsp;<input type='submit' id='Tool_InsertSmiley' name='Tool_InsertSmiley' class='toolbar_btnSmiley' style='background:url(" + basePath + "/editor/smiley/22.gif) no-repeat; width:21px; height:21px;' value='22' />";
        }

        /// <summary>
        /// �����ı���������
        /// </summary>
        /// <returns></returns>
        private static string FontNameToolBar(string basePath)
        {
            return "&nbsp;<select id='dropFontName' name='dropFontName' title='ѡ������' value='����'><option value='����'>����</option><option value='����'>����</option><option value='����_GB2312'>����</option><option value='������'>������</option><option value='����'>����</option><option value='����'>����</option><option value='Andale Mono'>Andale Mono</option><option value='Arial'>Arial</option><option value='Arial Black'>Arial Black</option><option value='Book Antiqua'>Book Antiqua</option><option value='Century Gothic'>Century Gothic</option><option value='Comic Sans MS'>Comic Sans MS</option><option value='Courier New'>Courier New</option><option value='Georgia'>Georgia</option><option value='Impact'>Impact</option><option value='Tahoma'>Tahoma</option><option value='Times New Roman' >Times New Roman</option><option value='Trebuchet MS'>Trebuchet MS</option><option value='Script MT Bold'>Script MT Bold</option><option value='Stencil'>Stencil</option><option value='Verdana'>Verdana</option><option value='Lucida Console'>Lucida Console</option></select>&nbsp;�ı���<input type='text' name='txtText' id='txtText' class='txtInput' style='width:150px;' />&nbsp;<input type='submit' id='Tool_CommandName' name='Tool_CommandName' class='toolbar_btn' style='background:url(" + basePath + "/editor/images/btnOk.gif) repeat-x; width:40px; height:19px;' value='FontName' />";
        }


        /// <summary>
        /// �����ı������С
        /// </summary>
        /// <returns></returns>
        private static string FontSizeToolBar(string basePath)
        {
            return "&nbsp;<select id='dropFontSize' name='dropFontSize' title='��ѡ�������С'><option value='2'>��С</option><option value='1'>һ��</option><option value='2'>����</option><option value='3'>����</option><option value='4'>�ĺ�</option><option value='5'>���</option><option value='6'>����</option><option value='7'>�ߺ�</option></select>&nbsp;�ı���<input type='text' name='txtText' id='txtText' class='txtInput' style='width:150px;' />&nbsp;<input type='submit' id='Tool_CommandName' name='Tool_CommandName' class='toolbar_btn' style='background:url(" + basePath + "/editor/images/btnOk.gif) repeat-x; width:40px; height:19px;' value='FontSize' />";
        }

        /// <summary>
        /// �����ı�������ɫ
        /// </summary>
        /// <returns></returns>
        private static string FontColorToolBar(string basePath)
        {
            return "&nbsp;<select id='dropFontColor' name='dropFontColor' title='������ɫ'><option value='Black'>��ɫ</option><option style='color: black; background-color: black' value='Black'>Black</option><option style='color: red; background-color: red' value='Red'>Red</option><option style='color: yellow; background-color: yellow' value='Yellow'>Yellow</option><option style='color: pink; background-color: pink' value='Pink'>Pink</option><option style='color: green; background-color: green' value='Green'>Green</option><option style='color: orange; background-color: orange' value='Orange'>Orange</option><option style='color: purple; background-color: purple' value='Purple'>Purple</option><option style='color: blue; background-color: blue' value='Blue'>Blue</option><option style='color: beige; background-color: beige' value='Beige'>Beige</option><option style='color: brown; background-color: brown' value='Brown'>Brown</option><option style='color: teal; background-color: teal' value='Teal'>Teal</option><option style='color: navy; background-color: navy' value='Navy'>Navy</option><option style='color: maroon; background-color: maroon' value='Maroon'>Maroon</option><option style='color: limegreen; background-color: limegreen' value='LimeGreen'>LimeGreen</option></select>&nbsp;�ı���<input type='text' name='txtText' id='txtText' class='txtInput' style='width:150px;' />&nbsp;<input type='submit' id='Tool_CommandName' name='Tool_CommandName' class='toolbar_btn' style='background:url(" + basePath + "/editor/images/btnOk.gif) repeat-x; width:40px; height:19px;' value='FontColor' />";
        }
    }
}
