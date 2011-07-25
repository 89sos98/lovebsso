<%@ Page Title="" Language="C#" MasterPageFile="~/fnadmin/Shared/Admin.master" AutoEventWireup="true"
    CodeFile="ArticleList.aspx.cs" Inherits="fnadmin_ArticleList"  %>
<%@ Register Src="Shared/Pager.ascx" TagName="Pager" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <table class="tableBorder" border="0" cellspacing="0" cellpadding="5">
        <tr>
            <th class="titleTd" colspan="2">
                文章管理
            </th>
        </tr>
        <tr>
            <td colspan="2" style="border: none;">
                分类：<select id="selCategory" runat="server" class="sel" onfocus="onTBox(this)" onblur="outTBox(this)"></select>
            </td>
        </tr>
        <tr>
            <td>
                方式：<select id="selKeymode" runat="server" class="sel" onfocus="onTBox(this)" onblur="outTBox(this)">
                    <option value="1">关键字</option>
                    <option value="2" selected="selected">标题</option>
                   <%-- <option value="3">内容</option>
                    <option value="4">标题+内容</option>--%>
                </select>
                关键字：<input id="txtKey" runat="server" type="text" class="tbox" onfocus="onTBox(this)"
                    onblur="outTBox(this)" />
            </td>
            <td style="border: none;">
                <input id="btnSearch" runat="server" type="image" src="~/resource/image/admin/bt_search.gif"
                    onserverclick="BtnSearch_Click" />
            </td>
        </tr>
    </table>
    <table class="tableBorder" border="0" cellspacing="0" cellpadding="5" style="margin-top: 5px;">
        <tr>
            <th width="30" class="titleTd" align="left">
                操作
            </th>
            <th class="titleTd" align="left">
                标题(点击进行编辑)
            </th>
            <th class="titleTd" align="left">
                类别
            </th>
            <th class="titleTd" align="left">
                更新时间
            </th>
        </tr>
        <asp:Repeater ID="rptArts" runat="server">
            <ItemTemplate>
                <tr>
                    <td>
                        <input name="arts" value='<%# Eval("ID") %>' type="checkbox" />
                    </td>
                    <td>
                        <a href='ArticleEdit.aspx?articleid=<%# Eval("ID") %>'>
                            <%# Eval("PName") %>
                        </a>
                    </td>
                    <td>
                        <%# CFunc.GetCategoryName(Eval("CategoryId").ToString())%>
                    </td>
                    <td>
                        <%# Eval("UpTime")%>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    <table class="tableBorder" border="0" cellspacing="0" cellpadding="5">
        <tr>
            <td style="border: none; height: 20px; line-height: 20px; width: 50px;">
                <input id="ckbAll" type="checkbox" onclick="checkAll(this)" />全选
            </td>
            <td>
                <input id="btnDelete" runat="server" type="image" src="~/resource/image/admin/bt_dele.gif"
                    onclick="return checkCheck();" onserverclick="BtnDelete_Click" />
            </td>
            <td colspan="3">
            </td>
            <td align="right" style="border: none; width: 50px;">
                <a href="ArticleEdit.aspx">发布文章</a>
            </td>
        </tr>
    </table>
    <uc1:Pager ID="Pager1" runat="server" />
</asp:Content>
