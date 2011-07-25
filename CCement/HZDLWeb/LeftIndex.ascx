<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LeftIndex.ascx.cs" Inherits="LeftIndex" %>
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
                              if (sanList[p].IsCorP == 1)
                              {%>
                        <li style="background-image: none; font-weight: normal; padding-left: 10px;"><a href="Prducat.aspx?productId=<%=sanList[p].Id %>" class="dd">
                            <%=CFunc.GetSubString(sanList[p].PName,0,14) %></a></li>
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
</div>
