using System;
using McIntalkin.Web.App;

namespace McIntalkin.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            new McIntalkinAppHost().Init();
        }
    }
}