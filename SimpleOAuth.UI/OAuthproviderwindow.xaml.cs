using SimpleOAuth.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace SimpleOAuth.UI
{
    /// <summary>
    /// Interaction logic for OAuthproviderwindow.xaml
    /// </summary>
    public partial class OAuthproviderwindow : Page
    {
        public OAuthproviderwindow()
        {
            InitializeComponent();
            this.DataContext = new TwosampleOAuthViewModel(
                        ConfigurationManager.AppSettings["googleClientID"],
                        ConfigurationManager.AppSettings["googleClientSecret"],
                        ConfigurationManager.AppSettings["twitterConsumerKey"],
                        ConfigurationManager.AppSettings["twitterConsumerSecret"]);
        }
    }
}
