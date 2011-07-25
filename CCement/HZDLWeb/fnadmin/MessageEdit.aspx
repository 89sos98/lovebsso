<%@ Page Title="" Language="C#" MasterPageFile="~/fnadmin/Shared/Admin.master" AutoEventWireup="true"
    CodeFile="MessageEdit.aspx.cs" Inherits="fnadmin_MessageEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <table class="tableBorder" border="1" cellspacing="0" cellpadding="5" style="width: 100%;
        border-collapse: collapse; border-color: #cccccc;">
        <tr>
            <th colspan="4" class="titleTd">
                <% if (Request.QueryString["mt"] == "2")
                   { %>在线订购信息
                <%}
                   else
                   { %>
                留言信息
                <% } %>
            </th>
        </tr>
        <% if (Request.QueryString["mt"] == "2")
           { %>
        <tr>
            <td width="15%" valign="middle" align="center">
                产品名称
            </td>
            <td colspan="3" width="35%" valign="middle">
                <asp:Label ID="labPro" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <% } %>
        <tr>
            <td width="15%" valign="middle" align="center">
                公司名称
            </td>
            <td colspan="3" width="35%" valign="middle" align="left">
                <asp:Label ID="labComp" runat="server" Text=""></asp:Label>
            </td>
            
        </tr>
        <tr>
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
        <% if (Request.QueryString["mt"] == "1")
           { %>
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
        <% } %>
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
        </tr>
        <tr>
            <td colspan="4" align="right" valign="middle">
            <input type="button" value="回复" onclick='javascript:location.href="MessageHuifu.aspx?msgid=<%=Request.QueryString["msgid"]%>&huifu=<%=Request.QueryString["huifu"]%>"' />
                <input type="button" value="返回" onclick='javascript:location.href="MessageList.aspx?mt=<%=Request.QueryString["mt"] %>"' />
            </td>
        </tr>
    </table>
</asp:Content>
