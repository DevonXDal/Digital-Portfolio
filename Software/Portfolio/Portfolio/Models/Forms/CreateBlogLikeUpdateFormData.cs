namespace Portfolio.Models.Forms
{
    /// <summary>
    /// This CreateBlogLikeUpdateFormData class is used to hold form data being sent in for creating
    /// a new blog like update.
    /// </summary>
    public class CreateBlogLikeUpdateFormData
    {
        /// <summary>
        /// The title of the new update.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The main body to the new update.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The image file, if included, that will be displayed in the update post card.
        /// </summary>
        public IFormFile? ImageFile { get; set; }
    }
}
