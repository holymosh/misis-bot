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

        public string AccessToken =
            "0b14f43ca3fb677819bcf0e590a6d5e03ece904638235f3a45c90036267667f1fe0ed0e43942c11a9f05c";

        private readonly IDictionary<EventType, Func<CallbackRequest, string>> _methods;
        private readonly IDictionary<string, Action<CallbackRequest>> _commands;
        private readonly Uri uri = new Uri(@"https://api.vk.com/method/messages.send");

        public VkCallbackApiHandler()
        {
            _methods = new Dictionary<EventType, Func<CallbackRequest, string>>()
            {
                {EventType.Сonfirmation, Confirm},
                {EventType.MessageNew, SendMessage}
            };
            _commands = new Dictionary<string, Action<CallbackRequest>>()
            {
                {"объединения", SendCommunityList }
            };
        }

        private void SendCommunityList(CallbackRequest callbackRequest)
        {
            var buildedClient = GetInitializeWebClient(callbackRequest);
            var message = "Список объединений : \n" +
                          "Студенческий совет НИТУ МИСиС - https://vk.com/studentmisis \n" +
                          "Центр карьеры - https://vk.com/ck_misis \n" +
                          "Клуб интернациональной дружбы - https://vk.com/clubinternational \n" +
                          "Наука - https://vk.com/sno_misis \n" +
                          "Профком - https://vk.com/profkom_misis \n" +
                          "Научный медиацентр - https://vk.com/sciencemisis \n" +
                          "MISiS Media - https://vk.com/misis_media \n" +
                          "Эндаумент фонд - https://vk.com/ef.misis \n" +
                          "Спорт клуб - https://vk.com/sportclubmisis \n" +
                          "Туризм - https://vk.com/tk_misis \n";
            buildedClient.QueryString.Add("message", message);
            var res = buildedClient.DownloadString(uri);
        }

        private string SendMessage(CallbackRequest callbackRequest)
        {
            var command = callbackRequest.Object.Body;
            var toExecute = _commands.SingleOrDefault(pair => pair.Key.Equals(command.Trim().ToLower())).Value;
            toExecute(callbackRequest);
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

        private WebClient GetInitializeWebClient( CallbackRequest callbackRequest)
        {
            var uri = new Uri(@"https://api.vk.com/method/messages.send");
            var client = new WebClient();
            client.QueryString.Add("v", "5.70");
            client.QueryString.Add("access_token", AccessToken);
            client.QueryString.Add("user_id", callbackRequest.Object.UserId.ToString());
            client.QueryString.Add("chat_id", callbackRequest.Object.UserId.ToString());
            return client;

        }
    }
}