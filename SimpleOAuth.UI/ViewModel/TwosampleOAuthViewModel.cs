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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SimpleOAuth.UI.ViewModel
{
    public class TwosampleOAuthViewModel : INotifyPropertyChanged
    {
        private bool isfacebook;
        private bool isgoogle;
        private ObservableCollection<Bookshelf> books;
        private const string Googlebooks = "Google Books";
        private const string Facebook = "Facebook";
        private string googleclientID;
        private string googleclientsecret;
        public string SelectedProvider { get; set; }
        public bool IsFacebook { get {
            return isfacebook;
            }
            set {
                isfacebook = value;
                NotifyPropertyChanged("IsFacebook");
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
        public TwosampleOAuthViewModel(string googleclientID, string googleclientsecret)
        {
            this.googleclientID = googleclientID;
            this.googleclientsecret = googleclientsecret;
            this.ProviderSelectedCommand = DelegateCommand.FromAsyncHandler(OnProviderSelected, CanProceed);
        }

        private bool CanProceed()
        {
            return true;
        }

        private async Task<int> OnProviderSelected()
        {
            IsFacebook = this.SelectedProvider == Facebook;
            IsGoogle = !isfacebook;
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
            return 0;
        }

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
        public ICommand ProviderSelectedCommand { get; set; }
        public ObservableCollection<string> OAuthProviders
        {
            get
            {
                return new ObservableCollection<string>() { Googlebooks, Facebook };
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
