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
        Console.WriteLine($"I'm a client.");
        
        _tcpClient.Connect(_hostname, _port);

        var writer = new StreamWriter(_tcpClient.GetStream());
        writer.AutoFlush = true;
        writer.WriteLine("Hello! I'm from client.");

        var reader = new StreamReader(_tcpClient.GetStream());
        Console.WriteLine(reader.ReadLine());
        
        _tcpClient.Close();
    }
}