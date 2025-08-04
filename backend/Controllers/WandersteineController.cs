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
        [ProducesResponseType(500)]
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
                return StatusCode(500, new { error = "Failed to fetch wandersteine" });
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
        [ProducesResponseType(500)]
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
                return StatusCode(500, new { error = "Failed to fetch wandersteine" });
            }
        }
    }
}