namespace Portfolio
{
    /// <summary>
    /// This class provides strong typing for all environment variables.
    /// If an environment variable is missing, this class leaves the value blank.
    /// 
    /// The name is shortened from Environment in order to reduce horizontal waste and keep code clean.
    /// 
    /// Author: Devon X. Dalrymple
    /// </summary>
    public class Env
    {
        private IConfiguration _config;

        /// <summary>
        /// Points to the database connection string.
        /// </summary>
        public string MainDb => _config.GetRequiredSection("ConnectionStrings")["MainDb"];

        // ----- Emails and Files -----

        /// <summary>
        /// The email to use for the Super User admin account and for email sending as a no-reply
        /// </summary>
        public string NoReplyAdminEmail => _config["NoReplyAdminEmail"];

        /// <summary>
        /// The password to use for the Super User admin account and for email sending as a no-reply
        /// </summary>
        public string NoReplyAdminPass => _config["NoReplyAdminPass"];

        /// <summary>
        /// This is the path that is taken to send emails using an email server. Like smtp.zoho.com or smtp.gmail.com.
        /// To use a gmail account, a special API logon setting and password are required.
        /// </summary>
        public string SMTPEmailPath => _config["SMTPEmailPath"];

        /// <summary>
        /// The port used to send the content to the email server specified by SMTPEmailPath.
        /// </summary>
        public int SMTPPort => int.Parse(_config["SMTPPort"]);

        /// <summary>
        /// Whether emails sent to the email sender should be saved to the specified file storage path.
        /// </summary>
        public bool ShouldStoreEmailToDiskInstead => bool.Parse(_config["StoreEmailsAsFiles"]);

        /// <summary>
        /// Where files taken in or created by the system should be stored to
        /// </summary>
        public string FileStoragePath => _config["FileStoragePath"];

        // ----- Auth0 -----

        /// <summary>
        /// The port number that this server will run on.
        /// </summary>
        public string Port => _config.GetRequiredSection("Auth0")["Port"];

        /// <summary>
        /// The base url of the client app connected through Auth0.
        /// </summary>
        public string ClientOriginUrl => _config.GetRequiredSection("Auth0")["ClientOriginUrl"];

        /// <summary>
        /// The generated Auth0 domain used for authentication services.
        /// </summary>
        public string Auth0Domain => _config.GetRequiredSection("Auth0")["Domain"];

        /// <summary>
        /// The name of the Auth0 application to target.
        /// </summary>
        public string Auth0Audience => _config.GetRequiredSection("Auth0")["Audience"];

        /// <summary>
        /// The email used for the admin account and that is targeted by submitted contact me forms.
        /// </summary>
        public string AdminEmail => _config["AdminEmail"];


        public Env(IConfiguration config)
        {
            _config = config;
        }
    }
}
