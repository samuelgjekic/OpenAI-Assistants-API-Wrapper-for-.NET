
namespace OpenAi_Assistant.Models
{
    public class OpenAiModel
    {


        /// <summary>
        ///     Most capable GPT-3.5 model and optimized for chat at 1/10th the cost of text-davinci-003. Will be updated with our
        ///     latest model iteration.
        /// </summary>
        public static string Gpt_3_5_Turbo => "gpt-3.5-turbo";

        /// <summary>
        ///     Same capabilities as the standard gpt-3.5-turbo model but with 4 times the context.
        /// </summary>
        public static string Gpt_3_5_Turbo_16k => "gpt-3.5-turbo-16k";


        /// <summary>
        ///     More capable than any GPT-3.5 model, able to do more complex tasks, and optimized for chat. Will be updated with
        ///     our latest model iteration.
        /// </summary>
        public static string Gpt_4 => "gpt-4";

    }
}
