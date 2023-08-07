using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace metron
{
    public class SubLink
    {
        public async Task<string> getSubLink()
        {
            string api = GlobalConfig.gateway;
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, api+"api/sublink?access_token=" + UserInfo.token);
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var details = JObject.Parse(result);
            if (details["ret"].ToString() == "1")
            {
                var sublink = details["data"].ToString().Split('?');
                return sublink[0]+ "?clash=1";
            }
            else
            {

                return null;
            }

        }
        public async Task<Boolean> download()
        {
            var sublink = await getSubLink();
            if (sublink == null)
            {
                return false;
            }
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, sublink);
            var response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            if(!Directory.Exists(System.Environment.CurrentDirectory + "/config/"))
            {
                Directory.CreateDirectory(System.Environment.CurrentDirectory + "/config/");
            }
            File.WriteAllText(System.Environment.CurrentDirectory + "/config/sub.yaml", result.ToString());
            return true;
        }
    }
}
