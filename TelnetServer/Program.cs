using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace TelnetServer
{
    /// <summary>
    /// Classe principal.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Stream que será lido.
        /// </summary>
        private readonly Stream _streamToRead;
        
        /// <summary>
        /// Stream onde será escrito.
        /// </summary>
        private readonly Stream _streamToWrite;

        /// <summary>
        /// Buffer de leitura.
        /// Quanto menor o valor, mais leituras são feitos com stream.BeginRead().
        /// </summary>
        private readonly byte[] _streamToReadBuffer = new byte[1024];

        /// <summary>
        /// Construtor.
        /// Usado para gerar uma instância capaz de ler a entrada de um stream e escrever em outro stream.
        /// </summary>
        /// <param name="streamToRead">Stream que será lido.</param>
        /// <param name="streamToWrite">Stream onde será escrito.</param>
        private Program(Stream streamToRead, Stream streamToWrite)
        {
            _streamToRead = streamToRead;
            _streamToWrite = streamToWrite;
        }

        /// <summary>
        /// Começa o processo de leitura assíncrona do stream.
        /// </summary>
        /// <returns></returns>
        private IAsyncResult BeginRead()
        {
            return _streamToRead.BeginRead(_streamToReadBuffer, 0, _streamToReadBuffer.Length, BeginReadCallback, null);
        }

        /// <summary>
        /// Callback para leitura do stream
        /// </summary>
        /// <param name="request"></param>
        private void BeginReadCallback(IAsyncResult request)
        {
            if (!request.IsCompleted) return;

            var bytes = 0;
            try
            {
                if (_streamToRead.CanRead) bytes = _streamToRead.EndRead(request);
            }
            catch (Exception exception)
            {
                Console.WriteLine("EndRead failed: {0}", exception);
            }

            if (bytes > 0)
            {
                Console.WriteLine("Bytes from {1} to {2}: {0}", bytes, _streamToRead.GetType().Name, _streamToWrite.GetType().Name);
                
                _streamToWrite.Write(_streamToReadBuffer, 0, bytes);
                _streamToWrite.Flush();
                
                BeginRead();
            }
            else
            {
                _streamToWrite.Close();
                Console.WriteLine("Closed: {0}", _streamToWrite.GetType().Name);
            }
        }
        
        /// <summary>
        /// Ponto de entrada da execução do programa.
        /// </summary>
        public static void Main()
        {
            var tcpListener = new TcpListener(IPAddress.Any, 23);
            tcpListener.Start();
            while (true)
            {
                var tcpClient = tcpListener.AcceptTcpClient();
                var tcpClientStream = tcpClient.GetStream();
                
                var process = Process.Start(ProcessStartInfoForCommand);
                
                var fromTcpClient = new Program(tcpClientStream, process?.StandardInput.BaseStream);
                var toTcpClientOutputDefault = new Program(process?.StandardOutput.BaseStream, tcpClientStream);
                var toTcpClientOutputError = new Program(process?.StandardError.BaseStream, tcpClientStream);
                fromTcpClient.BeginRead();
                toTcpClientOutputDefault.BeginRead();
                toTcpClientOutputError.BeginRead();


                Console.WriteLine("Connection from {0}", tcpClient.Client.RemoteEndPoint);
            }
        }

        /// <summary>
        /// Informações para inicialização do cmd.exe
        /// </summary>
        private static ProcessStartInfo ProcessStartInfoForCommand { get; } = new ProcessStartInfo("cmd.exe")
        {
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };
    }
}