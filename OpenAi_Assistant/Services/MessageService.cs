using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenAi_Assistant.Interfaces;
using OpenAi_Assistant.Models;
using System.Text;

namespace OpenAi_Assistant.Services
{
    internal class MessageService : IMessageService
    {
        private MessageModel messageModel;
        private readonly HttpClient httpClient;
        private readonly List<MessageModel> AllMessages;
        public MessageService(HttpClient _httpClient) 
        {
            
            httpClient = _httpClient;
            AllMessages = new List<MessageModel>();
        }

        // Method to create a new message and add it to the current thread
        public async Task<MessageModel> CreateMsg(string msg,string role,ThreadModel _currentThread)
        {
            ThreadModel currentThread = _currentThread;
            if (msg == null || msg == "")
            {
                return null;
            }
            var requestUri = $"https://api.openai.com/v1/threads/{currentThread.thread_id}/messages";
            var requestBody = new
            {
                role = role,
                content = msg
            };
            var response = await httpClient.PostAsync(requestUri, new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json"));
            var responseContent = await response.Content.ReadAsStringAsync();
            // Extract the thread ID from the response
            var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            messageModel = new MessageModel
            {
                msgId = responseObject?.id,
                createdAt = responseObject?.created_at,
                thread_id = responseObject?.thread_id,
                role = responseObject?.role,
                msgContent = responseObject?.content,
                msgValue = responseObject?.content[0]?.text?.value

            };
    
            messageModel.msgAsString = await GetJTokenContent(messageModel.msgValue); // Store the msg content value in the message model
            AllMessages.Add(messageModel);
            return messageModel;
        }
        public async Task<string>GetJTokenContent(JToken value)
        {
            //This method will return the text value of the msg content.
            var content_value = value.Value<string>();
            if (content_value != null)
            {
                return content_value;
            }
            return "Error getting the JString content..."; // On null value return error

        }

        public async Task<string> GetLatestResponse(ThreadModel currentThread)
        {
            var requestUri = $"https://api.openai.com/v1/threads/{currentThread.thread_id}/messages";
            
            var response = await httpClient.GetAsync(requestUri);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Extract the thread ID from the response
            var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
            var message_id = responseObject?.first_id;  // Get the id of the last msg in the list.
            if(!response.IsSuccessStatusCode)
            {
                return "Error: Could not fetch msg from list.";
            }

            var msgUri = $"https://api.openai.com/v1/threads/{currentThread.thread_id}/messages/{message_id}";
            var msgResponse = await httpClient.GetAsync(msgUri);
            var msgResponseContent = await msgResponse.Content.ReadAsStringAsync();
            var msgResponseObject = JsonConvert.DeserializeObject<dynamic>(msgResponseContent);
            if(!msgResponse.IsSuccessStatusCode)
            {
                return "Error: Could not fetch msg from last id.";
            }
            if (msgResponseObject != null)
            {
                MessageModel _messageModel = new MessageModel
                {
                    msgId = msgResponseObject?.id,
                    //
                    thread_id = msgResponseObject?.thread_id,
                    role = msgResponseObject?.role,
                    msgContent = msgResponseObject?.content,
                    msgValue = msgResponseObject?.content[0]?.text?.value

                };
                _messageModel.msgAsString = await GetJTokenContent(_messageModel.msgValue); // Store the msg content value in the message model
                AllMessages.Add(_messageModel);

                return _messageModel.msgAsString;
            } else
            {
                return "Error: Message was null";
            }
        }
    }
}

