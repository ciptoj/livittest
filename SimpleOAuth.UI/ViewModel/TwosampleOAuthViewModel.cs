using Google.Apis.Auth.OAuth2;
using Google.Apis.Books.v1;
using Google.Apis.Books.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TweetSharp;

namespace SimpleOAuth.UI.ViewModel
{
    public class TwosampleOAuthViewModel : INotifyPropertyChanged
    {
        private bool istwitter;
        private bool isgoogle;
        private ObservableCollection<Bookshelf> books;
        private const string Googlebooks = "Google Books";
        private const string Twitter = "Twitter";
        private string googleclientID;
        private string googleclientsecret;
        private string twitterconsumerkey;
        private string twitterconsumersecret;
        public string SelectedProvider { get; set; }

        public bool IsTwitter
        {
            get
            {
                return istwitter;
            }
            set
            {
                istwitter = value;
                NotifyPropertyChanged("IsTwitter");
            }
        }
        public bool IsGoogle
        {
            get
            {
                return isgoogle;
            }
            set
            {
                isgoogle = value;
                NotifyPropertyChanged("IsGoogle");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="googleclientID">Google client ID</param>
        /// <param name="googleclientsecret">Google client secret</param>
        public TwosampleOAuthViewModel(string googleclientID, string googleclientsecret, string twitterconsumerkey, string twitterconsumersecret)
        {
            this.googleclientID = googleclientID;
            this.googleclientsecret = googleclientsecret;
            this.twitterconsumerkey = twitterconsumerkey;
            this.twitterconsumersecret = twitterconsumersecret;
            this.ProviderSelectedCommand = DelegateCommand.FromAsyncHandler(OnProviderSelected, CanProceed);
        }

        private bool CanProceed()
        {
            return !String.IsNullOrEmpty(this.SelectedProvider);
        }
        /// <summary>
        /// When user select OAuth provider
        /// </summary>
        /// <returns></returns>
        private async Task OnProviderSelected()
        {
            IsTwitter = this.SelectedProvider == Twitter;
            IsGoogle = !istwitter;
            if (this.SelectedProvider == Googlebooks)
            {
                UserCredential credential;

                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets
                      {

                          ClientId = googleclientID,
                          ClientSecret = googleclientsecret
                      }
                    ,
                    new[] { BooksService.Scope.Books },
                    "user", CancellationToken.None, new FileDataStore("Books.ListMyLibrary"));


                // Create the service.
                var service = new BooksService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "Test",
                    });

                var bookshelves = await service.Mylibrary.Bookshelves.List().ExecuteAsync();
                Books = new ObservableCollection<Bookshelf>(bookshelves.Items);
            }
            else
            {
                //step 1
                TwitterService service = new TwitterService(twitterconsumerkey, twitterconsumersecret);
                OAuthRequestToken requestToken = service.GetRequestToken();
                Uri uri = service.GetAuthorizationUri(requestToken);
                Process.Start(uri.ToString());
            }
        }
        /// <summary>
        /// Books data from Google
        /// </summary>
        public ObservableCollection<Bookshelf> Books
        {
            get
            {
                return books;
            }
            set
            {
                books = value;
                NotifyPropertyChanged("Books");
            }
        }
        /// <summary>
        /// A behaviour of user selecting our provider dropdown
        /// </summary>
        public ICommand ProviderSelectedCommand { get; set; }
        public ObservableCollection<string> OAuthProviders
        {
            get
            {
                return new ObservableCollection<string>() { Googlebooks, Twitter };
            }
        }

        #region INotifyPropertyChanged Members
        private void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion INotifyPropertyChanged
    }
}
