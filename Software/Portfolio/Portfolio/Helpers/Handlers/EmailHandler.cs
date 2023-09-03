using Portfolio.Models.MainDb;
using System.Net;
using System.Net.Mail;

namespace Portfolio.Helpers.Handlers
{
    /// <summary>
    /// This EmailHandler class is used whenever any email related tasks need to be performed.
    /// If the user needs to be sent an email of some kind, possibly, with attachments, then this is the
    /// class that is used.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class EmailHandler
    {
        private readonly FileHandler _fileHandler;
        private readonly Env _env;
        public EmailHandler(FileHandler fileHandler, Env env)
        {
            _fileHandler = fileHandler;
            _env = env;
        }

        //https://www.c-sharpcorner.com/blogs/send-email-using-gmail-smtp9
        /// <summary>
        /// Sends an email to a user.
        /// </summary>
        /// <param name="config">Injected: The configuration of the project</param>
        /// <param name="recipientEmail">The user's email</param>
        /// <param name="subject">The subject of the email</param>
        /// <param name="body">The body of only text to send to the user</param>
        /// <param name="files">Any attachments to send to the user</param>
        public String? SendEmail(String recipientEmail, String subject, String body, ICollection<RelatedFile> files = null)
        {

            using (MailMessage mail = new MailMessage())
            {
                String senderEmail = _env.NoReplyAdminEmail;
                String senderPass = _env.NoReplyAdminPass;
                mail.From = new MailAddress(senderEmail);
                mail.To.Add(recipientEmail);
                mail.Subject = "The EASYNU System: " + subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                if (files != null)
                {
                    foreach (RelatedFile file in files)
                    {
                        mail.Attachments.Add(new Attachment(Path.GetFullPath(_fileHandler.GetAbsoluteFilePathOfRelatedFile(file))));
                    }
                }

                if (_env.ShouldStoreEmailToDiskInstead)
                {
                    String pathStored = Path.Combine(_env.FileStoragePath, "Emails");
                    DirectoryInfo directory = new DirectoryInfo(pathStored);
                    if (!directory.Exists)
                    {
                        directory.Create();
                    }

                    using (SmtpClient smtp = new SmtpClient())
                    {
                        // https://stackoverflow.com/questions/4706516/how-to-write-email-on-disk-instead-of-sending-to-real-address-in-asp-net
                        smtp.PickupDirectoryLocation = pathStored;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                        smtp.Send(mail);


                    }

                    return pathStored;
                }
                else
                {
                    using (SmtpClient smtp = new SmtpClient(_env.SMTPEmailPath, _env.SMTPPort))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(senderEmail, senderPass);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }

                    return null;
                }


            }
        }
    }
}
