using System.Net;
using System.Net.Sockets;
using System.Text;

string[] messages = new[] { "111111", "222222", "33333333", "4444444", "END" };

using TcpClient client = new TcpClient();
await client.ConnectAsync(IPAddress.Loopback, 7777);
var stream = client.GetStream();

var bufferResponse = new List<byte>();
int bufferByte;

Random random = new Random();

foreach(var mess in messages)
{
    Thread.Sleep(3000);
    byte[] buffer = Encoding.UTF8.GetBytes(mess + "\n");
    Console.WriteLine($"From us to Server: {mess}");
    await stream.WriteAsync(buffer);

    while ((bufferByte = stream.ReadByte()) != '\n')
        bufferResponse.Add((byte)bufferByte);
    Console.WriteLine($"From Server: {Encoding.UTF8.GetString(bufferResponse.ToArray())}");
    bufferResponse.Clear();
    Task.Delay(random.Next(3000, 5000));
}