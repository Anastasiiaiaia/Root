using System.Net;
using System.Net.Sockets;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Server
{
    internal class Program
    {
        static async Task Main()
        {
            var tcpListener = new TcpListener(IPAddress.Any, 8888);

            try
            {
                tcpListener.Start();
                Console.WriteLine("Сервер запущено. Очiкування пiдключень... ");

                while (true)
                {
                    var tcpClient = await tcpListener.AcceptTcpClientAsync();

                    Task.Run(async () => await ProcessClientAsync(tcpClient));

                }
            }
            finally
            {
                tcpListener.Stop();
            }

            async Task ProcessClientAsync(TcpClient tcpClient)
            {
                var stream = tcpClient.GetStream();

                var response = new List<byte>();
                int bytesRead = 10;
                while (true)
                {
                    while (((bytesRead = stream.ReadByte()) != '\n'))
                    {
                        response.Add(((byte)bytesRead));
                    }

                    var request = Encoding.UTF8.GetString(response.ToArray());

                    if (request == "END")
                    {
                        break;
                    }

                    Console.WriteLine($"Клiєнт {tcpClient.Client.RemoteEndPoint} запитав {request}");

                    double number = Convert.ToDouble(request);

                    double result = Math.Sqrt(number);

                    string answer = result.ToString() + '\n';

                    await stream.WriteAsync(Encoding.UTF8.GetBytes(answer));

                    response.Clear();
                }
                tcpClient.Close();
            }
        }
    }
}
