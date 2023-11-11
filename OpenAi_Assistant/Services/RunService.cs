using Newtonsoft.Json;
using OpenAi_Assistant.Models;
using OpenAi_Assistant.Interfaces;
using System.Runtime.CompilerServices;
using System.Text;


namespace OpenAi_Assistant.OpenAiAssistant.Services
{
    internal class RunService : IRunService

    {
        private readonly HttpClient httpClient;
        public RunService(HttpClient _httpClient)
        {
         httpClient = _httpClient;
        }

        public async Task<bool> CreateRun(ThreadModel currentThread, string assistantId)
        {
            var requestUri = $"https://api.openai.com/v1/threads/{currentThread.thread_id}/runs";
            var requestBody = new
            {
                assistant_id = assistantId,
            };
            var response = await httpClient.PostAsync(requestUri, new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json"));
            var responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            // Extract the thread ID from the response
            var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
            var run_id = responseObject?.id.ToString();

            var isCompletedStatus = await CheckRunStatus(currentThread, run_id); // Send the current thread and run operation to wait for completion.

            if(isCompletedStatus)
            {
                return true;
            } else
            {
                return false;
            }
            
            
        }

        // Method to check what status the run operation has, the method returns true when run method has completed.
        public async Task<bool> CheckRunStatus(ThreadModel currentThread, string run_id)
        {
            var requestUri = $"https://api.openai.com/v1/threads/{currentThread.thread_id}/runs/{run_id}";
            while (true)
            {
                var response = await httpClient.GetAsync(requestUri);
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);

                
                if (!response.IsSuccessStatusCode)
                {
                    // Handle error when response is not successful
                    return false;
                } else
                {
                    if (responseObject != null && responseObject.status != null && responseObject.status.ToString() == "completed")
                    {
                        return true;
                    }
                }
            }

        }



    }
       
 }
    

