using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrailmarksApi.Data;
using TrailmarksApi.Models;

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
        [ProducesResponseType(typeof(WandersteinResponse), 200)]
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
                    _logger.LogWarning($"Wanderstein with unique ID {uniqueId} not found");
                    return Problem(
                        title: "Resource not found",
                        statusCode: StatusCodes.Status404NotFound,
                        detail: $"The requested Wanderstein with ID '{uniqueId}' was not found"
                    );
                }

                var response = WandersteinResponse.FromEntity(wanderstein);
                _logger.LogInformation($"Retrieved Wanderstein with unique ID {uniqueId}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching Wanderstein with unique ID {uniqueId}");
                return Problem(
                    title: "An error occurred while fetching the Wanderstein",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }
    }
}