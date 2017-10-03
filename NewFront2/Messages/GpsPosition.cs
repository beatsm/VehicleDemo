namespace NewFront2.Messages
{

    public class GpsPosition
    {
        public GpsPosition(double longitude, double latitude, string id, string source)
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