namespace LocalExecution;

using WopWop.Core;

public class Program
{
    public static async RemoteTask<int> ProcessAsync()
    {
        int x = 0;
        int y = 4;
        int z = x + y;
        x = y - z;
    }
}