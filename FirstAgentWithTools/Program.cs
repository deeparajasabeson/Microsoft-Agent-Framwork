using Microsoft.Extensions.Configuration;
using Microsoft.Agents.AI;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Responses;

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var model = configuration["OpenAI:ModelId"]
    ?? throw new InvalidOperationException(
        "Missing OpenAI:ModelId in user secrets.");

var apiKey = configuration["OpenAI:ApiKey"]
    ?? throw new InvalidOperationException(
        "Missing OpenAI:ApiKey in user secrets.");

#pragma warning disable OPENAI001
var chatClient = new OpenAIClient(apiKey).GetResponsesClient();

var agent = chatClient.AsAIAgent( 
    model : model,
instructions: """ 
You are an AI assistant controlling a robot car capable of performing  
basic moves: forward, backward, turn left, turn right, and stop. 
You have to break down the provided complex commands into the basic  
moves you know. 
Respond only with the moves, without any additional explanations. 
Execute the corresponding basic moves using the available tools. 
""", 
name: "RobotCarAssistant",
tools: [.. MotorTools.AsAITools()] 
);
 
AgentSession session = await agent.CreateSessionAsync(); 

while (true)
{
    Console.Write("User: ");

    var input = Console.ReadLine();
    if (string.IsNullOrEmpty(input))
        continue;

    var command = input;
    if (string.IsNullOrWhiteSpace(command)) break;
    
    var prompt = $"""
    Complex command: "{input}"
    """;

    AgentResponse response = await agent.RunAsync(prompt, session);
    Console.WriteLine($"Assistant: {response.Text}");    
}
