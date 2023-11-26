namespace LocalExecution;

using WopWop.Core;

public class Program
{
    public static async Task Main(string[] args)
    {
        await ProcessAsync(7);
    }

    public static async RemoteTask<int> ProcessAsync(int parameter)
    {
        if (parameter % 2 == 0)
        {
            await Task.Delay(1000);
        }
        else
        {
            Console.WriteLine("No delay for you.");
        }

        return await DelayWithRandomResultAsync();
    }

    private static async Task<int> DelayWithRandomResultAsync()
    {
        Random rnd = new Random();
        await Task.Delay(1000);
        return rnd.Next();
    }
}