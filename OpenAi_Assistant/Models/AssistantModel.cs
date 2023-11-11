using OpenAi_Assistant.Models;


namespace OpenAi_Assistant.Models
{
    public class AssistantModel
    {
        public string? id { get; set; }
        public string? instructions { get; set; }
        public string? name { get; set; }

        public string? tool { get; set; }

        public string? description { get; set; }

        public int? created_at { get; set; }

        public string? apimodel { get; set; }


    }
}
