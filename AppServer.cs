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
    
    private Process? StartProcess()
    {
        try
        {
            var process = Process.Start(_processStartInfo);
            if (process != null)
            {
                return process;
            }
            Console.WriteLine($"{LogPrefix} Process cannot be started.");
        }
        catch (Exception exception)
        {
            Console.WriteLine($"{LogPrefix} Error when start the process: {exception.Message}");
        }
        return null;
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

                var process = StartProcess();
                if (process != null)
                {
                    Console.WriteLine($"{LogPrefix} Process executed: {process.StartInfo.FileName}");
                    
                    new PipeStream(tcpClient.GetStream(), process.StandardInput.BaseStream, nameof(tcpClient), nameof(process.StandardInput)).BeginRead();
                    new PipeStream(process.StandardOutput.BaseStream, tcpClient.GetStream(), nameof(process.StandardOutput), nameof(tcpClient)).BeginRead();
                    new PipeStream(process.StandardError.BaseStream, tcpClient.GetStream(), nameof(process.StandardError), nameof(tcpClient)).BeginRead();
                    
                    Console.WriteLine($"{LogPrefix} Pipe of stream was configured.");
                }
                else
                {
                    Console.WriteLine($"{LogPrefix} Closing.");
                    tcpClient.Close();
                }
            }
            
            Thread.Sleep(1);
        }
        
        Console.WriteLine($"{LogPrefix} Terminated.");
    }
}