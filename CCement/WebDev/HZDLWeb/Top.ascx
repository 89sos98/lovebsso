<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Top.ascx.cs" Inherits="Top" %>
<link href="css/style.css" rel="stylesheet" type="text/css" />
<link href="css/neiye_menu.css" rel="stylesheet" type="text/css" />

<script language="javascript" type="text/javascript" src="js/js_menu.js"></script>

<ul id="menu">
    <li class="menu"><a href="Index.aspx" class="dd">首 页</a></li>
    <li class="menu"><a href="JianJie.aspx" class="dd">公司介绍</a></li>
    <li class="menu"><a href="Prducat.aspx" class="dd">产品中心</a>
        <ul>
            <%System.Collections.Generic.List<Product> fistList = ProDal.GetLsit(0, 1);
              for (int i = 0; i < fistList.Count; i++)
              {
                  if (Convert.ToInt32(fistList[i].IsCorP) == 0)
                  {%>
            <li class="dd">
                <%=CFunc.GetSubString(fistList[i].PName,0,14) %>
                <ul>
                    <%System.Collections.Generic.List<Product> erList = ProDal.GetLsit(Convert.ToInt32(fistList[i].Id), 2);
                      for (int j = 0; j < erList.Count; j++)
                      {
                          if (erList[j].IsCorP == 0)
                          {%>
                    <li class="dd">
                        <%=CFunc.GetSubString(erList[j].PName,0,14) %>
                        <ul>
                            <%System.Collections.Generic.List<Product> sanList = ProDal.GetLsit(erList[j].Id, 3);
                              for (int p = 0; p < sanList.Count; p++)
                              {
                                  if (sanList[p].IsCorP==1)
                                  {%>
                                      <li><a href="Prducat.aspx?productId=<%=sanList[p].Id %>" class="dd"><%=CFunc.GetSubString(sanList[p].PName,0,14) %></a></li>
                                 <% }
                              } %>
                        </ul>
                    </li>
                    <%}
                          else
                          {%>
                    <li><a href="Prducat.aspx?productId=<%=erList[j].Id %>" class="dd">
                        <%=CFunc.GetSubString(erList[j].PName,0,14) %></a></li>
                    <%}

                      } %>
                </ul>
            </li>
            <%}
                  else
                  {%>
            <li><a href="Prducat.aspx?productId=<%=fistList[i].Id %>" class="dd">
                <%=CFunc.GetSubString(fistList[i].PName,0,14) %></a></li>
            <% }
              }
            %>
        </ul>
    </li>
    <li class="menu"><a href="Hr.aspx" class="dd">人才招聘</a></li>
    <li class="menu"><a href="LiuYan.aspx" class="dd">在线留言</a></li>
    <li class="menu"><a href="Contact.aspx" class="dd">联系我们</a></li>
</ul>
