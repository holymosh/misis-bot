using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;

namespace Domain
{
    public class VkCallbackApiHandler : IVkCallbackApiHandler
    {
        public string ConfirmationToken { get; set; }
        public string Secret { get; set; }
        private readonly IDictionary<EventType, Func<CallbackRequest, string>> _methods;

        public VkCallbackApiHandler()
        {
            _methods = new Dictionary<EventType, Func<CallbackRequest, string>>()
            {
                {EventType.Сonfirmation, Confirm},
                {EventType.MessageNew, SendMessage}
            };
        }

        private string SendMessage(CallbackRequest callbackRequest)
        {
            var uri = new Uri(@"https://api.vk.com/method/messages.send");
            var client = new WebClient();
            client.QueryString.Add("v","5.70");
            client.QueryString.Add("access_token", "0b14f43ca3fb677819bcf0e590a6d5e03ece904638235f3a45c90036267667f1fe0ed0e43942c11a9f05c");
            client.QueryString.Add("user_id",callbackRequest.Object.UserId.ToString());
            client.QueryString.Add("message","ЛИГА СИЛА");
            client.QueryString.Add("chat_id", callbackRequest.Object.UserId.ToString());
            var res = client.DownloadString(uri);
            return "ok";
        }

        private string Confirm(CallbackRequest callbackRequest)
        {
            return "f9d65ec1";
        }

        public string Handle(CallbackRequest callbackRequest)
        {
            var execute = _methods.SingleOrDefault(pair => pair.Key.Equals(callbackRequest.Type)).Value;
            return execute(callbackRequest);
        }
    }
}