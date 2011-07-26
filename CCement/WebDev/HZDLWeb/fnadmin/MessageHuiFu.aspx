<%@ Page Title="" Language="C#" MasterPageFile="~/fnadmin/Shared/Admin.master" AutoEventWireup="true"
    CodeFile="MessageHuiFu.aspx.cs" Inherits="fnadmin_MessageHuiFu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <script type="text/javascript" language="javascript">
        function onSubmit() {
            if ($("<%=txtName.ClientID %>").value == "") { alert("请输入回复人姓名！"); return false; }
            if ($("<%=txtContent.ClientID %>").value == "") { alert("请填写回复内容！"); return false; }
            //            var txtName = window document.getElementsByName("txtName").item;
            //            alert(txtName);
            //            var txtContent = document.getElementById("txtName").value;
            //            if (txtName == "" || txtContent == "") {
            //                alert("请将回复人和内容填写完整！");
            //                return false;
            //            } else {
            //                return true;
            //            }
            return true;
        }
    </script>

    <table class="tableBorder" border="1" cellspacing="0" cellpadding="5" style="width: 100%;
        border-collapse: collapse; border-color: #cccccc;">
        <tr>
            <th colspan="4" class="titleTd">
                <% if (Convert.ToInt32(Request.QueryString["huifu"]) == CFunc.WHuiFu)
                   { %>填写回复信息
                <%}
                   else
                   { %>
                修改回复信息
                <% } %>
            </th>
        </tr>
        <tr>
            <td width="15%" valign="middle" align="center">
                回复人：
            </td>
            <td colspan="3" width="35%" valign="middle">
                <input runat="server" type="text" id="txtName" name="txtName" />
            </td>
        </tr>
        <tr>
            <td width="15%" valign="middle" align="center">
                回复内容：
            </td>
            <td colspan="3" width="35%" valign="middle" align="left">
                <label>
                    <textarea name="txtContent" id="txtContent" runat="server" style="width: 325px; height: 100px;
                        border: 1px dashed #55a9c9;"></textarea>
                </label>
            </td>
        </tr>
        <%--<tr>
            <td width="15%" valign="middle" align="center">
                详细地址
            </td>
            <td colspan="3" width="35%" valign="middle" align="left">
                <asp:Label ID="labAddress" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td width="15%" valign="middle" align="center">
                联系人
            </td>
            <td width="35%" valign="middle">
                <asp:Label ID="labUser" runat="server" Text=""></asp:Label>
            </td>
            <td width="15%" valign="middle" align="center">
                联系电话
            </td>
            <td width="35%" valign="middle">
                <asp:Label ID="labTel" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td width="15%" valign="middle" align="center">
                Email
            </td>
            <td width="35%" valign="middle">
                <asp:Label ID="labEmail" runat="server" Text=""></asp:Label>
            </td>
            <td width="15%" valign="middle" align="center">
                传真
            </td>
            <td width="35%" valign="middle">
                <asp:Label ID="labFax" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td width="15%" valign="middle" align="center">
                操作时间
            </td>
            <td width="35%" valign="middle">
                <asp:Label ID="labTime" runat="server" Text=""></asp:Label>
            </td>
            <td width="15%" valign="middle" align="center">
                操作IP
            </td>
            <td width="35%" valign="middle">
                <asp:Label ID="labIp" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td width="15%" valign="middle" align="center">
                详细内容
            </td>
            <td colspan="3" valign="middle">
                <asp:Literal ID="litDesc" runat="server"></asp:Literal>
            </td>
        </tr>--%>
        <tr>
            <td colspan="4" align="right" valign="middle">
                <input id="btnSubmit" runat="server" type="submit" value="提交" onclick="return onSubmit();"
                    onserverclick="Button1_Click" />
                <input type="button" value="返回列表" onclick='javascript:location.href="MessageList.aspx?mt=1"' />
            </td>
        </tr>
    </table>
</asp:Content>
