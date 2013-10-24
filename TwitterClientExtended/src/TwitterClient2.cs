using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.AspNet.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using DotNetOpenAuth.Messaging;
using System.IO;
using Newtonsoft.Json;
namespace DotNetOpenAuth.AspNet.Clients
{
    public class TwitterClient2 : TwitterClient
    {
        public TwitterClient2(string consumerKey, string consumerSecret):base(consumerKey,consumerSecret,new CookieOAuthTokenManager())
        {
        }
        protected override DotNetOpenAuth.AspNet.AuthenticationResult VerifyAuthenticationCore(DotNetOpenAuth.OAuth.Messages.AuthorizedTokenResponse response)
        {
            Dictionary<string, string> extraData = new Dictionary<string, string>();
            extraData.Add("screen_name", response.ExtraData["screen_name"]);

            HttpWebRequest request = WebWorker.PrepareAuthorizedRequest(new MessageReceivingEndpoint(new Uri(string.Format("https://api.twitter.com/1.1/users/show.json?screen_name={0}",response.ExtraData["screen_name"])), HttpDeliveryMethods.GetRequest | HttpDeliveryMethods.AuthorizationHeaderRequest), response.AccessToken);
            using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
            using(Stream stream = resp.GetResponseStream())
            using(StreamReader sr = new StreamReader(stream))
            {
                
                dynamic obj = JsonConvert.DeserializeObject<dynamic>(sr.ReadToEnd());
                /// have a lookt at the full response https://dev.twitter.com/docs/api/1.1/get/users/show 
                extraData.Add("name", Convert.ToString(obj.name));
                extraData.Add("profile_image_url", Convert.ToString(obj.profile_image_url));
                extraData.Add("location", Convert.ToString(obj.location));
                extraData.Add("statuses_count", Convert.ToString(obj.statuses_count));
            }
            return new AuthenticationResult(true,"twitter",response.ExtraData["user_id"],response.ExtraData["screen_name"],extraData);
        }
    }
}