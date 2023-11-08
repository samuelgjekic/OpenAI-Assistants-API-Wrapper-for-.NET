# OpenAi_Assistant

https://platform.openai.com/docs/assistants/overview
Dotnet SDK for OpenAI Assistants API (Currently Beta)
_Unofficial._
_OpenAI doesn't have any official .Net SDK._



### Information
Last week the new OpenAI Assistant API became available, i started testing it out and this is the SDK i created for it. 
Keep in mind the API is still in beta, and this wrapper is far from finished.

- **Accepting Contributions**: I welcome contributions from the community to enhance and expand the functionality of this wrapper. Whether you want to add new features, improve existing code, or fix bugs, your contributions are highly appreciated!

### How to use

First you create the assistant, then you create the thread, when you have a thread you can add messages to it, when you
want a response you need to run the thread and wait for the run operation to be completed, when the run operation
completes we can read the response from the ai. Example:
```c#
// Best approach would be to load the api key from env vars
var aiService = new OpenAiAssistantService("YOUR-API-KEY"); 

// Create the assistant by passing the parameters, use models from OpenAiModel.<SelectedModel>
// You can pass instructions to the assistant, for example "You are a math tutor".
var assistant = await aiService.CreateAssistant(OpenAiModel.Gpt_3_5_Turbo,"Math tutor", ToolsModel.Code_Interpreter,"You are a math tutor");

// Create the thread that the assistant will run on
var thread =  await aiService.CreateThread();


// Send a new message to the thread with the user role.
var newMsg = await aiService.SendMsgToThread("msg","user");

// Start the run operation
 var run = await aiService.RunAssistant();

// Finally get the response from the assistant
var response = await aiService.GetResponseFromAssistant();

Console.WriteLine(response);


``` 
Each object returns an error message if it fails.


**For now you can access chatGPT3 turbo and chatGPT4 in the OpenAiModel class:**
```c#
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
``` 


### ToDo

- Need to make use of IHttpClientFactory
- Need to add file support
- Clean up code
- Run Steps
