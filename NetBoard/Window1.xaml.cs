using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ClashN;
using ClashCore.Handler;
using ClashN.Mode;
using ClashN.Handler;
using metron;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Reflection;

namespace NetBoard
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        

        public Window1()
        {
            InitializeComponent();
            Config _config = null;
            ConfigProc.LoadConfig(ref _config);
            var now = DateTime.Now;
            var end = UserInfo.expireIn;
            var span = end.Subtract(now);
            unusedDays.Text += span.Days;
            weclome.Text = "欢迎回来" + UserInfo.userName + "!";
            unusedTr.Content = UserInfo.transferEnable / (1024 * 1024 * 1024)+"G";
            UI.prGroup = proxiesGroup;
            UI.prs = proxies;
        }
        
        private void ConnectBtn_Loaded(object sender, RoutedEventArgs e)
        {

        }


        private void updateProxies(object sender, EventArgs e)
        {
            proxies.Items.Clear();
            var prg = Proxies.providers[proxiesGroup.SelectionBoxItem.ToString()]["proxies"].Value<JArray>();
            foreach(var prs in prg)
            {
                var cis = new ComboBoxItem();
                cis.Content = prs["name"];
                proxies.Items.Add(cis);
            }
        }

        private async void updateClashAsync(object sender, EventArgs e)
        {
            var prg = proxiesGroup.SelectionBoxItem.ToString();
            var prs = proxies.SelectionBoxItem.ToString();
            var client = new HttpClient();
            prg = System.Web.HttpUtility.UrlEncode(prg, System.Text.Encoding.UTF8);
            var strUrl = "http://127.0.0.1:9090/proxies/" + prg;
            
            var uri = new Uri(strUrl);
            Console.WriteLine(strUrl);
            if (strUrl != uri.ToString())
            {
                var fm_Info = typeof(Uri).GetField("m_Info", BindingFlags.NonPublic | BindingFlags.Instance);
                var m_Info = fm_Info.GetValue(uri);
                var tUriInfo = typeof(Uri).GetNestedType("UriInfo", BindingFlags.NonPublic |
                BindingFlags.Instance);
                var fString = tUriInfo.GetField("String");
                fString.SetValue(m_Info, strUrl);
            }
            var request = new HttpRequestMessage();
            request.RequestUri = uri;
            request.Method = HttpMethod.Put;
            var content = new StringContent("{\"name\":\"" + prs + "\"}", null, "application/json");
            request.Content = content;
            try
            {
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                
            }


        }

        private async void changeMode(object sender, MouseButtonEventArgs e)
        {
            string rule = "";
            switch (Mode.Content)
            {
                case "规则":
                    Mode.Content = "直连";
                    rule = "Direct";
                    break;
                case "直连":
                    Mode.Content = "全局";
                    rule = "Global";
                    break;
                case "全局":
                    Mode.Content = "规则";
                    rule = "Rule";
                    break;   
            }
            var client = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), "http://127.0.0.1:9090/configs");
            var content = new StringContent("{\"mode\":\""+rule+"\"}", null, "application/json");
            request.Content = content;
            
            try
            {
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
                Console.WriteLine("{\"mode\":\"+rule+\"}");
            }

        }
    }
}
