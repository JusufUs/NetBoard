using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace metron
{
    public class getUser
    {
        public async Task<Boolean> api()
        {
            string api = GlobalConfig.gateway;
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, api+"api/user/" + UserInfo.userId+ "?access_token="+ UserInfo.token);
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var details = JObject.Parse(result);
            if (details["ret"].ToString() == "1")
            {
                UserInfo.transferEnable = details["data"]["transfer_enable"].ToObject<Decimal>();
                UserInfo.expireIn = Convert.ToDateTime(details["data"]["expire_in"].ToString());
                UserInfo.userName = details["data"]["user_name"].ToString();
                Console.WriteLine(UserInfo.transferEnable);
                return true;
            }
            else
            {

                return false;
            }

        }
    }
}
