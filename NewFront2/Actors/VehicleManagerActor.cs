using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewFront2.Actors;
using NewFront2.Messages;
using Proto;

namespace TaxiShared
{
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
                case Terminated t:
                    var key = _idToVehicleLookup.FirstOrDefault(_ => _.Value.Equals(t.Who)).Key;
                    _idToVehicleLookup.Remove(key);
                    break;
                case GpsPosition p:
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