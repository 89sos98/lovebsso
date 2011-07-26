<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Pager.ascx.cs" Inherits="fnadmin_Shared_Pager" %>

<script type="text/javascript">
    function goclick(bl) {
        if (document.getElementById("<%=txtpage.ClientID %>").value == "")
        { alert("请填写要转到的页码!"); return false; }

        var reg = /^[1-9]*$/;
        if (!reg.test(document.getElementById("<%=txtpage.ClientID %>").value))
        { alert("请填写合法的页码!"); return false; }

        var a = parseInt(document.getElementById("<%=txtpage.ClientID %>").value);

        var b = parseInt("<%=PInfo.TotalPage %>");
        if (a > b)
        { alert("您填写的页码超过了最大页码!"); return false; }

        return true;
    }
</script>

<table width="98%" border="0" cellspacing="0" cellpadding="5">
    <tr>
        <td align="right">
            <table>
                <tr>
                    <td valign="middle">
                        共<%=PInfo.TotalPage %>页
                    </td>
                    <td valign="middle">
                        当前第<%=PInfo.CurrentPageIndex %>页
                    </td>
                    <td valign="middle">
                        转到<input id="txtpage" runat="server" type="text" style="width: 30px; border: solid 1px #cccccc;" />
                    </td>
                    <td valign="middle">
                        <input id="btnGo" runat="server" type="image" src="../../resource/image/admin/bt_go_search.gif"
                            onclick="return goclick();" onserverclick="BtnGo_Click" />
                    </td>
                </tr>
            </table>
        </td>
        <td align="right">
            <table>
                <tr>
                    <% if (PInfo.CurrentPageIndex <= 1)
                       { %>
                    <td>
                        上一页
                    </td>
                    <% }
                       else
                       { %>
                    <td>
                        <a href="<%=GetUrl(1) %>">首页</a>
                    </td>
                    <td>
                        <a href="<%=GetUrl(PInfo.CurrentPageIndex-1) %>">上一页</a>
                    </td>
                    <% } %>
                    <td>
                        <% for (int i = 1; i <= (PInfo.TotalPage < 8 ? PInfo.TotalPage : 8); i++)
                           {
                               if (i == PInfo.CurrentPageIndex)
                               { %>[<%=i %>]<%}
                               else
                               { %>
                        <a href="<%=GetUrl(i) %>">
                            <%=i %></a>
                        <%} %>
                        <%} %>
                    </td>
                    <% if (PInfo.CurrentPageIndex >= PInfo.TotalPage)
                       {%>
                    <td>
                        下一页
                    </td>
                    <%}
                       else
                       { %>
                    <td>
                        <a href="<%=GetUrl(PInfo.CurrentPageIndex+1) %>">下一页</a>
                    </td>
                    <td>
                        <a href="<%=GetUrl(PInfo.TotalPage) %>">尾页</a>
                    </td>
                    <%} %>
                </tr>
            </table>
        </td>
    </tr>
</table>
