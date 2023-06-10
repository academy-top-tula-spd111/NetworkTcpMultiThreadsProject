using System.Net;
using System.Net.Sockets;
using System.Text;

TcpListener listener = new TcpListener(IPAddress.Loopback, 7777);

try
{
    listener.Start();
    Console.WriteLine($"Server starting...");

    while(true)
    {
        var client = await listener.AcceptTcpClientAsync();
        Task.Run(async () => await ConnectClientAsync(client));
    }
}
finally
{
    listener.Stop();
}


async Task ConnectClientAsync(TcpClient client)
{
    string[] messages = new[] { "aaaaaaa", "bbbbbbb", "ccccccc" };

    var stream = client.GetStream();
    var bufferResponse = new List<byte>();
    int bufferByte;

    Random random = new Random();

    while(true)
    {
        while ((bufferByte = stream.ReadByte()) != '\n')
            bufferResponse.Add((byte)bufferByte);
        var response = Encoding.UTF8.GetString(bufferResponse.ToArray());
        if (response == "END") break;
        Console.WriteLine($"From Client: {response}");
        string message = messages[random.Next(0, messages.Length)];
        Console.WriteLine($"To Client: {message}");
        await stream.WriteAsync(Encoding.UTF8.GetBytes(message + "\n"));
        bufferResponse.Clear();
    }

    client.Close();
}