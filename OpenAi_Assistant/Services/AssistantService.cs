using Newtonsoft.Json;
using OpenAi_Assistant.Models;
using System.Reflection;
using System.Text;


namespace OpenAi_Assistant.Services
{
    internal class AssistantService
    {
        private readonly HttpClient httpClient;
        public AssistantService(HttpClient _httpClient) 
        {
          httpClient = _httpClient;
        }

        public async Task<AssistantModel> CreateAssistant(AssistantModel model)
        {
            
                var requestUri = "https://api.openai.com/v1/assistants";
                var requestBody = new
                {
                    name = model.name,
                    instructions = model.instructions,
                    tools = new[] { new { type = model.tool } },
                    model = model.apimodel
                };

                var response = await httpClient.PostAsync(requestUri, new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json"));
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response to get the assistants id
                    model.id = responseObject?.id;
                    model.created_at = responseObject?.created_at;


                }
                else
                {
                    return null;
                }

                return model;     
        }

        public async Task<AssistantModel> ModifyAssistant(AssistantModel model)
        {

            // Method not used in library yet, but will be used soon in a future update. 
            var requestUri = $"https://api.openai.com/v1/assistants/{model.id}";
            var requestBody = new
            {
                name = model.name,
                instructions = model.instructions,
                tools = new[] { new { type = model.tool } },
                model = model.apimodel
            };

            var response = await httpClient.PostAsync(requestUri, new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json"));
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
            if (response.IsSuccessStatusCode)
            {
                // Parse the response to get the assistants id if it changed
                model.id = responseObject?.id;
                model.created_at = responseObject?.created_at;
            }
            else
            {
                return null;
            }

            return model;
        }
    }
}
