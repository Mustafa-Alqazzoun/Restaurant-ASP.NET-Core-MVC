namespace Final_Project.Models
{
    public class LocationRequest
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double BufferRadius { get; set; } // In meters
    }
}
