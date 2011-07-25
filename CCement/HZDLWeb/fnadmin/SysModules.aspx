<%@ Page Title="" Language="C#" MasterPageFile="~/fnadmin/Shared/Admin.master" AutoEventWireup="true"
    CodeFile="SysModules.aspx.cs" Inherits="fnadmin_SysModules" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <table class="tableBorder" border="0" cellspacing="0" cellpadding="5">
        <tr>
            <th class="titleTd">
                系统模块管理
            </th>
        </tr>
        <tr>
            <td style="border: none;">
                功能介绍：
            </td>
        </tr>
    </table>
    <table class="tableBorder" border="0" cellspacing="0" cellpadding="5" style="margin-top: 5px;">
        <tr>
            <th colspan="2" class="titleTd" align="left">
                模块名称
            </th>
            <th class="titleTd" align="left">
                模块显示名称
            </th>
            <th class="titleTd" align="left">
                链接页面
            </th>
            <th class="titleTd" align="left">
                操作
            </th>
        </tr>
        <asp:Repeater ID="rptModule" runat="server" OnItemCommand="rptModule_ItemCommand">
            <ItemTemplate>
                <tr>
                    <td align="right" width="15">
                        <img src="../resource/image/admin/expanded.gif" style="width: 11px; height: 11px;
                            border: none; cursor: pointer;" alt="展开" title="展开" onclick="onChangeImg(this);onShowChildren('<%# Eval("moduleid") %>')" />
                    </td>
                    <td>
                        <asp:Label ID="labModuleId" runat="server" Text='<%# Eval("moduleid") %>' Visible="false"></asp:Label>
                        <a href='ModuleEdit.aspx?moduleid=<%# Eval("moduleid") %>'>
                            <%# Eval("mname")%></a>
                    </td>
                    <td>
                        <a href='ModuleEdit.aspx?moduleid=<%# Eval("moduleid") %>'>
                            <%# Eval("mname")%></a>
                    </td>
                    <td>
                        <a href="<%# Eval("href")%>">
                            <%# Eval("href")%></a>
                    </td>
                    <td>
                        <a href="ModuleEdit.aspx?moduleid=<%# Eval("moduleid") %>">修改</a>
                        <asp:LinkButton ID="LinkButton1" runat="server" CommandName="delete" CommandArgument='<%# Eval("moduleid") %>'>删除</asp:LinkButton>
                        <asp:LinkButton ID="LinkButton2" runat="server" CommandName="empty" CommandArgument='<%# Eval("moduleid") %>'>清空</asp:LinkButton>
                        <%--  <asp:LinkButton ID="LinkButton3" runat="server" CommandName="move" CommandArgument='<%# Eval("moduleid") %>'>下移</asp:LinkButton>--%>
                    </td>
                </tr>
                <asp:Repeater ID="rptSecond" runat="server" OnItemCommand="rptSecond_ItemCommand">
                    <ItemTemplate>
                        <tr class='<%# Eval("parentid") %>'>
                            <td align="right" width="15">
                            </td>
                            <td>
                                <a href='SysModuleEdit.aspx?moduleid=<%# Eval("moduleid") %>'>
                                    <%# Eval("mname")%></a>
                            </td>
                            <td>
                                <a href='SysModuleEdit.aspx?moduleid=<%# Eval("moduleid") %>'>
                                    <%# Eval("mtitle")%></a>
                            </td>
                            <td>
                                <a href="<%# Eval("href")%>">
                                    <%# Eval("href")%></a>
                            </td>
                            <td>
                                <a href="SysModuleEdit.aspx?moduleid=<%# Eval("moduleid") %>">修改</a>
                                <asp:LinkButton ID="LinkButton4" runat="server" CommandName="delete" CommandArgument='<%# Eval("moduleid") %>'>删除</asp:LinkButton>
                                <%--   <asp:LinkButton ID="LinkButton6" runat="server" CommandName="move" CommandArgument='<%# Eval("moduleid") %>'>下移</asp:LinkButton>--%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </ItemTemplate>
        </asp:Repeater>
        <tr>
            <td colspan="5" align="right">
                <a href="SysModuleEdit.aspx">添加模块</a>
            </td>
        </tr>
    </table>
</asp:Content>
