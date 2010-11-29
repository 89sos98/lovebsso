using System;
using System.Data;
using Hornow.Horn.Seamark.Attributes;
using Hornow.Horn.Seamark;
using Hornow.Horn.Web.Core.Common;
using Hornow.Horn.Srv.ORM.Entities;
using Hornow.Horn.Srv.ORM.Services;
using Hornow.Horn.Srv.ORM.Data.Bases;
using Hornow.Horn.Web.Core.Extension.Utils;
using Hornow.Horn.ORM.Core.Entities;
using System.Web.UI.WebControls;
using Hornow.Horn.WebUI.Controls;
using Hornow.Common.Core.Utils.Lang;
using Hornow.Horn.WebUI.Controls.jQuery.Controls;
using Hornow.Horn.ORM.Core.Services;
using Hornow.Horn.Web.Core.Extension.Security;
using Hornow.Horn.CRM.Fpi.Legacy.ORM.Entities;
using Hornow.Horn.CRM.Domain.ORM.Services;
using Hornow.Horn.CRM.Domain.ORM.Entities;
using Hornow.Common.Core.Utils.Collections;
using Hornow.Horn.Notification.Plugin;
using Hornow.Horn.Notification.Plugin.Event;
using Hornow.Horn.Workflow.Extensions.Domain.Model;
using Hornow.Horn.Workflow.Extensions.Domain;
using Hornow.Horn.CRM.App.Const;
using System.Collections;
using Hornow.Horn.Web.Core.Workflow;
using System.Collections.Generic;

namespace Hornow.Horn.CRM.Pages.Srv
{

    [FaceComponent]
    [PageInfo("Srv_SparePartsGatherAudit", "SparePartsGatherAuditHandle", PageType.New,
        "/Pages/Srv/SparePartsGatherAuditHandle.aspx", "区域需求申请汇总按产品线审核处理页面")]
    [SecurityExpression(SecExprType.SimpleExpression, "(Object:SparePartsGatherAudit,Deal)")]
    public partial class SparePartsGatherAuditHandle : EditableServlet
    {
        private List<object> auditId
        {
            get
            {
                if (ViewState["auditId_Keys"] == null)
                {
                    return new List<object>();
                }
                return (List<object>)ViewState["auditId_Keys"];
            }
            set
            {
                ViewState["auditId_Keys"] = value;
            }
        }

        /// <summary>
        /// 数据绑定
        /// </summary>
        protected override void BindData()
        {
            VwSrvSparePartsGatherAuditService service = new VwSrvSparePartsGatherAuditService();
            VwSrvSparePartsGatherAuditQuery query = new VwSrvSparePartsGatherAuditQuery();
            query.AppendEquals(VwSrvSparePartsGatherAuditColumn.Fid, base.PrimeKey);
            VwSrvSparePartsGatherAudit srv = service.FindSingle(query);
            if (srv != null)
            {
                //评审单编号
                this.FNmber.Text = srv.Fnumber;
                //产品线
                FProdLine.Text = srv.VFprodLine;
                //制单人
                this.FCreatorID.Text = srv.VFcreatorId;
                //制单时间
                this.FCreateDate.Text = ConvertUtil.ToDateTime(srv.FcreateDate);
                //审核人
                UrUsers urUsers = HornSecurityAccessor.GetUrUsers();
                this.FAuditorID.Text = urUsers.UserName;
                //审核时间
                this.FAuditDate.Text = ConvertUtil.ToDateTime(DateTime.Now);

                SrvSparePartsGatherService srvGatherService = new SrvSparePartsGatherService();
                SrvSparePartsGather srvGather = srvGatherService.GetByFid(NumberUtils.AttemptConvertLong(srv.FspGatherId));
                if (null != srvGather)
                {
                    //汇总起始时间
                    this.FStartDate.Text = ConvertUtil.ToDateTime(srvGather.FstartDate);
                    //汇总结束时间
                    this.FEndDate.Text = ConvertUtil.ToDateTime(srvGather.FendDate);
                }
                BindRepeater(ConvertUtil.ToString(srv.FspGatherId), srv.FprodLine);
            }
        }

        /// <summary>
        /// 处理工具栏事件
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void HandleToolbarTopItemClick(string itemId, object sender, EventArgs e)
        {
            base.HandleToolbarTopItemClick(itemId, sender, e);
            switch (itemId)
            {
                case "tbItemSubmit"://审核提交
                    DoItemAudit(true);
                    break;
                case "tbItemSave"://保存
                    DoItemAudit(false);
                    break;
            }
        }

        //绑定Repeater
        protected void BindRepeater(string fspGatherId, string fprodLine)
        {
            if (!string.IsNullOrEmpty(fspGatherId))
            {
                string lineSql = string.Format("SELECT FOrgId FROM VW_Srv_SparePartsApplyDetail WHERE FSpApplyID IN ( SELECT FSpApplyID FROM Srv_SparePartsGatherDetail WHERE FSpGatherID='{0}') AND FProdLine = {1} AND FState != '{2}' GROUP BY FOrgId"
                       , fspGatherId, fprodLine, CommonConst.TaskState_closed);
                DataTable fOrgId = ConnectionScope.Current.DataProvider.ExecuteDataSet(CommandType.Text, lineSql).Tables[0];
                DataColumn FspGatherId = new DataColumn("FspGatherId", Type.GetType("System.String"));
                DataColumn FProdLine = new DataColumn("FProdLine", Type.GetType("System.String"));
                fOrgId.Columns.Add(FspGatherId);
                fOrgId.Columns.Add(FProdLine);
                foreach (DataRow row in fOrgId.Rows)
                {
                    row["FspGatherId"] = fspGatherId;
                    row["FProdLine"] = fprodLine;
                }
                SparePartsApplyDetailAudit.DataSource = fOrgId;
                SparePartsApplyDetailAudit.DataBind();
            }
        }

        /// <summary>
        /// 审核
        /// </summary>
        protected void DoItemAudit(bool isAudit)
        {
            List<object> keyList = new List<object>();
            foreach (RepeaterItem item in this.SparePartsApplyDetailAudit.Items)
            {
                WebGridView webGridView = (WebGridView)item.FindControl("SparePartsApplyDetailGridView");
                SrvSparePartsApplyDetailService srvApplyService = new SrvSparePartsApplyDetailService();
                TList<SrvSparePartsApplyDetail> srvApplys = new TList<SrvSparePartsApplyDetail>();
                foreach (GridViewRow row in webGridView.Rows)
                {
                    DataKeyArray dataKeys = webGridView.DataKeys;
                    SrvSparePartsApplyDetail intDetail = srvApplyService.GetByFid(NumberUtils.AttemptConvertLong(dataKeys[row.RowIndex].Value));
                    if (intDetail != null)
                    {
                        //审核量
                        intDetail.FauditQuantity = NumberUtils.ConvertNullableDecimal(((WebTextBox)row.FindControl("FauditQuantity")).Text);
                        //审核说明
                        intDetail.FauditNote = ((WebTextBox)row.FindControl("FauditNote")).Text;
                        //删除标志
                        intDetail.FisDeleted = ((CheckBox)row.FindControl("CkbSelect")).Checked ? CommonConst.Logical_Deleted_Short : CommonConst.Logical_UnDeleted_Short;
                        if (intDetail.FisDeleted == CommonConst.Logical_Deleted_Short)
                            intDetail.FauditQuantity = 0;
                        if (isAudit)
                        {
                            //审核过
                            intDetail.FisAudited = CommonConst.Valid_Short;
                            if (!keyList.Contains(intDetail.FspApplyId))
                            {
                                keyList.Add(intDetail.FspApplyId);
                            }
                        }
                    }
                    srvApplys.Add(intDetail);
                }
                srvApplyService.Save(srvApplys);
            }
            HandelDate(keyList);
            SrvSparePartsGatherAuditService srvGatherAuditService = new SrvSparePartsGatherAuditService();
            SrvSparePartsGatherAudit srvGatherAudit = srvGatherAuditService.GetByFid(NumberUtils.AttemptConvertLong(base.PrimeKey));
            if (null != srvGatherAudit)
            {
                srvGatherAudit.FauditorId = Security.UserUtils.GetCurrentIdentityName();
                srvGatherAudit.FauditDate = ConvertUtils.ConvertNullableDateTime(DateTime.Now);

                VwSrvSparePartsGatherAuditService vwService = new VwSrvSparePartsGatherAuditService();
                VwSrvSparePartsGatherAuditQuery vwQuery = new VwSrvSparePartsGatherAuditQuery();
                vwQuery.AppendEquals(VwSrvSparePartsGatherAuditColumn.Fid, base.PrimeKey);
                VwSrvSparePartsGatherAudit srv = vwService.FindSingle(vwQuery);
                if (isAudit)
                {
                    srvGatherAudit.Fstate = CommonConst.TaskState_Audit;
                    SrvSparePartsGatherAuditService gatherAuditService = new SrvSparePartsGatherAuditService();
                    int unAuditCount = gatherAuditService.GetCount(string.Format("FSpGatherID = '{0}' AND FState = '{1}' AND FID <> '{2}'", srvGatherAudit.FspGatherId, CommonConst.TaskState_UnAudit, srvGatherAudit.Fid));
                    if (unAuditCount == 0)
                    {
                        SrvSparePartsGatherService sSSPGService = new SrvSparePartsGatherService();
                        SrvSparePartsGather sSPGather = sSSPGService.GetByFid(NumberUtils.AttemptConvertLong(srvGatherAudit.FspGatherId));
                        if (sSPGather != null)
                        {
                            sSPGather.Fstate = CommonConst.SparePartsGatherState_SubmitProdlineAudited;
                            sSSPGService.Update(sSPGather);
                        }
                    }
                    if (null != srv)
                        NotificationAccessor.AsynDispatchEvent(new DomainEvent("SparePartsGatherAudit", srv.Fid.ToString(), "SparePartsGatherAuditSubmit", DictionaryUtils.EasyDictionary("entity", srv)));
                }
                else
                {
                    if (null != srv)
                        NotificationAccessor.AsynDispatchEvent(new DomainEvent("SparePartsGatherAudit", srv.Fid.ToString(), "SparePartsGatherAuditSave", DictionaryUtils.EasyDictionary("entity", srv)));
                }
            }
            srvGatherAuditService.Save(srvGatherAudit);
            //导航
            if (IsDialogPage)
            {
                //关闭窗口
                ClientScriptHelper.RegisterStartupScript(ToolbarContainerTop, "parent.retVal.Force=true;parent.CloseAndReturn();");
            }
            else
            {
                base.RedirectTo("SparePartsGatherAuditDetail.aspx");
            }
        }

        /// <summary>
        /// 绑定内嵌的 GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                WebGridView webGridView = (WebGridView)e.Item.FindControl("SparePartsApplyDetailGridView");
                if (null != ((DataRowView)e.Item.DataItem)["FOrgId"] && null != ((DataRowView)e.Item.DataItem)["FspGatherId"] && null != ((DataRowView)e.Item.DataItem)["FProdLine"]) ;
                {
                    string FOrgId = ConvertUtil.ToString(((DataRowView)e.Item.DataItem)["FOrgId"]);
                    string FspGatherId = ConvertUtil.ToString(((DataRowView)e.Item.DataItem)["FspGatherId"]);
                    string FProdLine = ConvertUtil.ToString(((DataRowView)e.Item.DataItem)["FProdLine"]);

                    VwSrvSparePartsApplyDetailService service = new VwSrvSparePartsApplyDetailService();
                    VList<VwSrvSparePartsApplyDetail> detail = service.Get(string.Format("FOrgId = {0} AND FSpApplyID IN ( SELECT FSpApplyID FROM Srv_SparePartsGatherDetail WHERE FSpGatherID='{1}') AND FProdLine = {2} AND FState != '{3}'", FOrgId, FspGatherId, FProdLine, CommonConst.TaskState_closed));
                    if (null != detail && detail.Count > 0)
                    {
                        ((WebJTogglePanel)e.Item.FindControl("WebJTogglePanelTitle")).Title = detail[0].VForgId;
                        webGridView.DataSource = detail.ToDataSet(VwSrvSparePartsApplyDetailColumn.Fid).Tables[0];
                        webGridView.DataBind();
                    }
                }
            }
        }
        HnStockCurrentService hnStockCurrentService = new HnStockCurrentService();
        protected void SparePartsApplyDetailGridViewDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem == null)
            {
                return;
            }
            string FWhNumber = ConvertUtil.ToString(((DataRowView)e.Row.DataItem)["FWhNumber"]);
            //物料编号
            string FMatNumber = ConvertUtil.ToString(((DataRowView)e.Row.DataItem)["FMatNumber"]);
            //默认现存量为0
            string fMatExtantNum = "0";
            if (!string.IsNullOrEmpty(FWhNumber) && !string.IsNullOrEmpty(FMatNumber))
            {
                TList<HnStockCurrent> hnStockCurrent = hnStockCurrentService.Get(string.Format("FWhNumber = '{0}' AND FMatNumber = '{1}'", FWhNumber, FMatNumber));
                if (hnStockCurrent != null && hnStockCurrent.Count > 0)
                {
                    fMatExtantNum = ConvertUtil.DecimalToFormatString(hnStockCurrent[0].Fquantity);
                }
            }
            //现存量
            ((WebLabel)e.Row.FindControl("FMatExtantNum")).Text = fMatExtantNum;
            if (ConvertUtil.IsNullOrDBNull(((DataRowView)e.Row.DataItem)["FauditQuantity"]))
                //确认量
                ((WebTextBox)e.Row.FindControl("FauditQuantity")).Text = ConvertUtil.DecimalToFormatString(((DataRowView)e.Row.DataItem)["FrequireQuantity"]);
            else
                ((WebTextBox)e.Row.FindControl("FauditQuantity")).Text = ConvertUtil.DecimalToFormatString(((WebTextBox)e.Row.FindControl("FauditQuantity")).Text);


        }

        ///<summary>
        ///页面返回
        ///</summary>
        protected override void Row_Back()
        {
            base.RedirectTo("SparePartsGatherAuditList.aspx");
        }
        /// <summary>
        /// 页面取消
        /// </summary>
        protected override void Row_Cancel()
        {
            base.RedirectTo("SparePartsGatherAuditDetail.aspx");
        }
        /// <summary>
        /// 产品线经理已全部审核
        /// </summary>
        protected void HandelDate(List<object> auditId)
        {
            for (int i = 0; i < auditId.Count; i++)
            {
                SrvSparePartsApplyDetailService auditedService = new SrvSparePartsApplyDetailService();
                int auditedCount = auditedService.GetCount(string.Format("FSpApplyID = {0} AND FIsAudited != {1}", auditId[i], CommonConst.Valid_Short));
                if (auditedCount == 0)
                {
                    WorkflowDomain domain = WorkflowDomainAccessor.WorkflowDomainManager.GetActivedDomain(DomainTypes.SparePartsApply,
                                                                            ConvertUtil.ToString(auditId[i]), Workflows.TaskSparePartsApply);
                    SrvSparePartsApplyService srvSparePartsApplyService = new SrvSparePartsApplyService();
                    SrvSparePartsApply srvApply = srvSparePartsApplyService.GetByFid(NumberUtils.AttemptConvertLong(auditId[i]));
                    if (null != srvApply)
                    {
                        if (null != domain)
                        {
                            IDictionary context = new Hashtable();
                            context["domainType"] = DomainTypes.SparePartsApply;
                            context["domainId"] = ConvertUtil.ToString(auditId[i]);
                            context["domainNumber"] = srvApply.Fnumber;
                            context["workOwners"] = CollectionUtils.CreateList(srvApply.FcurHandler.Split(','));
                            context["auditName"] = this.FAuditorID.Text;
                            context["auditDate"] = this.FAuditDate.Text;
                            VwSrvSparePartsApplyDetailService service = new VwSrvSparePartsApplyDetailService();
                            VList<VwSrvSparePartsApplyDetail> detail = service.Get(string.Format(" FSpApplyID = {0}", auditId[i]));
                            if (null != detail && detail.Count > 0)
                                context["vwApplyDetail"] = detail;
                            //执行流程
                            WorkflowManagerWrapper.WorkflowManager.DoWorkflowAction(domain.EntryId, 501, context);
                        }
                    }
                }
            }
        }
    }
}
