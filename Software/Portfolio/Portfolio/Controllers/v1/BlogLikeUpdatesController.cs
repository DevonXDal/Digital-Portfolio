using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Helpers.Handlers;
using Portfolio.Models.Forms;
using Portfolio.Models.MainDb;
using Portfolio.Repositories.Interfaces;

namespace Portfolio.Controllers.v1
{
    /// <summary>
    /// This BlogLikeUpdatesController handles API calls related to the update posts that the user will view to be updated
    /// on current professional life news.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class BlogLikeUpdatesController : Controller
    {
        private readonly IRepo<BlogLikeUpdate> _updatesRepo;
        private readonly FileHandler _fileHandler;

        public BlogLikeUpdatesController(IRepo<BlogLikeUpdate> updatesRepo, FileHandler fileHandler) 
        {
            _fileHandler = fileHandler;
            _updatesRepo = updatesRepo;
        }

        /// <summary>
        /// Returns the full list of blog-like updates after fetching the related image files to send back to the client.
        /// </summary>
        /// <returns>The update posts in serialized form with the images</returns>
        [HttpGet]
        public JsonResult Index()
        {
            ICollection<BlogLikeUpdate> updatesChronological = _updatesRepo.Get().OrderByDescending(u => u.Created).ToList();
            
            // Gets the image as base64 for each if there is one.
            foreach (BlogLikeUpdate update in updatesChronological)
            {
                if (update.RelatedFile == null) continue;

                var bytes = _fileHandler.GetFileAsBytes(update.RelatedFile);
                update.ImageBase64 = _fileHandler.ToBase64(bytes);
            }

            return new JsonResult(updatesChronological);
        }

        /// <summary>
        /// Inserts the new blog-like update into the database.
        /// </summary>
        /// <returns>An Ok (200) response if successful otherwise Bad Request</returns>
        [HttpPut]
        [Authorize("perform:admin-actions")]
        public ActionResult Create(CreateBlogLikeUpdateFormData? formData)
        {
            if (formData == null || formData.Title == null || formData.Description == null)
            {
                return BadRequest("Did not include the necessary title and description to create the new blog-like update.");
            }

            // Create update entity.
            BlogLikeUpdate newUpdate = new()
            {
                Title = formData.Title,
                Description = formData.Description
            };

            // Create and link image file if it exists.
            if (formData.ImageFile != null && _fileHandler.IsRelatedFileImage(formData.ImageFile))
            {
                newUpdate.RelatedFileId = _fileHandler.CreateRelatedFile(formData.ImageFile, formData.Title + "Image");
            }

            // Add to the database
            _updatesRepo.Insert(newUpdate);

            return Ok();
        }

        /// <summary>
        /// Soft deletes the blog post with the id provided.
        /// </summary>
        /// <param name="id">The id that corresponds to the blog-like update post</param>
        /// <returns>An Ok (200) response if successful</returns>
        [HttpDelete]
        [Authorize("perform:admin-actions")]
        public ActionResult Delete(int id)
        {
            try
            {
                _updatesRepo.Delete(id);
            } catch (Exception _)
            {
                return BadRequest("There is no update post with that id, did not remove anything");
            }
            
            return Ok();
        }
    }
}
