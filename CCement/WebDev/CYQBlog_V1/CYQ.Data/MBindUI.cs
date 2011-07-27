namespace CYQ.Data
{
    using CYQ.Data.Table;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Windows.Forms;

    internal class MBindUI
    {
        public static void Bind(object ct, object source)
        {
            if (ct is GridView)
            {
                ((GridView) ct).DataSource = source;
                ((GridView) ct).DataBind();
            }
            else if (ct is Repeater)
            {
                ((Repeater) ct).DataSource = source;
                ((Repeater) ct).DataBind();
            }
            else if (ct is DataList)
            {
                ((DataList) ct).DataSource = source;
                ((DataList) ct).DataBind();
            }
            else if (ct is System.Web.UI.WebControls.DataGrid)
            {
                ((System.Web.UI.WebControls.DataGrid) ct).DataSource = source;
                ((System.Web.UI.WebControls.DataGrid) ct).DataBind();
            }
            else if (ct is System.Windows.Forms.DataGrid)
            {
                ((System.Web.UI.WebControls.DataGrid) ct).DataSource = source;
            }
            else if (ct is DataGridView)
            {
                ((DataGridView) ct).DataSource = source;
            }
        }

        public static void BindList(object ct, MDataTable source)
        {
            if (ct is System.Web.UI.WebControls.ListControl)
            {
                BindList(ct as System.Web.UI.WebControls.ListControl, source);
            }
            else
            {
                BindList(ct as System.Windows.Forms.ListControl, source);
            }
        }

        private static void BindList(System.Web.UI.WebControls.ListControl listControl, MDataTable source)
        {
            listControl.DataSource = source;
            listControl.DataTextField = source.Columns[0].ColumnName;
            listControl.DataValueField = source.Columns[1].ColumnName;
            listControl.DataBind();
        }

        private static void BindList(System.Windows.Forms.ListControl listControl, MDataTable source)
        {
            listControl.DataSource = source;
            listControl.DisplayMember = source.Columns[0].ColumnName;
            listControl.ValueMember = source.Columns[1].ColumnName;
        }

        public static string GetID(object ct)
        {
            if (ct is System.Web.UI.Control)
            {
                return ((System.Web.UI.Control) ct).ID;
            }
            if (ct is System.Windows.Forms.Control)
            {
                return ((System.Windows.Forms.Control) ct).Name;
            }
            return "cyq";
        }
    }
}

