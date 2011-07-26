<%@ Page Title="" Language="C#" MasterPageFile="~/fnadmin/Shared/Admin.master" AutoEventWireup="true"
    CodeFile="ArticleEdit.aspx.cs" Inherits="fnadmin_ArticleEdit" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <table class="tableBorder" border="0" cellspacing="0" cellpadding="5">
        <tr>
            <th colspan="2" class="titleTd">
                发布文章
            </th>
        </tr>
        <tr>
            <td width="100" align="center">
                标题：
            </td>
            <td>
                <input id="txtTitle" runat="server" type="text" class="tbox tboxL" onfocus="onTBox(this)"
                    onblur="outTBox(this)" />
            </td>
        </tr>
        <tr>
            <td align="center">
                分类：
            </td>
            <td>
                <select id="selCategory" runat="server" class="sel" style="width: 150px;" onfocus="onTBox(this)"
                    onblur="outTBox(this)">
                </select>
            </td>
        </tr>
        <%--<tr>
            <td align="center">
                展示图片：
            </td>
            <td>
                <img id="imgArticle" runat="server" src="~/resource/image/admin/img.gif" alt="图片展示"
                    width="160" height="120" />
            </td>
        </tr>
        <tr>
            <td align="center">
                上传图片：
            </td>
            <td>
                <input id="hidImg" type="hidden" runat="server" />
                <asp:FileUpload ID="fileUploadImg" runat="server" onchange="previewImage()" onfocus="onTBox(this)"
                    onblur="outTBox(this)" />
            </td>
        </tr>--%>
        <tr>
            <td align="center">
                内容：
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <textarea cols="60" id="content" name="content"><%=contentVal %></textarea>

                <script type="text/javascript" src="ckeditor/ckeditor.js"></script>

                <script type="text/javascript">
                    CKEDITOR.replace('content');
                </script>

            </td>
        </tr>
        <%--<tr>
            <td align="center">
                关键字：
            </td>
            <td>
                <input id="txtKey" class="tbox tboxL" runat="server" type="text" onfocus="onTBox(this)"
                    onblur="outTBox(this)" />
            </td>
        </tr>
        <tr>
            <td align="center">
                来源：
            </td>
            <td>
                <input id="txtSource" class="tbox tboxL" runat="server" type="text" onfocus="onTBox(this)"
                    onblur="outTBox(this)" />
            </td>
        </tr>
        <tr>
            <td align="center">
                作者：
            </td>
            <td>
                <input id="txtAuthor" class="tbox tboxL" runat="server" type="text" onfocus="onTBox(this)"
                    onblur="outTBox(this)" />
            </td>
        </tr>--%>
        <tr>
            <td colspan="2" align="center">
                <input id="btnSubmit" runat="server" type="submit" value="提交" onclick="return checkInput();"
                    onserverclick="BtnSubmit_Click" />&nbsp;&nbsp;
                <input id="Button1" type="button" value="返回" onclick="javascript:location.href='ArticleList.aspx';" />
            </td>
        </tr>
    </table>

    <script type="text/javascript">
        

        function trim(str) {
            return str.replace(/(^\s*)|(\s*$)/g, "");
        } // 去掉空格
        function checkInput() {
            if ($("<%=txtTitle.ClientID %>").value == "") { alert("请输入标题"); return false; }
            if ($("<%=selCategory.ClientID %>").value == "") { alert("请选择分类"); return false; }

            var reg = new RegExp("<(S*?)[^>]*>.*?|< .*? />");

            if (reg.test($("<%=txtTitle.ClientID %>").value)) { alert("含非法字符,请检查输入"); return false; }

//            var str = CKEDITOR.instances.content.getData();
//            str = str.replace("<br />", "");
//            str = str.replace("<br>", "");
//            str = trim(str);
//            if (str == "") { alert("请输入文章中文内容"); return false; }
            return true;
        }
    </script>

</asp:Content>
