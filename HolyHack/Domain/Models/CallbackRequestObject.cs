using Newtonsoft.Json;

namespace Domain.Models
{
    public class CallbackRequestObject
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("from_id")]
        public int FromId { get; set; }

        [JsonProperty("date")]
        public int Date { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("reply_to_user")]
        public int ReplyToUser { get; set; }

        [JsonProperty("reply_to_comment")]
        public int ReplyToComment { get; set; }

        [JsonProperty("owner_id")]
        public int OwnerId { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("deleter_id")]
        public int DeleterId { get; set; }

        [JsonProperty("photo_id")]
        public int PhotoId { get; set; }

        [JsonProperty("video_id")]
        public int VideoId { get; set; }

        [JsonProperty("post_id")]
        public int PostId { get; set; }

        [JsonProperty("topic_owner_id")]
        public int TopicOwnerId { get; set; }

        [JsonProperty("topic_id")]
        public int TopicId { get; set; }

    }
}