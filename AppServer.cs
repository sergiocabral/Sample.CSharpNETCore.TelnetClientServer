using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace TelnetClientServer;

public class AppServer
{
    private const string LogPrefix = $"[ {nameof(AppServer)} ]";
    
    private readonly TcpListener _tcpListener;
    private readonly ProcessStartInfo _processStartInfo;

    public AppServer(string executable, int port)
    {
        _tcpListener = new TcpListener(IPAddress.Any, port);
        _processStartInfo = new ProcessStartInfo(executable)
        {
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        Console.WriteLine($"{LogPrefix} Created. Executable \"{executable}\" to port {port}.");
    }

    public void Start()
    {
        Console.WriteLine("I'm a SERVER.");
        
        _tcpListener.Start();
        var tcpClient = _tcpListener.AcceptTcpClient();

        var writer = new StreamWriter(tcpClient.GetStream());
        writer.AutoFlush = true;
        writer.WriteLine("Hello! I'm from server.");

        var reader = new StreamReader(tcpClient.GetStream());
        Console.WriteLine(reader.ReadLine());
        
        tcpClient.Close();
    }
}