using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace kollus_samples_dotnet.WebForm
{
    public partial class LmsCallback : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "text/plain";
            StringBuilder sb = new StringBuilder();


            if ("GET".Equals(Request.HttpMethod.ToUpper()))
            {
                using (StreamReader reader = new StreamReader(Server.MapPath("/WebForm/lms.log")))
                {
                    String line = String.Empty;
                    while ((line = reader.ReadLine()) != null)
                    {
                        sb.Append(line);
                    }
                }
            }
            else if ("POST".Equals(Request.HttpMethod.ToUpper()))
            {
                String json_data = Request.Params["json_data"];

                using (StreamWriter writer = new StreamWriter(Server.MapPath("/WebForm/lms.log"), true, Encoding.UTF8))
                {
                    writer.WriteLine(String.Format("[{0}] {1}", DateTime.Now.ToLongTimeString(), json_data));
                }
                Response.StatusCode = 200;
            }
            else
            {
                sb.Append("This Method is not supported.");
            }

            Debug.Write(sb.ToString());
            Response.Write(sb.ToString());
            Response.End();
        }
    }
}