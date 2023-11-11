using Newtonsoft.Json;
using OpenAi_Assistant.Models;
using System.Text;
using OpenAi_Assistant.Interfaces;
using OpenAi_Assistant.OpenAiAssistant.Services;
using System.Xml.Linq;

namespace OpenAi_Assistant.Services
{
    /// <summary>
    ///     Service to make use of the assistant API provided by OpenAI. See documentation for more information.
    ///     Related guide: <a href="https://github.com/samuelgjekic/OpenAI-Assistants-API-Wrapper-for-.NET">Documentation</a>
    /// </summary>
    public partial class OpenAiAssistantService : IOpenAiAssistantService ,IDisposable
    {


       
        private string apiKey { get; set; }
        private readonly HttpClient httpClient;
        private readonly bool _disposeHttpClient; // <<<<< needs to be implemented.


        // We create the objects needed for the assistant
        private AssistantModel assistantModel { get; set; } 
        public  AssistantService assistant { get; set; }

        public ThreadModel currentThread { get; set; }



        ///<summary>
        /// Creates the assistant using the given parameters.
        ///<param name="apikey">The OpenAI Api key to be used, its better to load it from env variables</param>
        ///</summary>
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

        ///<summary>
        /// Creates the assistant using the given parameters.
        ///<param name="apimodel">The OpenAI model to be used</param>
        ///<param name="name">The name of the assistant</param>
        ///<param name="tool">The tool to be used, ex: ToolsModel.ai_tool</param>
        ///<param name="instructions">The instructions for the assistant</param>
        ///<returns>The AssistantObject with the assistant properties</returns>
        ///</summary>
        public async Task<AssistantModel> CreateAssistant(string apimodel,string name,string tool, string instructions) 
        {
           



            assistantModel = new AssistantModel()
            {
                name = name,
                tool = tool,
                instructions = instructions,
                apimodel = apimodel,

            };
            assistant = new AssistantService(httpClient);
            assistantModel = await assistant.CreateAssistant(assistantModel);
            return assistantModel;
        }


        ///<summary>
        /// Create the thread required to run the assistant
        ///<returns>Returns the thread id</returns>
        ///</summary>
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
        ///<summary>
        /// Run the thread to get response from assistant
        ///</summary>
        public async Task<string> RunAssistant()
        {
         
            try
            {
                RunService runService = new RunService(httpClient);
                var response = await runService.CreateRun(currentThread, assistantModel.id); // Run the assistant with given thread & assistant id.
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
        ///<summary>
        /// Get the response from the assistant
        /// <returns>Response from the assistant as string</returns>
        ///</summary>
        public async Task<string> GetResponseFromAssistant()
        {
       
            MessageService messageService = new MessageService(httpClient);
            var response = await messageService.GetLatestResponse(currentThread);
            return response;
        }

        ///<summary>
        /// Dispose the assistant service when you are not using it anymore
        ///</summary>
        public void Dispose()
        {
        
            httpClient.Dispose();
        }
    }
}
