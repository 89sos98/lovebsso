<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="fnadmin_Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
   <title>企业网站后台管理系统</title>
    <link rel="Stylesheet" href="../resource/css/admin.css" />

    <script type="text/javascript" src="../resource/js/JScript.js"></script>

    <script type="text/javascript">
        function onlogin() {
            if (document.getElementById("txtuname").value == "") {
                alert("用户名不能为空!"); return false;
            }
            if (document.getElementById("txtpwd").value == "") {
                alert("密码不能为空!"); return false;
            }
            if (document.getElementById("txtcode").value == "") {
                alert("请输入验证码!"); return false;
            }

            return true;
        }
        function changeCode(objId) {
            document.getElementById(objId).src = "../ValidCode.ashx?dt=" + Math.random();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%" cellspacing="0" cellpadding="0" border="0">
            <tr>
                <td style="height: 35px; background: url(../resource/image/admin/l_h1b.gif)" align="center"
                    valign="bottom">
                    <div style="width: 100%; height: 100%; background: url(../resource/image/admin/l_h0a.gif) no-repeat center" />
                </td>
            </tr>
            <tr>
                <td style="height: 54px; background: url(../resource/image/admin/l_h1b.gif)" align="center"
                    valign="top">
                    <div style="width: 100%; height: 100%; background: url(../resource/image/admin/l_h1a.gif) no-repeat center;" />
                </td>
            </tr>
            <tr>
                <td style="height: 68px; background: url(../resource/image/admin/l_h2b.gif)" align="center">
                    <div style="width: 100%; height: 100%; background: url(../resource/image/admin/l_h2a.gif) no-repeat center" />
                </td>
            </tr>
            <tr>
                <td height="93" background="../resource/image/admin/l_h3b.gif" align="center">
                    <div style="width: 100%; height: 100%; background: url(../resource/image/admin/l_h3a.gif) no-repeat center" />
                </td>
            </tr>
            <tr>
                <td height="160" background="../resource/image/admin/l_h4c.gif" align="center">
                    <table width="208" cellspacing="0" cellpadding="0" border="0">
                        <tr>
                            <td height="25" align="right" style="padding-right: 5px;">
                                用户名：
                            </td>
                            <td align="left" colspan="2">
                                <input type="text" id="txtuname" runat="server" class="tboxS" onfocus="onTBox(this)"
                                    onblur="outTBox(this)" />
                            </td>
                        </tr>
                        <tr>
                            <td height="25" align="right" style="padding-right: 5px;">
                                密&nbsp;&nbsp;码：
                            </td>
                            <td align="left" colspan="2">
                                <input type="password" id="txtpwd" runat="server" class="tboxS" onfocus="onTBox(this)"
                                    onblur="outTBox(this)" />
                            </td>
                        </tr>
                        <tr>
                            <td height="25" align="right" style="padding-right: 5px;">
                                验证码：
                            </td>
                            <td align="left">
                                <input type="text" size="4" id="txtcode" runat="server" style="width: 80px;" class="tboxS"
                                    onfocus="onTBox(this)" onblur="outTBox(this)" />
                            </td>
                            <td>
                                <img id="imgCode" style="cursor: pointer;" src="../ValidCode.ashx" alt="验证码看不清，点击换一个"
                                    onclick='changeCode("imgCode")' title="验证码看不清，点击换一个" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td valign="bottom" height="33" align="left" colspan="2">
                                <input id="btnLogin" runat="server" type="image" style="width: 50px; height: 20px;" src="~/resource/image/admin/btlogin.gif" onclick="return onlogin();" onserverclick="btnLogin_Click" />
                                <input type="image" style="width: 50px; height: 20px;" src="../resource/image/admin/bt_reset.gif" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td height="20" background="../resource/image/admin/l_h5b.gif" align="center">
                    <div style="width: 100%; height: 100%; background: url(../resource/image/admin/l_h5a.gif) no-repeat center" />
                </td>
            </tr>
            <tr>
                <td height="78" background="../resource/image/admin/l_h6b.gif" align="center">
                    <div style="width: 100%; height: 100%; background: url(../resource/image/admin/l_h6a.gif) no-repeat center" />
                </td>
            </tr>
            <tr>
                <td height="23" background="../resource/image/admin/l_h7b.gif" align="center">
                    <div style="width: 100%; height: 100%; background: url(../resource/image/admin/l_h7a.gif) no-repeat center" />
                </td>
            </tr>
            <tr>
                <td height="23" background="../resource/image/admin/l_h7b.gif" align="center">
                    <div style="width: 100%; height: 100%; background: url(../resource/image/admin/l_h8a.gif) no-repeat center" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>