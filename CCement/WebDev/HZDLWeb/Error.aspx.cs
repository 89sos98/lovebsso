using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CCement;

public partial class Error : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                if (Session != null)
                {
                    object obj = Session[HttpErrorModule.HttpErrorWrapper];
                    if (null != obj)
                    {
                        ErrorWrapper wrapper = (ErrorWrapper)obj;
                        ErrprPage.Text = wrapper.ErrorPage;
                        ErrorException.Text = wrapper.ErrorException != null ? wrapper.ErrorException.ToString() : string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                //log.Error("Error processing error page.", ex);
            }
        }
    }
}