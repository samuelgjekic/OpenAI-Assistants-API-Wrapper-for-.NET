using OpenAi_Assistant.Models;


namespace OpenAi_Assistant.Interfaces
{
    internal interface IOpenAiAssistantService
    {

        Task<AssistantModel> CreateAssistant(string apimodel, string name, string tool, string instructions);
        Task<string> CreateThread();

        Task<string> SendMsgToThread(string msg,string role);

        Task<AssistantModel> GetAssistantById(string assistant_id);

    }
}
