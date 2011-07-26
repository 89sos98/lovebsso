<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Pager.ascx.cs" Inherits="Pager" %>

<script type="text/javascript">
    function funChe() {
        var selPage = document.getElementById("selPage").value;
        window.location.href = "News.aspx?page=" + selPage;
    }
</script>

<% if (PInfo.Recordcount > PInfo.PageSize)
   { %>


            <table>
                <tr>
                    <td>
                        <a href="<%=GetUrl(1) %>">[首页]</a>
                    </td>
                    <% if (PInfo.CurrentPageIndex <= 1)
                       { %>
                    <td>
                        [上页]
                    </td>
                    <% }
                       else
                       { %>
                    <td>
                        <a href="<%=GetUrl(PInfo.CurrentPageIndex-1) %>">[上页]</a>
                    </td>
                    <% } %>
                    <%--<td>
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
                    </td>--%>
                    <% if (PInfo.CurrentPageIndex >= PInfo.TotalPage)
                       {%>
                    <td>
                        [下页]
                    </td>
                    <%}
                       else
                       { %>
                    <td>
                        <a href="<%=GetUrl(PInfo.CurrentPageIndex+1) %>">[下页]</a>
                    </td>
                    <%} %>
                    <td>
                        <a href="<%=GetUrl(PInfo.TotalPage) %>">[尾页]</a>
                    </td>
                    <td>
                        <asp:DropDownList ID="slePage" AutoPostBack="true" runat="server" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                        </asp:DropDownList>
                        
                    </td>
                    <td>
                       页共<%=PInfo.TotalPage%>页
                    </td>
                </tr>
            </table>
       
<% } %>
