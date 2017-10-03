using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewFront2.Actors;
using Proto;

namespace TaxiShared
{
    public static class Presenter
    {
        public class Sources
        {
            public Sources()
            {
            }

            public Sources(string[] sources)
            {
                Items = sources;
            }

            public string[] Items { get; }
        }

        public class SourceAvailable
        {
            public SourceAvailable(string sourceName)
            {
                SourceName = sourceName;
            }

            public string SourceName { get; set; }
        }

        public class Initialize
        {
            public Initialize(PID client)
            {
                Client = client;
            }

            public PID Client { get; }
        }

        public class Position
        {
            public Position(double longitude, double latitude, string id, string source)
            {
                Longitude = longitude;
                Latitude = latitude;
                Id = id;
                Source = source;
            }

            public double Longitude { get; }
            public double Latitude { get; }
            public string Id { get; }
            public string Source { get; }
        }
    }

    public class VehicleManagerActor : IActor
    {
        private readonly Dictionary<string, PID> _idToVehicleLookup;
        private readonly PID _presenter;

        public VehicleManagerActor(PID presenter)
        {
            _presenter = presenter;
            _idToVehicleLookup = new Dictionary<string, PID>();
        }


        public Task ReceiveAsync(IContext context)
        {
            switch (context.Message)
            {
                case Presenter.Sources _:
                    var sources = new string[_idToVehicleLookup.Keys.Count];
                    _idToVehicleLookup.Keys.CopyTo(sources, 0);
                    context.Respond(new Presenter.Sources(new[] {"Vehicles"}));
                    break;
                case Terminated t:
                    var key = _idToVehicleLookup.FirstOrDefault(_ => _.Value.Equals(t.Who)).Key;
                    _idToVehicleLookup.Remove(key);
                    break;
                case Presenter.Position p:
                    var id = p.Id;
                    if (_idToVehicleLookup.ContainsKey(id) == false)
                    {
                        var taxiCarActor =
                            Actor.Spawn(Actor.FromProducer(() => new VehicleActor(_presenter, id, p.Source)));
                        _idToVehicleLookup.Add(id, taxiCarActor);
                    }
                    var actor = _idToVehicleLookup[id];
                    var position = new Taxi.Position(p.Longitude, p.Latitude);
                    actor.Tell(position);
                    break;
            }
            return Actor.Done;
        }
    }
}