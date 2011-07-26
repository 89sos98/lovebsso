using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

public partial class Index : System.Web.UI.Page
{
    private readonly ILog log = LogManager.GetLogger(typeof(Index));
    protected void Page_Load(object sender, EventArgs e)
    {
        log.Info("I am Index Page!");
    }
}
