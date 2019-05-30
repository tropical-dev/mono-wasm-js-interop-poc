using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using WebAssembly;
using System.Text;

namespace WebGatewayMock {
    public class WebGateway {
        public static void Receive(JSObject msg) {
            Console.WriteLine(">>> " + msg);
            Console.WriteLine("C# GW message received, Type= " + msg.GetObjectProperty("Type") + " Payload=" + msg.GetObjectProperty("Payload"));
        }

        public static void Subscribe(JSObject jsObj, string method) {
           foreach (var msg in new GatewayMessage[] {
               new GatewayMessage() { Type = "LocationChanged", Payload = "{Lat:50.1, Long:0.01}"},
                new GatewayMessage( ){ Type = "LocationChanged", Payload = "{Lat:50.3, Long:0.012}"}, 
                 new GatewayMessage() { Type = "LocationChanged", Payload = "{Lat:50.1, Long:0.013}"},
                new GatewayMessage( ){ Type = "LocationChanged", Payload = "{Lat:50.0, Long:0.0125}"},
                 new GatewayMessage() { Type = "LocationChanged", Payload = "{Lat:50.4, Long:0.016}"},
                new GatewayMessage( ){ Type = "LocationChanged", Payload = "{Lat:50.6, Long:0.015}"}}
                )
           {
                Console.WriteLine("C# GW Sending new Position= " + msg.Payload);
                jsObj.Invoke(method, new object[] { new object[] {"type", msg.Type, "payload", msg.Payload}});
                System.Threading.Thread.Sleep(3000);
           }
        }
    }

    public class GatewayMessage {
        public string Type { get; set; }

        public string Payload { get; set; }
    }

    public class LocationEvent {

        public Guid Id { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }

        public double Alt { get; set; }

        public DateTime Timestamp { get; set; }
    }

    public class SteerAction {

        public Guid Id { get; set; }
        public double Angle { get; set; }
        
        public double Speed { get; set; }

        public DateTime  Timestamp { get; set; }
    }

}