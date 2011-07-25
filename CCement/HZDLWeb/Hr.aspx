<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Hr.aspx.cs" Inherits="Hr" %>

<%@ Register src="Pager.ascx" tagname="Pager" tagprefix="uc1" %>

<%@ Register src="Top.ascx" tagname="Top" tagprefix="uc2" %>

<%@ Register src="Left.ascx" tagname="Left" tagprefix="uc3" %>

<%@ Register src="Foot.ascx" tagname="Foot" tagprefix="uc4" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
    <title>人才招聘-杭州大路燃烧器有限公司</title>
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
        <uc2:Top ID="Top1" runat="server" />
    </div>
    <!--menu end-->
    <!--banner end-->
    <!--banner js-->

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

    <!--js end-->
    <div class="box">
        <div class="left">
            <div class="left_left">
                <div class="left_right">
                    <uc3:Left ID="Left1" runat="server" />
                </div>
            </div>
            <div class="touy">
            </div>
        </div>
        <!--right star-->
        <div class="right">
            <div class="ny_list">
                <p>
                    您现在的位置：杭州大路燃烧器有限公司(杭州达路机电工程有限公司) &gt; 人才招聘
                </p>
            </div>
            <div class="ny_nei">
            <%if (ds!=null&&ds.Tables.Count>0)
              {
                  if (ds.Tables[0].Rows.Count>0)
                  {
                      for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                      {
                          
                      %>
                      <ul>
                    <li>岗位：<%=ds.Tables[0].Rows[i]["jobname"]%></li></ul><br />
                    <%=HttpUtility.HtmlDecode(ds.Tables[0].Rows[i]["jobdesc"].ToString()) %><br />
                 <% }}
              } %>
              <br />
               <div style="text-align:center;">
               <uc1:Pager ID="Pager1" runat="server" /></div>
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

