using Newtonsoft.Json;
using OpenAi_Assistant.Models;
using System.Reflection;
using System.Text;


namespace OpenAi_Assistant.Services
{
    public class AssistantService
    {
        private readonly HttpClient httpClient;
        public AssistantService(HttpClient _httpClient) 
        {
          httpClient = _httpClient;
        }

        ///<summary>
        /// Creates the assistant using the AssistantService
        ///<param name="model">The AssistantModel to be created</param>
        ///<returns>The Assistant Object</returns>
        ///</summary>
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

        ///<summary>
        /// Modifies assistant with given AssistantModel. Note that ID is given by OpenAI.
        ///<param name="model">The AssistantModel with modified values</param>
        ///<returns>The AssistantObject with the new assistant properties</returns>
        ///</summary>
        public async Task<AssistantModel> ModifyAssistant(AssistantModel model)
        {

        

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
        ///<summary>
        /// Delete assistant with given assistant id
        ///<param name="model">The AssistantModel to be deleted</param>
        ///<returns>The AssistantObject with the assistant properties</returns>
        ///</summary>


        public async Task<AssistantModel> DeleteAssistant(AssistantModel model)
        {

          

            var requestUri = $"https://api.openai.com/v1/assistants/{model.id}";
            var response = await httpClient.DeleteAsync(requestUri);
            if (response.IsSuccessStatusCode)
            {
                model.id = null;
                model.created_at = null;
                model.tool = null;
                model.name  = null;
                model.instructions = null;
                model.apimodel = null;
                model.description  = null;
                return model;
            }
            else
            {
                return model;
            }

        }
    }
}
