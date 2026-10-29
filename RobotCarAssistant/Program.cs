using Microsoft.Extensions.Configuration;
using Microsoft.Agents.AI;
using OpenAI;
using OpenAI.Chat;

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var model = configuration["OpenAI:ModelId"];
var apiKey = configuration["OpenAI:ApiKey"];
var chatClient = new OpenAIClient(apiKey)
                        .GetChatClient(model);
AIAgent agent = chatClient.AsAIAgent("""
## Persona
You are an AI assistant controlling a roto car capable of performing basic moves: forward, backwar, turn left, turn right, and stop.

## Action
You have to break down the provided complex commands into the basic moves you know.

## Template
Use a JSON array like [move1, move2, move3] for the response.
Respond only with the moves, without any additional explanations.
""");

var prompt = """
Complex command:
"Move forward, turn left, forward, and return to the same place where it  
started" 
""";
// "There is a tree directly in front of the car.  Avoid it and then return to the original path."
//AgentResponse response = await agent.RunAsync(prompt);
//Console.WriteLine(response.Text);

AgentSession session = await agent.CreateSessionAsync();

var prompt1 = "go 10 meters forward then turn back";
Console.WriteLine($"User: {prompt1}");
Console.Write("Assistant: "); 
await foreach (AgentResponseUpdate update in agent 
.RunStreamingAsync(prompt1, session))  //❶ 
{ 
Console.Write(update.Text); 
} 
Console.WriteLine();

var prompt2 = "repeat the last maneuvers";
Console.WriteLine($"User: {prompt2}");
var response2 = await agent.RunAsync(prompt2, session);
Console.WriteLine($"Agent: {response2.Text}");
Console.WriteLine("\nHistory:");

if (session.TryGetInMemoryChatHistory(out var history))
{
    foreach (var message in history)
    {
        Console.WriteLine($"{message.Role}:  {message.Text}");
    }
}