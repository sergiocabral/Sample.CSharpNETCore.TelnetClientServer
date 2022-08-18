using System.Text.RegularExpressions;
using TelnetClientServer;

var regexMatchNameAndPort = new Regex(@"([^\s:]+):(\d+)").Match(string.Join(' ', args));

var name = regexMatchNameAndPort.Success ? regexMatchNameAndPort.Groups[1].Value : null;

var port = regexMatchNameAndPort.Success ? int.Parse(regexMatchNameAndPort.Groups[2].Value) : 23;

if (args.Contains("--server"))
{
    var executable = name ?? "cmd.exe";
    new AppServer(executable, port).Start();
} else {
    var hostname = name ?? "localhost";
    new AppClient(hostname, port).Start();
}