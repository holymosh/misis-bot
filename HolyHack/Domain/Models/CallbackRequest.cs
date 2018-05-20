using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Domain.Models
{
    public class CallbackRequest
    {
        [JsonProperty("type"),JsonConverter(typeof(StringEnumConverter))]
        public EventType Type { get; set; }

        [JsonProperty("object")]
        public CallbackRequestObject Object { get; set; }

        [JsonProperty("group_id")]
        public int GroupId { get; set; }

        public static CallbackRequest GetDefault()
        {
            return new CallbackRequest(){Type = EventType.BoardPostNew,GroupId = 1,Object = new CallbackRequestObject(){Id = 1}};
        }

    }
}