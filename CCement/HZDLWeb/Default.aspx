<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
    <title>���ݴ�·ȼ�������޹�˾</title>
    <link href="css/menu.css" rel="stylesheet" type="text/css" />
     <link href="css/neiye_menu.css" rel="Stylesheet" type="text/css" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />
   

    <script language="javascript" type="text/javascript" src="js/js_menu.js"></script>

    <style type="text/css">
        .STYLE1
        {
            font-size: 18px;
            color: #000000;
            font-weight: bold;
        }
    </style>
</head>
<body>
<form id="from1" runat="server">
    <div class="ny_caidan">
        <ul id="menu">
            <li class="menu"><a href="index.html">�� ҳ</a></li>
            <li class="menu"><a href="jinajie.html">��˾����</a></li>
            <li class="menu"><a href="prducat.html">��Ʒ����</a>
                <ul>
                    <li><a href="#">ȼ����</a>
                        <ul>
                            <li><a href="prducat.html">ú��ȼ����</a>
                                <ul>
                                    <li><a href="#">¯��ú��ȼ����</a></li>
                                    <li><a href="#">Ҥ��ú��ȼ����</a></li>
                                </ul>
                            </li>
                            <li><a href="#">ȼ��ȼ����</a></li>
                        </ul>
                    </li>
                    <li><a href="#">ú��ȼ��ϵͳ</a></li>
                    <li><a href="#">ú��ȼ��ϵͳ</a></li>
                    <li><a href="#">ú���Ʊ�ϵͳ</a>
                        <ul>
                            <li><a href="#">��ɨĥú���Ʊ�ϵͳ</a></li>
                            <li><a href="#">��ʽĥú���Ʊ�ϵͳ</a></li>
                        </ul>
                    </li>
                    <li><a href="#">�ȷ�ϵͳ</a>
                        <ul>
                            <li><a href="#">����ʽ�ȷ�ϵͳ</a></li>
                            <li><a href="#">ֱ��ʽ�ȷ�ϵͳ</a></li>
                        </ul>
                    </li>
                    <li><a href="#">����ϵͳ</a>
                        <ul>
                            <li><a href="#">����ʽ����ϵͳ</a></li>
                            <li><a href="#">ѹ��ʽ����ϵͳ</a></li>
                        </ul>
                    </li>
                    <li><a href="#">����������</a></li>
                    <li><a href="#">ι�ϻ�</a></li>
                    <li><a href="#">��������ϵͳ</a></li>
                </ul>
            </li>
            <li class="menu"><a href="hr.html">�˲���Ƹ</a></li>
            <li class="menu"><a href="liuyan.html">��������</a></li>
            <li class="menu"><a href="contact.html">��ϵ����</a></li>
        </ul>
    </div>

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

    <div class="box">
        <div class="left" >
            <div class="left_left">
                <div class="left_right">
                    <div class="logo">
                    </div>
                    <div class="product">
                    </div>
                    <div class="product_list">
                        <ul>
                            <li >ȼ����
                                <ul>
                                    <li style="background-image: none; font-weight: normal; padding: 0px;"><a href="prducat.html">
                                        ú��ȼ����</a>
                                        <ul>
                                            <li style="background-image: none; font-weight: normal; padding-left: 10px;"><a href="#">
                                                ¯��ú��ȼ����</a></li>
                                            <li style="background-image: none; font-weight: normal; padding-left: 10px;"><a href="#">
                                                Ҥ��ú��ȼ����</a></li>
                                        </ul>
                                    </li>
                                    <li style="background-image: none; font-weight: normal; padding: 0px;"><a href="#">ȼ��ȼ����</a></li>
                                </ul>
                            </li>
                            <li><a href="#"><strong>ú��ȼ��ϵͳ</strong></a></li>
                            <li><a href="#"><strong>ú��ȼ��ϵͳ</strong></a></li>
                            <li><a href="#"><strong>ú���Ʊ�ϵͳ</strong></a>
                                <ul>
                                    <li style="background-image: none; font-weight: normal; padding: 0px;"><a href="#">��ɨĥú���Ʊ�ϵͳ</a></li>
                                    <li style="background-image: none; font-weight: normal; padding: 0px;"><a href="#">��ʽĥú���Ʊ�ϵͳ</a></li></ul>
                            </li>
                            <li><a href="#"><strong>�ȷ�ϵͳ</strong></a>
                                <ul>
                                    <li style="background-image: none; font-weight: normal; padding: 0px;"><a href="#">����ʽ�ȷ�ϵͳ</a></li>
                                    <li style="background-image: none; font-weight: normal; padding: 0px;"><a href="#">ֱ��ʽ�ȷ�ϵͳ</a></li></ul>
                            </li>
                            <li><a href="#"><strong>����ϵͳ</strong></a>
                                <ul>
                                    <li style="background-image: none; font-weight: normal; padding: 0px;"><a href="#">����ʽ����ϵͳ</a></li>
                                    <li style="background-image: none; font-weight: normal; padding: 0px;"><a href="#">ѹ��ʽ����ϵͳ</a></li></ul>
                            </li>
                            <li><a href="#"><strong>����������</strong></a></li>
                            <li><a href="#"><strong>ι�ϻ�</strong></a></li>
                            <li><a href="#"><strong>��������ϵͳ</strong></a></li>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="touy">
            </div>
        </div>
        <!--right star-->
        <div class="right">
            <div class="ny_list">
                <p>
                    �����ڵ�λ�ã����ݴ�·ȼ�������޹�˾(���ݴ�·���繤�����޹�˾) > ��˾����</p>
            </div>
            <div class="jianjie">
                <div class="text2">
                    <img src="images/jj_img.jpg" width="391" height="328" style="float: right; margin: 10px 5px 3px 5px;" /><br />
                    <p style="padding: 0px; line-height: 25px; font-size: 14px; color: #666666;">
                        <span class="STYLE1">���ݴ�·ȼ�������޹�˾(���ݴ�·���繤�����޹�˾)</span>λ�ھ��÷����ͨ��ݡ��羰�����Ľ��Ϻ��ݡ�</p>
                    <p style="padding: 0px; line-height: 25px; font-size: 14px; color: #666666;">
                        ���ݴ�·ȼ�������޹�˾���㽭ʡ�������д��������ĸ߿Ƽ���ҵ�����ڴ������ƿ�����������й����Ƚ�ˮƽ�����ܹ���װ������Ҫ��Ʒ�У�����ú��ȼ����ȼ����������ȼ������ȼ��ȼ����
                        ������ȼ�ϻ��յ�ȼ�������ȷ�ϵͳ�豸��������������ϵͳ�豸�������������ϵͳ�豸�ȡ��ɳн�Ҥ¯ϵͳ�������ʩ�����蹤�̣��������ȹ�ϵͳ�����������ܸ���������ʩ�����̡�
                    </p>
                    <p style="padding: 0px; line-height: 25px; font-size: 14px; color: #666666;">
                        ��˾ȫ��Ա��������������ڸ��ţ�������һ�������ġ���·��Ʒ�ƣ�������������˾�����Ժ�����йظ�У�Լ�����û��㷺������������� ��˾��һ����ּ�ǣ�ʼ�ռ�֡�������һ���û����ϡ��ķ��롣���Ƚ��ļ����������Ĳ�Ʒ���ܵ���ʱ�ĸ�ˮƽ����Ӯ���˹���û���������������
                        ��Ʒ�Ⱥ��ڹ����ڶཨ�ġ�ұ�𡢻������������ҵʹ�ã������ڵ������ǵȹ��ҡ����ݴ�·ȼ�������޹�˾ȫ��Ա��Ը�����ȵļ�������Ӳ�Ĳ�Ʒ ������Ϊ���������Ͽͻ��ṩ���ʵķ���</p>
                </div>
                <div style="width: 776px; height: 1px; clear: both;">
                </div>
            </div>
        </div>
        <div class="clear">
        </div>
    </div>
    <div class="bottom_bg">
        <div class="bottom_right">
            <div class="bottom">
                <ul>
                    <li>��Ȩ���У����ݴ�·ȼ�������޹�˾(���ݴ�·���繤�����޹�˾) ��ַ���������ຼ����˾�������� �����ţ���ICP��05042342��</li>
                    <li>�绰��0571-86268356 ����֧�֣��й�ˮ����</li>
                </ul>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
