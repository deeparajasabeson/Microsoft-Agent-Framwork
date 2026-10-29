using Microsoft.Extensions.AI;
using System.ComponentModel;

public static class MotorTools
{
    private const int Delay = 1000;

    static public IEnumerable<AITool> AsAITools()
    {
        yield return AIFunctionFactory.Create(TurnRight);
        yield return AIFunctionFactory.Create(TurnLeft);
        yield return AIFunctionFactory.Create(Stop);
        yield return AIFunctionFactory.Create(Forward);
        yield return AIFunctionFactory.Create(Backward);
    }

    [Description("Basic command: Moves the robot car backward.")]
    public static async Task<string> Backward(
        [Description("The distance (in meters) backward.")] int distance)
    {
        Console.WriteLine($"[{DateTime.Now:hh:mm:ss:fff}] MOTORS: Backward: {distance}m");
        await Task.Delay(Delay);
        return $"Moved backward for {distance} meters.";
    }

    [Description("Basic command: Moves the robot car forward.")]
    public static async Task<string> Forward(
        [Description("The distance (in meters) forward.")] int distance)
    {
        Console.WriteLine($"[{DateTime.Now:hh:mm:ss:fff}] MOTORS: Forward: {distance}m");
        await Task.Delay(Delay);
        return $"Moved forward for {distance} meters.";
    }
    
    [Description("Basic command: Stops the robot car.")]
    public static async Task<string> Stop()
    {
        Console.WriteLine($"[{DateTime.Now:hh:mm:ss:fff}] MOTORS: Stop");
        await Task.Delay(Delay);
        return "Stopped the robot car.";
    }
    
    [Description("Basic command: Turns the robot car left.")]
    public static async Task<string> TurnLeft(
        [Description("The angle (in degrees) to turn left.")] int angle)
    {
        Console.WriteLine($"[{DateTime.Now:hh:mm:ss:fff}] MOTORS: Turn Left: {angle}°");
        await Task.Delay(Delay);
        return $"Turned anticlockwise for {angle} degrees.";
    }

    [Description("Basic command: Turns the robot car right.")]
    public static async Task<string> TurnRight(
        [Description("The angle (in degrees) to turn right.")] int angle)
    {
        Console.WriteLine($"[{DateTime.Now:hh:mm:ss:fff}] MOTORS: Turn Right: {angle}°");
        await Task.Delay(Delay);
        return $"Turned clockwise for {angle} degrees.";
    }   
}