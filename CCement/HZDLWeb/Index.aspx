<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Index" %>

<%@ Register src="Foot.ascx" tagname="Foot" tagprefix="uc1" %>

<%@ Register src="TopIndex.ascx" tagname="TopIndex" tagprefix="uc2" %>

<%@ Register src="LeftIndex.ascx" tagname="LeftIndex" tagprefix="uc3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
    <title>���ݴ�·ȼ�������޹�˾</title>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <link href="css/menu.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript" src="js/js_menu.js"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div class="box">
        <div class="left">
            <div class="left_left">
                <div class="left_right">
                   
                    <uc3:LeftIndex ID="LeftIndex1" runat="server" />
                </div>
            </div>
            <div class="touy">
            </div>
        </div>
        <!--right star-->
        <div class="right">
            <div class="caidan">
                <uc2:TopIndex ID="TopIndex1" runat="server" />
            </div>
            <div class="falsh">
                <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0"
                    width="776" height="286">
                    <param name="movie" value="images/index_banner.swf" />
                    <param name="quality" value="high" />
                    <param name="wmode" value="transparent" />
                    <embed src="images/index_banner.swf" width="776" height="286" quality="high" pluginspage="http://www.macromedia.com/go/getflashplayer"
                        type="application/x-shockwave-flash" wmode="transparent"></embed>
                </object>
            </div>
            <div class="jianjie">
                <div class="jian_img">
                    <div class="text">
                        <ul>
                            <li>�������ݴ�·ȼ�������޹�˾(���ݴ�·���繤�����޹�˾)λ�ھ��÷����ͨ��ݡ��羰�����Ľ��Ϻ��ݡ�</li>
                            <li>�������ݴ�·ȼ�������޹�˾���㽭ʡ�������д��������ĸ߿Ƽ���ҵ�����ڴ������ƿ�����������й����Ƚ�ˮƽ�����ܹ���װ������Ҫ��Ʒ�У�����ú��ȼ����ȼ����������ȼ������ȼ��ȼ���� 
                                ������ȼ�ϻ��յ�ȼ�������ȷ�ϵͳ�豸��������������ϵͳ�豸�������������ϵͳ�豸�ȡ��ɳн�Ҥ¯ϵͳ�������ʩ�����蹤�̣��������ȹ�ϵͳ�����������ܸ���������ʩ�����̡�</li>
                        </ul>
                    </div>
                    <div class="tet">
                        <ul><br />
                            <li>������˾ȫ��Ա��������������ڸ��ţ�������һ�������ġ���·��Ʒ�ƣ�������������˾�����Ժ�����йظ�У�Լ�����û��㷺������������� 
                                ��˾��һ����ּ�ǣ�ʼ�ռ�֡�������һ���û����ϡ��ķ��롣���Ƚ��ļ����������Ĳ�Ʒ���ܵ���ʱ�ĸ�ˮƽ����Ӯ���˹���û��������������� 
                                ��Ʒ�Ⱥ��ڹ����ڶཨ�ġ�ұ�𡢻������������ҵʹ�ã������ڵ������ǵȹ��ҡ����ݴ�·ȼ�������޹�˾ȫ��Ա��Ը�����ȵļ�������Ӳ�Ĳ�Ʒ 
                                ������Ϊ���������Ͽͻ��ṩ���ʵķ���</li>
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
                <uc1:Foot ID="Foot1" runat="server" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
