<%@ Page Title="" Language="C#" MasterPageFile="~/fnadmin/Shared/Admin.master" AutoEventWireup="true" CodeFile="JobEdit.aspx.cs" Inherits="fnadmin_JobEdit" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <table class="tableBorder" border="0" cellspacing="0" cellpadding="5">
        <tr>
            <th colspan="2" class="titleTd">
                发布职位
            </th>
        </tr>
        <tr>
            <td width="100" align="center">
                职位名称：
            </td>
            <td>
                <input id="txtJobname" runat="server" type="text" class="tbox tboxL" onfocus="onTBox(this)"
                    onblur="outTBox(this)" />
            </td>
        </tr>
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
         <tr>
            <td colspan="2" align="center">
                <input id="btnSubmit" runat="server" type="submit" value="提交" onclick="return checkInput()"
                    onserverclick="BtnSubmit_Click" />&nbsp;&nbsp;
                <input id="Button1" type="button" value="返回" onclick="javascript:location.href='JobList.aspx';" />
            </td>
        </tr>
    </table>

    <script type="text/javascript">
        function checkInput() {
            if ($("<%=txtJobname.ClientID %>").value == "") { alert("请输入职位名称"); return false; }
            
            return true;
        }
    </script>
</asp:Content>

