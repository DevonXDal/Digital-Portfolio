using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Helpers.Handlers;
using Portfolio.Models.Forms;

namespace Portfolio.Controllers.v1
{
    /// <summary>
    /// This ContactFormController class accepts requests related to the contact me form that
    /// users can insert information in order to start a potential conversation with me.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class ContactFormController : ControllerBase
    {
        private readonly EmailHandler _emailHandler;
        private readonly Env _env;

        public ContactFormController(EmailHandler emailHandler)
        {
            _emailHandler = emailHandler;
        }


        /// <summary>
        /// Accepts the necessary data from the contact form and calls for an email to be sent in order
        /// to contact me.
        /// </summary>
        /// <returns>Ok 200 if successful, otherwise Bad Request 400</returns>
        [HttpPut]
        public IActionResult SendContactForm(ContactMeFormData formData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Missing the required information to submit the form");
            }
            
            /*
             * EXAMPLE:
             * Subject: Potential Job: Jane Doe Sent a Contact Form Submission
             * Body:
             * Email: janed2@businessname.com
             * Phone Number: 3045551928
             * 
             * Hey this Jane, I am a hiring recruiter with Business Name.
             * ...
             */
            _emailHandler.SendEmail(_env.AdminEmail, $"Potential Job: {formData.Name} Sent a Contact Form Submission",
                $"Email: {formData.Email}\nPhone Number: {(formData.Phone ?? "N/A")}\n\n{formData.Message}");

            return Ok();
        }
    }
}
