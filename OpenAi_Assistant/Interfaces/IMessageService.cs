using OpenAi_Assistant.Models;


namespace OpenAi_Assistant.Interfaces
{
    internal interface IMessageService
    {

        Task<MessageModel> CreateMsg(string msg, string role, ThreadModel _currentThread);

    }
}
