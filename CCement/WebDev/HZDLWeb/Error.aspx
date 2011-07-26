<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>系统操作失败</title>
    <style type="text/css">
        body
        {
            font: 9px normal Arial, Helvetica, sans-serif;
            margin: 0;
            padding: 0;
        }
        *, * focus
        {
            outline: none;
            margin: 0;
            padding: 0;
        }
        
        .container
        {
            width: 500px;
            margin: 0 auto;
        }
        .toggle_container
        {
            margin: -1px 5px 0 5px;
            padding: 0;
            border: 1px solid #99bbe8;
            overflow: scroll;
            font-size: 9pt;
            clear: both;
        }
        .toggle_container .block
        {
            padding: 5px;
        }
        .toggle_container .block p
        {
            padding: 5px;
            margin: 5px;
        }
        #Table1
        {
            margin-top: 100px;
            cursor: pointer;
            border: 1px solid #99bbe8;
            background-image: url(/images/common_fill.gif);
            background-position: 0 -1px;
        }
    </style>
    <script type="text/javascript" src="/js/jquery.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".toggle_container").hide();
            $("#Table2").click(function () {
                $(".toggle_container").toggle();
            });
        });
    </script>
</head>
<body style="background-color: #f1f5f6">
    <div align="center" width="90%" style="margin: 5px;">
        <div class="trigger">
            <table id="Table1" cellpadding="0" cellspacing="0">
                <tr align="center">
                    <td>
                        <table id="Table2" border="0" cellpadding="0" cellspacing="0">
                            <tr align="center">
                                <td>
                                    <img src="/images/plaint.gif" />
                                </td>
                                <td>
                                    <font style="font-size: 11pt; font-weight: bold" color="#006699">系统操作失败！请联系系统管理员检查错误原因！</font>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div class="toggle_container">
            <div class="block" align="left">
                <table cellpadding="0" cellspacing="0">
                    <tr align="left">
                        <td>
                            错误页面：<pre><asp:Literal ID="ErrprPage" runat="server"></asp:Literal></pre>
                        </td>
                    </tr>
                    <tr align="left">
                        <td>
                            错误消息：<pre><asp:Literal ID="ErrorException" runat="server"></asp:Literal></pre>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</body>
</html>
