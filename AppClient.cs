using System.Net.Sockets;

namespace TelnetClientServer;

public class AppClient
{
    private const string LogPrefix = $"[ {nameof(AppClient)} ]";
    
    private readonly string _hostname;
    private readonly int _port;
    private readonly TcpClient _tcpClient;

    public AppClient(string hostname, int port)
    {
        _hostname = hostname;
        _port = port;
        _tcpClient = new TcpClient();
        
        Console.WriteLine($"{LogPrefix} Created to {hostname}:{port}.");
    }

    public void Start()
    {
        Console.WriteLine($"{LogPrefix} Starting and connecting.");

        try
        {
            _tcpClient.Connect(_hostname, _port);

            new PipeStream(_tcpClient.GetStream(), Console.OpenStandardOutput(), nameof(_tcpClient), nameof(Console.OpenStandardOutput)).BeginRead();
            new PipeStream(Console.OpenStandardInput(), _tcpClient.GetStream(), nameof(Console.OpenStandardInput), nameof(_tcpClient)).BeginRead();

            var originalConsoleOut = Console.Out;
            Console.SetOut(TextWriter.Null);

            while (_tcpClient.Connected && (!Console.KeyAvailable || Console.ReadKey(true).Key != ConsoleKey.Escape))
            {
                Thread.Sleep(1);
            }

            Console.SetOut(originalConsoleOut);
        }
        catch (Exception exception)
        {
            Console.WriteLine($"{LogPrefix} Error when try to connect: {exception.Message}");
        }

        Console.WriteLine();
        Console.WriteLine($"{LogPrefix} Terminated.");
    }
}