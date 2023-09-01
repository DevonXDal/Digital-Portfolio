using Portfolio.Models.MainDb.NotMapped;

namespace Portfolio.Models.MainDb
{
    /// <summary>
    /// This ApplicationUser class is used to represent the database model associated with an Auth0 account.
    /// This allows the site to associate various components on the site with this user.
    /// </summary>
    public class ApplicationUser : EntityBase
    {
        /// <summary>
        /// Represents the user id from Auth0. This is used to locate the particular account the current user is using
        /// and to track different users' saved data for this site.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The user's email provided by Auth0.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The full name given by Auth0's 'name' field.
        /// </summary>
        public string Name { get; set; }
    }
}