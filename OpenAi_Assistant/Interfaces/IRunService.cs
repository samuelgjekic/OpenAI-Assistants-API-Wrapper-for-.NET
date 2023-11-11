using OpenAi_Assistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAi_Assistant.Interfaces
{
    internal interface IRunService
    {
        Task<bool> CreateRun(ThreadModel currentThread, string assistantId);
        Task<bool> CheckRunStatus(ThreadModel currentThread, string run_id);
    }
}
