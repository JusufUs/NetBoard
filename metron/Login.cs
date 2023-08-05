using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace metron
{
    public class Login
    {
        public async Task<Boolean> LoginPanel(string account,string password, TextBlock loginInfo)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://user1.xn--yfrz5r03x5gq.net/api/token");
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(account), "email");
            content.Add(new StringContent(password), "passwd");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var details = JObject.Parse(result);
            if(details["ret"].ToString() == "1")
            {
                UserInfo.token = details["data"]["token"].ToString();
                UserInfo.userId = details["data"]["user_id"].ToString();
                loginInfo.Text = "登录成功";
                return true;
            }
            else
            {
                loginInfo.Text = "登录失败";
                return false;
            }
        }
    }
}
