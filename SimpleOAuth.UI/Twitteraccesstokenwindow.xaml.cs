using SimpleOAuth.UI.ViewModel;
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
using TweetSharp;

namespace SimpleOAuth.UI
{
    /// <summary>
    /// Interaction logic for Twitteraccesstokenwindow.xaml
    /// </summary>
    public partial class Twitteraccesstokenwindow : Page
    {
        public Twitteraccesstokenwindow(OAuthRequestToken requestToken, TwitterService service)
        {
            InitializeComponent();
            this.DataContext = new TwitterViewModel(requestToken, service);

        }
    }
}
