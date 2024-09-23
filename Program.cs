using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Chat
{
    static async Task Main(string[] args)
    {
        // Revisa si se agrego un argumento, en este caso el puerto
        if (args.Length < 1)
        {
            Console.WriteLine("Use: dotnet run <port>");
            return;
        }

        int port;
        if (!int.TryParse(args[0], out port))
        {
            Console.WriteLine("Puerto invalido.");
            return;
        }

        // Inicia el servidor
        _ = Task.Run(() => StartServer(port));

        // Inicia el cliente
        await StartClientLoop();
    }

    // Metodo que inicia el servidor
    static async Task StartServer(int port)
    {
        TcpListener listener = null;
        try
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine($"Server escuchando en el puerto {port}...");
            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                _ = Task.Run(() => HandleClient(client));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Server error: {ex.Message}");
        }
        finally
        {
            listener?.Stop();
        }
    }

    // Metodo para manejar la conexion con el cliente
    static async Task HandleClient(TcpClient client)
    {
        try
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string request = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            Console.WriteLine($"Chat: {request}");

            // Send a response back to the client
            byte[] response = Encoding.ASCII.GetBytes("Mensaje recibido");
            await stream.WriteAsync(response, 0, response.Length);

            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Client handling error: {ex.Message}");
        }
    }

    // Metodo para el loop del cliente
    static async Task StartClientLoop()
    {
        while (true)
        {
            Console.Write("Ingrese la IP de destino: ");
            string ip = Console.ReadLine();
            Console.Write("Ingrese el puerto de destino: ");
            if (!int.TryParse(Console.ReadLine(), out int port))
            {
                Console.WriteLine("Puerto invalido.");
                continue;
            }

            Console.Write("Mensaje a enviar: ");
            string message = Console.ReadLine();

            await SendMessage(ip, port, message);
        }
    }

    // Metodo para enviar mensajes a una IP y Puerto
    static async Task SendMessage(string ip, int port, string message)
    {
        try
        {
            using (TcpClient client = new TcpClient(ip, port))
            {
                NetworkStream stream = client.GetStream();
                byte[] data = Encoding.ASCII.GetBytes(message);
                await stream.WriteAsync(data, 0, data.Length);
                Console.WriteLine($"Enviado: {message}");

                // Respuesta server
                data = new byte[256];
                int bytesRead = await stream.ReadAsync(data, 0, data.Length);
                string response = Encoding.ASCII.GetString(data, 0, bytesRead);
                Console.WriteLine($"Recibido: {response}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Client error: {ex.Message}");
        }
    }
}
