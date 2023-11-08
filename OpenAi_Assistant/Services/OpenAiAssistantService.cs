using Newtonsoft.Json;
using OpenAi_Assistant.Models;
using System.Text;
using OpenAi_Assistant.Interfaces;
using OpenAi_Assistant.OpenAiAssistant.Services;

namespace OpenAi_Assistant.Services
{
    public partial class OpenAiAssistantService : IOpenAiAssistantService ,IDisposable
    {


        /// <summary>
        ///     Service to make use of the assistant API provided by OpenAI.
        ///     Related guide: <a href="https://platform.openai.com/docs/assistants/">OpenAI Assistants API</a>
        /// </summary>
        private string apiKey { get; set; }
        private readonly HttpClient httpClient;
        private readonly bool _disposeHttpClient; // <<<<< needs to be implemented.
        private string assistantId { get; set; } // the id of the assistant
   

        public OpenAiAssistantService(string ApiKey)
        {
            try
            {
                apiKey = ApiKey;
                httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                // During beta stage of the Assistant API, we need to add OpenAI-Beta as request header.
                httpClient.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v1");
            }
            catch(Exception AssistantConstructorException) 
            {
                Console.WriteLine("Error:" + AssistantConstructorException.Message); 
            }
        }

  
        public async Task<string> CreateAssistant(string apimodel,string name,string tool, string instructions) 
        {
            instructions = "You are a personal math tutor. Write and run code to answer math questions.";
            name = "Mattematikern";
            tool = "code_interpreter";
            /// <summary>
            ///     Creates the ai assistant with the given parameters.
            /// </summary>
            /// 
            try
            {
                var requestUri = "https://api.openai.com/v1/assistants";
                var requestBody = new
                {
                    name = name,
                    instructions = instructions,
                    tools = new[] { new { type = tool } },
                    model = apimodel
                };

                var response = await httpClient.PostAsync(requestUri, new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json"));
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response to get the thread ID
                    assistantId = responseObject?.id;

                } else
                {
                    return "Assistant creation request was not successful... ";
                }
                
                return responseContent ;

            } catch (Exception CreateAssistantException)
            {
                return CreateAssistantException.Message;
            }
        }

        /// <summary>
        ///     Method to create a new thread.
        /// </summary>
        public ThreadModel currentThread { get; set; } // The current thread object
        public async Task<string> CreateThread()
        {
            try
            {        
                ThreadService threadService = new ThreadService(httpClient);
                currentThread =  await  threadService.CreateThread();
                return currentThread.thread_id;
               
            } catch (Exception SendAndRecieveException)
            {
                return SendAndRecieveException.Message;
            }

        }

        /// <summary>
        ///     Method to send and recieve messages from/to thread.
        /// </summary>
        ///
        public async Task<string> SendMsgToThread(string msg,string role)
        {
            MessageModel messageModel= new MessageModel();
            MessageService msgService = new MessageService(httpClient);
            messageModel = await msgService.CreateMsg(msg,role,currentThread);
            if(messageModel != null) 
            {
                return "Added new user message to thread.";
            }
            return "Error: Could not add new user message to thread";
        }
        public async Task<string> RunAssistant()
        {
            try
            {
                RunService runService = new RunService(httpClient);
                var response = await runService.CreateRun(currentThread, assistantId); // Run the assistant with given thread & assistant id.
                if (response == true)
                {
                    return "Successfully completed run operation.";

                }else
                {
                    return "Could not complete run operation...";
                }
            }catch (Exception RunAssistantError)
            {
                return "Error:" + RunAssistantError.Message; // If assistant exception return the exception message. 
            }
        }

        public async Task<string> GetResponseFromAssistant()
        {
            MessageService messageService = new MessageService(httpClient);
            var response = await messageService.GetLatestResponse(currentThread);
            return response;
        }
        
        
        public void Dispose()
        {
            httpClient.Dispose();   // Dispose the http client when finished. 
        }
    }
}
