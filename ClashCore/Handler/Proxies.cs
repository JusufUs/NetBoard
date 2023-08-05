using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ClashCore.Handler
{
    public class Proxies
    {
        public static List<string> proxyGroup = new List<string>();
        public static JObject providers;
        public async Task getProxies()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://127.0.0.1:9090/providers/proxies");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var proxies = JObject.Parse(result);
            //proxies["providers"]["default"]["proxies"].Value<JArray>();
            providers = (JObject)proxies["providers"];
            foreach (var proxygroup in proxies["providers"])
            {
                proxyGroup.Add(proxygroup.First["name"].ToString());
                //Console.WriteLine(proxygroup.First["name"].ToString());
                JArray jr = proxygroup.First["proxies"].Value<JArray>();

                //Console.WriteLine(jr[0]);
            }


        }
    }
}
