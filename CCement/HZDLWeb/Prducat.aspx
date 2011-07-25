<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Prducat.aspx.cs" Inherits="Prducat" %>

<%@ Register Src="Top.ascx" TagName="Top" TagPrefix="uc1" %>
<%@ Register Src="Left.ascx" TagName="Left" TagPrefix="uc2" %>
<%@ Register Src="Foot.ascx" TagName="Foot" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
    <title>产品中心-杭州大路燃烧器有限公司</title>
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
        var titles = '杭州大路燃烧器有限公司产品展示|杭州大路燃烧器有限公司产品展示|杭州大路燃烧器有限公司产品展示|杭州大路燃烧器有限公司产品展示';
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
                    您现在的位置：杭州大路燃烧器有限公司(杭州达路机电工程有限公司) > 产品中心 >
                    <%=product.PName %></p>
            </div>
            <%=HttpUtility.HtmlDecode(product.PContent) %>
            <%--<div class="ny_nei">--%>
                
                <%--<p>
                    <strong>技术原理</strong>：</p>
                <p>
                    本系统采用目前最先进的煤粉控制燃烧技术。全系统经专业设计并选用最先进的四风道煤粉燃烧器做为主燃烧设备。热力稳定、燃烧充分燃尽率高，煤耗低，燃烧火焰的热力、形状等可控制。可以稳定燃烧多种煤质的煤粉。</p>
                <p>
                    煤粉的输送采用气力混合输送，确保喂煤系统的可靠、连续、准确、稳定性。</p>
                <p>
                    我公司可根据客户的具体要求进行专业设计与施工指导，是您建厂、改造的首选!</p>
                <p>
                    <strong>产品技术特点：</strong></p>
                <p>
                    1、 对不同煤质适应性强，可烧劣质煤、褐煤、挥发份4－5％的无烟煤；
                    <p>
                        2、 火焰活泼有力、形状规整、窑内温度分布合理，火焰参数可灵活调节;</p>
                    <p>
                        3、 煤粉通道采用陶瓷复合层，磨损极小;</p>
                    <p>
                        4、 头部采用高强耐热材料精密加，各风道不变、火焰决不跑偏;</p>
                    <p>
                        5、 操作更简单;</p>
                    <p>
                        6、 总热效率高，煤粉燃烧充分，热风洁净，对产品质量无影响；</p>
                    <p>
                        7、 安全性高，无爆炸等危险；耐用性强，运行费用低，维护简单；</p>
                    <p>
                        <strong>适用范围</strong>：</p>
                    <p>
                        化学反应类：加热油漆料、硬树脂等各类反应釜 活性碳窑 氢氟酸回转窑 水玻璃窑 等；</p>
                    <p>
                        烘干类：高炉渣、锰矿等各类矿石 铸造用烘干炉 等；</p>
                    <p>
                        蒸馏浓缩类：染料浓缩炉 氯化氢、氯化钠脱水炉 等；</p>
                    <p>
                        物理加热类：热处理炉 锻造炉 搪瓷窑 搪玻璃窑 等；</p>
                    <p>
                        熔 化 类：热镀锌炉 金、银冶炼炉 熔铝、熔铜炉 等；</p>
                    <p>
                        培 烧 类：石灰石 水泥窑 等；</p>--%>
            <%--</div>--%>
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
