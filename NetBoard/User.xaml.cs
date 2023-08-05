using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using metron;
namespace NetBoard
{
    /// <summary>
    /// User.xaml 的交互逻辑
    /// </summary>
    public partial class User : Window
    {
        public User()
        {
            InitializeComponent();
        }

        private async void loginClickAsync(object sender, RoutedEventArgs e)
        {
            var loginClass = new Login();
            var loginResult = await loginClass.LoginPanel(account.Text,password.Text,loginInfo);
            if (loginResult)
            {
                loginInfo.Text = "正在更新订阅...";
                var getuser = new getUser();
                await getuser.api();
                //var sublink = new SubLink();
                //await sublink.download();
                var controllPanel = new Window1();
                controllPanel.Show();
                this.Close();
            }
        }
    }
}
