<%@ Page Title="" Language="C#" MasterPageFile="~/fnadmin/Shared/Admin.master" AutoEventWireup="true" CodeFile="SysModuleEdit.aspx.cs" Inherits="fnadmin_SysModuleEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
 <table class="tableBorder" border="0" cellspacing="0" cellpadding="5">
        <tr>
            <th  class="titleTd" colspan="5" style=" text-align:center">
                添加模块
            </th>
        </tr>
        <tr>
            <td align="right" valign="middle" width="120">
                父级模块：
            </td>
            <td align="left" valign="middle" width="200">
                <select id="selParent" runat="server" class="sel" onfocus="onTBox(this)"
                    onblur="outTBox(this)">
                </select>
            </td>
        </tr>
        <tr>
            <td align="right" valign="middle" width="120">
                模块名称：
            </td>
            <td align="left" valign="middle" width="200">
                <input id="txtname" runat="server" type="text" class="tbox" onfocus="onTBox(this)"
                    onblur="outTBox(this)" onkeyup="keyupVal()" />
            </td>
        </tr>
        <tr>
            <td align="right" valign="middle" width="120">
                模块显示名称：
            </td>
            <td align="left" valign="middle" width="200">
                <input id="txttitle" runat="server" type="text" class="tbox" onfocus="onTBox(this)"
                    onblur="outTBox(this)" />
            </td>
        </tr>
        <tr>
            <td align="right" valign="middle" width="120">
                链接页面：
            </td>
            <td align="left" valign="middle" width="200">
                <input id="txthref" runat="server" type="text" class="tbox tboxL" onfocus="onTBox(this)"
                    onblur="outTBox(this)" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <input id="btnSubmit" runat="server" type="submit" value="提交" onclick="return checkInput()"
                    onserverclick="BtnSubmit_Click" />
            </td>
        </tr>
    </table>
    
     <script language="javascript" type="text/javascript">
         function keyupVal() {
             $("<%=txttitle.ClientID %>").value=$("<%=txtname.ClientID %>").value);
         }
        function checkinput() {
            if ($("<%=txtname.ClientID %>").value == "") {
                alert("请输入模块名称!"); return false;
            }
            if ($("<%=txttitle.ClientID %>").value == "") {
                alert("请输入模块显示名称!"); return false;
            }
            return true;
        }
    </script>
</asp:Content>

