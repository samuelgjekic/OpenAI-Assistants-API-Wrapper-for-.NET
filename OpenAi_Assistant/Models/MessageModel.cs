using Newtonsoft.Json.Linq;


namespace OpenAi_Assistant.Models
{
    public class MessageModel
    {
        public string msgId { get; set; }
        public int createdAt { get; set; }
        public string thread_id { get; set; }
        public string role { get; set; }
        public JArray msgContent { get; set; }
        public JToken msgValue { get; set; }

        public string? msgAsString { get; set; }
        private string assistant_id { get; set; }
        public string run_id { get; set; }

        
        
        
    }
}
