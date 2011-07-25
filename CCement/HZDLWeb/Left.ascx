<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Left.ascx.cs" Inherits="Left" %>
<link href="css/style.css" rel="stylesheet" type="text/css" />
<link href="css/neiye_menu.css" rel="stylesheet" type="text/css" />

<script language="javascript" type="text/javascript" src="js/js_menu.js"></script>

<div class="logo">
</div>
<div class="product">
</div>
<div class="product_list">

        <ul>
            <%System.Collections.Generic.List<Product> fistList = ProDal.GetLsit(0, 1);
              for (int i = 0; i < fistList.Count; i++)
              {
                  if (Convert.ToInt32(fistList[i].IsCorP) == 0)
                  {%>
            <li>
                <%=CFunc.GetSubString(fistList[i].PName,0,14) %>
                <ul>
                    <%System.Collections.Generic.List<Product> erList = ProDal.GetLsit(Convert.ToInt32(fistList[i].Id), 2);
                      for (int j = 0; j < erList.Count; j++)
                      {
                          if (erList[j].IsCorP == 0)
                          {%>
                    <li style="border-bottom: 1px dashed #3faec7; background-image: none; font-weight: normal;padding: 0px;">
                        <%=CFunc.GetSubString(erList[j].PName,0,14) %>
                            <%System.Collections.Generic.List<Product> sanList = ProDal.GetLsit(erList[j].Id, 3);
                              for (int p = 0; p < sanList.Count; p++)
                              {
                                  if (sanList[p].IsCorP==1)
                                  {%>
                                      <li style="background-image: none; font-weight: normal; padding-left: 10px;"><a href="Prducat.aspx?productId=<%=sanList[p].Id %>" class="dd"><%=CFunc.GetSubString(sanList[p].PName,0,14) %></a></li>
                                 <% }
                              } %>
                    </li>
                    <%}
                          else
                          {%>
                    <li style="border-bottom: 1px dashed #3faec7; background-image: none; font-weight: normal;padding: 0px;"><a href="Prducat.aspx?productId=<%=erList[j].Id %>" class="dd">
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
    

   <%-- <ul>
        <li>燃烧器
            <ul>
                <li style="background-image: none; font-weight: normal; padding: 0px;"><a href="prducat.html">
                    煤粉燃烧器</a>
                    <ul>
                        <li style="background-image: none; font-weight: normal; padding-left: 10px;"><a href="#">
                            炉用煤粉燃烧器</a></li>
                        <li style="background-image: none; font-weight: normal; padding-left: 10px;"><a href="#">
                            窑用煤粉燃烧器</a></li>
                    </ul>
                </li>
                <li style="background-image: none; font-weight: normal; padding: 0px;"><a href="#">燃气燃烧器</a></li>
            </ul>
        </li>
        <li><a href="#"><strong>煤粉燃烧系统</strong></a></li>
        <li><a href="#"><strong>煤气燃烧系统</strong></a></li>
        <li><a href="#"><strong>煤粉制备系统</strong></a>
            <ul>
                <li style="background-image: none; font-weight: normal; padding: 0px;"><a href="#">风扫磨煤粉制备系统</a></li>
                <li style="background-image: none; font-weight: normal; padding: 0px;"><a href="#">立式磨煤粉制备系统</a></li></ul>
        </li>
        <li><a href="#"><strong>热风系统</strong></a>
            <ul>
                <li style="background-image: none; font-weight: normal; padding: 0px;"><a href="#">换热式热风系统</a></li>
                <li style="background-image: none; font-weight: normal; padding: 0px;"><a href="#">直流式热风系统</a></li></ul>
        </li>
        <li><a href="#"><strong>喷雾系统</strong></a>
            <ul>
                <li style="background-image: none; font-weight: normal; padding: 0px;"><a href="#">回流式喷雾系统</a></li>
                <li style="background-image: none; font-weight: normal; padding: 0px;"><a href="#">压力式喷雾系统</a></li></ul>
        </li>
        <li><a href="#"><strong>气力螺旋泵</strong></a></li>
        <li><a href="#"><strong>喂料机</strong></a></li>
        <li><a href="#"><strong>粉体输送系统</strong></a></li>
    </ul>--%>
</div>
