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
    public class TwitterViewModel : BaseViewModel
    {
        private ObservableCollection<TwitterStatus> twittertimeline;
        private bool isdataloaded;
        public string PIN { get; set; }
        public ICommand PINenteredCommand { get; set; }
        private OAuthRequestToken requestToken;
        public bool IsLoaded { get; set; }
        private TwitterService service;
        public TwitterViewModel(OAuthRequestToken requestToken, TwitterService service)
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
            set { 
                isdataloaded = true; 
                NotifyPropertyChanged("Isdataloaded"); 
            }
        }
        private async Task OnPINEntered()
        {
            OAuthAccessToken access = service.GetAccessToken(requestToken, PIN);
            // Step 4 - User authenticates using the Access Token
            service.AuthenticateWith(access.Token, access.TokenSecret);
            IEnumerable<TwitterStatus> timeline = service.ListTweetsOnHomeTimeline(new ListTweetsOnHomeTimelineOptions() {});
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
            return !String.IsNullOrEmpty(this.PIN);
        }

    }
}
