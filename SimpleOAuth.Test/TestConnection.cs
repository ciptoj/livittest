using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AvalonUnitTesting;
using SimpleOAuth.UI;
using System.Windows;
using NUnit;
using NUnit.Mocks;
using NUnit.Framework;
using SimpleOAuth.UI.ViewModel;
using System.Configuration;
using Assert = NUnit.Framework.Assert;
using Google.Apis.Auth.OAuth2;
using SimpleOAuth.UI.Models;
namespace SimpleOAuth.Test
{
     [TestFixture]
    public class TestConnection
    {
         private string googleclientid;
         private string googleclientsecret;
         private string twitterconsumerkey;
         private string twitterconsumersecret;

         [SetUp]
         public void TestInit()
         {
             googleclientid = ConfigurationManager.AppSettings["googleClientID"];
             googleclientsecret = ConfigurationManager.AppSettings["googleClientSecret"];
             twitterconsumerkey = ConfigurationManager.AppSettings["twitterConsumerKey"];
             twitterconsumersecret = ConfigurationManager.AppSettings["twitterConsumerSecret"];
         }

        [Test]
        public void TwitterSuccessConnection()
        {
            OAuthproviderViewModel model = new OAuthproviderViewModel(googleclientid, googleclientsecret, twitterconsumerkey, twitterconsumersecret);
            var requestToken=model.GetTwitterRequestToken(twitterconsumerkey, twitterconsumersecret);
            Assert.AreEqual((object)true,(object)((Twitterrequesttokenmodel)requestToken.Result).requestToken.OAuthCallbackConfirmed);
        }
        [Test]
        public void GoogleSuccessConnection()
        {
            OAuthproviderViewModel model = new OAuthproviderViewModel(googleclientid, googleclientsecret, twitterconsumerkey, twitterconsumersecret);
            var credential = model.GetGoogleCredential();
            Assert.AreEqual(3600,((UserCredential)credential.Result).Token.ExpiresInSeconds);
        }
    }
}
