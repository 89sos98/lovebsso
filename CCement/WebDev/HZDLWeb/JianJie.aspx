<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JianJie.aspx.cs" Inherits="JianJie" %>

<%@ Register src="Top.ascx" tagname="Top" tagprefix="uc1" %>

<%@ Register src="Left.ascx" tagname="Left" tagprefix="uc2" %>

<%@ Register src="Foot.ascx" tagname="Foot" tagprefix="uc3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
    <title>简介-杭州大路燃烧器有限公司</title>
    <link href="css/menu.css" rel="stylesheet" type="text/css" />
<link href="css/style.css" rel="stylesheet" type="text/css" />

 <script language="javascript" type="text/javascript" src="js/js_menu.js"></script>
 <link href="css/neiye_menu.css" rel="stylesheet" type="text/css" />
 <style type="text/css">
<!--
.STYLE1 {
	font-size: 18px;
	color: #000000;
	font-weight: bold;
}
-->
 </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="ny_caidan">
        <uc1:Top ID="Top1" runat="server" />
    </div>

    <script src="js/(neiye_js)swfobject_source.js" type="text/javascript"></script>

    <div id="dplayer2" class="height">
    </div>

    <script language="javascript" type="text/javascript">
var titles = '杭州大路燃烧器有限公司产品展示|杭州大路燃烧器有限公司产品展示|杭州大路燃烧器有限公司产品展示|杭州大路燃烧器有限公司产品展示';
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
                    您现在的位置：杭州大路燃烧器有限公司(杭州达路机电工程有限公司) &gt; 公司介绍</p>
            </div>
            <div class="jianjie">
                <div class="jian_img">
                    <div class="text">
                        <ul>
                            <li>　　杭州大路燃烧器有限公司(杭州达路机电工程有限公司)位于经济发达、交通便捷、风景秀丽的江南杭州。</li>
                            <li>　　杭州大路燃烧器有限公司是浙江省首批具有创新能力的高科技企业，长期从事研制开发和制造具有国际先进水平的热能工程装备。主要产品有：多风道煤粉燃烧器燃烧器、重油燃烧器、燃气燃烧器 
                                、多种燃料混烧的燃烧器、热风系统设备、粉体物料输送系统设备、气体调质喷雾系统设备等。可承接窑炉系统的设计与施工建设工程，对已有热工系统进行增产节能改造的设计与施工工程。</li>
                        </ul>
                    </div>
                    <div class="tet">
                        <ul><br />
                            <li>　　公司全体员工经过多年的辛勤耕耘，创造了一个闻名的“大路”品牌，长期以来本公司与各大院所、有关高校以及广大用户广泛交流与合作。本 
                                公司的一贯宗旨是：始终坚持“质量第一，用户至上”的方针。以先进的技术、优良的产品和周到及时的高水平服务赢得了广大用户的信赖和赞誉， 
                                产品先后在国内众多建材、冶金、化工、发电等企业使用，并出口到东南亚等国家。杭州大路燃烧器有限公司全体员工愿以领先的技术，过硬的产品 
                                质量，为国内外新老客户提供优质的服务。</li>
                        </ul>
                    </div>
                </div>
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
