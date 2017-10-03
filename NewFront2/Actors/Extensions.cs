namespace NewFront2.Actors
{
    public static class Extensions
    {
        public static bool Contains(this ViewPort viewPort, double longitude, double latitude)
        {
            return viewPort.LatitudeNorthEast > latitude
                   && viewPort.LatitudeSouthWest < latitude
                   && viewPort.LongitudeNorthEast > longitude
                   && viewPort.LongitudeSouthWest < longitude;
        }
    }
}