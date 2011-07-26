<%@ Page Title="" Language="C#" MasterPageFile="~/fnadmin/Shared/Admin.master" AutoEventWireup="true"
    CodeFile="MessageList.aspx.cs" Inherits="fnadmin_MessageList" %>

<%@ Register Src="Shared/Pager.ascx" TagName="Pager" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <table class="tableBorder" border="0" cellspacing="0" cellpadding="5">
        <tr>
            <th colspan="2" class="titleTd">
                留言信息
            </th>
        </tr>
        <tr>
            <td style="border: none;">
                <% if (Request.QueryString["mt"] == "2")
                   { %>
                产品：
                <select id="selPro" runat="server" class="sel" onfocus="onTBox(this)" onblur="outTBox(this)">
                </select>
                <% }
                   else
                   { %>
                <%--<input id="txtpro" runat="server" type="text" class="tbox" onfocus="onTBox(this)"
                    onblur="outTBox(this)" />--%>
                关键字：<input id="txtkey" runat="server" type="text" class="tbox" onfocus="onTBox(this)"
                    onblur="outTBox(this)" />
                <% } %>
            </td>
            <td style="border: none;">
                <input id="btnSearch" runat="server" type="image" src="~/resource/image/admin/btsearch.gif"
                    onserverclick="BtnSearch_Click" />
            </td>
        </tr>
    </table>
    <table class="tableBorder" border="0" cellspacing="0" cellpadding="5" style="margin-top: 5px;">
        <tr>
            <th class="titleTd" align="left" valign="middle" width="10%">
                选择
            </th>
            <% if (Request.QueryString["mt"] == "2")
               { %>
            <th class="titleTd" align="left" valign="middle" width="20%">
                订购产品
            </th>
            <% } %>
            <th class="titleTd" align="left" valign="middle" width="10%">
                留言者
            </th>
            <th class="titleTd" align="left" valign="middle" width="30%">
                留言内容(点击进行查看)
            </th>
            <th class="titleTd" align="left" valign="middle" width="15%">
                留言时间
            </th>
            <th class="titleTd" align="left" valign="middle" width="15%">
                是否回复
            </th>
        </tr>
        <%if (ds != null && ds.Tables.Count > 0)
          {
              if (ds.Tables[0].Rows.Count > 0)
              {
                  for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                  {%>
        <tr>
            <td>
                <input name="arts" value='<%=ds.Tables[0].Rows[i]["msgid"] %>' type="checkbox" /><%--<%=ds.Tables[0].Rows[i]["msgid"] %>--%>
            </td>
            <% if (Request.QueryString["mt"] == "2")
               { %>
            <td>
                <%--<%# GetProductName(Eval("productid").ToString())%>--%>
            </td>
            <% } %>
            <td>
                <%=ds.Tables[0].Rows[i]["username"] %>
            </td>
            <td title='<%=ds.Tables[0].Rows[i]["content"] %>'>
                <% if (Convert.ToInt32(ds.Tables[0].Rows[i]["huifu"]) != 0)
                   {%>
                <a href='MessageEdit.aspx?mt=<%=Request.QueryString["mt"] %>&msgid=<%=ds.Tables[0].Rows[i]["msgid"] %>&huifu=<%=CFunc.YHuiFu %>'>
                    <%= CFunc.GetSubString(ds.Tables[0].Rows[i]["content"].ToString(), 0, 50)%></a>
                <%}
                   else
                   {%>
                <a href='MessageEdit.aspx?mt=<%=Request.QueryString["mt"] %>&msgid=<%=ds.Tables[0].Rows[i]["msgid"] %>&huifu=<%=CFunc.WHuiFu %>'>
                    <%= CFunc.GetSubString(ds.Tables[0].Rows[i]["content"].ToString(), 0, 50)%></a>
                <% } %>
            </td>
            <td>
                <%=ds.Tables[0].Rows[i]["uptime"] %>
            </td>
            <td>
                <% if (Convert.ToInt32(ds.Tables[0].Rows[i]["huifu"]) != 0)
                   {%>
                <a href='MessageHuiFu.aspx?msgid=<%=ds.Tables[0].Rows[i]["msgid"] %>&huifu=<%=CFunc.YHuiFu %>'>
                    查阅并修改回复</a>
                <%}
                   else
                   {%>
                <a href='MessageEdit.aspx?mt=<%=Request.QueryString["mt"] %>&msgid=<%=ds.Tables[0].Rows[i]["msgid"] %>&huifu=<%=CFunc.WHuiFu %>'>
                    阅读回复</a>
                <%} %>
            </td>
        </tr>
        <%}
              }

          } %>
        <%--  <asp:Repeater ID="rptMsg" runat="server">
            <ItemTemplate>
                <tr>
                    <td>
                        <input name="arts" value='<%# Eval("msgid") %>' type="checkbox" />
                    </td>
                     <% if (Request.QueryString["mt"] == "2")
                        { %>
                        <td>
                          <%# GetProductName(Eval("productid").ToString())%>
                        </td>
               <% } %>
                    <td>
                        <%# Eval("username")%>
                    </td>
                    <td title='<%# Eval("content")%>'>
                        <a href='MessageEdit.aspx?mt=<%=Request.QueryString["mt"] %>&msgid=<%# Eval("msgid")%>'>
                            <%# CFunc.GetSubString(Eval("content").ToString(),0,300)%></a>
                    </td>
                    <td>
                        <%# Eval("uptime")%>
                    </td>
                    <td>
                           <% if (Convert.ToInt32(Eval("huifu"))!=0)
                          {%>
                              <a href='MessageEdit.aspx?mt=<%=Request.QueryString["mt"] %>&msgid=<%# Eval("msgid")%>&huifu=1'></a>
                          <%}
                          else
                          {%>
                             <a href='MessageEdit.aspx?mt=<%=Request.QueryString["mt"] %>&msgid=<%# Eval("msgid")%>'> 请阅读回复</a>
                          <%} %>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>--%>
        <tr>
            <td style="border: none; height: 20px; line-height: 20px;">
                <input id="ckbAll" type="checkbox" onclick="checkAll(this)" />全选
            </td>
            <td colspan="3">
                <input id="btnDelete" runat="server" type="image" src="~/resource/image/admin/bt_dele.gif"
                    onclick="return checkCheck();" onserverclick="BtnDelete_Click" />
            </td>
        </tr>
    </table>
    <uc1:Pager ID="Pager1" runat="server" />
</asp:Content>
