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
        Console.WriteLine($"{LogPrefix} Starting. Press ESC to exit.");
        
        _tcpListener.Start();
        
        while (!Console.KeyAvailable || Console.ReadKey(true).Key != ConsoleKey.Escape)
        {
            if (_tcpListener.Pending())
            {
                Console.WriteLine($"{LogPrefix} Connection received.");
                
                var tcpClient = _tcpListener.AcceptTcpClient();
                
                // TODO: Falta executar o terminal e direcionar entrada e saída para o TcpClient.
                // Process.Start(_processStartInfo);
                new StreamWriter(tcpClient.GetStream()) { AutoFlush = true }.WriteLine("OUT OF SERVICE");

                Console.WriteLine($"{LogPrefix} Closing.");
                tcpClient.Close();
            }
            
            Thread.Sleep(1);
        }
        
        Console.WriteLine($"{LogPrefix} Terminated.");
    }
}