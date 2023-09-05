using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Controllers
{
    /// <summary>
    /// Handles basic API paths that don't relate to a certain page or database function. Also processes redirects as necessary.
    /// </summary>
    [ApiController]
    public class DefaultsController : ControllerBase
    {
        public DefaultsController() { }


        /// <summary>
        /// Returns an Ok 200 response to show that the API could be reached successfully
        /// </summary>
        /// <returns>Ok (200) response</returns>
        [HttpGet("api/Is-Alive")]
        public OkResult IsAlive()
        {
            return Ok();
        }
    }
}
