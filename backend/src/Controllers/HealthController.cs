using Microsoft.AspNetCore.Mvc;

namespace TrailmarksApi.Controllers
{
    /// <summary>
    /// Health check controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Health check endpoint
        /// </summary>
        /// <returns>Service health status</returns>
        /// <response code="200">Service is healthy</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult GetHealth()
        {
            return Ok(new
            {
                status = "healthy",
                service = "trailmarks-backend"
            });
        }
    }
}