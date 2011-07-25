<%@ Page Title="" Language="C#" MasterPageFile="~/fnadmin/Shared/Admin.master" AutoEventWireup="true" CodeFile="Category.aspx.cs" Inherits="fnadmin_Category" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <table class="tableBorder" border="0" cellspacing="0" cellpadding="5">
        <tr>
            <th class="titleTd">
                类别管理
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
            <th colspan="2" class="titleTd" align="left" style="width: 70%">
                类别名称(点击进入编辑)
            </th>
            <th class="titleTd" align="left" style="width: 30%">
                操作
            </th>
        </tr>
        <asp:Repeater ID="rptCategory" runat="server" OnItemCommand="rptCategory_ItemCommand">
            <ItemTemplate>
                <tr>
                    <td align="right">
                        <img src="../resource/image/admin/expanded.gif" style="width: 11px; height: 11px;
                            border: none; cursor: pointer;" alt="收起" title="收起" onclick="onChangeImg(this);onShowChildren('<%# Eval("categoryid") %>')" />
                    </td>
                    <td>
                        <asp:Label ID="labCateId" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                        <a href='CategoryEdit.aspx?area=1&categoryid=<%# Eval("ID") %>'>
                            <%# Eval("PName")%>
                        </a>
                    </td>
                    <td>
                        <a href='CategoryEdit.aspx?area=1&categoryid=<%# Eval("ID") %>'>修改</a>
                        <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="confirmDel()" CommandName="delete"
                            CommandArgument='<%# Eval("ID") %>'>删除</asp:LinkButton>
                        <asp:LinkButton ID="LinkButton2" runat="server" CommandName="empty" CommandArgument='<%# Eval("ID") %>'>清空</asp:LinkButton>
                    </td>
                </tr>
                <asp:Repeater ID="rptSecond" runat="server" OnItemCommand="rptSecond_ItemCommand">
                    <ItemTemplate>
                        <tr class='<%# Eval("CategoryId") %>'>
                            <td align="right">
                            </td>
                            <td>
                                <a href='CategoryEdit.aspx?area=1&categoryid=<%# Eval("ID") %>'>
                                    <%# Eval("PName")%>
                                </a>
                            </td>
                            <td>
                                <a href='CategoryEdit.aspx?area=1&categoryid=<%# Eval("ID") %>'>修改</a>
                                <asp:LinkButton ID="LinkButton3" runat="server" CommandName="delete" CommandArgument='<%# Eval("ID") %>'>删除</asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </ItemTemplate>
        </asp:Repeater>
        <tr>
            <td colspan="3" align="right">
                <a href='CategoryEdit.aspx?area=<%=Request.QueryString["area"] %>'>新建类别</a>
            </td>
        </tr>
    </table>

    <script type="text/javascript">
        function confirmDel() {
            if (confirm("系统默认将该类别下的文章保留至默认类别下,您确定要删除该类别吗?"))
                return true;
            else
                return false;
        }
    </script>
</asp:Content>

