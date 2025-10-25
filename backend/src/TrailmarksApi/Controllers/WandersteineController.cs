using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using TrailmarksApi.Data;
using TrailmarksApi.Models;
using TrailmarksApi.Services;

namespace TrailmarksApi.Controllers
{
    /// <summary>
    /// Controller for managing Wandersteine (hiking stones)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class WandersteineController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<WandersteineController> _logger;

        public WandersteineController(ApplicationDbContext context, ILogger<WandersteineController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Get the 5 most recently added Wandersteine
        /// </summary>
        /// <returns>List of the 5 most recent hiking stones</returns>
        /// <response code="200">Returns the list of recent Wandersteine</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("recent")]
        [ProducesResponseType(typeof(IEnumerable<WandersteinResponse>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetRecentWandersteine()
        {
            try
            {
                var wandersteine = await _context.Wandersteine
                    .OrderByDescending(w => w.CreatedAt)
                    .Take(5)
                    .ToListAsync();

                var responses = wandersteine.Select(WandersteinResponse.FromEntity).ToList();
                
                _logger.LogInformation($"Retrieved {responses.Count} recent Wandersteine");
                return Ok(responses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching recent Wandersteine");
                return Problem(
                    title: "An error occurred while fetching recent Wandersteine",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        /// <summary>
        /// Get Wandersteine within a specified radius of a location
        /// </summary>
        /// <param name="latitude">Latitude of the center point (defaults to Bochum: 51.4818)</param>
        /// <param name="longitude">Longitude of the center point (defaults to Bochum: 7.2162)</param>
        /// <param name="radiusKm">Search radius in kilometers (defaults to 100km if not specified, 50km if position provided)</param>
        /// <returns>List of hiking stones within the specified radius</returns>
        /// <response code="200">Returns the list of nearby Wandersteine</response>
        /// <response code="400">If the provided coordinates are invalid</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("nearby")]
        [ProducesResponseType(typeof(IEnumerable<WandersteinResponse>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetNearbyWandersteine(
            [FromQuery] double? latitude = null,
            [FromQuery] double? longitude = null,
            [FromQuery] double? radiusKm = null)
        {
            try
            {
                // Default to Bochum coordinates if not provided
                const double defaultLatitude = 51.4818;
                const double defaultLongitude = 7.2162;
                
                var centerLat = latitude ?? defaultLatitude;
                var centerLon = longitude ?? defaultLongitude;
                
                // Default radius: 50km if position provided, 100km if using default (Bochum)
                var searchRadius = radiusKm ?? (latitude.HasValue && longitude.HasValue ? 50.0 : 100.0);

                // Validate coordinates
                var centerCoord = new GeoCoordinate(centerLat, centerLon);
                if (!centerCoord.IsValid())
                {
                    return Problem(
                        title: "Invalid coordinates",
                        statusCode: StatusCodes.Status400BadRequest,
                        detail: "The provided latitude or longitude values are outside valid ranges"
                    );
                }

                // Use NetTopologySuite with PostGIS for efficient radius filtering in database
                // The LocationPoint property is a PostGIS Point that EF Core can query directly
                var radiusInMeters = searchRadius * 1000;
                
                // Create center point using NetTopologySuite with WGS84 SRID (4326)
                const int WGS84_SRID = 4326;
                var geometryFactory = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(srid: WGS84_SRID);
                var centerPoint = geometryFactory.CreatePoint(new Coordinate(centerLon, centerLat));
                
                // Query using LINQ - EF Core with NetTopologySuite translates Distance() to PostGIS ST_Distance
                // The LocationPoint column is a PostGIS geography point for accurate spherical distance
                var nearbyWandersteine = await _context.Wandersteine
                    .Where(w => w.LocationPoint != null && w.LocationPoint.Distance(centerPoint) <= radiusInMeters)
                    .OrderByDescending(w => w.CreatedAt)
                    .ToListAsync();

                var responses = nearbyWandersteine.Select(WandersteinResponse.FromEntity).ToList();
                
                _logger.LogInformation(
                    $"Retrieved {responses.Count} Wandersteine within {searchRadius}km of ({centerLat}, {centerLon})");
                
                return Ok(responses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching nearby Wandersteine");
                return Problem(
                    title: "An error occurred while fetching nearby Wandersteine",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        /// <summary>
        /// Get all Wandersteine
        /// </summary>
        /// <returns>List of all hiking stones</returns>
        /// <response code="200">Returns the list of all Wandersteine</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<WandersteinResponse>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetAllWandersteine()
        {
            try
            {
                var wandersteine = await _context.Wandersteine
                    .OrderByDescending(w => w.CreatedAt)
                    .ToListAsync();

                var responses = wandersteine.Select(WandersteinResponse.FromEntity).ToList();
                
                _logger.LogInformation($"Retrieved {responses.Count} Wandersteine");
                return Ok(responses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all Wandersteine");
                return Problem(
                    title: "An error occurred while fetching Wandersteine",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        /// <summary>
        /// Get a specific Wanderstein by its unique identifier
        /// </summary>
        /// <param name="uniqueId">The unique identifier (e.g., WS-2024-001)</param>
        /// <returns>The requested hiking stone</returns>
        /// <response code="200">Returns the requested Wanderstein</response>
        /// <response code="404">If the Wanderstein was not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{uniqueId}")]
        [ProducesResponseType(typeof(WandersteinDetailResponse), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> GetWandersteinByUniqueId(string uniqueId)
        {
            try
            {
                var wanderstein = await _context.Wandersteine
                    .FirstOrDefaultAsync(w => w.UniqueId == uniqueId);

                if (wanderstein == null)
                {
                    _logger.LogWarning("Wanderstein with unique ID {UniqueId} not found", uniqueId);
                    return Problem(
                        title: "Resource not found",
                        statusCode: StatusCodes.Status404NotFound,
                        detail: $"The requested Wanderstein with ID '{uniqueId}' was not found"
                    );
                }

                var response = WandersteinDetailResponse.FromEntity(wanderstein);
                _logger.LogInformation("Retrieved Wanderstein with unique ID {UniqueId}", uniqueId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Wanderstein with unique ID {UniqueId}", uniqueId);
                return Problem(
                    title: "An error occurred while fetching the Wanderstein",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }
    }
}