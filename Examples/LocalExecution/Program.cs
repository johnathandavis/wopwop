namespace LocalExecution;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        Console.WriteLine("Welcome to the delay show!");
        await Task.Delay(1000);
        
        if (args.Length % 2 == 0)
        {
            Console.WriteLine("Double delay penalty.");
            await Task.Delay(2000);
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