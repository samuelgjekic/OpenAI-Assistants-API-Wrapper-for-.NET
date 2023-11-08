using Newtonsoft.Json;
using OpenAi_Assistant.Interfaces;
using OpenAi_Assistant.Models;


namespace OpenAi_Assistant.Services
{
    internal class ThreadService : IThreadService
    {
        private readonly HttpClient httpClient;
        public ThreadService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }
        public async Task<ThreadModel> CreateThread()
        {
                var requestUri = "https://api.openai.com/v1/threads";
                var response = await httpClient.PostAsync(requestUri, null);
                var responseContent = await response.Content.ReadAsStringAsync();


                // Extract the thread ID from the response
                var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                ThreadModel currentThread = new ThreadModel
                {
                    thread_id = responseObject?.id,
                    createdAt = responseObject?.created_at
                };
                return currentThread;
            
        }
    }
}
