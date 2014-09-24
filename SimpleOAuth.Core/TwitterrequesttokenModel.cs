using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetSharp;

namespace SimpleOAuth.UI
{
    public class Twitterrequesttokenmodel
    {
        public TwitterService service { get; set; }
        public OAuthRequestToken requestToken { get; set; }
        public Uri uri { get; set; }
                
    }
}
