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
        private readonly IVkProxy _vkProxy;
        public string AccessToken =
            "0b14f43ca3fb677819bcf0e590a6d5e03ece904638235f3a45c90036267667f1fe0ed0e43942c11a9f05c";

        private readonly IDictionary<EventType, Func<CallbackRequest, string>> _methods;
        private readonly IDictionary<string, Action<CallbackRequest>> _commands;
        private readonly Uri uri = new Uri(@"https://api.vk.com/method/messages.send");

        public VkCallbackApiHandler(IVkProxy vkProxy)
        {
            _vkProxy = vkProxy;
            _methods = new Dictionary<EventType, Func<CallbackRequest, string>>()
            {
                {EventType.Сonfirmation, Confirm},
                {EventType.MessageNew, SendMessage},
                
            };
            _commands = new Dictionary<string, Action<CallbackRequest>>()
            {
                {"объединения", SendCommunityList },
                {"карта", SendBuildingsMap },
                {"расписание", SendSchedule },
                {"tag", SendPostsFromMisisGroup }
            };
        }

        private void SendPostsFromMisisGroup(CallbackRequest callbackRequest)
        {
            if (_vkProxy.AccessToken.Equals(String.Empty))
            {
                _vkProxy.GetPosts(GetInitializeWebClient(callbackRequest) , uri , callbackRequest.Object.Body);
            }
        }

        private void SendSchedule(CallbackRequest callbackRequest)
        {
            var buildedClient = GetInitializeWebClient(callbackRequest);
            buildedClient.QueryString.Add("attachment" , "doc141213476_466619620,doc141213476_466619622,doc141213476_466619680,doc141213476_466619682,doc141213476_466619754,doc141213476_466619783");
            var res = buildedClient.DownloadString(uri);
        }

        private void SendBuildingsMap(CallbackRequest callbackrequest)
        {
            var buildedClient = GetInitializeWebClient(callbackrequest);
            buildedClient.QueryString.Add("attachment" , "photo-166642622_456239017");
            var res = buildedClient.DownloadString(uri);

        }

        private void SendCommunityList(CallbackRequest callbackRequest)
        {
            var buildedClient = GetInitializeWebClient(callbackRequest);
            var message = "Список объединений : \n" +
                          "Лига разработчиков МИСиС - https://vk.com/lodmisis \n" +
                          "НИТУ «МИСиС» - https://vk.com/nust_misis \n" +
                          "ТурКлуб МИСиС - https://vk.com/tk_misis \n" +
                          "Студенческий совет НИТУ «МИСиС» - https://vk.com/studentmisis \n" +
                          "Студсовет Общежитий НИТУ \"МИСиС\" - https://vk.com/studac_new \n" +
                          "Центр карьеры НИТУ \"МИСиС\" - https://vk.com/ck_misis \n " +
                          "Цитатник МИСиС - https://vk.com/public80799683 \n" +
                          "СКБ НИТУ \"МИСиС\" - https://vk.com/skbmisis \n" +
                          "СНО НИТУ «МИСиС» - https://vk.com/sno_misis \n " +
                          "Lignum | Литературное сообщество НИТУ \"МИСиС\" - https://vk.com/lignummisis \n" +
                          "КВН МИСиС - https://vk.com/kvn_misis \n" +
                          "Управление культуры и молодежной политики МИСиС - https://vk.com/ykmp_misis \n" +
                          "Эндаумент-фонд НИТУ \"МИСиС\" - https://vk.com/ef.misis \n " +
                          "Лингвистика в МИСиС - https://vk.com/lingvistikavmisis \n" +
                          "СтудОК НИТУ \"МИСиС\" - https://vk.com/studok_misis \n" +
                          "Киберспорт НИТУ \"МИСиС\" - https://vk.com/cybersport_nustmisis \n" +
                          "Волонтерский центр НИТУ \"МИСиС\" - https://vk.com/volunteer_misis \n" +
                          "Хоккейный клуб НИТУ \"МИСиС\" \"Стальные медведи\" - https://vk.com/fan.steelbears \n" +
                          "Кейс-движение CUP MISIS CASE - https://vk.com/cupmisiscase \n " +
                          "Сделано Лигой Разработчиков - https://lod-misis.ru"

                ;
                          
            buildedClient.QueryString.Add("message", message);
            var res = buildedClient.DownloadString(uri);
        }

        private string SendMessage(CallbackRequest callbackRequest)
        {
            var command = callbackRequest.Object.Body;
            var toExecute = _commands.SingleOrDefault(pair => pair.Key.Equals(command.Trim().ToLower())).Value;
            if (toExecute == null)
            {
                if (command.Contains("tag"))
                {
                    var crutchCommand = _commands.SingleOrDefault(pair => pair.Key.Equals("tag")).Value;
                    callbackRequest.Object.Body = callbackRequest.Object.Body.Split(' ')[1];
                    crutchCommand(callbackRequest);
                }
                else
                {
                    SendCommandList(callbackRequest);
                }
            }
            else
            {
                toExecute(callbackRequest);
            }
            return "ok";
        }

        private void SendCommandList(CallbackRequest callbackRequest)
        {
            var buildedClient = GetInitializeWebClient(callbackRequest);
            var message = "Список команд : \n" +
                          "1)hashtag - получение записи по тегу, например tag студент \n"  +
                          "2) объединения - получить список студенческих объединений университета \n" +
                          "3) расписание - расписание занятий \n"+
                          "4) карта - карта корпусов \n" +
                          "Сделано Лигой Разработчиков - https://lod-misis.ru/"
                          ;
            buildedClient.QueryString.Add("message",message);
            var res = buildedClient.DownloadString(uri);

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