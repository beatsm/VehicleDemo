using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using NewFront2.Actors;
using Proto;
using TaxiFrontend.Hubs;
using TaxiShared;

namespace TaxiFrontend.Actors
{
    public class PresentingActor : IActor
    {
        private static readonly Dictionary<string, ViewPort> UserBounds = new Dictionary<string, ViewPort>();
        private readonly IHubContext _hubContext;

        public PresentingActor()
        {
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<PositionHub>();
        }

        private async Task PositionChanged(Taxi.PositionBearing position)
        {
            var zoomedInUsers = FindUsersSeeingThisVehicle(position);
            _hubContext.Clients.Clients(zoomedInUsers).positionChanged(position);            
        }

        //TODO: inconsistency between messages and methods. 
        private async Task SourceChanged(Presenter.SourceAvailable s)
        {
            _hubContext.Clients.All.sourceAdded(s.SourceName);
        }

        private List<string> FindUsersSeeingThisVehicle(Taxi.PositionBearing position)
        {
            return
                UserBounds.Where(b => b.Value.Contains(position.Longitude, position.Latitude))
                    .Select(b => b.Key)
                    .ToList();
        }

        public class Disconnected
        {
            public Disconnected(string userId)
            {
                UserId = userId;
            }

            public string UserId { get; }
        }

        public class UpdatedBounds
        {
            public UpdatedBounds(double latitudeNorthEast, double longitudeNorthEast, double latitudeSouthWest,
                double longitudeSouthWest, double zoomLevel)
            {
                LatitudeNorthEast = latitudeNorthEast;
                LongitudeNorthEast = longitudeNorthEast;
                LatitudeSouthWest = latitudeSouthWest;
                LongitudeSouthWest = longitudeSouthWest;
                ZoomLevel = zoomLevel;
            }

            public string UserId { get; set; }
            public double LatitudeNorthEast { get; }
            public double LongitudeNorthEast { get; }
            public double LatitudeSouthWest { get; }
            public double LongitudeSouthWest { get; }
            public double ZoomLevel { get; }
        }

        public async Task ReceiveAsync(IContext context)
        {
            switch (context.Message)
            {
                case Taxi.PositionBearing p:
                    PositionChanged(p);
                    break;
                case Presenter.SourceAvailable s:
                    SourceChanged(s);
                    break;
                case UpdatedBounds bounds:
                    //create a new viewport for the user
                    UserBounds[bounds.UserId] = new ViewPort(bounds.LatitudeNorthEast, bounds.LongitudeNorthEast,
                        bounds.LatitudeSouthWest, bounds.LongitudeSouthWest, bounds.ZoomLevel);
                    break;
                case Disconnected disconnected:
                    UserBounds.Remove(disconnected.UserId);
                    break;
            }
        }
    }

    public class ViewPort
    {
        public ViewPort(double latitudeNorthEast, double longitudeNorthEast, double latitudeSouthWest,
            double longitudeSouthWest, double zoomLevel)
        {
            LatitudeNorthEast = latitudeNorthEast;
            LongitudeNorthEast = longitudeNorthEast;
            LatitudeSouthWest = latitudeSouthWest;
            LongitudeSouthWest = longitudeSouthWest;
            ZoomLevel = zoomLevel;
        }

        public double LatitudeNorthEast { get; }
        public double LongitudeNorthEast { get; }
        public double LatitudeSouthWest { get; }
        public double LongitudeSouthWest { get; }
        public double ZoomLevel { get; }
    }
}