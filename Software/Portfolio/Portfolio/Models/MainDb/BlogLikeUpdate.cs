using Portfolio.Models.MainDb.NotMapped;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolio.Models.MainDb
{
    /// <summary>
    /// This BlogLikeUpdate class represents the update posts that keep visitors up-to-date on any news of my professional life.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class BlogLikeUpdate : EntityBase
    {
        /// <summary>
        /// The title of the blog-like update post.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The information to be included in the blog-like post.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The id of the related file to fetch from the database.
        /// </summary>
        public int? RelatedFileId { get; set; }

        /// <summary>
        /// The RelatedFile entity that corresponds to this blog-like update post.
        /// </summary>
        [ForeignKey(nameof(RelatedFileId))]
        public virtual RelatedFile? RelatedFile { get; set; }

        /// <summary>
        /// The base64 form of the image, so that it can be sent back as json.
        /// </summary>
        [NotMapped]
        public string? ImageBase64 { get; set; }

    }
}
