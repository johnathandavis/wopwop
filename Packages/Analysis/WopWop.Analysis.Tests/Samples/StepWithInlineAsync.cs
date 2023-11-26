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
        string x = (await DelayWithRandomResultAsync()).ToString();
        
        await Task.Delay(1000);
        return int.Parse(x);
    }

    private static async Task<int> DelayWithRandomResultAsync()
    {
        Random rnd = new Random();
        await Task.Delay(1000);
        return rnd.Next();
    }
}