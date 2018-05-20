using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model.RequestParams;

namespace Domain.Models
{
    public class VkProxy : IVkProxy
    {
        public string Code { get; set; }
        public string AccessToken { get; set; }
        private readonly VkApi _api;
        private IVkProxy _vkProxyImplementation;

        public VkProxy()
        {
            AccessToken = string.Empty;
            _api = new VkApi();
            _api.Authorize(new ApiAuthParams()
            {
                //UserId = 358536316,
                Login = "a.kolesnikov.official@gmail.com",
                Password = "Rjktcj564236",
                ApplicationId = 6484929,
                Settings = Settings.All,
            });
        }

        public void GetPosts(WebClient webClient, Uri uri, string hashtag)
        {
            webClient.QueryString.Add("message","");
            webClient.QueryString.Add("attachment", "");
            var posts = _api.Wall.Get(new WallGetParams()
            {
                OwnerId = -62258607,
            });
            var sorted = posts.WallPosts.Where(post => post.Text.Contains("#"+hashtag));
            foreach (var postToSend in sorted)
            {
                webClient.QueryString.Remove("message");
                //webClient.QueryString.Remove("attachment");
                webClient.QueryString.Add("message",postToSend.Text);
                //webClient.QueryString.Add("attachment",postToSend.Attachment);
                webClient.DownloadString(uri);
            }
        }

    }
}