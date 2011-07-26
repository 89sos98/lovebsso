<%@ Page Title="" Language="C#" MasterPageFile="~/fnadmin/Shared/Admin.master" AutoEventWireup="true"
    CodeFile="SysUserEdit.aspx.cs" Inherits="fnadmin_SysUserEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <table class="tableBorder" border="0" cellspacing="0" cellpadding="5">
        <% if (Request.QueryString["type"] == "1")
           { %>
        <tr>
            <th class="titleTd" colspan="2">
                ����Ա����--��ӹ���Ա
            </th>
        </tr>
        <tr>
            <td align="right">
                ��̨��¼���ƣ�
            </td>
            <td>
                <input id="txtname" runat="server" type="text" class="tbox" onfocus="onTBox(this)"
                    onblur="outTBox(this)" />
            </td>
        </tr>
        <tr>
            <td align="right">
                ��̨��¼���룺
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
                ����Ա����--�޸�����
            </th>
        </tr>
        <tr>
            <td align="right">
                ��̨��¼���ƣ�
            </td>
            <td>
                <%=Request.QueryString["uname"]%>
            </td>
        </tr>
        <tr>
            <td align="right">
                �����룺
            </td>
            <td>
                <input id="txtNewpwd" runat="server" type="text" class="tbox" onfocus="onTBox(this)"
                    onblur="outTBox(this)" />
            </td>
        </tr>
        <tr>
            <td align="right">
                ������ȷ�ϣ�
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
                ����ԱȨ�޹���(��ѡ����Ӧ��Ȩ�޷��������Ա)
            </th>
        </tr>
        <tr>
            <th class="titleTd" align="left" colspan="2">
                >>ȫ��Ȩ��
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
                <input id="btnSubmit" runat="server" type="submit" value="�ύ" onclick="return checkinput()"
                    onserverclick="BtnSubmit_Click" />&nbsp;&nbsp;
                <input type="button" value="����" onclick="javascript:location.href='SysUsers.aspx';" />
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">

        function checkinput() {
            if ($("<%=txtname.ClientID %>")) {
                if ($("<%=txtname.ClientID %>").value == "") {
                    alert("�������̨��¼����!"); return false;
                }

                if ($("<%=txtpwd.ClientID %>").value == "") {
                    alert("�������̨��¼����!"); return false;
                }
            }
            if ($("<%=txtNewpwd.ClientID %>")) {
                if ($("<%=txtNewpwd.ClientID %>").value == "") {
                    alert("������µĵ�¼����!"); return false;
                }
                if ($("<%=txtNewpwdRe.ClientID %>").value == "") {
                    alert("���ٴ�����µĵ�¼����!"); return false;
                }

                if ($("<%=txtNewpwd.ClientID %>").value != $("<%=txtNewpwdRe.ClientID %>").value) {
                    alert("�����������벻ƥ��!"); return false;
                }
            }
            return true;
        }
    </script>

</asp:Content>
