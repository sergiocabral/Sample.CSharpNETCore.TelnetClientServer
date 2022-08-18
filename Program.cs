using System.Text.RegularExpressions;

var regexMatchNameAndPort = new Regex(@"([^\s:]+):(\d+)").Match(string.Join(' ', args));

var name = regexMatchNameAndPort.Success ? regexMatchNameAndPort.Groups[1].Value : null;

var port = regexMatchNameAndPort.Success ? int.Parse(regexMatchNameAndPort.Groups[2].Value) : 23;

if (args.Contains("--server")) {
    var executable = name ?? "cmd.exe";
    Console.WriteLine($"Server: {executable}:{port}");
} else {
    var hostname = name ?? "localhost";
    Console.WriteLine($"Client: {hostname}:{port}");
}