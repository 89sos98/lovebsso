<%@ Page Title="" Language="C#" MasterPageFile="~/fnadmin/Shared/Admin.master" AutoEventWireup="true"
    CodeFile="ProductEdit.aspx.cs" Inherits="fnadmin_ProductEdit" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <table class="tableBorder" border="0" cellspacing="0" cellpadding="5">
        <tr>
            <th colspan="2" class="titleTd">
                发布产品
            </th>
        </tr>
        <tr>
            <td width="100">
                产品名称：
            </td>
            <td>
                <input id="txtPname" runat="server" type="text" class="tbox tboxL" onfocus="onTBox(this)"
                    onblur="outTBox(this)" />
            </td>
        </tr>
        <tr>
            <td>
                产品系列：
            </td>
            <td>
                <select id="selCategory" runat="server" style="width: 150px;" class="sel" onfocus="onTBox(this)"
                    onblur="outTBox(this)">
                </select>
            </td>
        </tr>
        <tr>
            <td>
                产品图片：
            </td>
            <td>
                <input id="hidImg" type="hidden" runat="server" />
                <img id="imgProduct" runat="server" src="../resource/image/admin/img.gif" alt="图片展示"
                    width="160" height="120" />
            </td>
        </tr>
        <tr>
            <td>
                上传图片：
            </td>
            <td>
                <asp:FileUpload ID="fileUploadImg" runat="server"  onfocus="onTBox(this)"
                    onblur="outTBox(this)" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                产品描述：
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <textarea cols="60" id="desc" name="desc"><%=descVal %></textarea>

                <script type="text/javascript" src="ckeditor/ckeditor.js"></script>

                <script type="text/javascript">
                    CKEDITOR.replace('desc');
                </script>

            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <input id="btnSubmit" runat="server" type="submit" value="提交" onclick="return checkInput()"
                    onserverclick="BtnSubmit_Click" />&nbsp;&nbsp;
                <input id="Button1" type="button" value="返回" onclick="javascript:location.href='ProductList.aspx';" />
            </td>
        </tr>
    </table>

    <script type="text/javascript">
        function checkInput() {
            if ($("<%=txtPname.ClientID %>").value == "") { alert("请输入产品名称"); return false; }
            if ($("<%=selCategory.ClientID %>").value == "") { alert("请选择产品系列"); return false; }

            var reg = new RegExp("<(S*?)[^>]*>.*?|< .*? />");
            if (reg.test($("<%=txtPname.ClientID %>").value)) { alert("含非法字符,请检查输入"); return false; }
            
            var str = CKEDITOR.instances.desc.getData();
            str = str.replace("<br />", "");
            str = str.replace("<br>", "");
            str = $.trim(str);
            if (str == "") { alert("请输入产品描述"); return false; }
            return true;
        }
    </script>

</asp:Content>
