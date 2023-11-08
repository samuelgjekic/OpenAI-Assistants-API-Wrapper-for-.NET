

using OpenAi_Assistant.Models;

namespace OpenAi_Assistant.Interfaces
{
    internal interface IThreadService
    {
        Task<ThreadModel> CreateThread();
    }
}
