<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LiuYan.aspx.cs" Inherits="LiuYan" %>

<%@ Register Src="Top.ascx" TagName="Top" TagPrefix="uc1" %>
<%@ Register Src="Left.ascx" TagName="Left" TagPrefix="uc2" %>
<%@ Register Src="Pager.ascx" TagName="Pager" TagPrefix="uc3" %>
<%@ Register src="Foot.ascx" tagname="Foot" tagprefix="uc4" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
    <title>��������-���ݴ�·ȼ�������޹�˾</title>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <link href="css/neiye_menu.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript" src="js/js_menu.js"></script>

    <script language="javascript" type="text/javascript">
        function onSubmit() {
            var txtName = document.getElementById("txtName").value;
            var txtContent = document.getElementById("txtContent").value;
            if (txtName == "" || txtContent == "") {
                alert("�뽫�û���������������д������");
                return false;
            }
            else {
                return true;
            }
        }
    </script>

    <style type="text/css">
        .STYLE2
        {
            font-size: 14px;
        }
        .aa
        {
            width: 225px;
            height: 24px;
            border: 1px dashed #55a9c9;
        }
        .cc
        {
            float: left;
            width: 325px;
            height: 100px;
            border: 1px dashed #55a9c9;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="ny_caidan">
        <uc1:Top ID="Top1" runat="server" />
    </div>
    <!--menu end-->
    <!--banner end-->
    <!--banner js-->

    <script src="js/(neiye_js)swfobject_source.js" type="text/javascript"></script>

    <div id="dplayer2" class="height">
    </div>

    <script language="javascript" type="text/javascript">
        var titles = '���ݴ�·ȼ�������޹�˾��Ʒչʾ|���ݴ�·ȼ�������޹�˾��Ʒչʾ|���ݴ�·ȼ�������޹�˾��Ʒչʾ|���ݴ�·ȼ�������޹�˾��Ʒչʾ';
        var imgs = 'images/ny_img1.jpg|images/ny_img2.jpg|images/ny_img3.jpg|images/ny_img4.jpg';
        var urls = '#|#|#|#';
        var pw = 985;
        var ph = 246;
        var sizes = 14;
        var Times = 4000;
        var umcolor = 0xFFFFFF;
        var btnbg = 0xFF7E00;
        var txtcolor = 0xFFFFFF;
        var txtoutcolor = 0x000000;
        var flash = new SWFObject('flash/focus1.swf', 'mymovie', pw, ph, '7', '');
        flash.addParam('allowFullScreen', 'true');
        flash.addParam('allowScriptAccess', 'always');
        flash.addParam('quality', 'high');
        flash.addParam('wmode', 'Transparent');
        flash.addVariable('pw', pw);
        flash.addVariable('ph', ph);
        flash.addVariable('sizes', sizes);
        flash.addVariable('umcolor', umcolor);
        flash.addVariable('btnbg', btnbg);
        flash.addVariable('txtcolor', txtcolor);
        flash.addVariable('txtoutcolor', txtoutcolor);
        flash.addVariable('urls', urls);
        flash.addVariable('Times', Times);
        flash.addVariable('titles', titles);
        flash.addVariable('imgs', imgs);
        flash.write('dplayer2');
    </script>

    <!--js end-->
    <div class="box">
        <div class="left">
            <div class="left_left">
                <div class="left_right">
                    <uc2:Left ID="Left1" runat="server" />
                </div>
            </div>
            <div class="touy">
            </div>
        </div>
        <!--right star-->
        <div class="right">
            <div class="ny_list">
                <p>
                    �����ڵ�λ�ã����ݴ�·ȼ�������޹�˾(���ݴ�·���繤�����޹�˾) &gt; ��������
                </p>
            </div>
            <div class="ny_nei">
                <ul>
                    
                        <%if (ds != null && ds.Tables.Count > 0)
                          {
                              if (ds.Tables[0].Rows.Count > 0)
                              {
                                  for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                  {
                                 
                        %> <li>
                        <strong>
                            <%=ds.Tables[0].Rows[i]["username"] %>�����ԣ�</strong><%=ds.Tables[0].Rows[i]["content"] %><br />
                       <%System.Data.DataSet ds2 = GetHuiFu(Convert.ToInt32(ds.Tables[0].Rows[i]["msgid"]));
                         if (ds2!=null&&ds2.Tables.Count>0)
                         {
                             if (ds2.Tables[0].Rows.Count>0)
                             {%>
                                <p style="color:#3695c3; font-size:12px; font-weight:normal;"><strong>
                            <%=ds2.Tables[0].Rows[0]["username"] %>�Ļظ���</strong><%=ds2.Tables[0].Rows[0]["content"] %><br /></p>
                             <%}
                             else
                             {%>
                                <p style="color:#3695c3; font-size:12px; font-weight:normal;"> ���޻ظ�</p>
                             <%}
                         }
                         else
                         {%>
                             <p style="color:#3695c3; font-size:12px; font-weight:normal;"> ���޻ظ�</p>
                         <%} %>  
                         </li>
                        <%}
                              }
                          } %>
                    
                </ul>
                <br />
                <div style="text-align:center;">
                <uc3:Pager ID="Pager1" runat="server" /></div>
            </div>
            <div class="ny_nei">
                <p style="text-align: center;">
                    ��ӭ��λ����������������б�����ǽ�����̵�ʱ���ڣ���������ϸ�����ܵ��ķ��񣬲��ڴ�ף��˳�����⣬����֧���뽨�齫�����ǲ��ϸĽ���ԭ������</p>
                <table width="80%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="26%" height="40" align="right">
                            <span class="STYLE2">�� ����</span>
                        </td>
                        <td width="74%" height="40" align="left">
                            <label>
                                <input type="text" runat="server" id="txtName" name="txtName" class="aa" />
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td height="40" align="right">
                            <span class="STYLE2">�� ����</span>
                        </td>
                        <td height="40" align="left">
                            <input type="text" runat="server" id="txtPho" name="txtPho" class="aa" />
                        </td>
                    </tr>
                    <tr>
                        <td height="40" align="right">
                            <span class="STYLE2">��˾���ƣ�</span>
                        </td>
                        <td height="40" align="left">
                            <input type="text" runat="server" id="txtGS" name="txtGS" class="aa" />
                        </td>
                    </tr>
                    <tr>
                        <td height="40" align="right">
                            <span class="STYLE2">��˾��ַ��</span>
                        </td>
                        <td height="40" align="left">
                            <input type="text" id="txtAdd" runat="server" name="txtAdd" class="aa" />
                        </td>
                    </tr>
                    <tr>
                        <td height="40" align="right">
                            <span class="STYLE2">E_MAIL��</span>
                        </td>
                        <td height="40" align="left">
                            <input type="text" name="txtEmail" id="txtEmail" runat="server" class="aa" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <span class="STYLE2">�������ݣ�</span>
                        </td>
                        <td align="left">
                            <label>
                                <textarea name="txtContent" id="txtContent" runat="server" style="width:325px; height:100px; border:1px dashed #55a9c9;"></textarea>
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td height="50" align="right">
                            &nbsp;
                        </td>
                        <td height="50" align="left">
                            <label>
                                &nbsp;
                                <input id="btnSubmit" runat="server" type="submit" value="�ύ" onclick="return onSubmit();"
                                    onserverclick="Button1_Click" />
                                <asp:Button ID="Button2" runat="server" Text="����" />
                            </label>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="clear">
        </div>
    </div>
    <div class="bottom_bg">
        <div class="bottom_right">
            <div class="bottom">
               <uc4:Foot ID="Foot1" runat="server" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
