namespace Portfolio.Models.Forms
{
    /// <summary>
    /// This ContactMeFormData class holds the information sent in by a user to submit the
    /// contact form to reach out to me.
    /// </summary>
    public class ContactMeFormData
    {
        /// <summary>
        /// The email the site visitor would want me to contact them back with.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Who the site visitor is, that I may end up speaking to.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// What, if provided, is the site visitor's phone number.
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// The details about why they are contacting me.
        /// </summary>
        public string Message { get; set; }
    }
}
