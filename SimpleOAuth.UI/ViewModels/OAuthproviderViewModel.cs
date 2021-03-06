﻿
using Google.Apis.Books.v1;
using Google.Apis.Books.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using SimpleOAuth.UI.Models;
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
using Google.Apis.Util.Store;
using Google.Apis.Auth.OAuth2;


namespace SimpleOAuth.UI.ViewModel
{
    /// <summary>
    /// User landed oauth provider selection view model
    /// </summary>
    public class OAuthproviderViewModel : BaseViewModel
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
        /// <param name="twitterconsumerkey">Twitter consumer key</param>
        ///   <param name="twitterconsumersecret">Twitter consumer secret</param>
        public OAuthproviderViewModel(string googleclientID, string googleclientsecret, string twitterconsumerkey, string twitterconsumersecret)
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
                credential = await GetGoogleCredential();
                var service = GetGoogleBookService(credential);
                var bookshelves = await GetBookShelves(service);
                Books = new ObservableCollection<Bookshelf>(bookshelves.Items);
            }
            else
            {
                //step 1
                var model = await GetTwitterRequestToken(twitterconsumerkey, twitterconsumersecret);
                Process.Start(model.uri.ToString());
                //navigate to second page
                Twitteraccesstokenwindow twitteraccesswindow = new Twitteraccesstokenwindow(model.requestToken, model.service);
                Navigator.NavigationService.Navigate(twitteraccesswindow);
            }
        }
        /// <summary>
        /// Get book shelves
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        private static async Task<Bookshelves> GetBookShelves(BooksService service)
        {
            var bookshelves = await service.Mylibrary.Bookshelves.List().ExecuteAsync();
            return bookshelves;
        }
        /// <summary>
        /// Get google book service
        /// </summary>
        /// <param name="credential"></param>
        /// <returns></returns>
        private static BooksService GetGoogleBookService(UserCredential credential)
        {
            var service = new BooksService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Test",
            });
            return service;
        }
        /// <summary>
        /// Get google credential
        /// </summary>
        /// <returns></returns>
        public async Task<UserCredential> GetGoogleCredential()
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
            return credential;
        }
        /// <summary>
        /// Get Twitter Request Token
        /// </summary>
        /// <param name="twitterconsumerkey"></param>
        /// <param name="twitterconsumersecret"></param>
        /// <returns></returns>
        public async Task<Twitterrequesttokenmodel> GetTwitterRequestToken(string twitterconsumerkey, string twitterconsumersecret)
        {
            Twitterrequesttokenmodel model = new Twitterrequesttokenmodel();
            model.service = new TwitterService(twitterconsumerkey, twitterconsumersecret);
            model.requestToken = model.service.GetRequestToken();
            model.uri = model.service.GetAuthorizationUri(model.requestToken);
            return model;
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
        /// <summary>
        /// a behaviour to change to next page for twitter
        /// </summary>
        public ICommand ChangetoSecondpageTwitterCommand { get; set; }

        public ObservableCollection<string> OAuthProviders
        {
            get
            {
                return new ObservableCollection<string>() { Googlebooks, Twitter };
            }
        }


    }
}
