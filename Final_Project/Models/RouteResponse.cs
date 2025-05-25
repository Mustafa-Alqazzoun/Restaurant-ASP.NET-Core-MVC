namespace Final_Project.Models
{
    public class RouteResponse
    {
        public string NearestBranchName { get; set; }
        public string NearestBranchAddress { get; set; }
        public double NearestBranchLatitude { get; set; }
        public double NearestBranchLongitude { get; set; }
        public double Distance { get; set; } // Changed from float to double
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}