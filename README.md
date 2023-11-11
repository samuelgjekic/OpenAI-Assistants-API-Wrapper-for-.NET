# OpenAi Assistant .NET API Wrapper

Dotnet SDK for OpenAI Assistants API (Currently Beta)
_Unofficial._
_OpenAI doesn't have any official .Net SDK._

Link to the OpenAI Assistant page:
https://platform.openai.com/docs/assistants/overview

**Install package:**
```
dotnet add package OpenAi_Assistant --version 1.0.3
``` 

```
NuGet\Install-Package OpenAi_Assistant -Version 1.0.3
``` 

### Information
Last week the new OpenAI Assistant API became available, i started testing it out and this is the SDK i created for it. 
Keep in mind the API is still in beta, and this wrapper is far from finished.

- **Accepting Contributions**: I welcome contributions from the community to enhance and expand the functionality of this wrapper. Whether you want to add new features, improve existing code, or fix bugs, your contributions are highly appreciated!


### Changelog 1.0.3
```
- Added methods to modify or delete existing assistant
- Fixed some code issues

``` 

### Changelog 1.0.2

```
- Added AssistantService to handle interactions with the OpenAI Assistants API.
- Introduced AssistantModel object to represent the properties of an assistant.
- Implemented IRunService interface to define the necessary functionalities for running the assistant.
- Updated the Assistant object to include relevant properties obtained when creating the assistant through the OpenAI Assistants API, such as ID and Name.

These changes enhance the professionalism and functionality of the codebase, providing a more robust and efficient system for managing and running assistants.
``` 

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
await aiService.CreateThread();


// Send a new message to the thread with the user role.
await aiService.SendMsgToThread("msg","user");

// Start the run operation
await aiService.RunAssistant();

// Finally get the response from the assistant
var response = await aiService.GetResponseFromAssistant();

Console.WriteLine(response);

// You should dispose of the aiService when you are no longer using it.
aiService.Dispose();


// You can now retrieve assistant properties because the CreateAssistant method now returns the assistant object.
// Example:
var assistant_id = assistant.id;


// To modify existing assistant, change the values of the assistant object
assistant.name = "new name";
assistant.instructions = "new instructions";

// After changing the assistant object you will have to confirm the changes by calling the API to modify the assistant.
// This will send the modified assistant object and update the assistant in the thread.
assistant = aiService.assistant.ModifyAssistant(assistant);

// Delete existing assistant
assistant = aiService.assistant.DeleteAssistant(assistant);

``` 


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
