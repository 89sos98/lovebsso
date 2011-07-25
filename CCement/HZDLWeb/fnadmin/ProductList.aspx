<%@ Page Title="" Language="C#" MasterPageFile="~/fnadmin/Shared/Admin.master" AutoEventWireup="true"
    CodeFile="ProductList.aspx.cs" Inherits="fnadmin_ProductList" %>

<%@ Register Src="Shared/Pager.ascx" TagName="Pager" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <table class="tableBorder" border="0" cellspacing="0" cellpadding="5">
        <tr>
            <th colspan="2" class="titleTd">
                产品管理
            </th>
        </tr>
        <tr>
            <td style="border: none;">
                产品系列：<select id="selCategory" runat="server" class="sel" onfocus="onTBox(this)" onblur="outTBox(this)"></select>
                产品名称：<input id="txtPname" runat="server" type="text" class="tbox" onfocus="onTBox(this)"
                    onblur="outTBox(this)" />
            </td>
            <td align="left" width="50%" style="border: none;">
                <input id="btnSearch" runat="server" type="image" src="../resource/image/admin/btsearch.gif"
                    onserverclick="BtnSearch_Click" />
            </td>
        </tr>
    </table>
    <table class="tableBorder" border="0" cellspacing="0" cellpadding="5" style="margin-top: 5px;">
        <tr>
            <th class="titleTd" align="left">
            </th>
            <th class="titleTd" align="left">
                图片
            </th>
            <th class="titleTd" align="left" style="padding-left: 10px;">
                名称(点击进行编辑)
            </th>
            <th class="titleTd" align="left">
                所属分类
            </th>
            <th class="titleTd" align="left">
                更新时间
            </th>
        </tr>
        <asp:Repeater ID="rptPro" runat="server">
            <ItemTemplate>
                <tr>
                    <td width="5%">
                        <input name="arts" value='<%# Eval("productid") %>' type="checkbox" />
                    </td>
                    <td valign="top" style="width: 82px; padding: 0px;">
                        <%-- <% if (string.IsNullOrEmpty(i.img))
                           { %>
                        <img style="width: 82px; height: 62px;" src="../resource/image/admin/img.gif" alt="图片展示" />
                        <% }
                           else
                           { %>--%>
                        <img style="width: 80px; height: 60px; border: solid 1px #000000;" src='<%# string.IsNullOrEmpty(Eval("img").ToString()) ? "../resource/image/admin/img.gif" : Eval("img")%>'
                            alt="图片展示" />
                        <%-- <%} %>--%>
                    </td>
                    <td style="padding-left: 10px;">
                        <a href='ProductEdit.aspx?productid=<%# Eval("productid") %>'>
                            <%# Eval("pname")%></a>
                    </td>
                    <td>
                        <%# CFunc.GetCategoryName(Eval("categoryid").ToString())%>
                    </td>
                    <td>
                        <%#Eval("updatetime") %>
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
            <td align="right" style="border: none;">
                <a href="ProductEdit.aspx">发布产品</a>
            </td>
        </tr>
    </table>
    <uc1:Pager ID="Pager1" runat="server" />
</asp:Content>
