using System.Net.NetworkInformation;

namespace LocalExecution;

using WopWop.Core;

public class Program
{
    public const int Pi = 3;
    public int num = 17;
    public int Avg { get; set; }
    public static async RemoteTask<int> ProcessAsync(int z)
    {
        int x = 0;
        x = z;
        Avg = num;
        num = Pi;
    }
}