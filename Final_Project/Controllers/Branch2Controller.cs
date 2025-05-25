//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Final_Project.Models;
//using NetTopologySuite.Geometries;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Final_Project.Controllers
//{
//    public class Branch2Controller : Controller
//    {
//        private readonly RestaurantContext _context;

//        public Branch2Controller(RestaurantContext context)
//        {
//            _context = context;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> FindNearest([FromBody] LocationRequest request)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            var userLocation = GeometryFactory.Default.CreatePoint(
//                new Coordinate(request.Longitude, request.Latitude));

//            var branchesInRange = await _context.Branches2
//                .Where(b => b.Location != null && b.Location.Distance(userLocation) <= request.BufferRadius)
//                .Select(b => new
//                {
//                    b.Name,
//                    b.Address,
//                    b.Latitude,
//                    b.Longitude,
//                    Distance = (double)b.Location.Distance(userLocation)
//                })
//                .ToListAsync();

//            if (!branchesInRange.Any())
//            {
//                return Json(new RouteResponse { IsSuccess = false, Message = "No branches found within 1 km." });
//            }

//            var nearestBranch = branchesInRange.OrderBy(b => b.Distance).First();

//            return Json(new RouteResponse
//            {
//                NearestBranchName = nearestBranch.Name,
//                NearestBranchAddress = nearestBranch.Address,
//                NearestBranchLatitude = nearestBranch.Latitude,
//                NearestBranchLongitude = nearestBranch.Longitude,
//                Distance = nearestBranch.Distance,
//                IsSuccess = true
//            });
//        }
//    }
//}