using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace kollus_samples_dotnet
{


    public partial class DownloadVideo : System.Web.UI.Page
    {
        private readonly string customKey = "1ba769ae9972603d72658b8566dc4fa2e5782dcfede54c527831b70f4aa9d7f3";
        private readonly string secretKey = "hdyang2";
        private List<String> mcKeys = new List<string>();
        private readonly int expiredTime = 5;
        private readonly String cuid = "CLIENT_USER_ID";

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

        public HtmlControl Player
        {
            get { return this.wPlayer; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Int64 exp = (Int64)DateTime.Now.AddMinutes(expiredTime).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            var payload = JObject.FromObject(new { cuid = cuid, expt = exp });
            mcKeys.Add("Qils7XzI");
            JArray mcs = new JArray();
            foreach (String key in mcKeys)
            {
                JObject mc = JObject.FromObject(new { mckey = key });
                mcs.Add(mc);
            }
            payload.Add("mc", mcs);
            Console.WriteLine(payload.ToString());
            String token = createToken(payload.ToString().Trim());
            String url = String.Format("http://v.kr.kollus.com/s?jwt={0}&custom_key={1}&download&force_exclusive_player", token, customKey);

            Player.Attributes.Add("src", url);
        }
    }
}