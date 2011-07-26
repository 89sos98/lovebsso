<%@ Page Title="" Language="C#" MasterPageFile="~/fnadmin/Shared/Admin.master" AutoEventWireup="true"
    CodeFile="SysUserEdit.aspx.cs" Inherits="fnadmin_SysUserEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <table class="tableBorder" border="0" cellspacing="0" cellpadding="5">
        <% if (Request.QueryString["type"] == "1")
           { %>
        <tr>
            <th class="titleTd" colspan="2">
                管理员管理--添加管理员
            </th>
        </tr>
        <tr>
            <td align="right">
                后台登录名称：
            </td>
            <td>
                <input id="txtname" runat="server" type="text" class="tbox" onfocus="onTBox(this)"
                    onblur="outTBox(this)" />
            </td>
        </tr>
        <tr>
            <td align="right">
                后台登录密码：
            </td>
            <td>
                <input id="txtpwd" runat="server" type="text" class="tbox" onfocus="onTBox(this)"
                    onblur="outTBox(this)" />
            </td>
        </tr>
        <% }
           else if (Request.QueryString["type"] == "2")
           {  %>
        <tr>
            <th class="titleTd" colspan="2">
                管理员管理--修改密码
            </th>
        </tr>
        <tr>
            <td align="right">
                后台登录名称：
            </td>
            <td>
                <%=Request.QueryString["uname"]%>
            </td>
        </tr>
        <tr>
            <td align="right">
                新密码：
            </td>
            <td>
                <input id="txtNewpwd" runat="server" type="text" class="tbox" onfocus="onTBox(this)"
                    onblur="outTBox(this)" />
            </td>
        </tr>
        <tr>
            <td align="right">
                新密码确认：
            </td>
            <td>
                <input id="txtNewpwdRe" runat="server" type="text" class="tbox" onfocus="onTBox(this)"
                    onblur="outTBox(this)" />
            </td>
        </tr>
        <% }
           else if (Request.QueryString["type"] == "3")
           { %>
        <tr>
            <th class="titleTd" colspan="2">
                管理员权限管理(请选择相应的权限分配给管理员)
            </th>
        </tr>
        <tr>
            <th class="titleTd" align="left" colspan="2">
                >>全局权限
            </th>
        </tr>
        <asp:Repeater ID="rptM" runat="server">
            <ItemTemplate>
                <tr>
                    <td colspan="2">
                        <b>
                            <%#Eval("mname") %></b>
                        <asp:Label ID="labId" runat="server" Visible="false" Text='<%#Eval("moduleid") %>'></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:CheckBoxList ID="cblM" runat="server" RepeatColumns="5" RepeatDirection="Horizontal">
                        </asp:CheckBoxList>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <% } %>
        <tr>
            <td>
            </td>
            <td>
                <input id="btnSubmit" runat="server" type="submit" value="提交" onclick="return checkinput()"
                    onserverclick="BtnSubmit_Click" />&nbsp;&nbsp;
                <input type="button" value="返回" onclick="javascript:location.href='SysUsers.aspx';" />
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">

        function checkinput() {
            if ($("<%=txtname.ClientID %>")) {
                if ($("<%=txtname.ClientID %>").value == "") {
                    alert("请输入后台登录名称!"); return false;
                }

                if ($("<%=txtpwd.ClientID %>").value == "") {
                    alert("请输入后台登录密码!"); return false;
                }
            }
            if ($("<%=txtNewpwd.ClientID %>")) {
                if ($("<%=txtNewpwd.ClientID %>").value == "") {
                    alert("请输出新的登录密码!"); return false;
                }
                if ($("<%=txtNewpwdRe.ClientID %>").value == "") {
                    alert("请再次输出新的登录密码!"); return false;
                }

                if ($("<%=txtNewpwd.ClientID %>").value != $("<%=txtNewpwdRe.ClientID %>").value) {
                    alert("两次密码输入不匹配!"); return false;
                }
            }
            return true;
        }
    </script>

</asp:Content>
