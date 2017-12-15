using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace kollus_samples_dotnet.Views
{
    public partial class PlayCallback : System.Web.UI.Page
    {
        private readonly String CUID = "test";
        private readonly int expMinutes = 5;
        private readonly String secretKey = "hdyang";

        private String encodeBase64Safe(String src)
        {
            if (string.IsNullOrEmpty(src))
            { return src; }
            byte[] temp = Encoding.UTF8.GetBytes(src.Trim());
            return Convert.ToBase64String(temp).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }
        private String encodeBase64Safe(byte[] src)
        {
            return Convert.ToBase64String(src).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }
        private String createToken(String payload)
        {
            var header = JObject.FromObject(new { typ = "JWT", alg = "HS256" });

            String h = encodeBase64Safe(header.ToString());
            String p = encodeBase64Safe(payload);
            String c = String.Format("{0}.{1}", h, p);
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
            byte[] signature = hmac.ComputeHash(Encoding.UTF8.GetBytes(c));
            String sig = encodeBase64Safe(signature);
            hmac.Dispose();
            return String.Format("{0}.{1}", c, sig);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "text/plain";
            StringBuilder sb = new StringBuilder();

            if (!"POST".Equals(Request.HttpMethod.ToUpper()))
            {
                sb.Append("This Method is not supported.");
            }
            else
            {
                int kind = Convert.ToInt32(Request.Params["kind"].ToString());
                string client_user_id = Request.Params["client_user_id"].ToString();
                string mediacontentkey = Request.Params["media_content_key"].ToString();

                int result = client_user_id.Equals(CUID) ? 1 : 0;
                String msg = result == 0 ? "This video is not permitted to you." : "";
                var payload = JObject.FromObject(new { kind= kind, result=result, message=msg });
                switch (kind) {
                    case 1:
                        Int64 expDate = (Int64)(DateTime.Now.AddMinutes(expMinutes).Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                        payload.Add("expiration_date", expDate);
                        break;
                    case 3:
                        break;
                }
                sb.Append(createToken(payload.ToString()));
            }


            Debug.Write(sb.ToString());
            Response.Write(sb.ToString());
            Response.End();
        }
    }
}