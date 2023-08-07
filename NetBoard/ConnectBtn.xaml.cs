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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClashN;
using ClashCore.Handler;
using ClashN.Mode;
using ClashN.Handler;

namespace NetBoard
{
    /// <summary>
    /// ConnectBtn.xaml 的交互逻辑
    /// </summary>
    public partial class ConnectBtn : UserControl
    {
        public Boolean connectment = false;
        private Stopwatch sw = new Stopwatch();
        private Timer timer = new Timer();
        private CoreHandler ch = new CoreHandler();
        private Config _config = LazyConfig.Instance.Config;

        public ConnectBtn()
        {
            InitializeComponent();
        }
        public void init()
        {
            timer.Elapsed += (s, e) =>
            {
                DateTime dt = DateTime.FromBinary(sw.Elapsed.Ticks);
                this.Dispatcher.Invoke(() =>
                {
                    tips.Text = $"{dt:HH:mm:ss}";
                });
            };
        }

        private async void StatementAsync(object sender, RoutedEventArgs e)
        {
            if (connectment)
            {
                sw.Stop();
                timer.Stop();
                tips.Text = "未连接!";
                ch.CoreStop();
                SysProxyHandle.UpdateSysProxy(LazyConfig.Instance.Config, true);
            }
            else
            {
                init();
                sw.Start();
                timer.Start();
                
                ch.CoreStart();
                SysProxyHandle.UpdateSysProxy(LazyConfig.Instance.Config, false);
                var pr = new ClashCore.Handler.Proxies();
                await pr.getProxies();
                Proxies.proxyGroup.ForEach((prg) =>
                {
                    var ci = new ComboBoxItem();
                    ci.Content = prg;
                    Console.WriteLine(prg);
                    UI.prGroup.Items.Add(ci);
                });
                UI.prGroup.IsEnabled = true;
                UI.prs.IsEnabled = true;
            }
            connectment = !connectment;
        }
    }
}
