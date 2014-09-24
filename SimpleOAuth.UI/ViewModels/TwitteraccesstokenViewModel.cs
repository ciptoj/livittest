using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TweetSharp;

namespace SimpleOAuth.UI.ViewModel
{
    /// <summary>
    /// Twitter second page view model
    /// </summary>
    public class TwitteraccesstokenViewModel : BaseViewModel
    {
        private ObservableCollection<TwitterStatus> twittertimeline;
        private bool isdataloaded;
        private string pin;
        public string PIN
        {
            get
            {
                return pin;
            }
            set
            {
                pin = value;
                NotifyPropertyChanged("PIN");
            }
        }
        public ICommand PINenteredCommand { get; set; }
        private OAuthRequestToken requestToken;
        public bool IsLoaded { get; set; }
        private TwitterService service;
        public TwitteraccesstokenViewModel(OAuthRequestToken requestToken, TwitterService service)
        {
            this.service = service;
            this.requestToken = requestToken;
            this.PINenteredCommand = DelegateCommand.FromAsyncHandler(OnPINEntered, CanProceed);
        }

        public bool Isdataloaded
        {
            get
            {
                return isdataloaded;
            }
            set
            {
                isdataloaded = true;
                NotifyPropertyChanged("Isdataloaded");
            }
        }
        /// <summary>
        /// behaviour when user click load
        /// </summary>
        /// <returns></returns>
        private async Task OnPINEntered()
        {
            OAuthAccessToken access = service.GetAccessToken(requestToken, PIN);
            // Step 4 - User authenticates using the Access Token
            service.AuthenticateWith(access.Token, access.TokenSecret);
            IEnumerable<TwitterStatus> timeline = service.ListTweetsOnHomeTimeline(new ListTweetsOnHomeTimelineOptions() { });
            Twittertimeline = new ObservableCollection<TwitterStatus>(timeline);
            Isdataloaded = true;
        }

        public ObservableCollection<TwitterStatus> Twittertimeline
        {
            get
            {
                return twittertimeline;
            }
            set
            {
                twittertimeline = value;
                NotifyPropertyChanged("Twittertimeline");
            }
        }
        private bool CanProceed()
        {

            return true;
        }

    }
}
