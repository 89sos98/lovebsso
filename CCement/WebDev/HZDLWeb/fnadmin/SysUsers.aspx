<%@ Page Title="" Language="C#" MasterPageFile="~/fnadmin/Shared/Admin.master" AutoEventWireup="true"
    CodeFile="SysUsers.aspx.cs" Inherits="fnadmin_SysUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <table class="tableBorder" border="0" cellspacing="0" cellpadding="5">
        <tr>
            <th width="50" class="titleTd" style="height: 25px">
                操作
            </th>
            <th class="titleTd" style="height: 25px">
                用户名
            </th>
            <th class="titleTd" style="height: 25px">
                上次登录时间
            </th>
            <th class="titleTd" style="height: 25px">
                上次登录IP
            </th>
             <th class="titleTd" style="height: 25px">
                操作
            </th>
        </tr>
        <asp:Repeater ID="rptUsers" runat="server">
            <ItemTemplate>
                <tr>
                    <td>
                        <input name="arts" value='<%#Eval("userid") %>' type="checkbox" />
                    </td>
                    <td>
                            <%# Eval("username")%>
                    </td>
                    <td>
                        <%# Eval("lastlogintime")%>
                    </td>
                    <td>
                        <%# Eval("lastloginip")%>
                    </td>
                    <td>
                    <a href='SysUserEdit.aspx?type=2&uid=<%#Eval("userid") %>&uname=<%#Eval("username") %>'>修改密码</a> |  <a href="SysUserEdit.aspx?type=3&uid=<%#Eval("userid") %>">编辑权限</a>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <tr>
            <td style="border: none; height: 20px; line-height: 20px;">
                <input id="ckbAll" type="checkbox" onclick="checkAll(this)" />全选
            </td>
            <td colspan="3">
                <input id="btnDelete" runat="server" type="image" src="../resource/image/admin/bt_dele.gif"
                    onclick="return checkCheck();" onserverclick="BtnDelete_Click" />
            </td>
            <td align="right">
                <a href="SysUserEdit.aspx?type=1"><b>添加管理员</b></a>
            </td>
        </tr>
    </table>
</asp:Content>
