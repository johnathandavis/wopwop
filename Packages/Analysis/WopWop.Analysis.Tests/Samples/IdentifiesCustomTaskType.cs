namespace LocalExecution;

using WopWop.Core;

public class Program
{
    public static async Task Main(string[] args)
    {
        await ProcessAsync();
    }

    public static async RemoteTask<int> ProcessAsync()
    {
        await Task.Delay(1000);
        
        await Task.Delay(1000);
    }
}