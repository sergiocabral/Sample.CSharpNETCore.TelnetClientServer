namespace TelnetClientServer;

public class PipeStream
{
    private readonly string _logPrefix;
    private static int _instanceCount;

    private readonly Stream _reader;
    private readonly Stream _writer;
    private readonly byte[] _buffer = new byte[1024];

    public PipeStream(Stream reader, Stream writer, string? readerName = null, string? writerName = null)
    {
        _reader = reader;
        _writer = writer;
        _logPrefix = $"[ {nameof(PipeStream)}#{++_instanceCount} : {readerName ?? reader.GetType().Name} -> {writerName ?? writer.GetType().Name} ]";
        
        Console.WriteLine($"{_logPrefix} Created.");
    }

    public void BeginRead()
    {
        Console.WriteLine($"{_logPrefix} Reading.");
        
        _reader.BeginRead(_buffer, 0, _buffer.Length, BeginReadCallback, null);
    }

    private void BeginReadCallback(IAsyncResult asyncResult)
    {
        if (!asyncResult.IsCompleted)
        {
            Console.WriteLine($"{_logPrefix} Not complete to read.");
            
            return;
        }

        var bytesRead = 0;
        try
        {
            if (_reader.CanRead)
            {
                bytesRead = _reader.EndRead(asyncResult);
                
                Console.WriteLine($"{_logPrefix} Bytes read: {bytesRead}");
            }
            else
            {
                Console.WriteLine($"{_logPrefix} Can't read.", _logPrefix);
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine($"{_logPrefix} Error when read: {exception.Message}");
        }

        if (bytesRead > 0 && _writer.CanWrite)
        {
            Console.WriteLine($"{_logPrefix} Writing.", _logPrefix);

            try
            {
                _writer.Write(_buffer, 0, bytesRead);
                _writer.Flush();
                
                BeginRead();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{_logPrefix} Error when write: {exception.Message}");
            }
        }
        else
        {
            Console.WriteLine($"{_logPrefix} Closing.", _logPrefix);
            
            _reader.Close();
            _writer.Close();
        }
    }
}