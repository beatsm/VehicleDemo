
namespace NewFront2.Messages
{

    public class PositionBearing
    {
        public PositionBearing(double longitude, double latitude, double bearing, GpsStatus status, string id,
            string source)
        {
            Bearing = bearing;
            Latitude = latitude;
            Longitude = longitude;
            Id = id;
            GpsStatus = status;
            Source = source;
        }

        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Bearing { get; set; }
        public string Id { get; set; }
        public string Source { get; set; }
        public GpsStatus GpsStatus { get; set; }
    }
}