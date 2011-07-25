<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Lefter.ascx.cs" Inherits="fnadmin_Shared_Lefter" %>
<table id="tablefter" cellspacing="0" cellpadding="0" border="0">
    <tr style="background: url(../../resource/image/admin/h2cbg.gif) repeat-x;">
        <td style="width: 13px; height: 13px; background: url(../../resource/image/admin/h2a.gif) no-repeat bottom right;">
        </td>
        <td style="width: 21px; height: 13px; background: url(../../resource/image/admin/h2cbg.gif) repeat-x bottom left;">
        </td>
        <td style="height: 13px; background: url(../../resource/image/admin/h2cbg.gif) repeat-x bottom left;">
        </td>
        <td style="width: 4px; height: 13px; background: url(../../resource/image/admin/h2d.gif) bottom left;">
        </td>
    </tr>
    <tr style="background: url(../../resource/image/admin/h2e3bg.gif) repate;">
        <td style="background: url(../../resource/image/admin/h2e1.gif) repeat-y top right;">
        </td>
        <td style="background: url(../../resource/image/admin/h2e2.gif) repeat-y top left;">
        </td>
        <td style="background: url(../../resource/image/admin/h2e3bg.gif) top left;">
        </td>
        <td style="background: url(../../resource/image/admin/h2e4.gif) top left;">
        </td>
    </tr>
    <tr>
        <td style="background: url(../../resource/image/admin/h2e1.gif) repeat-y top right;">
        </td>
        <td style="background: url(../../resource/image/admin/h2e2.gif) repeat-y top left;">
        </td>
        <td valign="top" style="background: url(../../resource/image/admin/h2e3bg.gif) repeat;">
            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                <asp:Repeater ID="rptFirst" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <table width="100%" cellspacing="0" cellpadding="0" border="0">
                                    <tr style="background: url(../../resource/image/admin/h2fm2.gif);">
                                        <td style="width: 2px; height: 26px; background: url(../../resource/image/admin/h2fm1.gif) no-repeat;">
                                        </td>
                                        <td style="background: url(../../resource/image/admin/h2fm2.gif)">
                                            <img alt="" src="../../resource/image/admin/default.gif" />&nbsp;
                                            <%# Eval("mname")%>
                                        </td>
                                        <td style="width: 2px; height: 26px; background: url(../../resource/image/admin/h2fm3.gif) no-repeat top right;">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="display: block;">
                                <asp:Label ID="labId" runat="server" Text='<%# Eval("moduleid")%>' Visible="false"></asp:Label>
                                <table width="98%" cellspacing="0" cellpadding="0" border="0" align="center">
                                    <asp:Repeater ID="rptSecond" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td height="21">
                                                    <div align="left">
                                                        &nbsp;&nbsp;&nbsp;&nbsp;<img width="3" height="11" src="../../resource/image/admin/arrow1.gif">
                                                        <a title='<%# Eval("mname")%>' target="_self" href='<%# Eval("href")%>'>
                                                            <%# Eval("mname")%></a></div>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    
                                </table>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </td>
        <td style="background: url(../../resource/image/admin/h2e4.gif) top left;">
        </td>
    </tr>
    <tr id="trrepeat">
        <td style="background: url(../../resource/image/admin/h2e1.gif) top right;">
        </td>
        <td style="background: url(../../resource/image/admin/h2e2.gif) top left;">
        </td>
        <td align="center" valign="middle" style="background: url(../../resource/image/admin/h2e3bg.gif)">
            <img alt="" width="107" height="5" src="../../resource/image/admin/url.gif" />
        </td>
        <td style="background: url(../../resource/image/admin/h2e4.gif)">
        </td>
    </tr>
    <tr style="background: url(../../resource/image/admin/h31dbg.gif) repeat;">
        <td style="width: 13px; height: 13px; background: url(../../resource/image/admin/h31a.gif) no-repeat top right;">
        </td>
        <td style="width: 21px; height: 13px; background: url(../../resource/image/admin/h31b.gif) top left;">
        </td>
        <td>
            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                <tr>
                    <td style="width: 5px; height: 13px; background: url(../../resource/image/admin/h31c.gif) no-repeat top left;">
                    </td>
                    <td style="width: 6px; height: 13px; background: url(../../resource/image/admin/h31e.gif) no-repeat top right;">
                    </td>
                </tr>
            </table>
        </td>
        <td style="width: 4px; background: url(../../resource/image/admin/h31f.gif) top left;">
        </td>
    </tr>
</table>
