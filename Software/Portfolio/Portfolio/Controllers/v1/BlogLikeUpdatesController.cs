using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Controllers.v1
{
    /// <summary>
    /// This BlogLikeUpdatesController handles API calls related to the update posts that the user will view to be updated
    /// on current professional life news.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class BlogLikeUpdatesController : Controller
    {
        /// <summary>
        /// Returns the full list of blog-like updates after fetching the related image files to send back to the client.
        /// </summary>
        /// <returns>The update posts in serialized form with the images</returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Inserts the new blog-like update into the database.
        /// </summary>
        /// <returns>A Created (201) response if successful</returns>
        [HttpPut]
        [Authorize("perform:admin-actions")]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Soft deletes the blog post with the id provided.
        /// </summary>
        /// <param name="id">The id that corresponds to the blog-like update post</param>
        /// <returns>An Ok (200) response if successful</returns>
        [HttpPost]
        [Authorize("perform:admin-actions")]
        public ActionResult Delete(int id)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
