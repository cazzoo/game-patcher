using System;
using System.Net;

namespace RFUpdater.Patcher.Source_files
{
    [System.ComponentModel.DesignerCategory("Code")]
    class WebClientWithTimeout : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest wr = base.GetWebRequest(address);
            wr.Timeout = 5000;
            return wr;
        }
    }
}
