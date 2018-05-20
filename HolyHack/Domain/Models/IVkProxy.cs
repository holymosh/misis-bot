using System;
using System.Net;
using System.Threading.Tasks;

namespace Domain.Models
{
    public interface IVkProxy
    {
        string Code { get; set; }
        string AccessToken { get; set; }
        void GetPosts(WebClient webClient, Uri uri, string hashtag);
    }
}