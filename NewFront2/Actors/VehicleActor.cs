using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewFront2.Messages;
using NExtra.Geo;
using Proto;

namespace NewFront2.Actors
{
    public static partial class Taxi
    {

        public class Position : IEquatable<Position>
        {
            public Position(double longitude, double latitude)
            {
                Longitude = longitude;
                Latitude = latitude;
            }

            public double Longitude { get; }
            public double Latitude { get; }

            public bool Equals(Position other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((Position) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (Latitude.GetHashCode() * 397) ^ Longitude.GetHashCode();
                }
            }

            public static bool operator ==(Position left, Position right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(Position left, Position right)
            {
                return !Equals(left, right);
            }
        }
    }

    public class VehicleActor : IActor
    {
        private const int TailLength = 20;
        private readonly string _id;
        private readonly Queue<Taxi.Position> _positions = new Queue<Taxi.Position>();
        private readonly PID _presenter;
        private readonly string _source;

        public VehicleActor(PID presenter, string id, string source)
        {
            _presenter = presenter;
            _id = id;
            _source = source;
        }

        public Task ReceiveAsync(IContext context)
        {
            switch (context.Message)
            {
                case Taxi.Position p:
                    RememberPosition(p);
                    //TODO: this makes all vehicles become parked the first tick
                    if (_positions.All(p2 => p2 == p))
                        _presenter.Tell(new PositionBearing(p.Longitude, p.Latitude, Bearing(), GpsStatus.Parked,
                            _id,
                            _source));
                    else
                        _presenter.Tell(new PositionBearing(p.Longitude, p.Latitude, Bearing(), GpsStatus.Active,
                            _id,
                            _source));
                    break;
            }
            return Actor.Done;
        }

        private void RememberPosition(Taxi.Position p)
        {
            _positions.Enqueue(p);
            if (_positions.Count > TailLength)
                _positions.Dequeue();
        }

        private double Bearing()
        {
            if (_positions.Count < 2)
                return 0;

            var lasts = _positions.Take(TailLength / 2).ToList();
            var firsts = _positions.Skip(lasts.Count).Take(TailLength / 2).ToList();
            var p1 = new Position(lasts.Sum(p => p.Latitude) / lasts.Count, lasts.Sum(p => p.Longitude) / lasts.Count);
            var p2 = new Position(firsts.Sum(p => p.Latitude) / firsts.Count,
                firsts.Sum(p => p.Longitude) / firsts.Count);

            //   var p2 = _positions.Last();
            //   var p1 = _positions.First();
            var c = new PositionBearingCalculator(new AngleConverter());
            var bearing = c.CalculateBearing(new Position(p1.Latitude, p1.Longitude),
                new Position(p2.Latitude, p2.Longitude));
            return bearing;
        }
    }
}