<%@ Page Title="" Language="C#" MasterPageFile="~/fnadmin/Shared/Admin.master" AutoEventWireup="true" CodeFile="CategoryEdit.aspx.cs" Inherits="fnadmin_CategoryEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <table class="tableBorder" width="100%" border="0" cellspacing="0" cellpadding="5">
        <tr>
            <th colspan="2" class="titleTd">
                新建分类
            </th>
        </tr>
        <tr>
            <td align="right">
                父级分类：
            </td>
            <td>
                <select id="selParent" runat="server" class="sel" onfocus="onTBox(this)" onblur="outTBox(this)">
                </select>
            </td>
        </tr>
        <tr>
            <td align="right">
                分类名：
            </td>
            <td>
                <input id="txtCname" runat="server" type="text" class="tbox" onfocus="onTBox(this)"
                    onblur="outTBox(this)" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <input id="btnSubmit" runat="server" type="submit" value="提交" onclick="return checkInput()"
                    onserverclick="btnSubmit_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <input id="btnBack" type="button" value="返回" onclick='backList(<%=Request.QueryString["area"] %>)' />
            </td>
        </tr>
    </table>

    <script type="text/javascript">
        function checkInput() {
            if ($("<%=txtCname.ClientID %>").value == "") { alert("请输入分类名"); return false; }
            return true;
        }
        
        function backList(area) {
            location.href = "Category.aspx?area=" + area;
        }
        
    </script>
</asp:Content>

