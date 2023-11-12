using OpenAi_Assistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAi_Assistant.Interfaces
{
    internal interface IAssistantService
    {
        Task<AssistantModel> CreateAssistant(AssistantModel model);
        Task<AssistantModel> ModifyAssistant(AssistantModel model);
        Task<AssistantModel> DeleteAssistant(AssistantModel model);

        Task<AssistantModel> GetAssistantById(string assistant_id);
    }
}
