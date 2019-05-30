using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using WebAssembly;
using WebAssembly.Net.Http.HttpClient;
// using WebAssembly.Net.WebSockets;
using ClientWebSocket = WebAssembly.Net.WebSockets.ClientWebSocket;
using System.Net.WebSockets;
using System.Threading;
using System.Text;
using System.Threading.Tasks;

public class WebSocketClient {
    // Uri u = new Uri("ws://localhost:3333");
   
    ClientWebSocket cws = null;
    ArraySegment<byte> buf = new ArraySegment<byte>(new byte[1024]);

    public void Start(string address) { Connect(address); }

    public async Task<int> TheAnswerToLifeUniverseAndEverything() {
        return await Task.Run(() => 42);
    }

    async void Connect(string address) {
        var ans = await TheAnswerToLifeUniverseAndEverything();
        Console.WriteLine("The answer is: " + ans);

        Uri u = new Uri(address);
        cws = new ClientWebSocket();
        try {
            await cws.ConnectAsync(u, CancellationToken.None);
            if (cws.State == WebSocketState.Open) Console.WriteLine("connected");
            SayHello();
            GetStuff();
        }
        catch (Exception e) { 
            Console.WriteLine("Error, couldn't connect to ws address: " + address 
                + " with exception " + e.Message 
                + " " + e.InnerException != null ? e.InnerException.Message : " "); }
    }

    async void SayHello() {
        ArraySegment<byte> b = new ArraySegment<byte>(Encoding.UTF8.GetBytes("hello ws"));
        await cws.SendAsync(b, WebSocketMessageType.Text, true, CancellationToken.None);
    }

    async void GetStuff() {
        WebSocketReceiveResult r = await cws.ReceiveAsync(buf, CancellationToken.None);
        Console.WriteLine("Got: " + Encoding.UTF8.GetString(buf.Array, 0, r.Count));
        SayHello();
        GetStuff();
    }
}

public class WebSocketProgram {
    public static void Main(string address) {
        Console.WriteLine("Starting WebSocket client on:" + address);
        var wsClient = new WebSocketClient();
        wsClient.Start(address);
    }
}