namespace Crypto_Trading.WebSocket;

using System.Net.WebSockets;
using System.Text;

public class WebSocket
{
    private readonly string _baseUrl = "wss://stream.binance.com:9443";
    
    public async Task ReadFromSocketAsync(string symbol, TimeSpan duration)
    {
        using var cancellationToken = new CancellationTokenSource(duration);
        using var webSocket = new ClientWebSocket();

        try
        {
            var uri = new Uri($"{_baseUrl}/ws/{symbol.ToLower()}@aggTrade");
            await webSocket.ConnectAsync(uri, cancellationToken.Token);

            var buffer = new byte[4096];

            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(buffer, cancellationToken.Token);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    break;
                }

                Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, result.Count));
            }
        }
        catch (OperationCanceledException)
        {
            //Do nothing if duration has elasped
        }
        catch (Exception error)
        {
            Console.Error.WriteLine($"AggTrade error: {error.Message}");
        }
        finally
        {
            if (webSocket.State == WebSocketState.Open)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "done", CancellationToken.None);
            }
        }
    }
}