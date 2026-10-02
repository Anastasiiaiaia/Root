using System.Net.Sockets;
using System.Text;

namespace _20._09dz
{
    internal class Program
    {
        static async Task Main()
        {
            using (TcpClient tcpClient = new TcpClient())
            {
                await tcpClient.ConnectAsync("127.0.0.1", 8888);
                var stream = tcpClient.GetStream();

                while (true)
                {
                    Console.WriteLine("1 - Корiнь");
                    Console.WriteLine("2 - Вихiд");
                    Console.WriteLine("Вибiр клiєнта: ");

                    string choice = Console.ReadLine();

                    string request;

                    if (choice == "1")
                    {
                        Console.Write("Введiть число: ");
                        string number = Console.ReadLine();

                        request = number;
                    }
                    else if (choice == "2")
                    {
                        request = "END\n";

                        await stream.WriteAsync(Encoding.UTF8.GetBytes(request));

                        break;
                    }
                    else
                    {
                        Console.WriteLine("Невiрний вибiр.");
                        return;
                    }

                    request += '\n';
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(request));

                    var response = new List<byte>();
                    int bytesRead = 10;

                    while ((bytesRead = stream.ReadByte()) != '\n')
                    {
                        response.Add((byte)bytesRead);
                    }

                    var result = Encoding.UTF8.GetString(response.ToArray());

                    Console.WriteLine($"Вiдповiдь сервера: {result}");
                }

            }
        }
    }
}
