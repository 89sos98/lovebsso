using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fnadmin_MessageList : System.Web.UI.Page
{
    protected DataSet ds = new DataSet();
    string strSql = null;
    SqlManage sqlM = SqlManage.GetInstance();
    PagerInfo pinfo = new PagerInfo();
    sbyte? mtype;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["mt"]))
            mtype = sbyte.Parse(Request.QueryString["mt"]);
        if (!IsPostBack)
        {
            if (!CFunc.HasPageRight("/fnadmin/MessageList.aspx?mt=" + mtype))
                Response.Write("<script>alert('您没有权限访问该页');history.back();</script>");

            BindData(mtype, null, null);
        }
    }

    // 绑定页面数据
    private void BindData(sbyte? _mtype, int? _pro, string _key)
    {
        #region 绑定产品下拉框
        switch (_mtype) {
            case (sbyte)MessageType.Message:
                break;
            case (sbyte)MessageType.Product:
                if (selPro.Items.Count <= 0)
                {
                    selPro.Items.Add(new ListItem("全部", null));
                    strSql = "select [Product].[categoryid],[Category].[cname],[Product].[productid],[Product].[pname] from [Product] inner join [Category] on [Product].[categoryid]=[Category].[categoryid] group by [Product].[categoryid],[Category].[cname],[Product].[productid],[Product].[pname]";
                    DataSet ds = sqlM.GetDataSet(CommandType.Text, strSql);
                    if (null != ds && null != ds.Tables[0])
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            selPro.Items.Add(new ListItem(dr["cname"].ToString() + "\n——" + dr["pname"].ToString(), dr["productid"].ToString()));
                        }
                    }
                }
                break;
            default: break;
        }
        #endregion

        #region 获取订购信息记录数
        strSql = "select count([msgid]) from [Message] where 1=1";
        string strWhere = null;
        if (null != _mtype)
            strWhere += " and [msgtype]=" + _mtype;
        if (null != _pro)
            strWhere += " and [productid]=" + _pro;
        if (!string.IsNullOrEmpty(_key))
            strWhere += " and [content] like '%" + _key + "%'";

        strSql += strWhere;
        object objcount = sqlM.GetFistColumn(CommandType.Text, strSql);
        #endregion

        #region 绑定分页控件
        int count = 0;
        if (DBNull.Value != objcount && null != objcount)
            count = int.Parse(objcount.ToString());

        pinfo.Recordcount = count;
        pinfo.PageSize = 20;
        pinfo.TotalPage = (count % pinfo.PageSize == 0) ? count / pinfo.PageSize : count / pinfo.PageSize + 1;
        if (!string.IsNullOrEmpty(Request.QueryString["page"]))
            pinfo.CurrentPageIndex = int.Parse(Request.QueryString["page"]);
        else
            pinfo.CurrentPageIndex = 1;

        Pager1.PInfo = pinfo;
        #endregion

        #region 获取文章列表
        if (count > 0)
        {
            if (pinfo.CurrentPageIndex > 1)
                strSql = "select top " + pinfo.PageSize + " * from [Message] where [msgid] not in(select top " + (pinfo.CurrentPageIndex - 1) * pinfo.PageSize + " [msgid] from [Message] where 1=1";
            else
                strSql = "select top " + pinfo.PageSize + " * from [Message] where 1=1";
                
            if (pinfo.CurrentPageIndex > 1)
                strSql += strWhere + " order by [uptime] desc)" + strWhere + " order by [uptime] desc";
            else
                strSql += strWhere + " order by [uptime] desc";

            ds = sqlM.GetDataSet(CommandType.Text, strSql);

            //rptMsg.DataSource = ds;
            //rptMsg.DataBind();
        }
        #endregion
    }

    //搜索
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        switch (mtype)
        {
            case (sbyte)MessageType.Message:
                BindData(mtype, null, txtkey.Value.Trim());
                break;
            case (sbyte)MessageType.Product:
                BindData(mtype, int.Parse(selPro.Value), null);
                break;
            default:
                BindData(mtype, null, null);
                break;
        }
       
    }

    //删除选中项
    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request.Form["arts"]))
            return;
        else
        {
            strSql = "delete from [Message] where [msgid] in (" + Request.Form["arts"] + ")";
            sqlM.ExecuteSql(CommandType.Text, strSql);
        }

        //绑定数据
        if (mtype == (sbyte)MessageType.Message)
            BindData(mtype, null, txtkey.Value.Trim());
        else if (mtype == (sbyte)MessageType.Product)
        {
            int? proid = null;
            if (!string.IsNullOrEmpty(selPro.Value))
                proid = int.Parse(selPro.Value);
            BindData(mtype, proid, null);
        }
    }

    protected string GetProductName(string _proId) {
        if (string.IsNullOrEmpty(_proId))
            return null;
        strSql = "select [pname] from [Product] where [productid]=" + _proId;
        object obj = sqlM.GetFistColumn(CommandType.Text, strSql);
        if (null != obj && DBNull.Value != obj)
            return obj.ToString();
        else
            return null;
    }

}
