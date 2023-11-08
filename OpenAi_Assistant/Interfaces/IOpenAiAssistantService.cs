

namespace OpenAi_Assistant.Interfaces
{
    internal interface IOpenAiAssistantService
    {

        Task<string> CreateAssistant(string apimodel, string name, string tool, string instructions);
        Task<string> CreateThread();

        Task<string> SendMsgToThread(string msg,string role);


    }
}
