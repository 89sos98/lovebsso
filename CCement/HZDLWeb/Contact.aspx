<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Contact.aspx.cs" Inherits="Contact" %>

<%@ Register src="Top.ascx" tagname="Top" tagprefix="uc1" %>

<%@ Register src="Left.ascx" tagname="Left" tagprefix="uc2" %>

<%@ Register src="Foot.ascx" tagname="Foot" tagprefix="uc3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
    <title>��ϵ����-���ݴ�·ȼ�������޹�˾</title>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <link href="css/neiye_menu.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript" src="js/js_menu.js"></script>

    <style type="text/css">
        <!
        -- .STYLE1
        {
            font-size: 12px;
        }
        -- ></style>
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
var imgs='images/ny_img1.jpg|images/ny_img2.jpg|images/ny_img3.jpg|images/ny_img4.jpg';
var urls='#|#|#|#';
var pw = 985;
var ph = 246;
var sizes = 14;
var Times = 4000;
var umcolor = 0xFFFFFF;
var btnbg =0xFF7E00;
var txtcolor =0xFFFFFF;
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
                    �����ڵ�λ�ã����ݴ�·ȼ�������޹�˾(���ݴ�·���繤�����޹�˾) > ��ϵ����</p>
            </div>
            <div class="ny_cont">
                <p>
                    ��˾���ƣ����ݴ�·ȼ�������޹�˾(���ݴ�·���繤�����޹�˾)</p>
                <p>
                    ��ϵ��ַ���������ຼ����˾������</p>
                <p>
                    ��ϵ�绰��0571-86268356</p>
                <p>
                    ��ҵ���䣺hzdalu@sohu.com</p>
                <p>
                    ��ҵ��ַ��www.hzdalu.com</p>
                <p>
                    ��ϵ�ˣ�������</p>
            </div>
            <div class="ny_table">
            </div>
        </div>
        <div class="clear">
        </div>
    </div>
    <div class="bottom_bg">
        <div class="bottom_right">
            <div class="bottom">
                <uc3:Foot ID="Foot1" runat="server" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
