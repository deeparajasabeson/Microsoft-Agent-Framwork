using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAI;

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var model = configuration["OpenAI:ModelId"]
    ?? throw new InvalidOperationException(
        "Missing OpenAI:ModelId in user secrets.");

var apiKey = configuration["OpenAI:ApiKey"]
    ?? throw new InvalidOperationException(
        "Missing OpenAI:ApiKey in user secrets.");  

var prompt = """
You are an AI assistant controlling a robot car capable of performing basic moves: forward, backward, turn left, turn right, and stop.
You have to break down the provided complex commands into the basic moves you know.
There is a tree in front of the robot car.
Avoid it and resume the original path.
Respond with a JSON array like [move1, move2, move3].
Do not respond with reasoning, comments, or any additional text.
""";

Console.WriteLine("Press any key to see the User Prompt...");
Console.ReadKey();
Console.WriteLine($"User: {prompt}");

IChatClient baseClient = new OpenAIClient(apiKey)
                                .GetChatClient(model)
                                .AsIChatClient();
using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Trace);
});

IChatClient chatClient = new ChatClientBuilder(baseClient)
                                .UseLogging(loggerFactory)
                                .Build();

ChatResponse chatResponse = await chatClient.GetResponseAsync(prompt);

Console.WriteLine("Press any key to see the chatResponse...");
Console.ReadKey();
Console.WriteLine($"Assistant (IChatClient) : {chatResponse.Text }");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();