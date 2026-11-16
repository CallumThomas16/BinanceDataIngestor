namespace Crypto_Trading.WebSocket;

class Program
{
    static async Task Main(string[] args)
    {
        var webSocket = new WebSocket();
        await webSocket.ReadFromSocketAsync("bnbusdt", TimeSpan.FromSeconds(60));
    }
}