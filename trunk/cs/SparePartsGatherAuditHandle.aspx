<%@ Page Language="C#" MasterPageFile="~/Masters/MainMaster.Master" AutoEventWireup="true"
    CodeBehind="SparePartsGatherAuditHandle.aspx.cs" Inherits="Hornow.Horn.CRM.Pages.Srv.SparePartsGatherAuditHandle" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
    <title>无标题页</title>

    <script type="text/javascript">
        function selectAll(obj, group) {
            $("span[hnFmatNumber='" + group + "']").children("input").each(function() {
                $(this).attr("checked", obj.checked);
            });
        }
    </script>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="MainContentToolBarDiv">
                <hn:WebToolbarContainer runat="server" ID="ToolbarContainerTop">
                    <hn:WebToolbar runat="server" ID="ToolbarTop" OnItemClick="ToolbarTop_ItemClick"
                        ToolbarKey="toolBarSparePartsGatherAuditHandle" />
                </hn:WebToolbarContainer>
            </div>
            <div class="MainContentQueryDiv">
                <div class="squarebox">
                    <table class="CommonDetailView">
                        <tr>
                            <td class="queryCell" width="15%">
                                评审单编号
                            </td>
                            <td width="35%">
                                <asp:Label ID="FNmber" runat="server"> </asp:Label>
                            </td>
                            <td class="queryCell" width="15%">
                                产品线
                            </td>
                            <td width="35%">
                                <asp:Label ID="FProdLine" runat="server"> </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="queryCell" width="15%">
                                制单人
                            </td>
                            <td width="35%">
                                <asp:Label ID="FCreatorID" runat="server"> </asp:Label>
                            </td>
                            <td class="queryCell" width="15%">
                                制单时间
                            </td>
                            <td width="35%">
                                <asp:Label ID="FCreateDate" runat="server"> </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="queryCell" width="15%">
                                汇总起始时间
                            </td>
                            <td width="35%">
                                <asp:Label ID="FStartDate" runat="server"> </asp:Label>
                            </td>
                            <td class="queryCell" width="15%">
                                汇总结束时间
                            </td>
                            <td width="35%">
                                <asp:Label ID="FEndDate" runat="server"> </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="queryCell" width="15%">
                                审核人
                            </td>
                            <td width="35%">
                                <asp:Label ID="FAuditorID" runat="server"> </asp:Label>
                            </td>
                            <td class="queryCell" width="15%">
                                审核时间
                            </td>
                            <td width="35%">
                                <asp:Label ID="FAuditDate" runat="server"> </asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <hn:WebJTabControl Collapsible="true" ID="DetailInfoTabs" runat="server" InnerTab="true">
                <hn:WebJTab ID="ApplyInfoTab" runat="server" Text="按区域清单" TabName="ApplyInfo">
                </hn:WebJTab>
                <hn:WebJTabPage ID="ApplyInfoPage" TabName="ApplyInfo" runat="server">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Repeater runat="server" ID="SparePartsApplyDetailAudit" OnItemDataBound="Repeater_ItemDataBound">
                                <ItemTemplate>
                                    <hn:WebJTogglePanel ID="WebJTogglePanelTitle" runat="server">
                                        <hn:WebGridView ID="SparePartsApplyDetailGridView" runat="server" Width="100%" DataKeyDbField="FID"
                                            OnRowDataBound="SparePartsApplyDetailGridViewDataBound" DataKeyNames="Fid" PageSettingKey="SparePartsGatherAuditHandle_sparepartsapplydetailgridview" >
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="center">
                                                    <HeaderTemplate>
                                                        删除标志
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CkbSelect" runat="server" Checked='<%# ((short)1).Equals(DataBinder.Eval(Container.DataItem,"FisDeleted")) %>' hnFmatNumber='<%# DataBinder.Eval(Container.DataItem,"FmatNumber") %>'
                                                            OnClick=<%# "selectAll(this,'" +DataBinder.Eval(Container.DataItem,"FmatNumber") +"');" %> />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <hn:WebBoundField DataField="VFprodType" HeaderText="产品型号" HeaderStyle-Width="100px" />
                                                <hn:WebBoundField DataField="FmatNumber" HeaderText="物料编号" HeaderStyle-Width="100px" />
                                                <hn:WebBoundField DataField="FmatName" HeaderText="物料名称" HeaderStyle-Width="100px"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <hn:WebBoundField DataField="FmatModel" HeaderText="规格型号" HeaderStyle-Width="70px" />
                                                <hn:WebBoundField DataField="FmatUnit" HeaderText="单位" HeaderStyle-Width="50px" />
                                                <hn:WebBoundField DataField="VFwhNumber" HeaderText="区域仓库" HeaderStyle-Width="80px" />
                                                <hn:WebTemplateField HeaderStyle-Width="40px" HeaderText="现存量">
                                                    <ItemTemplate>
                                                        <hn:WebLabel Width="99%" ID="FmatExtantNum" runat="server" ItemStyle-HorizontalAlign="Right"></hn:WebLabel>
                                                    </ItemTemplate>
                                                </hn:WebTemplateField>
                                                <hn:WebTemplateField HeaderStyle-Width="40px" HeaderText="需求量">
                                                    <ItemTemplate>
                                                        <%#  Hornow.Horn.Web.Core.Extension.Utils.ConvertUtil.DecimalToFormatString(DataBinder.Eval(Container.DataItem,"FrequireQuantity")) %>
                                                    </ItemTemplate>
                                                </hn:WebTemplateField>
                                                <hn:WebBoundField FieldKey="FrequireNote" DataField="FrequireNote" HeaderText="需求说明"
                                                    HeaderStyle-Width="100px" />
                                                <hn:WebTemplateField HeaderStyle-Width="40px" HeaderText="确认量">
                                                    <ItemTemplate>
                                                        <hn:WebTextBox Width="38px" ID="FauditQuantity" Required="true" runat="server" RequiredFieldType="Number"
                                                            Text='<%# DataBinder.Eval(Container.DataItem,"FauditQuantity") %>'></hn:WebTextBox>
                                                    </ItemTemplate>
                                                </hn:WebTemplateField>
                                                <hn:WebTemplateField HeaderStyle-Width="100px" HeaderText="审核说明">
                                                    <ItemTemplate>
                                                        <hn:WebTextBox Width="99%" ID="FauditNote" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"FauditNote") %>'></hn:WebTextBox>
                                                    </ItemTemplate>
                                                </hn:WebTemplateField>
                                            </Columns>
                                        </hn:WebGridView>
                                    </hn:WebJTogglePanel>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </hn:WebJTabPage>
            </hn:WebJTabControl>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
