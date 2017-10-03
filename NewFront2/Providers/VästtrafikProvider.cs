using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NewFront2.Messages;
using Newtonsoft.Json;
using Proto;
using TaxiShared;

namespace NewFront2.Providers
{
    public static class VästtrafikProvider
    {
        public static async void Run(PID publisher)
        {
            const string url =
                "http://reseplanerare.vasttrafik.se/bin/query.exe/dny?&look_minx=0&look_maxx=99999999&look_miny=0&look_maxy=99999999&tpl=trains2json&performLocating=1";
            var c = new WebClient();
            while (true)
            {
                var data = await c.DownloadDataTaskAsync(new Uri(url));
                var str = Encoding.UTF8.GetString(data);
                dynamic res = JsonConvert.DeserializeObject(str);

                foreach (var bus in res.look.trains)
                {
                    string id = bus.trainid;
                    double lat = bus.y/1000000d;
                    double lon = bus.x/1000000d;

                    publisher.Tell(new GpsPosition(lon, lat, id, "Västtrafik"));
                }

                //how long should we wait before polling again?
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }
}