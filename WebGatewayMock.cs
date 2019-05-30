using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using WebAssembly;
using System.Text;
using Newtonsoft.Json;

namespace WebGatewayMock {
    public class WebGateway {
        public static void Receive(JSObject msg) {
            Console.WriteLine(">>> " + msg);
            Console.WriteLine("C# GW message received, Type= " + msg.GetObjectProperty("Type") + " Payload=" + msg.GetObjectProperty("Payload"));
        }

        public static void ReceiveJson(string jsonMsg) {
            Console.WriteLine(">>> " + jsonMsg);
            var msg = FromJson<SteerAction>(jsonMsg);
            Console.WriteLine("C# GW message received, Type= " + msg.Type + 
            " event=" + msg.Payload);
        }

        public static void Subscribe(JSObject jsObj, string method) {

            var events = Enumerable
                        .Range(0, 10)
                        .Select(i => new LocationEvent(){Id = Guid.NewGuid(), 
                                                        Timestamp = DateTime.Now, 
                                                        Lat = 50 + 0.01 * i, 
                                                        Long=0.01 + 0.01 * i, 
                                                        Alt = 0.00 })
                        .Select(e => ToJson("LocationChanged", e));

           foreach (var msg in events)
           {
                Console.WriteLine("C# GW Sending new Position= " + msg);
                // jsObj.Invoke(method, new object[] { new object[] {"type", msg.Type, "payload", msg.Payload}});
                jsObj.Invoke(method, new object[] { msg});
                System.Threading.Thread.Sleep(3000);
           }
        }

        public static string ToJson<T>(string messageType,  T payloadObj) {
            var msg = new GatewayMessage<T> {
                Type = messageType,
                Payload = payloadObj
            };
            return  JsonConvert.SerializeObject(msg);
        }

        public static GatewayMessage<T> FromJson<T>(string payloadJson) {
            return JsonConvert.DeserializeObject<GatewayMessage<T>>(payloadJson);
        }
    }

    public class GatewayMessage<T> {
        public string Type { get; set; }

        public T Payload { get; set; }
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

        public override string ToString(){
            return String.Format("SteerAction  Id: {0}, Angle: {1}, Speed: {2}, Timestamp: {3}",
                    Id, Angle, Speed, Timestamp);
        }
    }

}