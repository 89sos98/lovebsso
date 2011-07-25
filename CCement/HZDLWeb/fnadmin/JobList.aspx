<%@ Page Title="" Language="C#" MasterPageFile="~/fnadmin/Shared/Admin.master" AutoEventWireup="true" CodeFile="JobList.aspx.cs" Inherits="fnadmin_JobList" %>
<%@ Register Src="Shared/Pager.ascx" TagName="Pager" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <table class="tableBorder" border="0" cellspacing="0" cellpadding="5">
        <tr>
            <th colspan="2" class="titleTd">
                ְλ����
            </th>
        </tr>
        <tr>
            <td style="border: none;">
                ְλ���ƣ�<input id="txtJob" runat="server" type="text" class="tbox" onfocus="onTBox(this)"
                    onblur="outTBox(this)" />
                
            </td>
            <td style="border: none;">
                <input id="btnSearch" runat="server" type="image" src="../resource/image/admin/btsearch.gif"
                    onserverclick="BtnSearch_Click" />
            </td>
        </tr>
    </table>
    <table class="tableBorder" border="0" cellspacing="0" cellpadding="5" style="margin-top: 5px;">
        <tr>
            <th class="titleTd" align="left" width="20%">
                ѡ��
            </th>
            <th class="titleTd" align="left" width="60%">
                ��λ����(������б༭)
            </th>
            <th class="titleTd" align="left" width="20%">
                ����ʱ��
            </th>
           
           <%-- <th class="titleTd" align="left">
                ��Ч����
            </th>--%>
        </tr>
        <asp:Repeater ID="rptJob" runat="server">
            <ItemTemplate>
                <tr>
                    <td>
                        <input name="arts" value='<%# Eval("jobid") %>' type="checkbox" />
                    </td>
                    <td valign="top">
                        <a href='JobEdit.aspx?jobid=<%# Eval("jobid") %>'>
                            <%# Eval("jobname")%></a>
                    </td>
                    <td>
                         <%# Eval("publishtime") %>
                    </td>
                   <%-- <td>
                     <%# Eval("validuntil")%>
                    </td>--%>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <tr>
            <td style="border: none; height: 20px; line-height: 20px;">
                <input id="ckbAll" type="checkbox" onclick="checkAll(this)" />ȫѡ
            </td>
            <td >
                <input id="btnDelete" runat="server" type="image" src="../resource/image/admin/bt_dele.gif"
                    onclick="return checkCheck();" onserverclick="BtnDelete_Click" />
            </td>
            <td align="right">
                <a href="JobEdit.aspx"><b>����ְλ</b></a>
            </td>
        </tr>
    </table>
    <uc1:Pager ID="Pager1" runat="server" />
</asp:Content>